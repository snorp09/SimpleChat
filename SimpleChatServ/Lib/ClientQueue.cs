using System.Collections.Generic;
using SimpleChatLib.Types;
using SimpleChatServ.Lib;

public class ClientQueue
{
    private readonly Queue<Client> _clients = new();

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
}