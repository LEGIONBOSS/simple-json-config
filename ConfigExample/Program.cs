using ConfigLib;
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
            Config.FilePath = FilePath;

            // Load the file
            Config.Load();

            // Load / create some values
            string name = Config.GetValue("name", "John Doe");
            Config.SetValue("name", name);

            IPAddress ip = IPAddress.Parse(Config.GetValue("ip_address", "127.0.0.1"));
            Config.SetValue("ip_address", ip.ToString());

            int port = int.Parse(Config.GetValue("port", "1234"));
            Config.SetValue("port", port.ToString());

            // List the current values
            List();

            // Save the values
            Config.Save();
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
            foreach (KeyValuePair<string, string> item in Config.GetAllValues())
            {
                Console.WriteLine($"\t{item.Key} = {item.Value}");
            }
            Console.WriteLine();
        }
    }
}
