using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Client : Network_Device
    {
        private List<OS_Account> os_Accounts = new List<OS_Account>();
        public List<OS_Account> OS_Accounts
        {
            get { return os_Accounts; }
            set { os_Accounts = value; }
        }

        private Mail_Program mailProgram = new Mail_Program();
        public Mail_Program MailProgram
        {
            get { return mailProgram; }
            set { mailProgram = value; }
        }

        private Client_CommandPrompt CommandPrompt = new Client_CommandPrompt();

        public Client()
        {
            Devicetype = DeviceType.Client;
        }
        public Client(string ip, string name, IP_Address gateway)
        {
            IP = new IP_Address().Parse(ip);
            Name = name;
            Gateway = gateway;
            Devicetype = DeviceType.Client;
        }

        public void OpenCommandPrompt()
        {
            CommandPrompt.ClientName = Name;
            CommandPrompt.Network = Network;
            CommandPrompt.IP = IP;
            CommandPrompt.Gateway = Gateway;
            CommandPrompt.MailProgram = MailProgram;
            CommandPrompt.Open();
        }

        public string LogIn(string logIn, string password)
        {
            string ret = "";
            foreach (OS_Account acc in os_Accounts)
            {
                if (acc.LogIn == logIn)
                {
                    if (RC4.CryptToString(password, acc.PasswordCrypt) == acc.Password)
                    {
                        acc.LoginStatus = OS_Account.LogInStatus.LoggedIn;
                        return "Login successful";
                    }
                    else return "\nLogin failed! Hint: " + acc.LogInHint;
                }
                else
                    ret = "User does not exist";
            }
            return ret;
        }

        public void LogOut()
        {
            foreach (OS_Account acc in os_Accounts)
            {
                if (acc.LoginStatus == OS_Account.LogInStatus.LoggedIn && !acc.OwnAccount)
                {
                    acc.LoginStatus = OS_Account.LogInStatus.LoggedOut;
                    return;
                }
            }
        }

        public void CreateOSUser(string logIn, string logInHint, string password, string passwordCrypt, bool ownAccount)
        {
            OS_Account acc = new OS_Account();
            acc.LogIn = logIn;
            acc.LogInHint = logInHint;
            acc.Password = RC4.CryptToString(password, passwordCrypt);
            acc.PasswordCrypt = passwordCrypt;
            acc.OwnAccount = ownAccount;
            os_Accounts.Add(acc);
        }

        public Mail_Program CreateMailUser(string name, string surname, string mailAddress, Action<MailMessage> action)
        {
            mailProgram.Owner = new User(name, surname, mailAddress);
            mailProgram.OnMessageSending = action;
            return mailProgram;
        }
    }

    [Serializable()]
    class OS_Account
    {
        public string LogIn { get; set; }

        public string Password { get; set; }

        public string LogInHint { get; set; }

        public string PasswordCrypt { get; set; }

        public LogInStatus LoginStatus { get; set; }

        public enum LogInStatus { LoggedIn, LoggedOut }

        private List<File> documents = new List<File>();

        public List<File> Documents
        {
            get { return documents; }
            set { documents = value; }
        }

        public bool OwnAccount { get; set; }
    }
}
