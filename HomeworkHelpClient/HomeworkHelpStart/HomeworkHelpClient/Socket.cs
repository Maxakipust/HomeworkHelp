using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace HomeworkHelpClient
{
    class Socket
    {
        public TcpClient Client;
        private NetworkStream NetStream;
        //private BackgroundWorker ListenWorker;

        public Socket(string ip, int port)
        {
            Client = new TcpClient();
            Client.Connect(ip, port);
            NetStream = Client.GetStream();
            //ListenWorker = new BackgroundWorker();
            //ListenWorker.DoWork += ListenWorker_DoWork;
            //ListenWorker.RunWorkerCompleted += ListenWorker_RunWorkerCompleted;
        }

        public bool Send(byte[] data, string dataType)
        {
            try
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
                return true;
            }
            catch(Exception ex)
            {
                File.AppendText("Logs.txt").Write(ex.ToString()+Environment.NewLine);
                return false;
            }
        }

        public int Listen(ref byte[] data, ref string dataType)
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
            dataType = Encoding.ASCII.GetString(type);

            int bytesLeft = dataLength;
            data = new byte[dataLength];

            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;
                bytesRead = NetStream.Read(data, allBytesRead, nextPacketSize);
                allBytesRead += bytesRead;
                bytesLeft -= bytesRead;
            }
            return allBytesRead;
        }
    }
}
