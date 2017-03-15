using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.IO;
using Sodium;

public class ChatClient
{
    private NetworkStream NetStream;
    public TcpClient client;
    public BackgroundWorker ListenWorker = new BackgroundWorker();
    public Action<string,string> onMsg;
    public string N = "";
    private KeyPair KP;

    public ChatClient(int port, Action<string,string> onMessage, string name)
    {
        KP = Encryptor.GenerateKeyPair();
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
        Encryptor.Encrypt(data, KP);
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
        try
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
            data = Encryptor.Decrypt(data, KP);
            onMsg(Encoding.ASCII.GetString(data), dataType);
        }
        catch
        {

        }
    }
    public void sendString(string data, string type)
    {
        send(Encoding.ASCII.GetBytes(data), type);
    }
}
public class Encryptor
{
    public static KeyPair GenerateKeyPair()
    {
        return PublicKeyBox.GenerateKeyPair();
    }

    public static byte[] Encrypt(byte[] message, byte[] yourPrivateKey, byte[] theirPublicKey)
    {
        var nonce = PublicKeyBox.GenerateNonce();
        var cipher = PublicKeyBox.Create(message, nonce, yourPrivateKey, theirPublicKey);
        var output = new byte[nonce.Length + cipher.Length];
        nonce.CopyTo(output, 0);
        cipher.CopyTo(output, cipher.Length);
        return output;
    }

    public static byte[] Decrypt(byte[] cipherText, byte[] yourPrivateKey, byte[] theirPublicKey)
    {
        var nonce = new byte[24];
        var cipher = new byte[cipherText.Length - 24];
        Array.Copy(cipherText, nonce, 24);
        Array.Copy(cipherText, 24, cipher, 0, cipherText.Length - 24);
        return PublicKeyBox.Open(cipher, nonce, yourPrivateKey, theirPublicKey);
    }
}