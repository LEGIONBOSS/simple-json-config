using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ConfigExample
{
    internal class Program
    {
        private static string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test_config.json");

        static void Main(string[] args)
        {
            // Start with a clean slate
            Clean();

            // Set the file path
            Config.Config.FilePath = FilePath;

            // Load the file
            Config.Config.Load();

            // Load / create some values
            string name = Config.Config.GetValue("name", "John Doe");
            Config.Config.SetValue("name", name);
            
            IPAddress ip = IPAddress.Parse(Config.Config.GetValue("ip_address", "127.0.0.1"));
            Config.Config.SetValue("ip_address", ip.ToString());
            
            int port = int.Parse(Config.Config.GetValue("port", "1234"));
            Config.Config.SetValue("port", port.ToString());

            // List the current values
            List();

            // Save the values
            Config.Config.Save();
        }

        private static void Clean()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

        private static void List()
        {
            Console.WriteLine("Current Values:");
            foreach (KeyValuePair<string, string> item in Config.Config.GetAllValues())
            {
                Console.WriteLine($"\t{item.Key} = {item.Value}");
            }
            Console.WriteLine();
        }
    }
}
