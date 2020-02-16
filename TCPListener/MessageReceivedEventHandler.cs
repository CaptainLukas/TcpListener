using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    public class MessageReceivedEventHandler:EventArgs
    {
        public string Message
        {
            get;
            private set;
        }

        public MessageReceivedEventHandler(string message)
        {
            this.Message = message;
        }
    }
}
