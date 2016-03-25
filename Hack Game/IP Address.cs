using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class IP_Address
    {
        public int Oct1 { get; set; }
        public int Oct2 { get; set; }
        public int Oct3 { get; set; }
        public int Oct4 { get; set; }

        public IP_Address() { }

        public IP_Address(int oct1, int oct2, int oct3, int oct4)
        {
            Oct1 = oct1;
            Oct2 = oct2;
            Oct3 = oct3;
            Oct4 = oct4;
        }

        public IP_Address Parse(string src)
        {
            string[] oct = src.Split('.');
            Oct1 = Convert.ToInt32(oct[0]);
            Oct2 = Convert.ToInt32(oct[1]);
            Oct3 = Convert.ToInt32(oct[2]);
            Oct4 = Convert.ToInt32(oct[3]);
            return this;
        }

        public bool TryParse(string src)
        {
            if (src.Count(c => c == '.') == 3)
            {
                string[] oct = src.Split('.');
                int oct1 = 0;
                int oct2 = 0;
                int oct3 = 0;
                int oct4 = 0;

                if (int.TryParse(oct[0], out oct1) && int.TryParse(oct[1], out oct2) && int.TryParse(oct[2], out oct3) && int.TryParse(oct[3], out oct4))
                {
                    Oct1 = oct1;
                    Oct2 = oct2;
                    Oct3 = oct3;
                    Oct4 = oct4;
                    return true;
                }
                else
                {
                    Oct1 = 0;
                    Oct2 = 0;
                    Oct3 = 0;
                    Oct4 = 0;
                    return false;
                }
            }
            else
            {
                Oct1 = 0;
                Oct2 = 0;
                Oct3 = 0;
                Oct4 = 0;
                return false;
            }
        }

        public override string ToString()
        {
            return Oct1 + "." + Oct2 + "." + Oct3 + "." + Oct4;
        }
    }
}
