using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    class Level
    {
        public Router MainRouter { get; set; }

        public Network_Device StartDevice { get; set; }

        public ChapterScreen ChapterScreen { get; set; }

        public void Start()
        {
            ChapterScreen.ShowScreen();
            switch (StartDevice.Devicetype)
            {
                case Network_Device.DeviceType.Client:
                    Client c = (Client)StartDevice;
                    c.OpenCommandPrompt();
                    break;
                case Network_Device.DeviceType.Router:
                    Router r = (Router)StartDevice;
                    r.OpenCommandPrompt();
                    break;
                case Network_Device.DeviceType.Server:
                    Server s = (Server)StartDevice;
                    s.OpenCommandPrompt();
                    break;
            }
        }
    }
}
