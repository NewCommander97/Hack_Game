using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Server_CommandPrompt : CommandPrompt
    {
        public AlarmControl AlarmControl { get; set; }

        private List<File> files = new List<File>();

        public List<File> Files
        {
            get { return files; }
            set { files = value; }
        }

        public override void Open()
        {
            Console.Title = "Command prompt - " + ClientName;
            Console.Clear();
            bool end = false;
            while (!end)
            {
                Console.Write(ClientName + "> ");
                string command = Console.ReadLine().ToLower();
                switch (command.Split(' ')[0])
                {
                    case "logout":
                        end = true;
                        break;
                    case "whois":
                        if (command.Split(' ').Length == 1)
                            Console.WriteLine("Command incomplete! How to use: whois [name]");
                        else WhoIs(command.Split(' ')[1]);
                        break;
                    case "whoami":
                        WhoAmI();
                        break;
                    case "telnet":
                        if (command.Split(' ').Length == 1)
                            Console.WriteLine("Command incomplete! How to use: telnet [ip address]");
                        else
                        {
                            Console.Clear();
                            Telnet(command.Split(' ')[1]);
                            Console.Title = "Command prompt - " + ClientName;
                            Console.Clear();
                        }
                        break;
                    case "netscan":
                        Netscan();
                        break;
                    case "alarmsystem":
                        if (AlarmControl != null)
                            AlarmControl.Login();
                        else Console.WriteLine("Alarm system not initialized!");
                        break;
                    case "files":
                        ListFiles();
                        break;
                    case "file":
                        if (command.Split(' ').Length == 1)
                            Console.WriteLine("Command incomplete! How to use: file [id]");
                        else if (command.Split(' ').Length == 2)
                        {
                            int id = 0;
                            if (int.TryParse(command.Split(' ')[1], out id))
                                OpenFile(id);
                            else Console.WriteLine("Parameter incompatible! Use file [id]");
                        }
                        break;
                    case "crypt":
                        if (command.Split(' ').Length == 1)
                            Console.WriteLine("Command incomplete! How to use: crypt [id]");
                        else if (command.Split(' ').Length == 2)
                        {
                            int id = 0;
                            if (int.TryParse(command.Split(' ')[1], out id))
                                CryptFile(id);
                            else Console.WriteLine("Parameter incompatible! Use crypt [id]");
                        }
                        break;
                    case "help":
                        Help();
                        break;
                    case "":
                        break;
                    default:
                        Console.WriteLine("Unknown command \"" + command + "\". Type help for more information.");
                        break;
                }
            }
        }

        public override void Help()
        {
            ConsoleTable ct = new ConsoleTable();
            ct.Columns.AddRange(new string[] { "Command", "Description" });
            ct.Rows.Add(new string[] { "whois [name]", "Shows available information about a client" });
            ct.Rows.Add(new string[] { "whoami", "Show available information about this client" });
            ct.Rows.Add(new string[] { "telnet [ip]", "Connect to another client" });
            ct.Rows.Add(new string[] { "netscan", "Show all devices in current network" });
            ct.Rows.Add(new string[] { "alarmsystem", "Open the administration tool for the alarm server" });
            ct.Rows.Add(new string[] { "files", "List all files on the fileserver" });
            ct.Rows.Add(new string[] { "file [id]", "Open a file on the fileserver" });
            ct.Rows.Add(new string[] { "crypt [id]", "Encrypt/Decrypt a file on the fileserver" });
            ct.Rows.Add(new string[] { "logout", "Logout from client" });
            ct.Rows.Add(new string[] { "help", "Open this help window" });
            ct.DrawToConsole("Command Prompt Help", true);
        }

        public void ListFiles()
        {
            if (Files.Count == 0)
                Console.WriteLine("No files available!");
            else
            {
                ConsoleTable ct = new ConsoleTable();
                ct.Columns.AddRange(new string[] { "ID", "Filename", "Protected" });
                int counter = 1;
                foreach (File f in Files)
                {
                    ct.Rows.Add(new string[] { counter.ToString(), f.Filename, f.PasswordProtected.ToString() });
                    counter++;
                }
                ct.DrawToConsole("Available files", true);
            }
        }

        public void CryptFile(int id)
        {
            if (files[id - 1].PasswordProtected)
            {
                Console.WriteLine("This file is password protected! Please enter the password!");
                Console.Write("Password: ");
                string password = Console.ReadLine();
                files[id - 1].CryptFile(password);
            }
            else
            {
                Console.WriteLine("This file is not password protected! Please enter a new password!");
                Console.Write("Password: ");
                string password = Console.ReadLine();
                files[id - 1].CryptFile(password);
            }
        }

        public void OpenFile(int id)
        {
            int cWidth = Console.WindowWidth;
            for (int i = 0; i < cWidth; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("Filename: " + files[id - 1].Filename);
            for (int i = 0; i < cWidth; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine(Encoding.ASCII.GetString(files[id - 1].Content));
            for (int i = 0; i < cWidth; i++)
            {
                Console.Write("-");
            }
        }
    }
}
