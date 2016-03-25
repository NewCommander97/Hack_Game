using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class CommandPrompt
    {
        private Network network;

        public Network Network
        {
            get { return network; }
            set { network = value; }
        }

        public string ClientName { get; set; }
        public IP_Address Gateway { get; set; }
        public IP_Address IP { get; set; }

        // General Methods

        public virtual void Open() { }

        public virtual void Help() { }

        // For User Executable Methods

        public virtual void WhoIs(string name)
        {
            Network_Device nwd = Network.FindDeviceByName(name);
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

        public virtual void WhoAmI()
        {
            ConsoleTable ct = new ConsoleTable();
            ct.Rows.Add(new string[] { "Name", ClientName });
            ct.Rows.Add(new string[] { "IP-Address", IP.ToString() });
            if (Gateway != null)
                ct.Rows.Add(new string[] { "Default Gateway", Gateway.ToString() });
            else
                ct.Rows.Add(new string[] { "Default Gateway", "Not connected!" });
            ct.DrawToConsole("WhoAmI Result", false);
        }

        public virtual void Telnet(string ipAddress)
        {
            if (new IP_Address().TryParse(ipAddress) == false)
            {
                Console.WriteLine("Invalid IP-Address!");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Connecting to " + ipAddress + "...");
            Network_Device nwd = null;
            if (ipAddress == Gateway.ToString())
                nwd = network.IPAddresses[Gateway.ToString()];
            else
            {
                network.IPAddresses.TryGetValue(Gateway.ToString(), out nwd);
                Router dr = (Router)nwd;
                nwd = dr.Transfer(new IP_Address().Parse(ipAddress));
            }
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

        public void Netscan()
        {
            if (Network == null || Network.Devices.Count == 0)
            {
                Console.WriteLine("No devices found");
            }
            else
            {
                ConsoleTable ct = new ConsoleTable();
                ct.Columns.AddRange(new string[] { "Name", "IP-Address", "Type" });
                foreach (Network_Device nwd in Network.Devices)
                {
                    ct.Rows.Add(new string[] { nwd.Name, nwd.IP.ToString(), nwd.Devicetype.ToString() });
                }
                ct.DrawToConsole("Netscan Result", true);
            }
        }
    }
}
