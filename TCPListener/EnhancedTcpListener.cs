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
        Thread worker;

        TcpListener listener;

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
            worker = new Thread(new ThreadStart(Worker));
            worker.Start();
        }

        public void StopListening()
        {
            while(this.worker.IsAlive)
            {
                try
                {
                    this.worker.Abort();
                }
                catch (ThreadAbortException)
                {
                }
            }
            Console.WriteLine("Stopped");
        }

        private void Worker()
        {
            Console.WriteLine("Worker started.");
            string data;
            int i;
            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                while((i= stream.Read(buffer,0,buffer.Length))!= 0)
                {
                    data = Encoding.UTF8.GetString(buffer, 0, i);
                    this.FireMessageReceivedEvent(data);

                    byte[] msg = Encoding.UTF8.GetBytes("Test");
                    stream.Write(msg, 0, msg.Length);
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