using System;
using System.IO;
using System.Linq;
using System.Net;
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

                if (!ValidPath(path))
                {
                    Console.WriteLine("\nInvalid File!\n");
                    canContinue = false;
                }
                else
                    canContinue = true;

            } while (!canContinue);

            // Read all lines of file
            string[] users = File.ReadAllLines(path);

            foreach (var user in users)
            {
                string url = "https://api.bitbucket.org/2.0/users/" + user;
                var request = WebRequest.Create(url) as HttpWebRequest;

                Console.WriteLine("===============================================================================================");
                Console.WriteLine("User being retrieved: " + user);
                Console.WriteLine("URL: " + url);

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    string json = reader.ReadToEnd();
                    Console.WriteLine("Output Request: \n" + json);
                }
                Console.WriteLine("===============================================================================================");

                if (user != users.Last()) // If is not the last user in list
                    Thread.Sleep(5000);   // Wait 5 seconds
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true); // Do not display the pressed key
            Console.WriteLine("Application will close in 5 seconds");
            Thread.Sleep(5000);
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
