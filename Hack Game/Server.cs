using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Server : Network_Device
    {
        public string Password { get; set; }

        public string PasswordCrypt { get; set; }

        private List<File> files = new List<File>();

        public List<File> Files
        {
            get { return files; }
            set { files = value; }
        }

        public AlarmControl AlarmControl { get; set; }

        private Server_CommandPrompt CommandPrompt = new Server_CommandPrompt();

        public Server()
        {
            Devicetype = DeviceType.Server;
        }

        public Server(string ip, string name, string password, string passwordCrypt)
        {
            IP = new IP_Address().Parse(ip);
            Name = name;
            Password = RC4.CryptToString(password, passwordCrypt);
            PasswordCrypt = passwordCrypt;
            Devicetype = DeviceType.Server;
        }

        public void OpenCommandPrompt()
        {
            CommandPrompt.ClientName = Name;
            CommandPrompt.Gateway = Gateway;
            CommandPrompt.Network = Network;
            CommandPrompt.IP = IP;
            CommandPrompt.AlarmControl = AlarmControl;
            CommandPrompt.Files = Files;
            CommandPrompt.Open();
        }

        public string LogIn(string password)
        {
            if (RC4.CryptToString(password, PasswordCrypt) == Password)
                return "Login successful";
            else return "Login failed";
        }
    }
}
