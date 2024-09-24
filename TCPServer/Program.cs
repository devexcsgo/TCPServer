using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

Console.WriteLine("TcpServerTest");

TcpListener listener = new TcpListener(IPAddress.Any, 7);
listener.Start();
while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    IPEndPoint iPEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;

    Console.WriteLine("client connected ");
    Task.Run(() => HandleClient(socket));
}

listener.Stop();


void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);

    while (socket.Connected)
    {
        string message = reader.ReadLine();
        Console.WriteLine(message);
        writer.WriteLine(message);
        writer.Flush();
        if (message == "stop")
        {
            writer.WriteLine("Server shutting down");
            writer.Flush();
            socket.Close();
        }
        if (message == "help")
        {
            writer.WriteLine("Hello World\n Following commands are available\n 1. Random\n 2. Add\n 3. Subtract");
            writer.Flush();
        }
        if (message == "Random")
        {
            writer.WriteLine("Enter the minimum number:");
            writer.Flush();
            int min = Convert.ToInt32(reader.ReadLine());

            writer.WriteLine("Enter the maximum number:");
            writer.Flush();
            int max = Convert.ToInt32(reader.ReadLine());

            Random random = new Random();
            int randomNumber = random.Next(min, max + 1);

            writer.WriteLine(randomNumber);
            writer.Flush();
        }
        if (message == "Add")
        {
            writer.WriteLine("Enter first number");
            writer.Flush();
            int num1 = Convert.ToInt32(reader.ReadLine());
            writer.WriteLine("Enter second number");
            writer.Flush();
            int num2 = Convert.ToInt32(reader.ReadLine());
            int sum = num1 + num2;
            writer.WriteLine(sum);
            writer.Flush();
        }
        if (message == "Subtract")
        {
            writer.WriteLine("Enter first number");
            writer.Flush();
            int num1 = Convert.ToInt32(reader.ReadLine());
            writer.WriteLine("Enter second number");
            writer.Flush();
            int num2 = Convert.ToInt32(reader.ReadLine());
            int sub = num1 - num2;
            writer.WriteLine(sub);
            writer.Flush();
        }

    }

    Console.WriteLine("stop");

}