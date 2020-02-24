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
            Console.CursorVisible = false;
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
            newListener();
            this.ui = new UserInterface();
        }

        public void handleFlow()
        {
            ConsoleKey key;
            do
            {
                this.ui.PrintLine("Please enter key to choose following options:", ConsoleColor.DarkGray);
                this.ui.PrintLine("\"c\": connect to other\n\"d\": disconnect current connection\n\"g\": wait for connection\n\"p\": change port\n\"r\": receive a message\n\"s\": send a message\n\"Esc\": exit");
                key = UserInput.getKey(true);
                switch (key)
                {
                    case ConsoleKey.C:
                        //connectTo
                        IPAddress ip;
                        try
                        {
                            this.ui.Print("Enter ip address:");
                            ip = UserInput.getIPAddress();
                        }
                        catch(Exception)
                        {
                            this.ui.PrintError("invalid ip address");
                            break;
                        }
                        listener.ConnectTo(ip);
                        break;

                    case ConsoleKey.D:
                        //disconnect
                        listener.Disconnect();
                        break;

                    case ConsoleKey.G:
                        //get connected
                        listener.waitForConnection();
                        break;
                    case ConsoleKey.P:
                        //change port
                        this.ChangePort();
                        break;
                    case ConsoleKey.R:
                        //read message
                        listener.receiveMessage();
                        break;

                    case ConsoleKey.S:
                        //send message
                        if(!this.listener.Connected)
                        {
                            this.ui.PrintError("not connected");
                            break;
                        }
                        this.ui.Print("Please enter message to send: ");
                        listener.sendMessage(UserInput.getInput());
                        break;
                    case ConsoleKey.Escape:
                        listener.SilentDisconnect();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        this.ui.PrintLine("no action for this input");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }

            } while (key != ConsoleKey.Escape);
        }

        private void ChangePort()
        {
            this.ui.Print("the current port is ");
            this.ui.PrintLine(this.listener.Port.ToString(), ConsoleColor.White);
            this.ui.PrintLine("if you want to keep it, input \"k\"");

            int port;
            bool valid = false;
            do
            {
                this.ui.Print("please enter new port: ");
                string input = UserInput.getInput();
                if (input == "k")
                {
                    this.ui.PrintLine("port stays the same");
                    return;
                }
                valid = Int32.TryParse(input, out port);
                if (!valid)
                {
                    this.ui.PrintError("invalid input");
                }
            } while (!valid);

            try
            {
                this.listener.Port = port;
                this.ui.PrintLine("Port is set to: " + this.listener.Port);
            }
            catch(Exception)
            {
                this.ui.PrintError("Port out of range");
            }
        }

        private void newListener()
        {
            this.listener = new EnhancedStaticNetwork();
            this.listener.MessageReceived += this.ReceivedMessageHandler;
            this.listener.ErrorOccured += this.ErrorOccuredHandler;
            this.listener.LogMessage += this.LogMessageHandler;
        }

        private void LogMessageHandler(object sender, LogMessageEventHandler e)
        {
            this.ui.PrintLine(e.Message);
        }

        private void ErrorOccuredHandler(object sender, ErrorOccuredEventHandler e)
        {
            this.ui.PrintError(e.Error);
        }

        private void ReceivedMessageHandler(object sender, MessageReceivedEventHandler e)
        {
            this.ui.PrintLine(e.Message,ConsoleColor.Cyan);
        }
    }
}
