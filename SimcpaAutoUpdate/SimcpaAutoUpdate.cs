using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Xml;

namespace SimcpaAutoUpdate
{
    public partial class SimcpaAutoUpdate : ServiceBase
    {
        private bool __service_running = false;
        //private int __update_interval = 1000 * 60 * 1; //every 1 minute
        private ServiceController _controller;
        private WebClient __web_client = new WebClient();
        private UpdateConfigure __local_configure = new UpdateConfigure();
        private string __update_config_file = "";
        private string __service_name = "SimcpaService";
        private string __log_file = "";
        private string __app_path = "";
        private string __tmp_path = "";
        private string __simcpa_cfg_file_name = "\\Config\\config.xml";
        private string __euid = "0";

        public SimcpaAutoUpdate()
        {
            InitializeComponent();
            //load local setting
            __app_path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase; // AppDomain.CurrentDomain.BaseDirectory;
            __update_config_file = __app_path + "update.xml";
            __log_file = __app_path + "update.log";
            __tmp_path = __app_path + "\\tmp\\";
            __simcpa_cfg_file_name = __app_path + __simcpa_cfg_file_name;
            try
            {
                if (!Directory.Exists(__tmp_path))
                {
                    Directory.CreateDirectory(__tmp_path);
                }
            }
            catch (System.Exception ex)
            {
            	
            }
           
            
        }
        private void Log(string msg)
        {
            try
            {
                msg = "[" + DateTime.Now.ToLocalTime().ToString() + "]" + msg;
                using (FileStream fs = new FileStream(__log_file, FileMode.OpenOrCreate | FileMode.Append))
                {
                    lock (fs)
                    {
                        if (!fs.CanWrite)
                        {
                            return;
                        }

                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(msg);
                        sw.Dispose();
                        sw.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
            	
            }
            
        }
        private bool isVerUpdate(string serverVer, string localVer)
        {
            bool ret = false;
            try
            {
                string[] server_verion = serverVer.Split('.');
                string[] local_verion = localVer.Split('.');
                if (server_verion.Length == 4 && local_verion.Length == 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (int.Parse(server_verion[i]) > int.Parse(local_verion[i]))
                        {
                            ret = true;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log("比较版本号异常：" + ex.Message);
            }
            
            return ret;
        }
        private bool canUpdate(UpdateConfigure cfg, string euid)
        {
            //read configure
            try
            {
                if (cfg.filter.black.Trim() == "*" && cfg.filter.white.Trim() == "*")
                {
                    return true;
                }
                string[] black_list = cfg.filter.black.Trim().Split(',');
                string[] white_list = cfg.filter.white.Trim().Split(',');
                if (black_list.Length == 0 && white_list.Length == 0)
                {
                    return true;
                }

                if (white_list.Length > 0)
                {
                    if (white_list[0] == "*")
                    {
                        //check black list
                        if (true == black_list.Contains(euid))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //has white list
                        if (true == white_list.Contains(euid))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                Log("can Update 出错：" + ex.Message + ", euid " + euid);
                return false;
            }
                
            return true;
        }
        private void doCommand(string cmd)
        {
            Log("执行命令：" + cmd);
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;

                p.Start();

                p.StandardInput.WriteLine("cd " + __app_path);
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.WriteLine("exit");
                p.StandardInput.AutoFlush = true;
                string output = p.StandardOutput.ReadToEnd();
                Log(output);
               
                p.Close();
            }
            catch (System.Exception ex)
            {
                Log("执行命令失败：" + ex.Message);
            }
            

        }
        private string getEuid()
        {
            string euid = "0";
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.Load(__simcpa_cfg_file_name);
                XmlNode syscfg = xmldoc.SelectSingleNode("config/system");
                foreach (XmlNode item in syscfg)//遍历 
                {
                    if (item.Name == "restaurant_id")
                    {
                        euid = item.InnerText;
                        __euid = euid;
                        break;
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                Log("读取Euid 出错：" + ex.Message);
            }
            return euid;
        }
        private void StartUpdateCheckingThread()
        {
            string euid = "0";
            while (true == __service_running)
            {
                //读取远程配置
                //Log("开始检查更新:" + __local_configure.updateUrl);
                try
                {
                    //读取配置
                    euid = getEuid();
                    string update_url = __local_configure.updateUrl + "?euid=" + euid + "&ver=" + __local_configure.version;
                    //Log("请求参数：" + update_url);
                    UpdateConfigure remote_configure = ConfigureReader.ReadRemote(update_url);
                    if (true == isVerUpdate(remote_configure.version, __local_configure.version)
                        && true == canUpdate(remote_configure, euid))
                    {
                        Log("开始更新,正在暂停服务，版本号：" + remote_configure.version);
                        //执行升级操作，首先暂停服务
                        StopService(__service_name);
                        Thread.Sleep(30000);//等待30m服务停止完成
                        //开始下载
                        bool updateRes = true;
                        foreach (UpdateFileInfo file in remote_configure.files)
                        {
                            if (false == Download(file.url, file.destInstallPath))
                            {
                                Log("下载文件失败：" + file.name + "," + file.url + "," + file.destInstallPath);
                                updateRes = false;
                                break;
                            }
                            else
                            {
                                Log("下载文件完成：" + file.name + "," + file.url + "," + file.destInstallPath);
                            }
                        }
                        //StartService(__service_name);
                        //全部成功，直接下载配置更新文件
                        if (true == updateRes)
                        {
                            if (false == Download(__local_configure.updateUrl, "./update.xml"))
                            {
                                //如果失败，直接保存配置文件信息，否则使用网络配置
                                ConfigureReader.SaveConfig(remote_configure, __update_config_file);
                            }
                            Log("下载更新配置文件：" + __local_configure.updateUrl);

                            //重启服务
                            //执行命令行
                            foreach (UpdateCommand command in remote_configure.commands)
                            {
                                doCommand(command.command);
                            }

                            //更新本地配置
                            __local_configure = remote_configure;
                            Log("版本更新完成！");
                        }
                        else
                        {
                            Log("版本更新失败！");
                        }

                        StartService(__service_name);
                        
                    }
                }
                catch (System.Exception ex)
                {
                    Log("更新异常： " + ex.Message);
                }
                Thread.Sleep(__local_configure.updateInterval);
                try
                {
                    monitorService(__local_configure.monitor);
                }
                catch (System.Exception ex)
                {
                    Log("监控服务异常： " + ex.Message);
                }
                Thread.Sleep(__local_configure.updateInterval);
            }

        }
        protected override void OnStart(string[] args)
        {
            Log("服务启动，读取配置："+__update_config_file);
            __local_configure = ConfigureReader.ReadLocal(__update_config_file);
            if (__local_configure == null || __local_configure.version==null ||
                __local_configure.updateUrl == null)
            {
                Log("服务启动出现异常，请检查配置文件：" + __update_config_file);
                OnStop();
            }
            __service_running = true;
            Thread update_thread = new Thread(new ThreadStart(StartUpdateCheckingThread));
            update_thread.Start();
            Log("服务启动完成！");
        }

        protected override void OnStop()
        {
            Log("服务结束中...");
            __service_running = false;
        }

        private bool Download(string url, string localFile)
        {
            string tmp_file_name = __tmp_path + localFile;
            string dest_file_name = __app_path + localFile;
            try
            {
                if (File.Exists(tmp_file_name))
                {
                    //删除文件
                    File.Delete(tmp_file_name);
                }
            }
            catch (System.Exception ex)
            {
                Log("删除文件失败：" + ex.Message);
            }
            Log("已经清空文件:" + tmp_file_name);
            try
            {
                __web_client.DownloadFile(url, tmp_file_name);
            }
            catch (System.Exception ex)
            {
                Log("下载文件(" + url + ")失败：" + ex.Message);
                return false;
            }
            Log("正在拷贝文件...");
            bool succ = false;
            int try_count = 1;
            while(try_count <= 5)
            {
                try
                {
                    File.Copy(tmp_file_name, dest_file_name, true);
                    succ = true;
                    break;
                }
                catch (System.Exception ex)
                {
                    Log("复制文件失败，重试：" + try_count.ToString() + ",exception:  " + ex.Message);
                }
                try_count++;
                Thread.Sleep(2000 * try_count);//等待1秒
            }

            Log("复制文件完成" + dest_file_name);
            return succ;
        }
        #region 服务控制
        private void StopService(string serviceName)
        {
            Log("正在停止服务："+serviceName);
            try
            {
                this._controller = new ServiceController(serviceName);
                this._controller.Stop();
                this._controller.WaitForStatus(ServiceControllerStatus.Stopped);
                this._controller.Close();
            }
            catch (System.Exception ex)
            {
                Log("停止服务：" + serviceName + ",异常： " + ex.Message);
                return;
            }
            Log("已经停止服务：" + serviceName);
        }

        private void StartService(string serviceName)
        {
            Log("正在启动服务：" + serviceName);
            try
            {
                this._controller = new ServiceController(serviceName);
                this._controller.Start();
                this._controller.WaitForStatus(ServiceControllerStatus.Running);
                this._controller.Close();
            }
            catch (System.Exception ex)
            {
                Log("启动服务：" + serviceName + ",异常： " + ex.Message);
                return;
            }
            Log("已经启动服务：" + serviceName);
        }

        private void ResetService(string serviceName)
        {
          
            StopService(serviceName);
            StartService(serviceName);
            
        }
        private bool serviceStatus(string serviceName)
        {
            bool ret = true;
            try
            {
                this._controller = new ServiceController(serviceName);
                switch(this._controller.Status)
                {
                    case ServiceControllerStatus.Stopped:
                        ret = false;
                        break;
                    case ServiceControllerStatus.StartPending:
                    case ServiceControllerStatus.Running:
                    case ServiceControllerStatus.Paused:
                    default:
                        break;
                }
                this._controller.Close();
                
            }
            catch (System.Exception ex)
            {
                //服务未安装，则自动安装服务
                Log("服务未安装，正在安装服务" + serviceName);
                doCommand("C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe " + serviceName + ".exe");
                ret = false;
            }
            return ret;
        }
        private bool monitorService(List<string> services)
        {
            foreach (string serviceName in services)
            {
                if (false == serviceStatus(serviceName))
                {
                    Log("【监控】服务停止了，准备启动" + serviceName);
                    StartService(serviceName);
                }
            }
            return true;
        }
        #endregion

    }
}
