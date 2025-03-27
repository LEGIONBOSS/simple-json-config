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

            // Load the values
            Config.Config.Load();

            // Set some values
            string name = "John Doe";
            IPAddress ip = new IPAddress(new byte[] { 127, 0, 0, 1 });
            int port = 1234;
            Config.Config.SetValue("name", name);
            Config.Config.SetValue("ip_address", ip.ToString());
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
