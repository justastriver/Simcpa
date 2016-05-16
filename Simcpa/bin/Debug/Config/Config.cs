using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace QuchifanPrinter.Config
{
    
    class Setting
    {
        public SystemConfig system = new SystemConfig();
        public ArrayList printers = new ArrayList();//store of Deviceinfo
    }
    class DeviceInfo
    {
        public string id;
        public string name;
        public string print_width;
        public string type;
        public string addr;
        public string desc;

    }
    class SystemConfig
    {
        public string restaurant_id = "";
        public string server_ip = "192.168.2.13";
        public string server_port = "6666";
    }
    
}
