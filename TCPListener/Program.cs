using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Control control = new Control();
        }
    }

    class Control
    {
        public Control()
        {
            EnhancedTcpListener listener = new EnhancedTcpListener(45688);
            listener.StartListening();
            listener.MessageReceived += ReceivedMessageHandler;
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Escape);

            listener.StopListening();
            Console.ReadLine();
        }

        public void ReceivedMessageHandler(object sender, MessageReceivedEventHandler e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
