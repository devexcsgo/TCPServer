using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
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
            socket.Close();
        }
    }

    Console.WriteLine("stop");

}