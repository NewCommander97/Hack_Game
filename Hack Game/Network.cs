using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Network
    {
        private List<Network_Device> devices = new List<Network_Device>();

        public List<Network_Device> Devices
        {
            get { return devices; }
            set { devices = value; }
        }

        public IP_Address NetID { get; set; }

        private Dictionary<string, Network_Device> ipAddresses = new Dictionary<string, Network_Device>();

        public Dictionary<string, Network_Device> IPAddresses
        {
            get { return ipAddresses; }
            set { ipAddresses = value; }
        }

        public Network() { }

        public Network(string ip)
        {
            NetID = new IP_Address().Parse(ip);
        }

        public Network_Device FindDeviceByIP(string ip)
        {
            foreach (Network_Device device in devices)
            {
                if (device.IP.ToString() == ip)
                    return device;
            }
            return null;
        }

        public Network_Device FindDeviceByName(string name)
        {
            foreach (Network_Device device in devices)
            {
                if (device.Name.ToLower() == name)
                    return device;
            }
            return null;
        }

        public void AddToDevices(Network_Device nd, IP_Address ip, bool addDevice)
        {
            IPAddresses.Add(ip.ToString(), nd);
            if (addDevice)
                devices.Add(nd);
        }
    }
}
