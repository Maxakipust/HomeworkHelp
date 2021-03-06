﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.ComponentModel;
using System.IO;
using Sodium;

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
        public Action<string, string, int> _onReceiveData;
        public Action<TcpSocketServer> onLeave;
        public bool on = true;
        public Func<TcpSocketServer,int> getIndex;
        private KeyPair KP;

        public TcpSocketServer(TcpClient Socket, Action<string,string,int> onReceiveData,Action<TcpSocketServer> Leave, Func<TcpSocketServer,int> getindx)
        {
            KP = Encryptor.GenerateKeyPair();
            getIndex = getindx;
            onLeave = Leave;
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
            if (on)
            ListenWorker.RunWorkerAsync();
        }

        public void ListenWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int bytesRead = 0;
            int bufferSize = 1024;
            int allBytesRead = 0;

            byte[] length = new byte[4];
            byte[] typeLengthArr = new byte[4];
            try
            {
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
                _onReceiveData(Encoding.ASCII.GetString(data), dataType,getIndex(this));
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                onLeave(this);
            }
        }

        public void send(byte[] data,string dataType)
        {
            data = Encryptor.Encrypt(data, KP);
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
    public class Encryptor
    {
        public static KeyPair GenerateKeyPair()
        {
            return PublicKeyBox.GenerateKeyPair();
        }

        public static byte[] Encrypt(byte[] message, KeyPair keypair)
        {
            var nonce = PublicKeyBox.GenerateNonce();
            var cipher = PublicKeyBox.Create(message, nonce, keypair.PrivateKey, keypair.PublicKey);
            var output = new byte[nonce.Length + cipher.Length];
            nonce.CopyTo(output, 0);
            cipher.CopyTo(output, cipher.Length);
            return output;
        }

        public static byte[] Decrypt(byte[] cipherText, KeyPair keypair)
        {
            var nonce = new byte[24];
            var cipher = new byte[cipherText.Length - 24];
            Array.Copy(cipherText, nonce, 24);
            Array.Copy(cipherText, 24, cipher, 0, cipherText.Length - 24);
            return PublicKeyBox.Open(cipher, nonce, keypair.PrivateKey, keypair.PublicKey);
        }
    }
}