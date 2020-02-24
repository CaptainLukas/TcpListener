using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPListener
{
    class EnhancedStaticNetwork
    {
        public event EventHandler<MessageReceivedEventHandler> MessageReceived;

        public event EventHandler<ErrorOccuredEventHandler> ErrorOccured;

        public event EventHandler<LogMessageEventHandler> LogMessage;

        private int port;

        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;

        public bool Connected
        {
            get;
            private set;
        }

        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                if (value <= 0 || value > 65535)
                {
                    throw new ArgumentOutOfRangeException("port is out of range");
                }
                else
                {
                    this.port = value;
                }
            }
        }

        public EnhancedStaticNetwork()
        {
            this.Connected = false;
            this.port = 45688;
        }

        public void ConnectTo(IPAddress ip)
        {
            if (this.Connected)
            {
                this.FireErrorOccuredEvent("Already connected");
                return;
            }

            try
            {
                this.FireLogMessageEvent("Connecting");
                this.client = new TcpClient();
                this.client.Connect(new IPEndPoint(ip, this.Port));
                this.stream = this.client.GetStream();
                this.FireLogMessageEvent("Connected");
                this.Connected = true;
            }
            catch(Exception)
            {
                this.FireErrorOccuredEvent("Connection failed");
                this.Connected = false;
            }
        }

        public void Disconnect()
        {
            if (!this.Connected)
            {
                this.FireErrorOccuredEvent("not connected");
                return;
            }
            try
            {
                this.stream.Close();
                this.FireLogMessageEvent("stream disconnected");
            }
            catch (Exception)
            {
                this.FireErrorOccuredEvent("stream closing error");
            }

            try
            {
                this.client.Close();
                this.FireLogMessageEvent("client disconnected");
            }
            catch(Exception)
            {
                this.FireErrorOccuredEvent("client closing error");
            }
            try
            {
                this.listener.Stop();
                this.FireLogMessageEvent("listener stopped");
            }
            catch(Exception)
            {
                this.FireErrorOccuredEvent("listener stopping error");
            }
            this.Connected = false;
        }
        public void SilentDisconnect()
        {
            if (!this.Connected)
            {
                return;
            }
            try
            {
                this.stream.Close();
            }
            catch (Exception)
            {
                this.FireErrorOccuredEvent("stream closing error");
            }

            try
            {
                this.client.Close();
            }
            catch (Exception)
            {
                this.FireErrorOccuredEvent("client closing error");
            }
            try
            {
                this.listener.Stop();
            }
            catch (Exception)
            {
                this.FireErrorOccuredEvent("listener stopping error");
            }
            this.Connected = false;
        }

        public void waitForConnection()
        {
            if (this.Connected)
            {
                this.FireErrorOccuredEvent("Already connected");
                return;
            }
            this.FireLogMessageEvent("Waiting for connection...");
            this.listener = new TcpListener(IPAddress.Any,this.Port);
            this.listener.Start(1);
            this.client = listener.AcceptTcpClient();
            this.stream = this.client.GetStream();
            this.FireLogMessageEvent("Connected");
            this.Connected = true;
        }

        public void sendMessage(string message)
        {
            if (!this.Connected)
            {
                this.FireErrorOccuredEvent("not connected");
                return;
            }
            try
            {
                message = ("CON4" + (Encoding.UTF8.GetBytes(message)).Length + "E" + message);
                byte[] msg = Encoding.UTF8.GetBytes(message);
                this.stream.Write(msg, 0, msg.Length);
            }
            catch (Exception)
            {
            }
        }

        public void receiveMessage()
        {
            if (!this.Connected)
            {
                this.FireErrorOccuredEvent("not connected");
                return;
            }
            this.FireLogMessageEvent("Waiting for incoming message");
            string message;
            byte[] buffer=new byte[4];
            this.stream.Read(buffer, 0, buffer.Length);
            message = Encoding.UTF8.GetString(buffer);

            buffer = new byte[1];
            string number = "";
            string temp = "";
            do
            {
                this.stream.Read(buffer, 0, buffer.Length);
                temp = Encoding.UTF8.GetString(buffer);
                if (temp == "E")
                    break;

                number += temp;

            } while (number != "E");
            
            message += number + "E";

            int num = Convert.ToInt32(number);
            buffer = new byte[num];

            this.stream.Read(buffer, 0, buffer.Length);

            message += Encoding.UTF8.GetString(buffer);

            this.FireMessageReceivedEvent(message);
        }

        protected void FireMessageReceivedEvent(string message)
        {
            if (this.MessageReceived != null)
            {
                this.MessageReceived(this, new MessageReceivedEventHandler(message));
            }
        }

        protected void FireErrorOccuredEvent(string error)
        {
            if (this.ErrorOccured != null)
            {
                this.ErrorOccured(this, new ErrorOccuredEventHandler(error));
            }
        }

        protected void FireLogMessageEvent(string message)
        {
            if (this.LogMessage != null)
            {
                this.LogMessage(this, new LogMessageEventHandler(message));
            }
        }
    }
}