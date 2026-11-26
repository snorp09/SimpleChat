using System.Net;
using System.Net.Sockets;
using SimpleChatLib.Types;

namespace SimpleChatServ.Lib;

public class Client(Socket clientSocket)
{
    public Socket ClientSocket { get; set; } = clientSocket;
    public string UserId { get; set; }
    public string Name { get; set; }

    public Task SendMessage(Message message)
    {
        var messageBytes = message.ToBytes();
        return ClientSocket.SendAsync(messageBytes);
    }

    public async Task<Message> ReceiveMessage()
    {
        var buffer = new byte[1024];
        var receivedBytes = await ClientSocket.ReceiveAsync(buffer);
        var rawMsg = System.Text.Encoding.UTF8.GetString(buffer, 0, receivedBytes).Trim();
        return Message.ParseMessage(rawMsg);
    }
}
