using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Control control = new Control();
            control.getconnected();
        }
    }

    class Control
    {
        public void connectto()
        {
            EnhancedTcpListener listener = new EnhancedTcpListener(45688);
            listener.ConnectTo();
            listener.sendMessage("HalloWelt");
            Thread.Sleep(500);
            listener.Disconnect();
        }

        public void getconnected()
        {
            EnhancedTcpListener listener = new EnhancedTcpListener(45688);
            listener.StartListening();
            listener.MessageReceived += ReceivedMessageHandler;
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.M)
                {
                    Console.WriteLine("Please Type Message");
                    listener.sendMessage(Console.ReadLine());
                }
            } while (key != ConsoleKey.Escape);

            listener.StopListening();
            Console.ReadKey();
        }

        public void ReceivedMessageHandler(object sender, MessageReceivedEventHandler e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
