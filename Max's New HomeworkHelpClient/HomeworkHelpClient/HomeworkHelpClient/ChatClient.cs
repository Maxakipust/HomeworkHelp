using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.IO;

public class ChatClient
{
    private NetworkStream NetStream;
    public TcpClient client;
    public BackgroundWorker ListenWorker = new BackgroundWorker();
    public Action<string,string> onMsg;
    public string N = "";

    public ChatClient(int port, Action<string,string> onMessage, string name)
    {
        N = name;
        onMsg = onMessage;
        string x = File.ReadAllText("IP.txt");
        string ip = x == "Local" ? Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString() : x;
        Console.WriteLine("connecting to " + ip + ":" + port.ToString());
        client = new TcpClient();
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
        while (!client.Connected)
        {
            try
            {
                client.Connect(ep);
            }
            catch (Exception ex)
            {

            }
        }
        NetStream = client.GetStream();
        ListenWorker.DoWork += ListenWorker_DoWork;
        ListenWorker.RunWorkerCompleted += ListenWorker_RunWorkerCompleted;
        ListenWorker.RunWorkerAsync();
    }

    public void send(byte[] data, string dataType)
    {
        int bufferSize = 1024;
        byte[] dataLength = BitConverter.GetBytes(data.Length);//length
        byte[] dataTypeArr = Encoding.ASCII.GetBytes(dataType);//type
        byte[] dataTypeLength = BitConverter.GetBytes(dataTypeArr.Length);//typeLength
        byte[] package = new byte[8 + dataTypeArr.Length + data.Length];
        dataLength.CopyTo(package, 0);
        dataTypeLength.CopyTo(package, 4);
        dataTypeArr.CopyTo(package, 8);
        data.CopyTo(package, 8 + dataTypeArr.Length);

        int bytesSent = 0;
        int bytesLeft = package.Length;

        while (bytesLeft > 0)
        {
            int packetSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;

            NetStream.Write(package, bytesSent, packetSize);
            bytesSent += packetSize;
            bytesLeft -= packetSize;
        }
    }

    public void ListenWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        ListenWorker.RunWorkerAsync();
    }

    public void ListenWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        int bytesRead = 0;
        int bufferSize = 1024;
        int allBytesRead = 0;

        byte[] length = new byte[4];
        byte[] typeLengthArr = new byte[4];

        bytesRead = NetStream.Read(length, 0, 4);//length
        int dataLength = BitConverter.ToInt32(length, 0);

        bytesRead += NetStream.Read(typeLengthArr, 0, 4);//typeLength
        int typeLength = BitConverter.ToInt32(typeLengthArr, 0);

        byte[] type = new byte[typeLength];
        bytesRead += NetStream.Read(type, 0, typeLength);//type
        string dataType = Encoding.ASCII.GetString(type);

        int bytesLeft = dataLength;
        byte[] data = new byte[dataLength];

        while (bytesLeft > 0)
        {
            int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;
            bytesRead = NetStream.Read(data, allBytesRead, nextPacketSize);
            allBytesRead += bytesRead;
            bytesLeft -= bytesRead;
        }
        onMsg(Encoding.ASCII.GetString(data),dataType);
    }
    public void sendString(string data, string type)
    {
        send(Encoding.ASCII.GetBytes(data), type);
    }
}