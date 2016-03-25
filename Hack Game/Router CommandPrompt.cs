using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Router_CommandPrompt : CommandPrompt
    {
        private List<Network> networks = new List<Network>();

        public List<Network> Networks
        {
            get { return networks; }
            set { networks = value; }
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
                    case "networks":
                        ListNetworks();
                        break;
                    case "netscan":
                        if (command.Split(' ').Length == 1)
                            Netscan();
                        else if (command.Split(' ').Length == 2)
                        {
                            if (new IP_Address().TryParse(command.Split(' ')[1]))
                                Netscan(new IP_Address().Parse(command.Split(' ')[1]));
                            else Console.WriteLine("Invalid IP-Address!");
                        }
                        else Console.WriteLine("Command incomplete! How to use: netscan | netscan [ip address]");
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
            ct.Rows.Add(new string[] { "whoami", "Show available information about this router" });
            ct.Rows.Add(new string[] { "telnet [ip]", "Connect to another client" });
            ct.Rows.Add(new string[] { "netscan", "Show all devices in current network" });
            ct.Rows.Add(new string[] { "netscan [network id]", "Show all devices in this network" });
            ct.Rows.Add(new string[] { "networks", "Show all connected networks" });
            ct.Rows.Add(new string[] { "logout", "Logout from router" });
            ct.Rows.Add(new string[] { "help", "Open this help window" });
            ct.DrawToConsole("Command Prompt Help", true);
        }

        public void ListNetworks()
        {
            ConsoleTable ct = new ConsoleTable();
            ct.Columns.AddRange(new string[] { "Network ID", "Connected Devices" });
            foreach (Network net in Networks)
            {
                ct.Rows.Add(new string[] { net.NetID.ToString(), net.Devices.Count.ToString() });
            }
            ct.DrawToConsole("Conneced Networks", true);
        }

        public override void WhoIs(string name)
        {
            Network_Device nwd = null;
            foreach (Network nw in networks)
            {
                Network_Device net = nw.FindDeviceByName(name);
                if (net != null)
                {
                    nwd = net;
                    break;
                }
            }
            if (nwd != null)
            {
                ConsoleTable ct = new ConsoleTable();
                if (nwd.Devicetype == Network_Device.DeviceType.Client)
                {
                    Client cl = (Client)nwd;
                    ct.Rows.Add(new string[] { "IP-Address", nwd.IP.ToString() });
                    foreach (OS_Account acc in cl.OS_Accounts)
                    {
                        ct.Rows.Add(new string[] { "OS-Account", acc.LogIn });
                    }
                }
                else if (nwd.Devicetype == Network_Device.DeviceType.Router || nwd.Devicetype == Network_Device.DeviceType.Server)
                {
                    ct.Rows.Add(new string[] { "IP-Address", nwd.IP.ToString() });
                }
                ct.DrawToConsole("WhoIs " + nwd.Name, false);
            }
            else Console.WriteLine("Device not found in this network!");
        }

        public override void Telnet(string ipAddress)
        {
            if (new IP_Address().TryParse(ipAddress) == false)
            {
                Console.WriteLine("Invalid IP-Address!");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Connecting to " + ipAddress + "...");
            Network_Device nwd = null;
            foreach (Network nw in networks)
            {
                Network_Device net = nw.FindDeviceByIP(ipAddress);
                if (net != null)
                {
                    nwd = net;
                    break;
                }
            }
            if (nwd == null && Network != null)
                nwd = Network.FindDeviceByIP(ipAddress);
            if (nwd == null && Gateway != null && ipAddress == Gateway.ToString())
                nwd = Network.IPAddresses[Gateway.ToString()];
            if (nwd == null)
            {
                Console.WriteLine("Device not found in this network! Continue with any key...");
                Console.ReadKey();
                return;
            }
            if (nwd.Devicetype == Network_Device.DeviceType.Client)
            {
                Client cl = (Client)nwd;
                Console.Write("\nPlease enter your login information:\n\nUsername: ");
                string username = Console.ReadLine();
                Console.Write("\nPassword: ");
                string password = Console.ReadLine();
                Console.WriteLine("\n Authenticating...");
                string result = cl.LogIn(username, password);
                Console.WriteLine(result);
                if (result == "Login successful")
                    cl.OpenCommandPrompt();
                else Console.ReadLine();
            }
            else if (nwd.Devicetype == Network_Device.DeviceType.Router)
            {
                Router r = (Router)nwd;
                Console.Write("\nRouter " + r.ID + "\n\nPassword: ");
                string password = Console.ReadLine();
                Console.WriteLine("\n Authenticating...");
                string result = r.LogIn(password);
                Console.WriteLine(result);
                if (result == "Login successful")
                    r.OpenCommandPrompt();
                else Console.ReadLine();
            }
            else if (nwd.Devicetype == Network_Device.DeviceType.Server)
            {
                Server s = (Server)nwd;
                Console.Write("\nCisco Server Version 2.0\n\nPassword: ");
                string password = Console.ReadLine();
                Console.WriteLine("\n Authenticating...");
                string result = s.LogIn(password);
                Console.WriteLine(result);
                if (result == "Login successful")
                    s.OpenCommandPrompt();
                else Console.ReadLine();
            }
        }

        public void Netscan(IP_Address ip)
        {
            Network nw = networks.Find(n => n.NetID.ToString() == ip.ToString());
            if (nw == null)
                Console.WriteLine("No network with IP \"" + ip.ToString() + "\" found!");
            else
            {
                if (nw.Devices.Count == 0)
                {
                    Console.WriteLine("No devices found");
                }
                else
                {
                    ConsoleTable ct = new ConsoleTable();
                    ct.Columns.AddRange(new string[] { "Name", "IP-Address", "Type" });
                    foreach (Network_Device nwd in nw.Devices)
                    {
                        ct.Rows.Add(new string[] { nwd.Name, nwd.IP.ToString() , nwd.Devicetype.ToString() });
                    }
                    ct.DrawToConsole("Netscan Result", true);
                }
            }
        }
    }
}