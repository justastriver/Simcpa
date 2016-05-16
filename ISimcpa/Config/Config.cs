using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ISimcpa.Config
{
    
    public class Setting
    {
        public string db_file_name = "Simcpa.mdb";
        public string config_file_name = "";
        public SystemConfig system = new SystemConfig();
        public ArrayList printers = new ArrayList();//store of Deviceinfo
    }
    public enum PrintWidth{MM58 ,MM80};

    public class DeviceInfo
    {
        public string id;
        public string name;
        public PrintWidth print_width;
        public string type;
        public string addr;
        public string desc;

    }
    public class SystemConfig
    {
        public string restaurant_id = "0";
        public string server_ip = "192.168.2.13";
        public string server_port = "6666";
        public int heartbeat_interval = 5000;
    }
    public class DeviceListCompare : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            return int.Parse(((DeviceInfo)x).id) - int.Parse(((DeviceInfo)y).id);
        }

    }
}
