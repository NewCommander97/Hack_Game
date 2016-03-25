using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    class Program
    {
        static List<Mail_Program> mail_programs = new List<Mail_Program>();

        static void Main(string[] args)
        {
            ChapterScreen cs = new ChapterScreen();
            cs.Title = "Chapter One: Test";
            cs.Description = "You are a hacker who carries out orders for a generous salary. You must chop a server and turn an alarm system off today. If you are ready, you simply can log off of your PC.\n\nOne tip: You can find the password for the server on \"Client2\"!";
            //cs.ShowScreen();

            //Router
            Router mainRouter = new Router("192.168.1.1", "MainRouter", "R-5790", "cisco", "Security");
            /*Router administrationRouter = new Router("192.168.1.2", "AdministrationRouter", "R-5810", "cisco", "Security");
            administrationRouter.Gateway = mainRouter.IP;
            Router serverRouter = new Router("192.168.1.3", "ServerRouter", "R-5810", "cisco", "Security");
            serverRouter.Gateway = mainRouter.IP;
            Router salesRouter = new Router("192.168.1.4", "SalesRouter", "R-5810", "cisco", "Security");
            salesRouter.Gateway = mainRouter.IP;

            //Server
            Server securityServer = new Server("192.168.3.2", "SecurityServer", "!9a^T7m5", "Security");
            securityServer.Gateway = new IP_Address().Parse("192.168.2.1");
            Server fileServer = new Server("192.168.3.3", "Fileserver", "qm865Be3", "Security");
            fileServer.Gateway = new IP_Address().Parse("192.168.2.1");

            #region Clients

            // Administration
            Client adminClient1 = new Client("192.168.2.2", "Admin-PC1", new IP_Address().Parse("192.168.2.1"));
            adminClient1.CreateOSUser("Admin", "No hint", "68Y5d7sS", "Security", false);
            mail_programs.Add(adminClient1.CreateMailUser("Jefferson", "Sam", "sam.jefferson@steal-industries.com", SendMessage));

            Client adminClient2 = new Client("192.168.2.3", "Admin-PC2", new IP_Address().Parse("192.168.2.1"));
            adminClient2.CreateOSUser("Admin", "No hint", "68Y5d7sS", "Security", false);
            mail_programs.Add(adminClient2.CreateMailUser("Johnson", "Ben", "ben.johnson@steal-industries.com", SendMessage));


            //Sales
            Client salesClient1 = new Client("192.168.4.2", "Sales-PC1", new IP_Address().Parse("192.168.4.1"));
            salesClient1.CreateOSUser("Admin", "No hint", "68Y5d7sS", "Security", false);
            mail_programs.Add(salesClient1.CreateMailUser("Simmons", "Lola", "ben.johnson@steal-industries.com", SendMessage));

            Client salesClient2 = new Client("192.168.4.3", "Sales-PC2", new IP_Address().Parse("192.168.4.1"));
            salesClient2.CreateOSUser("Admin", "No hint", "68Y5d7sS", "Security", false);
            mail_programs.Add(salesClient2.CreateMailUser("Simmons", "Lola", "ben.johnson@steal-industries.com", SendMessage));

            #endregion

            //AlarmControl
            AlarmControl serverAlarmControl = new AlarmControl("BYXE4619", "Security", securityServer.Name, adminClient1.MailProgram.Owner);

            #region AlarmSystems
            Alarm_System as1 = new Alarm_System("Main entrance");
            as1.StateLog = true;
            as1.State = Alarm_System.AlarmState.On;
            serverAlarmControl.AlarmSystems.Add(as1);

            Alarm_System as2 = new Alarm_System("Underground car park");
            as2.StateLog = true;
            as2.State = Alarm_System.AlarmState.On;
            serverAlarmControl.AlarmSystems.Add(as2);

            Alarm_System as3 = new Alarm_System("Warehouse");
            as3.StateLog = true;
            as3.State = Alarm_System.AlarmState.On;
            serverAlarmControl.AlarmSystems.Add(as3);
            #endregion

            Network nw1 = new Network("192.168.1.0");
            Network nw2 = new Network("192.168.2.0");
            Network nw3 = new Network("192.168.3.0");
            Network nw4 = new Network("192.168.4.0");

            nw1.AddToDevices(mainRouter, mainRouter.IP, false);
            nw1.AddToDevices(administrationRouter, administrationRouter.IP, true);
            nw1.AddToDevices(serverRouter, serverRouter.IP, true);
            nw1.AddToDevices(salesRouter, salesRouter.IP, true);

            administrationRouter.Network = nw1;
            serverRouter.Network = nw1;
            salesRouter.Network = nw1;

            nw2.AddToDevices(administrationRouter, new IP_Address().Parse("192.168.2.1"), false);
            administrationRouter.Networks.Add(nw2);
            nw2.AddToDevices(adminClient1, adminClient1.IP, true);
            nw2.AddToDevices(adminClient2, adminClient2.IP, true);

            adminClient1.Network = nw2;
            adminClient2.Network = nw2;

            nw3.AddToDevices(serverRouter, new IP_Address().Parse("192.168.3.1"), false);
            serverRouter.Networks.Add(nw3);
            nw3.AddToDevices(securityServer, securityServer.IP, true);
            nw3.AddToDevices(fileServer, fileServer.IP, true);

            securityServer.Network = nw3;
            fileServer.Network = nw3;

            nw4.AddToDevices(salesRouter, new IP_Address().Parse("192.168.4.1"), false);
            salesRouter.Networks.Add(nw4);
            nw4.AddToDevices(salesClient1, salesClient1.IP, true);
            nw4.AddToDevices(salesClient2, salesClient2.IP, true);

            salesClient1.Network = nw4;
            salesClient2.Network = nw4;

            mainRouter.Networks.Add(nw1);

            using (FileStream fs = new FileStream("test.save", FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(ObjectToByteArray(mainRouter));
            }
            */
            using (FileStream fs = new FileStream("test.save", FileMode.OpenOrCreate, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                mainRouter = (Router)ByteArrayToObject(br.ReadBytes((int)fs.Length));
            }

            mainRouter.OpenCommandPrompt();

            //adminClient1.LogIn("Admin", "68Y5d7sS");
            //adminClient1.OpenCommandPrompt();

            //if (aS.AlarmControl.AlarmSystems[0].State == Alarm_System.AlarmState.Off)
            //    Console.WriteLine("Correct");
            //else Console.WriteLine("Incorrect");
            //Console.ReadLine();
        }

        // Method to send Mails between the mail programs
        // It always has to exist in the Program.cs
        static void SendMessage(MailMessage m)
        {
            bool addressFound = false;
            foreach (Mail_Program mp in mail_programs)
            {
                if (mp.Owner.MailAddress == m.To.MailAddress)
                {
                    mp.Inbox.CreateMessage(m);
                    addressFound = true;
                    return;
                }
            }
            if (addressFound == false)
            {
                foreach (Mail_Program mp in mail_programs)
                {
                    if (mp.Owner.MailAddress == m.From.MailAddress)
                    {
                        MailMessage msg = new MailMessage(new User("DAEMON", "MAILER", "MAILER-DAEMON@MAILER-DAEMON.com"), m.From, "We were not able to deliver your mail from the " + m.Date.ToShortDateString() +  "!", DateTime.Now);
                        mp.Inbox.CreateMessage(msg);
                        return;
                    }
                }
            }
        }

        static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        static object ByteArrayToObject(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return bf.Deserialize(ms);
            }
        }
    }
}