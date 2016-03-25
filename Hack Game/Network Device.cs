using System;

namespace Hack_Game
{
    [Serializable()]
    class Network_Device
    {
        public string Name { get; set; }

        public IP_Address IP { get; set; }

        public IP_Address Gateway { get; set; }

        public Network Network { get; set; }

        public enum DeviceType { Client, Router, Server }

        public DeviceType Devicetype { get; set; }
    }
}
