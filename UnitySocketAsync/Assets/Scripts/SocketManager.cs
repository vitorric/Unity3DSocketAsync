using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace SocketAsync
{
    public class SocketManager
    {
        private Socket socket;
        private byte[] sendbuffer = new byte[0];
        private byte[] receivedBuffer = new byte[65536];
        private double doubleReceivedBuffer = 0;

        private Action callback;
        private Action callbackCancel;
        private Action<float> progress;

        /// <summary>
        /// Create Instance of SocketManager
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="callbackEnd"></param>
        /// <param name="callbackCancel"></param>
        public SocketManager(byte[] buffer, Action callbackEnd, Action callbackCancel, Action<float> progress = null)
        {
            socket = new Socket(
                       AddressFamily.InterNetwork,
                       SocketType.Stream,
                       ProtocolType.Tcp)
            {
                NoDelay = true
            };

            if (progress != null)
                this.progress = progress;

            this.callbackCancel = callbackCancel;

            callback = callbackEnd;
            sendbuffer = buffer;

            sendDataAsync();
        }

        public void sendDataAsync()
        {
            IPAddress ip = IPAddress.Parse(SocketConfig.IP);
            IPEndPoint end = new IPEndPoint(ip, SocketConfig.Port);

            socket.Connect(end);

            sendData();

            //progress(sendbuffer);

            socket.BeginReceive(receivedBuffer,
                0,
                receivedBuffer.Length,
                SocketFlags.None,
                new AsyncCallback(receiveCallback), null);
            return;
        }

        private void sendData()
        {
            SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
            socketAsyncData.SetBuffer(sendbuffer, 0, sendbuffer.Length);
            socket.SendAsync(socketAsyncData);
        }

        private void receiveCallback(IAsyncResult ar)
        {
            try
            {
                int received = socket.EndReceive(ar);

                byte[] recData = new byte[received];
                Array.Copy(receivedBuffer, 0, recData, 0, received);

                //the serve can response more than 1 one per time, because of this, the use SPLIT
                //Ex.: 1|2|3
                //for this code, we need the last position
                string[] responseServer = Encoding.UTF8.GetString(recData).Split('|');

                //-2 is used because the last position is always empty, and Length is not 1 and is not 0
                //-1 because of last empty position, -1 because of length starting at 1 = -2
                doubleReceivedBuffer = Convert.ToDouble(responseServer[responseServer.Length - 2]);

                if (progress != null)
                {
                    //if (PreloaderEnvioAula.Instance.CancelarEnvio)
                    //{
                    //    callbackCancel();
                    //    socket.Close();
                    //    return;
                    //}

                    progress((float) doubleReceivedBuffer);
                }

                //Checksum
                if (doubleReceivedBuffer == sendbuffer.Length)
                {
                    socket.Close();
                    callback();
                    return;
                }

                socket.BeginReceive(receivedBuffer, 0, receivedBuffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), null);
            }
            catch (Exception e)
            {
                callbackCancel();
                Debug.Log(e.Message);
                socket.Close();
            }
        }
    }

}