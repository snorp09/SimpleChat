using System.Collections.Generic;
using SimpleChatLib.Types;
using SimpleChatServ.Lib;

public class ClientQueue
{
    private Queue<Client> _clients = new();

    public void Enqueue(Client client)
    {
        _clients.Enqueue(client);
    }

    public Client Dequeue()
    {
        return _clients.Dequeue();
    }

    public int Count => _clients.Count;

    public Task SendMessageToAll(Message message)
    {
        var tasks = _clients.Select(client => client.SendMessage(message));
        return Task.WhenAll(tasks);
    }

    public async Task AliveCheck()
    {
        int disconnectedCount = 0;
        foreach (var client in _clients)
        {
            client.StillAlive = false;
        }
        var tasks = _clients.Select(client => client.SendMessage(new Message
        {
            Source = "Server",
            Command = Commands.Ping,
        }));
        await Task.WhenAll(tasks);

        //This is kind of a hacky sleep trick, giving clients some time to respond to the ping before checking their StillAlive status.
        await Task.Delay(10000); // Wait for 10 seconds before checking StillAlive flags.
        foreach (var client in _clients)
        {
            if (!client.StillAlive)
            {
                await DisconnectClient(client);
                disconnectedCount++;
            }
        }
        Console.WriteLine($"Disconnected clients: {disconnectedCount}");
    }

    public async Task DisconnectClient(Client client)
    {
        _clients = new Queue<Client>(_clients.Where(c => c != client && c.StillAlive));
        foreach (var c in _clients)
        {
            await c.SendMessage(new Message
            {
                Source = client.UserId,
                Command = Commands.Disconnect,
            });
        }
        await client.Disconnect();
    }
}