using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using ISimcpa.Config;

namespace ISimcpa.Util
{
    
    public class XMLParser
    {
        public static Setting Read(string config_file_name)
        {
            Setting setting = new Setting();
            ArrayList printerList=new ArrayList();
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(config_file_name);
                //XmlNode root = xmldoc.DocumentElement;

                #region Load System Setting

                XmlNode syscfg = xmldoc.SelectSingleNode("config/system");
                foreach (XmlNode item in syscfg)//遍历 
                {
                    if(item.Name == "restaurant_id")
                    {
                        setting.system.restaurant_id = item.InnerText;
                    }
                    else if (item.Name == "server_ip")
                    {
                        setting.system.server_ip = item.InnerText;

                    }else if (item.Name == "server_port")
                    {
                        setting.system.server_port = item.InnerText;
                    }
                    else if (item.Name == "heartbeat_interval")
                    {
                        int interval = 5000;
                        try
                        {
                            interval = int.Parse(item.InnerText);
                        }
                        catch (System.Exception ex)
                        {
                            Logger.Error("parse xml failed: " + ex.Message);
                        }
                        if (interval < 1000)
                        {
                            interval = 1000;
                        }
                        setting.system.heartbeat_interval = interval;
                    }
                }
#endregion

                #region Load Printers

                XmlNode node = xmldoc.SelectSingleNode("config/printers");
                XmlNodeList printers = node.ChildNodes;
                foreach (XmlNode printer in printers)
                {
                    DeviceInfo info = new DeviceInfo();
                    foreach (XmlNode item in printer)//遍历 
                    {
                        
                        if (item.Name == "id")
                        {
                            info.id = item.InnerText;
                        }else if (item.Name == "name")
                        {
                            info.name = item.InnerText;
                        }else if (item.Name == "print_width")
                        {
                            string print_width = item.InnerText;
                            if (print_width.IndexOf("80") >= 0)
                            {
                                info.print_width = PrintWidth.MM80;
                            }
                            else
                            {
                                info.print_width = PrintWidth.MM58;
                            }
                            
                        } 
                        else if(item.Name == "type")
                        {
                            info.type = item.InnerText;
                        }else if (item.Name == "addr")
                        {
                            info.addr = item.InnerText;
                        }else if (item.Name == "desc")
                        {
                            info.desc = item.InnerText;
                        }
                    }
                    printerList.Add(info);
                }
                setting.printers = printerList;
#endregion
            }
            catch (Exception e)
            {
                Logger.Error("parse xml failed 2: " + e.Message);
            }
            return setting;
        }

        public static bool SavePrinterConfig(ArrayList printerList, string config_file_name)
        {
 
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(config_file_name);
                //XmlNode root = xmldoc.DocumentElement;
                //root.RemoveAll();//clear all
                XmlNode printer_root = xmldoc.SelectSingleNode("config/printers");
                printer_root.RemoveAll();
                foreach (DeviceInfo printer in printerList)
                {
                    XmlNode printer_node = xmldoc.CreateElement("printer");
                    XmlNode node_id = xmldoc.CreateElement("id");
                    node_id.InnerText = printer.id;
                    printer_node.AppendChild(node_id);

                    XmlNode node_name = xmldoc.CreateElement("name");
                    node_name.InnerText = printer.name;
                    printer_node.AppendChild(node_name);

                    XmlNode node_print_width = xmldoc.CreateElement("print_width");
                    string print_width = "58mm";
                    if (printer.print_width == PrintWidth.MM80)
                    {
                        print_width = "80mm";
                    }
                    node_print_width.InnerText = print_width;
                    printer_node.AppendChild(node_print_width);

                    XmlNode node_type = xmldoc.CreateElement("type");
                    node_type.InnerText = printer.type;
                    printer_node.AppendChild(node_type);

                    XmlNode node_addr = xmldoc.CreateElement("addr");
                    node_addr.InnerText = printer.addr;
                    printer_node.AppendChild(node_addr);

                    XmlNode node_desc = xmldoc.CreateElement("desc");
                    node_desc.InnerText = printer.desc;
                    printer_node.AppendChild(node_desc);

                    printer_root.AppendChild(printer_node);
                    
                }
                xmldoc.Save(config_file_name);//保存到books.xml

              
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public static bool SaveSystemConfig(SystemConfig config, string config_file_name)
        {

            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(config_file_name);
                //XmlNode root = xmldoc.DocumentElement;
                //root.RemoveAll();//clear all
                XmlNode system_root = xmldoc.SelectSingleNode("config/system");
                system_root.RemoveAll();

                XmlNode node_id = xmldoc.CreateElement("restaurant_id");
                node_id.InnerText = config.restaurant_id;
                system_root.AppendChild(node_id);

                XmlNode node_server_ip = xmldoc.CreateElement("server_ip");
                node_server_ip.InnerText = config.server_ip;
                system_root.AppendChild(node_server_ip);

                XmlNode node_server_port = xmldoc.CreateElement("server_port");
                node_server_port.InnerText = config.server_port;
                system_root.AppendChild(node_server_port);

                XmlNode node_hb_interval = xmldoc.CreateElement("heartbeat_interval");
                node_hb_interval.InnerText = config.heartbeat_interval.ToString();
                system_root.AppendChild(node_hb_interval);


                xmldoc.Save(config_file_name);//保存到books.xml


            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}
