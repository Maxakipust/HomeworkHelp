using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.ComponentModel;

namespace HomeworkHelpClient
{
    class Sucure_Socket
    {
        TcpClient client;
        NetworkStream netStream;
        BackgroundWorker ListenWorker;
        Action<byte[], string> _dataRecived;

        public Sucure_Socket(int port, Action<byte[],string> dataReceived)
        {
            _dataRecived = dataReceived;
            string ip = File.ReadAllText("IP.txt");
            if (ip == "Local")
            {
                ip = Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString();
            }
            Console.WriteLine("Listening on: " + ip);
            client = new TcpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            /*while (!client.Connected)
            {
                try
                {
                    client.Connect(ep);
                }
                catch (Exception ex)
                {

                }
            }
            netStream = client.GetStream();

            ListenWorker.DoWork += ListenWorker_DoWork;
            ListenWorker.RunWorkerCompleted += ListenWorker_RunWorkerCompleted;
            */
        }

        private void ListenWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ListenWorker.RunWorkerAsync();
        }

        private void ListenWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            listen();
        }

        public string listen()
        {
            try
            {
                int bytesRead = 0;
                int bufferSize = 1024;
                int allBytesRead = 0;

                byte[] length = new byte[4];
                byte[] typeLengthArr = new byte[4];

                bytesRead = netStream.Read(length, 0, 4);//length
                int dataLength = BitConverter.ToInt32(length, 0);
                

                bytesRead += netStream.Read(typeLengthArr, 0, 4);//typeLength
                int typeLength = BitConverter.ToInt32(typeLengthArr, 0);
                
                byte[] type = new byte[typeLength];
                bytesRead += netStream.Read(type, 0, typeLength);//type
                string dataType = Encoding.ASCII.GetString(type);

                int bytesLeft = dataLength;
                byte[] data = new byte[dataLength];

                while (bytesLeft > 0)//package
                {
                    int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;
                    bytesRead = netStream.Read(data, allBytesRead, nextPacketSize);
                    allBytesRead += bytesRead;
                    bytesLeft -= bytesRead;
                }
                _dataRecived(data, dataType);
            }
            catch (Exception ex)
            {

            }
            return "";
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

                netStream.Write(package, bytesSent, packetSize);
                bytesSent += packetSize;
                bytesLeft -= packetSize;
            }
        }

        public void sendString(string data)
        {
            send(Encoding.ASCII.GetBytes(data), "text");
        }
    }
}
