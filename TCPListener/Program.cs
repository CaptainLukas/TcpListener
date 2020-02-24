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
        private UserInterface ui;

        public Control()
        {
            this.listener = new EnhancedStaticNetwork();
            this.listener.MessageReceived += this.ReceivedMessageHandler;
            this.ui = new UserInterface();
        }

        public void handleFlow()
        {
            ConsoleKey key;
            do
            {
                this.ui.PrintLine("Please enter key to choose following options:");
                this.ui.PrintLine("\"c\": connect to other\n\"d\": disconnect current connection\n\"g\": wait for connection\n\"r\": receive a message\n\"s\": send a message");
                key = UserInput.getKey(true);
                switch (key)
                {
                    case ConsoleKey.C:
                        //connectTo
                        this.ui.Print("Enter ip address:");
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
                        this.ui.Print("Please enter message to send: ");
                        listener.sendMessage(UserInput.getInput());
                        break;
                    case ConsoleKey.Escape:
                        listener.Disconnect();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        this.ui.PrintLine("no action for this input");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        public void LogMessage(object sender, LogMessageEventHandler e)
        {
            this.ui.PrintLine(e.Message);
        }

        public void ErrorOccuredHandler(object sender, ErrorOccuredEventHandler e)
        {
            this.ui.PrintError(e.Error);
        }

        public void ReceivedMessageHandler(object sender, MessageReceivedEventHandler e)
        {
            this.ui.PrintLine(e.Message,ConsoleColor.Cyan);
        }
    }
}
