using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Exercise_Alfasoft
{
    class Program
    {
        static void Main(string[] args)
        {
            bool canContinue;
            string path;
            do
            {
                Console.WriteLine("Input users file path:");
                path = Console.ReadLine();

                canContinue = ValidPath(path);

                if (!canContinue)
                {
                    Console.WriteLine("\nInvalid File!\n");
                }

            } while (!canContinue);

            // Read all lines of file
            string[] users = File.ReadAllLines(path);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true); // Do not display the pressed key
            for (int i = 5; i != 0; i--)
            {
                Console.WriteLine("Application will close in {0} seconds", i);
                Thread.Sleep(1000);
            }

            Environment.Exit(0);
        }

        /// <summary>
        /// Check if a path is valid and is a text file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if path is valid and False if path is not valid</returns>
        private static bool ValidPath(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            else
            {
                if (Path.GetExtension(path) != ".txt")
                {
                    return false;
                }
            }

            return true;
        }
    }
}
