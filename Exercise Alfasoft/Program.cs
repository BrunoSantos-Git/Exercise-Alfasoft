using System;
using System.IO;

namespace Exercise_Alfasoft
{
    class Program
    {
        static void Main(string[] args)
        {
            bool canContinue;
            string path = "";
            do
            {
                Console.WriteLine("Input users file path:");
                path = Console.ReadLine();

                if (!ValidPath(path))
                {
                    Console.WriteLine("\nInvalid File!\n");
                    canContinue = false;
                }
                else
                    canContinue = true;

            } while (!canContinue);

            string[] users = File.ReadAllLines(path);
            Console.WriteLine("Users in file:");
            foreach (var user in users)
            {
                Console.WriteLine(user);
            }

            Console.ReadLine();
        }

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
