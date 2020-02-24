using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    public class UserInterface
    {
        private ConsoleColor ForeGround
        {
            get;
            set;
        }

        private ConsoleColor BackGround
        {
            get;
            set;
        }

        public UserInterface()
        {
            this.ForeGround = ConsoleColor.Gray;
            this.BackGround = ConsoleColor.Black;
            Console.ForegroundColor = this.ForeGround;
            Console.BackgroundColor = this.BackGround;
        }

        public void PrintLine(string message)
        {
            Console.WriteLine(message);
        }
        public void PrintLine(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = this.ForeGround;
        }

        public void PrintLine(string message, ConsoleColor foregroundcolor, ConsoleColor backgroundcolor)
        {
            Console.ForegroundColor = foregroundcolor;
            Console.BackgroundColor = backgroundcolor;
            Console.WriteLine(message);
            Console.ForegroundColor = this.ForeGround;
            Console.BackgroundColor = this.BackGround;
        }

        public void Print(string message)
        {
            Console.Write(message);
        }
        public void Print(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = this.ForeGround;
        }
        public void Print(string message, ConsoleColor foregroundcolor, ConsoleColor backgroundcolor)
        {
            Console.ForegroundColor = foregroundcolor;
            Console.BackgroundColor = backgroundcolor;
            Console.Write(message);
            Console.ForegroundColor = this.ForeGround;
            Console.BackgroundColor = this.BackGround;
        }

        public void PrintError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("Error occured:");
            Console.BackgroundColor = this.BackGround;
            Console.Write(" ");
            Console.WriteLine(error);
            Console.ForegroundColor = this.ForeGround;
        }
    }
}
