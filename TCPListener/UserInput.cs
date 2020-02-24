using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPListener
{
    public class UserInput
    {
        public static ConsoleKey getKey(bool showkey)
        {
            ConsoleKey key = Console.ReadKey(showkey).Key;
            return key;
        }

        public static string getInput()
        {
            string input;
            input = Console.ReadLine();
            return input;
        }

        public static IPAddress getIPAddress()
        {
            IPAddress ip = null;
            bool valid = false;
            do
            {
                try
                {
                    string ipstring = Console.ReadLine();

                    ip = IPAddress.Parse(ipstring);
                }
                catch (Exception)
                {
                    Console.WriteLine("not a valid address");
                    continue;
                }
                valid = true;
            } while(!valid);
            return ip;
        }
    }
}
