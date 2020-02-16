using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    class TcpListener
    {
        public event EventHandler<MessageReceivedEventHandler> MessageReceived;

        private int port;


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

        public TcpListener(int port)
        {
            this.Port = port;
        }

        public void StartListening()
        {

        }

        public void StopListening()
        {

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