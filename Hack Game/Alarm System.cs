using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class Alarm_System
    {
        public string SystemName { get; set; }

        public enum AlarmState { On, Off }

        private AlarmState state;

        public AlarmState State
        {
            get { return state; }
            set { state = value; if (StateLog) Log += DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "> State Changed to " + state.ToString(); }
        }

        public bool StateLog { get; set; }

        public string Log { get; set; }

        public Alarm_System() { }

        public Alarm_System(string systemName)
        {
            SystemName = systemName;
        }
    }
}
