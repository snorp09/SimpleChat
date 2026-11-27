using System.Net;
using System.Net.Sockets;
using SimpleChatLib.Types;
using SimpleChatServ.Lib;

var ipEndPoint = new IPEndPoint(IPAddress.Any, 12346);
using var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
listener.Bind(ipEndPoint);
listener.Listen(100);
Console.WriteLine("Server is listening on port 12346...");
var clientQueue = new ClientQueue();
var aliveCheckTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));

_ = Task.Run(async () =>
{
    while (await aliveCheckTimer.WaitForNextTickAsync())
    {
        Console.WriteLine("Performing alive check...");
        await clientQueue.AliveCheck();
    }
});

while (true)
{
    var clientSocket = await listener.AcceptAsync();
    Console.WriteLine("Client connected: " + clientSocket.RemoteEndPoint);
    var client = new Client(clientSocket);
    clientQueue.Enqueue(client);
    _ = Task.Run(async () =>
    {
        while (true)
        {
            var message = await client.ReceiveMessage();
            await MessageHandler.HandleMessage(message, client, clientQueue);
        }
    });
}

