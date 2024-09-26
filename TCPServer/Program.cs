using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

Console.WriteLine("TcpServerTest");

TcpListener listener = new TcpListener(IPAddress.Any, 7); 
listener.Start();
while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    Console.WriteLine("Client connected");
    Task.Run(() => HandleClient(socket));
}

void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);

    while (socket.Connected)
    {
        string message = reader.ReadLine();
        if (message == null) break;

        var request = JsonSerializer.Deserialize<Request>(message);
        string response = HandleRequest(request);
        writer.WriteLine(response);
        writer.Flush();
    }

    socket.Close();
}

string HandleRequest(Request request)
{
    switch (request.Command.ToLower())
    {
        case "random":
            int min = request.Parameters[0];
            int max = request.Parameters[1];
            Random random = new Random();
            int randomNumber = random.Next(min, max + 1);
            return JsonSerializer.Serialize(new { result = randomNumber });

        case "add":
            int sum = request.Parameters[0] + request.Parameters[1];
            return JsonSerializer.Serialize(new { result = sum });

        case "subtract":
            int sub = request.Parameters[0] - request.Parameters[1];
            return JsonSerializer.Serialize(new { result = sub });

        case "stop":
            return JsonSerializer.Serialize(new { result = "Server shutting down" });

        case "help":
            return JsonSerializer.Serialize(new { result = "Hello World\n Following commands are available\n 1. Random\n 2. Add\n 3. Subtract" });

        default:
            return JsonSerializer.Serialize(new { result = "Unknown command" });
    }
}

public class Request
{
    public string Command { get; set; }
    public int[] Parameters { get; set; }
}