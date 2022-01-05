using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCoin.Console.Helpers
{
    public static class ConsoleWriter
    {
        /// <summary>
        /// Outputs foreground as Yellow.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Response (string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Outputs foreground as Dark Magenta.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Info(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Outputs foreground as Red.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Important(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Outputs foreground as Gray.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Notification(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Outputs foreground as Cyan.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Update(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Outputs foreground as White.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public static void Normal(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(message);
        }
    }
}
