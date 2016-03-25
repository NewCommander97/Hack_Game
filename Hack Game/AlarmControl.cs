using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class AlarmControl
    {
        public string ClientName { get; set; }

        private List<Alarm_System> alarmSystems = new List<Alarm_System>();

        public List<Alarm_System> AlarmSystems
        {
            get { return alarmSystems; }
            set { alarmSystems = value; }
        }

        public Action<MailMessage> sendAction;

        public AlarmControl() { }

        public AlarmControl(string password, string passwordCrypt, string clientName, User contact)
        {
            Password = RC4.CryptToString(password, passwordCrypt);
            PasswordCrypt = passwordCrypt;
            ClientName = clientName;
            Contact = contact;
        }

        public void AlarmSystem()
        {
            Console.Title = "Command prompt - " + ClientName + " > Alarm Server";
            Console.Clear();
            bool end = false;
            while (!end)
            {
                Console.Write(ClientName + "@alarmserver> ");
                string command = Console.ReadLine().ToLower();
                switch (command.Split(' ')[0])
                {
                    case "exit":
                        end = true;
                        break;
                    case "systems":
                        ListSystems();
                        break;
                    case "system":
                        int id = 0;
                        if (int.TryParse(command.Split(' ')[1], out id))
                            System(id, command.Split(' ')[2]);
                        else Console.WriteLine("Parameter incompatible! Use system [id] [command]");
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

        private void Help()
        {
            ConsoleTable ct = new ConsoleTable();
            ct.Columns.AddRange(new string[] { "Command", "Description" });
            ct.Rows.Add(new string[] { "systems", "List all connected alarm systems" });
            ct.Rows.Add(new string[] { "system [id] [command]", "Configure the alarm system with <on>, <off> or show the <log>. Enable log with <logOn> and disable with <logOff>" });
            ct.Rows.Add(new string[] { "exit", "Exit administration tool" });
            ct.Rows.Add(new string[] { "help", "Open this help window" });
            ct.DrawToConsole("Alarm System Help", true);
        }

        private void ListSystems()
        {
            if (alarmSystems.Count == 0)
                Console.WriteLine("No systems found!");
            else
            {
                ConsoleTable ct = new ConsoleTable();
                ct.Columns.AddRange(new string[] { "ID", "Name" });
                int counter = 1;
                foreach (Alarm_System sys in alarmSystems)
                {
                    ct.Rows.Add(new string[] { counter.ToString(), sys.SystemName });
                    counter++;
                }
                ct.DrawToConsole("Connected Systems", true);
            }
        }

        private void System(int id, string command)
        {
            if (id > alarmSystems.Count)
            {
                Console.WriteLine("The alarm system with id " + id + " does not exist!");
                return;
            }
            id--;
            switch (command)
            {
                case "on":
                    alarmSystems[id].State = Alarm_System.AlarmState.On;
                    break;
                case "off":
                    alarmSystems[id].State = Alarm_System.AlarmState.Off;
                    break;
                case "log":
                    Console.WriteLine("System log of system \"" + alarmSystems[id].SystemName + "\":\n\n" + alarmSystems[id].Log);
                    break;
                case "logon":
                    if (alarmSystems[id].StateLog == true)
                        Console.WriteLine("System log already enabled!");
                    else alarmSystems[id].StateLog = true;
                    break;
                case "logoff":
                    if (alarmSystems[id].StateLog == false)
                        Console.WriteLine("System log already disabled!");
                    else alarmSystems[id].StateLog = false;
                    break;
                default:
                    break;
            }
        }

        public string Password { get; set; }

        public string PasswordCrypt { get; set; }

        public string LoginCode { get; set; }

        public User Contact { get; set; }

        public void GetLogInCode()
        {
            LoginCode = GenerateLoginCode();
            Mail_Program mp = new Mail_Program(new User(ClientName, "Alarm Server", "no-reply@alarm-server.com"));
            mp.OnMessageSending = sendAction;
            MailMessage msg = new MailMessage(mp.Owner, Contact, "Dear " + Contact.Surname + " " + Contact.Name + ",\n\nHere is the new code for your LogIn:\n\n" + LoginCode + "\n\nPlease pay attention to the secrecy of the code!\n\nWith regards\n\nAlarm Systems Ltd.", DateTime.Now);
            mp.SendMessage(msg);
        }

        public string LogIn(string password, string loginCode)
        {
            if (RC4.CryptToString(password, PasswordCrypt) == Password)
            {
                if (loginCode == LoginCode)
                    return "Login successful";
                else
                {
                    Mail_Program mp = new Mail_Program(new User(ClientName, "Alarm Server", "no-reply@alarm-server.com"));
                    MailMessage msg = new MailMessage(mp.Owner, Contact, "Dear " + Contact.Surname + " " + Contact.Name + ",\n\nSomebody tried to login at the alarm server \"" + ClientName + "\". You should check the alarm system for unusual activities.\n\nWith regards\n\nAlarm Systems Ltd.", DateTime.Now);
                    mp.OnMessageSending = sendAction;
                    mp.SendMessage(msg);
                    return "Login failed";
                }
            }
            else
            {
                Mail_Program mp = new Mail_Program(new User(ClientName, "Alarm Server", "no-reply@alarm-server.com"));
                MailMessage msg = new MailMessage(mp.Owner, Contact, "Dear " + Contact.Surname + " " + Contact.Name + ",\n\nSomebody tried to login at the alarm server \"" + ClientName + "\". You should check the alarm system for unusual activities.\n\nWith regards\n\nAlarm Systems Ltd.", DateTime.Now);
                mp.OnMessageSending = sendAction;
                mp.SendMessage(msg);
                return "Login failed";
            }
        }

        private string GenerateLoginCode()
        {
            Random r = new Random();
            return r.Next(0, 21).ToString() + (char)r.Next(65, 90) + r.Next(0, 21).ToString() + (char)r.Next(65, 90) + r.Next(0, 21).ToString() + (char)r.Next(65, 90) + r.Next(0, 21).ToString() + (char)r.Next(65, 90) + r.Next(0, 21).ToString() + (char)r.Next(65, 90) + r.Next(0, 21).ToString() + (char)r.Next(65, 90) + r.Next(0, 21).ToString() + (char)r.Next(65, 90);
        }

        // Menu
        public void Login()
        {
            Console.Clear();
            Console.WriteLine("--------------------------AlarmServer Login--------------------------\nPlease enter the valid login information:\n");
            Console.Write("Password: ");
            string password = Console.ReadLine();
            Console.Write("\nLogin code: ");
            string code = Console.ReadLine();
            string result = LogIn(password, code);
            if (result == "Login successful")
                AlarmSystem();
            else
            {
                Console.WriteLine(result);
                Console.ReadLine();
                GetLogInCode();
            }
        }
    }
}
