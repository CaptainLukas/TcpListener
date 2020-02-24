using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    public class LogMessageEventHandler
    {
        public string Message
        {
            get;
            private set;
        }
        public LogMessageEventHandler(string message)
        {
            this.Message = message;
        }
    }
}
