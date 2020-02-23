using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private EnhancedStaticNetwork listener;

        public Control()
        {
            this.listener = new EnhancedStaticNetwork();
            this.listener.MessageReceived += this.ReceivedMessageHandler;
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
                        listener.waitForConnection();
                        break;

                    case ConsoleKey.R:
                        //read message
                        listener.receiveMessage();
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
