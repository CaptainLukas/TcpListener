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
            control.handleFlow();
        }
    }

    class Control
    {
        private EnhancedTcpListener listener;

        public void connectto()
        {
            EnhancedTcpListener listener = new EnhancedTcpListener(45688);
            listener.ConnectTo();
        }

        public void getconnected()
        {
            EnhancedTcpListener listener = new EnhancedTcpListener(45688);
            listener.StartListening();
            listener.MessageReceived += ReceivedMessageHandler;
            ConsoleKey key;
            do
            {
                key = UserInput.getKey();
                if (key == ConsoleKey.M)
                {
                    Console.WriteLine("Please Type Message");
                    listener.sendMessage(Console.ReadLine());
                }
            } while (key != ConsoleKey.Escape);

            listener.StopListening();
            Console.ReadKey();
        }

        public void handleFlow()
        {
            ConsoleKey key;
            do
            {
                key = UserInput.getKey();
                switch (key)
                {
                    case ConsoleKey.C:
                        //connectTo
                        listener.ConnectTo(UserInput.getIPAddress());
                        break;

                    case ConsoleKey.D:
                        //disconnect
                        listener.Disconnect();
                        break;

                    case ConsoleKey.G:
                        //get connected

                        break;

                    case ConsoleKey.R:
                        //read message
                        break;

                    case ConsoleKey.S:
                        //send message
                        Console.WriteLine("Please enter message to send: ");
                        listener.sendMessage(UserInput.getInput());
                        break;

                    default:
                        Console.WriteLine("no action for this input");
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        public void ReceivedMessageHandler(object sender, MessageReceivedEventHandler e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
