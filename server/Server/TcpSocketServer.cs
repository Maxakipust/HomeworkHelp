using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.IO;

namespace Server
{
    class TcpSocketServer
    {
        public int id = 0;
        public TcpClient client;
        private NetworkStream NetStream;
        private BackgroundWorker ListenWorker;
        public string Data = "";
        public string savedFile = "";
        public string savedDir = "";
        public bool screencast = false;
        public bool Control = false;
        public Action<string, string> _onReceiveData;

        public TcpSocketServer(TcpClient Socket, Action<string,string> onReceiveData)
        {
            client = Socket;
            NetStream = client.GetStream();
            ListenWorker = new BackgroundWorker();
            ListenWorker.DoWork += ListenWorker_DoWork;
            ListenWorker.RunWorkerCompleted += ListenWorker_RunWorkerCompleted;
            ListenWorker.RunWorkerAsync();
            _onReceiveData = onReceiveData;
        }

        public bool connected()
        {
            if (client.Connected)
            {
                return !(client.Client.Poll(1000, SelectMode.SelectRead) && client.Client.Available == 0);
            }
            return false;
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
            _onReceiveData(Encoding.ASCII.GetString(data), dataType);
        }

        public void send(byte[] data,string dataType)
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
    }
}