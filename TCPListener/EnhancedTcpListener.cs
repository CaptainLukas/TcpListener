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

        private int port = 45688;

        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;
        private bool connected;

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
                    throw new IndexOutOfRangeException("port is out of range");
                }
                this.port = value;
            }
        }

        public EnhancedStaticNetwork()
        {
            this.connected = false;
        }

        public void ConnectTo(IPAddress ip)
        {
            if (this.connected)
            {
                Console.WriteLine("Already connected");
                return;
            }

            try
            {
                this.client = new TcpClient();
                this.client.Connect(new IPEndPoint(ip, this.Port));
                this.stream = this.client.GetStream();
                Console.WriteLine("Connected");
                this.connected = true;
            }
            catch(Exception)
            {
                Console.WriteLine("Connection failed");
                this.connected = false;
            }
        }

        public void Disconnect()
        {
            if (!this.connected)
            {
                Console.WriteLine("not connected");
                return;
            }
            try
            {
                this.stream.Close();
                Console.WriteLine("stream disconnected");
            }
            catch (Exception)
            {
                Console.WriteLine("stream closing error");
            }

            try
            {
                this.client.Close();
                Console.WriteLine("client disconnected");
            }
            catch(Exception)
            {
                Console.WriteLine("client closing error");
            }
            try
            {
                this.listener.Stop();
                Console.WriteLine("listener stopped");
            }
            catch(Exception)
            {
                Console.WriteLine("listener stopping error");
            }
        }

        public void waitForConnection()
        {
            if (this.connected)
            {
                Console.WriteLine("Already connected");
                return;
            }

            this.listener = new TcpListener(IPAddress.Any,this.Port);
            this.listener.Start(1);
            this.client = listener.AcceptTcpClient();
            this.stream = this.client.GetStream();
            Console.WriteLine("Connected");
            this.connected = true;
        }

        public void sendMessage(string message)
        {
            if (!this.connected)
            {
                Console.WriteLine("not connected");
                return;
            }
            try
            {
                message = ("CON4" + Encoding.UTF8.GetBytes(message).Length + "E" + message);
                byte[] msg = Encoding.UTF8.GetBytes(message);
                this.stream.Write(msg, 0, msg.Length);
            }
            catch (Exception)
            {
            }
        }

        public void receiveMessage()
        {
            if (!this.connected)
            {
                Console.WriteLine("not connected");
                return;
            }

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

            Console.WriteLine("Number"+number);
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
    }
}