using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.IO.Compression;
using System.Threading;

namespace tcp_Chat
{
    class TCPServer
    {
        public int id;
        private NetworkStream NetStream;
        public TcpClient client;
        public BackgroundWorker ListenWorker = new BackgroundWorker();
        public Action<string,int> onMsg;

        public TCPServer(TcpClient s,int ID, Action<string,int> onMessage)
        {
            Console.WriteLine("Connected to " + s.Client.RemoteEndPoint.ToString());
            id = ID;
            client = s;
            NetStream = client.GetStream();
            onMsg = onMessage;
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
                if (dataType == "text")//text
                {
                    onMsg(Encoding.ASCII.GetString(data), id);
                }else if(dataType == "name")//name
                {
                    onMsg(Encoding.ASCII.GetString(data) + " has connected", id);
                }
            }
            catch
            {

            }
        }
        public bool connected()
        {
            if (client.Connected)
            {
                return !(client.Client.Poll(1000, SelectMode.SelectRead) && client.Client.Available == 0);
            }
            return false;
        }
    }
}
