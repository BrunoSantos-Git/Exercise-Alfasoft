using System;
using System.Collections.Generic;
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
            var lastRequest = Properties.Settings.Default.RunRequest; // Get last time app request made
            var now = DateTime.Now; // Get current date with time
            if (lastRequest == null || (now - lastRequest).TotalSeconds >= 60)
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
                        Console.WriteLine("Output: \n{0}", json);
                    }
                    Console.WriteLine("===============================================================================================");

                    if (user != users.Last()) // If is not the last user in list
                        Thread.Sleep(5000);   // Wait 5 seconds
                }

                AddToLogFile(requests);

                // Save last app run
                Properties.Settings.Default.RunRequest = DateTime.Now;
                Properties.Settings.Default.Save();
            }
            else
            {
                Console.WriteLine("The application was run in less than 60 seconds ago");
            }

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
                w.WriteLine("\n===============================================================================================\n");
            }
        }
    }
}