using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Router : Network_Device
    {
        private List<Network> networks = new List<Network>();

        public List<Network> Networks
        {
            get { return networks; }
            set { networks = value; }
        }

        public string Password { get; set; }

        public string PasswordCrypt { get; set; }

        public string ID { get; set; }

        private Router_CommandPrompt CommandPrompt = new Router_CommandPrompt();

        public Router()
        {
            Devicetype = DeviceType.Router;
        }

        public Router(string ip, string name, string id, string password, string passwordCrypt)
        {
            IP = new IP_Address().Parse(ip);
            Name = name;
            ID = id;
            Password = RC4.CryptToString(password, passwordCrypt);
            PasswordCrypt = passwordCrypt;
            Devicetype = DeviceType.Router;
        }

        public void OpenCommandPrompt()
        {
            CommandPrompt.ClientName = Name;
            CommandPrompt.Network = Network;
            CommandPrompt.Networks = Networks;
            CommandPrompt.IP = IP;
            CommandPrompt.Gateway = Gateway;
            CommandPrompt.Open();
        }

        public string LogIn(string password)
        {
            if (RC4.CryptToString(password, PasswordCrypt) == Password)
                return "Login successful";
            else return "Login failed";
        }

        public Network_Device Transfer(IP_Address ip)
        {
            Network_Device nwd = null;
            if (Network != null)
            {
                nwd = Network.FindDeviceByIP(ip.ToString());
                if (nwd != null)
                    return nwd;
            }
            nwd = null;
            foreach (Network net in Networks)
            {
                nwd = net.FindDeviceByIP(ip.ToString());
            }
            return nwd;
        }
    }
}
