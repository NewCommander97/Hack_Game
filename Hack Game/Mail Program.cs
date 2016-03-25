using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Mail_Program
    {
        private User owner = new User();
        public User Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private Box inbox = new Box();
        public Box Inbox
        {
            get { return inbox; }
            set { inbox = value; }
        }

        private Box outbox = new Box();
        public Box Outbox
        {
            get { return outbox; }
            set { outbox = value; }
        }

        public Mail_Program() { }

        public Mail_Program(User owner)
        {
            Owner = owner;
        }

        public Action<MailMessage> OnMessageSending;

        public void SendMessage(MailMessage msg)
        {
            OnMessageSending(msg);
            Outbox.CreateMessage(msg);
        }
    }
    [Serializable()]
    class User
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string MailAddress { get; set; }

        public User() { }

        public User(string mailAddress)
        {
            MailAddress = mailAddress;
        }

        public User(string name, string surname, string mailAddress)
        {
            Name = name;
            Surname = surname;
            MailAddress = mailAddress;
        }
    }
    [Serializable()]
    class Box
    {
        private List<MailMessage> messages = new List<MailMessage>();
        public List<MailMessage> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        public void DeleteAllMessages()
        {
            messages.Clear();
        }

        public void DeleteMessage(int id)
        {
            messages.RemoveAt(id);
        }

        public void DeleteMessage(MailMessage msg)
        {
            messages.Remove(msg);
        }

        public void CreateMessage(MailMessage msg)
        {
            messages.Add(msg);
        }
    }
    [Serializable()]
    class MailMessage
    {
        public User From { get; set; }

        public User To { get; set; }

        public DateTime Date { get; set; }

        public string Message { get; set; }

        public MailMessage() { }

        public MailMessage(User from, User to, string message, DateTime dt)
        {
            From = from;
            To = to;
            Message = message;
            Date = dt;
        }
    }
}
