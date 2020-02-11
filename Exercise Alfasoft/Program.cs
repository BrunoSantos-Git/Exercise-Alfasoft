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
            List<string> requests = new List<string>();
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

            foreach (var user in users)
            {
                string url = "https://api.bitbucket.org/2.0/users/" + user;
                var request = WebRequest.Create(url) as HttpWebRequest;

                Console.WriteLine("===============================================================================================");
                Console.WriteLine("User being retrieved: {0}", user);
                Console.WriteLine("URL: {0}", url);

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    string json = reader.ReadToEnd();
                    requests.Add(json);
                    Console.WriteLine("Output: {0}\n", json);
                }
                Console.WriteLine("===============================================================================================");

                if (user != users.Last()) // If is not the last user in list
                    Thread.Sleep(5000);   // Wait 5 seconds
            }

            AddToLogFile(requests);

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

        /// <summary>
        /// Create log file with requests made
        /// </summary>
        /// <param name="requests"></param>
        private static void AddToLogFile(List<string> requests)
        {
            string currPath = Directory.GetCurrentDirectory();
            string logFilePath = Path.Combine(currPath, "log.txt");

            using (StreamWriter w = File.AppendText(logFilePath))
            {
                w.WriteLine("[Log Entry: " + DateTime.Now + "]");
                foreach (var request in requests)
                {
                    w.WriteLine("Request: " + request);
                }
                w.WriteLine("===============================================================================================\n");
            }
        }
    }
}