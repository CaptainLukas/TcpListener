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
    class EnhancedTcpListener
    {
        public event EventHandler<MessageReceivedEventHandler> MessageReceived;

        private int port;

        private Byte[] buffer;
        private Thread worker;

        private TcpListener listener;
        private TcpClient client;
        private NetworkStream stream;

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

        public EnhancedTcpListener(int port)
        {
            this.Port = port;
            this.buffer = new Byte[256];
        }

        public void StartListening()
        {
            Console.WriteLine("Starting");
            this.listener = new TcpListener(IPAddress.Any,this.port);
            this.listener.Start();
            this.client = listener.AcceptTcpClient();
            this.stream = this.client.GetStream();
            worker = new Thread(new ThreadStart(Worker));
            worker.Start();
        }

        public void StopListening()
        {
            if (this.client != null)
            {
                this.client.Close();
            }

            if (this.listener != null)
            {
                this.listener.Stop();
            }

            if (this.worker.IsAlive)
            {
                
                try
                {
                    this.worker.Abort();
                }
                catch (ThreadAbortException)
                {
                    Console.WriteLine("Aborted");
                }
            }
            Console.WriteLine("Stopped");
        }

        public void sendMessage(string message)
        {
            message = ("CON4" + Encoding.UTF8.GetBytes(message).Length+"E" + message);
            byte[] msg = Encoding.UTF8.GetBytes(message);
            this.stream.Write(msg, 0, msg.Length);
        }

        private void Worker()
        {
            Console.WriteLine("Worker started.");
            string data;
            int i;
            while(true)
            {
                Console.WriteLine("Connected");
                try
                {
                    while ((i = this.stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        data = Encoding.UTF8.GetString(buffer, 0, i);
                        this.FireMessageReceivedEvent(data);
                    }
                }
                catch(Exception)
                {
                    Console.WriteLine("Connection closed");
                    break;
                }
            }
        }

        protected void FireMessageReceivedEvent(string message)
        {
            if (MessageReceived != null)
            {
                this.MessageReceived.Invoke(this, new MessageReceivedEventHandler(message));
            }
        }
    }
}