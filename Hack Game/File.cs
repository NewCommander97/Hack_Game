using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class File
    {
        public string Filename { get; set; }

        public byte[] Content { get; set; }

        public bool PasswordProtected { get; set; }

        public File() { }

        public File(string filename, string content)
        {
            Filename = filename;
            Content = Encoding.ASCII.GetBytes(content);
        }

        public void CryptFile(string password)
        {
            Content = RC4.Crypt(Content, password);
            PasswordProtected = !PasswordProtected;
        }
    }
}
