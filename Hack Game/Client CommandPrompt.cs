using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Client_CommandPrompt : CommandPrompt
    {
        private Mail_Program mailProgram = new Mail_Program();

        public Mail_Program MailProgram
        {
            get { return mailProgram; }
            set { mailProgram = value; }
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
                    case "blackmail":
                        if (command.Split(' ').Length > 1)
                            Console.WriteLine("Command to long! How to use: blackmail");
                        else BlackMail();
                        Console.Title = "Command prompt - " + ClientName;
                        Console.Clear();
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
            ct.Rows.Add(new string[] { "blackmail", "Open the mail program \"Blackmail\"" });
            ct.Rows.Add(new string[] { "telnet [ip]", "Connect to another client" });
            ct.Rows.Add(new string[] { "netscan", "Show all devices in current network" });
            ct.Rows.Add(new string[] { "logout", "Logout from client" });
            ct.Rows.Add(new string[] { "help", "Open this help window" });
            ct.DrawToConsole("Command Prompt Help", true);
        }

        // BlackMail Methods

        void BlackMail()
        {
            Console.Title = "Command prompt - " + ClientName + " > Blackmail";
            Console.Clear();
            bool end = false;
            while (!end)
            {
                Console.Write(ClientName + "@blackmail> ");
                string command = Console.ReadLine().ToLower();
                switch (command.Split(' ')[0])
                {
                    case "exit":
                        end = true;
                        break;
                    case "inbox":
                        if (command.Split(' ').Length == 1)
                            BlackMailInbox();
                        else if (command.Split(' ').Length == 2)
                        {
                            int id = 0;
                            if (int.TryParse(command.Split(' ')[1], out id))
                                BlackMailInboxMessage(id);
                            else Console.WriteLine("Parameter incompatible! Use inbox [number]");
                        }
                        break;
                    case "outbox":
                        if (command.Split(' ').Length == 1)
                            BlackMailOutbox();
                        else if (command.Split(' ').Length == 2)
                        {
                            int id = 0;
                            if (int.TryParse(command.Split(' ')[1], out id))
                                BlackMailOutboxMessage(id);
                            else Console.WriteLine("Parameter incompatible! Use outbox [number]");
                        }
                        break;
                    case "write":
                        BlackMailWrite();
                        break;
                    case "help":
                        BlackMailHelp();
                        break;
                    case "":
                        break;
                    default:
                        Console.WriteLine("Unknown command \"" + command + "\". Type help for more information.");
                        break;
                }
            }
        }

        void BlackMailHelp()
        {
            ConsoleTable ct = new ConsoleTable();
            ct.Columns.AddRange(new string[] { "Command", "Description" });
            ct.Rows.Add(new string[] { "inbox", "List of all mails in the inbox" });
            ct.Rows.Add(new string[] { "inbox [number]", "View the mail from the inbox" });
            ct.Rows.Add(new string[] { "outbox", "List of all mails in the outbox" });
            ct.Rows.Add(new string[] { "outbox [number]", "View the mail from the outbox" });
            ct.Rows.Add(new string[] { "write", "Write a new mail" });
            ct.Rows.Add(new string[] { "exit", "Exit blackmail" });
            ct.Rows.Add(new string[] { "help", "Open this help window" });
            ct.DrawToConsole("Blackmail Help", true);
        }

        void BlackMailWrite()
        {
            Console.Clear();
            Console.WriteLine("-----------------Blackmail - Write a message-----------------\n");
            Console.Write("To: ");
            string mailAddress = Console.ReadLine();
            bool correctDate = false;
            DateTime dt = DateTime.Now;
            while (!correctDate)
            {
                Console.Write("\nDate: ");
                if (DateTime.TryParse(Console.ReadLine(), out dt))
                {
                    correctDate = true;
                }
                else Console.WriteLine("\n\nUnknown date format - Please try again!\n");
            }
            Console.WriteLine("\nMessage: (Ctrl+Z and then Enter to exit)\n");
            string message = "";
            string line = "";
            bool firstline = true;
            do
            {
                line = Console.ReadLine();
                if (!firstline)
                {
                    message += "\n";
                }
                if (line != null)
                    message += line;
                firstline = false;
            }
            while (line != null);

            mailProgram.SendMessage(new MailMessage(mailProgram.Owner, new User(mailAddress), message, dt));

            Console.WriteLine("\nMessage sent...");
            Console.ReadLine();
        }

        void BlackMailInbox()
        {
            int counter = 1;
            if (mailProgram.Inbox.Messages.Count == 0)
                Console.WriteLine("No messages in inbox!");
            else
            {
                ConsoleTable ct = new ConsoleTable();
                ct.Columns.AddRange(new string[] { "Number", "From", "Date" });
                foreach (MailMessage msg in mailProgram.Inbox.Messages)
                {
                    ct.Rows.Add(new string[] { counter.ToString(), msg.From.Surname + " " + msg.From.Name, msg.Date.ToShortDateString() });
                    counter++;
                }
                ct.DrawToConsole("Inbox", true);
            }
        }

        void BlackMailOutbox()
        {
            int counter = 1;
            if (mailProgram.Outbox.Messages.Count == 0)
                Console.WriteLine("No messages in outbox!");
            else
            {
                ConsoleTable ct = new ConsoleTable();
                ct.Columns.AddRange(new string[] { "Number", "From", "Date" });
                foreach (MailMessage msg in mailProgram.Outbox.Messages)
                {
                    ct.Rows.Add(new string[] { counter.ToString(), msg.From.Surname + " " + msg.From.Name, msg.Date.ToShortDateString() });
                    counter++;
                }
                ct.DrawToConsole("Outbox", true);
            }
        }

        void BlackMailInboxMessage(int id)
        {
            if (id > mailProgram.Inbox.Messages.Count || id < 1)
            {
                Console.WriteLine("The message with the number " + id + " does not exist!");
                return;
            }
            MailMessage msg = mailProgram.Inbox.Messages[id - 1];
            Console.WriteLine("-------------------Message " + id + "-------------------\n");
            Console.WriteLine("From: " + msg.From.Surname + " " + msg.From.Name + " (" + msg.From.MailAddress + ") Date: " + msg.Date.ToShortDateString());
            Console.WriteLine("\n" + msg.Message + "\n");
            foreach (char c in "-------------------Message " + id + "-------------------\n")
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        void BlackMailOutboxMessage(int id)
        {
            if (id > mailProgram.Outbox.Messages.Count || id < 1)
            {
                Console.WriteLine("The message with the number " + id + " does not exist!");
                return;
            }
            MailMessage msg = mailProgram.Outbox.Messages[id - 1];
            Console.WriteLine("-------------------Message " + id + "-------------------\n");
            Console.WriteLine("From: " + msg.To.Surname + " " + msg.To.Name + " (" + msg.From.MailAddress + ") Date: " + msg.Date.ToShortDateString());
            Console.WriteLine("\n" + msg.Message + "\n");
            foreach (char c in "-------------------Message " + id + "-------------------\n")
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }
    }
}
