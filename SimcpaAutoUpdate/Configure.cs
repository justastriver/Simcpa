using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Data;

namespace SimcpaAutoUpdate
{
    class UpdateFileInfo
    {
        public string name;
        public string url;
        public string destInstallPath;

    }
    class UpdateCommand
    {
        public string command;
    }

    /// <summary>
    /// 1. 优先级black > white
    /// 2. [black:*, white:*] : 错误配置，将全部更新
    /// 3. [black:123, white:*] : 正确配置，除black list 以外全部更新
    /// 4. [black:*, white:234] : 正确配置，只更新white list
    /// 5. [black:123, white:234] : 错误配置，等同于4，只更新white list
    /// TODO， 将来支持正则，通配符
    /// </summary>
    class Filter
    {
        public string black = "*";
        public string white = "*";
    }
    class UpdateConfigure
    {
        public string version = "";
        public DateTime updateDate = new DateTime();
        public string updateUrl;
        public int updateInterval = 10000;
        public List<UpdateFileInfo> files = new List<UpdateFileInfo>();
        public List<UpdateCommand> commands = new List<UpdateCommand>();
        public Filter filter = new Filter();
        public List<string/*service name*/> monitor = new List<string>();
    }
    class ConfigureReader
    {
        public static string GetRemoteContent(string url)
        {    
            string content = "";
            try   
            {
                HttpWebRequest oHttp_Web_Req = (HttpWebRequest)WebRequest.Create(url);     
                Stream stream = oHttp_Web_Req.GetResponse().GetResponseStream();    
                using (StreamReader respStreamReader = new StreamReader(stream, Encoding.UTF8))     
                {   
                    string line = string.Empty;   
                    while ((line = respStreamReader.ReadLine()) != null)      
                    {                                       
                        content += line;
                    }          
                }          
            }       
            catch (Exception ex) { 

            }  
            return content;
        }

        public static UpdateConfigure ReadRemote(string url)
        {
            UpdateConfigure setting = new UpdateConfigure();
            string xmlContent = GetRemoteContent(url);
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(xmlContent);
                setting = Read(xmldoc);
            }
            catch (System.Exception ex)
            {
            	
            }
            
            return setting;
        }
        private static UpdateConfigure Read(XmlDocument xmldoc)
        {
            UpdateConfigure setting = new UpdateConfigure();

            try
            {
                #region Update Root
                XmlNode syscfg = xmldoc.SelectSingleNode("update");
                foreach (XmlNode item in syscfg)//遍历 
                {
                    if (item.Name == "version")
                    {
                        setting.version = item.InnerText;
                    }
                    else if (item.Name == "date")
                    {
                        setting.updateDate = Convert.ToDateTime(item.InnerText);

                    }
                    else if (item.Name == "url")
                    {
                        setting.updateUrl = item.Attributes["href"].Value;
                    }
                    else if (item.Name == "interval")
                    {
                        setting.updateInterval = int.Parse(item.InnerText);
                        if (setting.updateInterval < 1000)
                        {
                            setting.updateInterval = 10000;
                        }
                    }
                }
                #endregion 

                #region Filter
                XmlNode filter_node = xmldoc.SelectSingleNode("update/filter");
                if (filter_node != null)
                {
                    foreach (XmlNode item in filter_node)//遍历 
                    {
                        if (item.Name == "black")
                        {
                            if (item.InnerText != "")
                                setting.filter.black = item.InnerText;
                        }
                        else if (item.Name == "white")
                        {
                            if (item.InnerText != "")
                                setting.filter.white = item.InnerText;
                        }
                    }

                }
                #endregion

                #region Monitor
                XmlNode monitor_node = xmldoc.SelectSingleNode("update/monitor");
                if (monitor_node != null)
                {
                    foreach (XmlNode item in monitor_node)//遍历 
                    {

                        if (item.InnerText != "")
                        {
                            setting.monitor.Add(item.InnerText.Trim());
                        }

                    }
                }
                #endregion

                #region Load Files

                XmlNode node = xmldoc.SelectSingleNode("update/files");
                XmlNodeList files = node.ChildNodes;
                foreach (XmlNode file in files)
                {
                    UpdateFileInfo info = new UpdateFileInfo();
                    info.name = file.Attributes["name"].Value;
                    info.url = file.Attributes["href"].Value;
                    info.destInstallPath = file.Attributes["local_path"].Value;


                    setting.files.Add(info);

                }

                #endregion

                #region Load Command

                XmlNode comm_node = xmldoc.SelectSingleNode("update/commands");
                XmlNodeList commands = comm_node.ChildNodes;
                foreach (XmlNode command in commands)
                {
                    UpdateCommand info = new UpdateCommand();
                    info.command = command.InnerText;
                    setting.commands.Add(info);
                }

                #endregion
            }
            catch (System.Exception ex)
            {

            }

            return setting;
        }
        public static UpdateConfigure ReadLocal(string config_file_name)
        {
            UpdateConfigure setting = new UpdateConfigure();
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(config_file_name);
                setting = Read(xmldoc);

            }
            catch (System.Exception ex)
            {
                return null;
            }

            return setting;
        }

        public static bool SaveConfig(UpdateConfigure configure, string config_file_name)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(config_file_name);
                XmlNode root = xmldoc.DocumentElement;
                root.RemoveAll();//clear all
                XmlNode ver = xmldoc.CreateElement("version");
                ver.InnerText = configure.version;
                root.AppendChild(ver);
                XmlNode dt = xmldoc.CreateElement("date");
                dt.InnerText = configure.updateDate.ToString();
                root.AppendChild(dt);
                XmlNode url = xmldoc.CreateElement("url");
                url.Attributes["href"].Value = configure.updateUrl;
                
                XmlNode files_root = xmldoc.SelectSingleNode("update/files");
                
                foreach (UpdateFileInfo file in configure.files)
                {
                    XmlNode file_node = xmldoc.CreateElement("file");
                    file_node.Attributes["name"].Value = file.name;
                    file_node.Attributes["href"].Value = file.url;
                    file_node.Attributes["local_path"].Value = file.destInstallPath;
                    files_root.AppendChild(file_node);
                    
                }
                root.AppendChild(files_root);

                #region cmd
                XmlNode cmd_root = xmldoc.SelectSingleNode("update/commands");
                foreach (UpdateCommand command in configure.commands)
                {
                    XmlNode cmd_node = xmldoc.CreateElement("command");
                    cmd_node.InnerText = command.command;
                    
                    cmd_root.AppendChild(cmd_node);

                }
                root.AppendChild(cmd_root);
                #endregion 

                #region filter
                XmlNode filter_root = xmldoc.SelectSingleNode("update/filter");
                XmlNode filter_black_node = xmldoc.CreateElement("black");
                filter_black_node.InnerText = configure.filter.black;
                filter_root.AppendChild(filter_black_node);

                XmlNode filter_white_node = xmldoc.CreateElement("white");
                filter_white_node.InnerText = configure.filter.white;
                filter_root.AppendChild(filter_white_node);

                root.AppendChild(filter_root);

                #endregion 

                #region Monitor
                XmlNode monitor_root = xmldoc.SelectSingleNode("update/monitor");
                foreach (string service_name in configure.monitor)
                {
                    XmlNode service_node = xmldoc.CreateElement("service");
                    service_node.InnerText = service_name;
                    monitor_root.AppendChild(service_node);
                }

                root.AppendChild(monitor_root);

                #endregion 
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
