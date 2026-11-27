using System.Net;
using System.Net.Sockets;
using SimpleChatLib.Types;

namespace SimpleChatServ.Lib;

public class Client(Socket clientSocket)
{
    public Socket ClientSocket { get; set; } = clientSocket;
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool StillAlive {get; set; } = true;

    public Task SendMessage(Message message)
    {
        var messageBytes = message.ToBytes();
        return ClientSocket.SendAsync(messageBytes);
    }

    public async Task<Message> ReceiveMessage()
    {
        CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var buffer = new byte[1024];
        var receivedBytes = await ClientSocket.ReceiveAsync(buffer, SocketFlags.None, cts.Token);
        var rawMsg = System.Text.Encoding.UTF8.GetString(buffer, 0, receivedBytes).Trim();
        return Message.ParseMessage(rawMsg);
    }

    public async Task Disconnect()
    {
        // if (ClientSocket?.Connected is not true)
        //     return;
        // await SendMessage(new Message
        // {
        //     Source = UserId != string.Empty ? UserId : "Unknown",
        //     Command = Commands.Disconnect,
        // });
        await ClientSocket.DisconnectAsync(false);
    }
}
