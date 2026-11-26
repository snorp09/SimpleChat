namespace SimpleChatServ.Lib;
using SimpleChatLib.Types;
public static class MessageHandler
{
    public static async Task HandleMessage(Message message, Client client, ClientQueue clientQueue)
    {
        switch (message.Command)
        {
            case Commands.Ok:
                // Handle OK command
                break;
            case Commands.Join:
                await HandleJoin(message, client, clientQueue);
                break;
            case Commands.Ping:
                await HandlePing(client);
                break;
            case Commands.Say:
                await HandleSay(message, client, clientQueue);
                break;
        }
    }

    public static async Task HandleJoin(Message message, Client client, ClientQueue clientQueue)
    {
        // Handle Join command
        var okMessage = new Message
        {
            Source = "Server",
            Command = Commands.Received,
        };
        await client.SendMessage(okMessage);
        if (message.Source == "NewUser")
        {
            client.UserId = message.Args[0];
            client.Name = message.Args[0];
        }
        else
        {
            client.UserId = message.Source;
            client.Name = message.Args[0];
        }
        Message joinMessage = new Message
        {
            Source = client.UserId,
            Command = Commands.Join,
            Args = new string[] { client.Name }
        };
        await clientQueue.SendMessageToAll(joinMessage);
    }

        public static async Task HandlePing(Client client)
    {
        var pongMessage = new Message
        {
            Source = "Server",
            Command = Commands.Ok,
        };
        await client.SendMessage(pongMessage);
    }

    public static async Task HandleSay(Message message, Client client, ClientQueue clientQueue)
    {
        // Handle Say command
        var receivedMessage = new Message
        {
            Source = "Server",
            Command = Commands.Received,
        };
        await client.SendMessage(receivedMessage);
        await clientQueue.SendMessageToAll(message);
    }
}
