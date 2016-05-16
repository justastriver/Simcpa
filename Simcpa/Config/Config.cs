using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Simcpa.Config
{
    
    class Setting
    {
        public SystemConfig system = new SystemConfig();
        public ArrayList printers = new ArrayList();//store of Deviceinfo
    }
    enum PrintWidth{MM58 ,MM80};

    class DeviceInfo
    {
        public string id = "";
        public string name = "";
        public PrintWidth print_width= new PrintWidth();
        public string type = "";
        public string addr = "";
        public string desc = "default description";

    }
    class SystemConfig
    {
        public string restaurant_id = "";
        public string server_ip = "192.168.2.13";
        public string server_port = "6666";
    }
    class DeviceListCompare : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            return int.Parse(((DeviceInfo)x).id) - int.Parse(((DeviceInfo)y).id);
        }

    }
}
