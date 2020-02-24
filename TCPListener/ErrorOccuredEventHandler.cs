using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    public class ErrorOccuredEventHandler:EventArgs
    {
        public string Error
        {
            get;
            private set;
        }

        public ErrorOccuredEventHandler(string error)
        {
            this.Error = error;
        }
    }
}
