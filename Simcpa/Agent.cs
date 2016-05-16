using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Printing;
using ISimcpa.Config;
using ISimcpa.Util;
using System.Collections;
using ISimcpa.Task;
using System.Threading;
using System.Management;
using System.ServiceProcess;
using System.IO;
using System.Net;

namespace Simcpa
{
    public partial class Agent : Form
    {
        private string __version = "1.0.0.3";//每次修改需要更新内部程序版本号

        private List<String> fPrinters = new List<String>();

        private string config_file_name = "./Config/config.xml";
        private string app_path = "";
        private Setting setting = new Setting();
        private bool is_running = false;
        SynchronizationContext m_SyncContext = null;

        public Agent()
        {
            InitializeComponent();
            m_SyncContext = SynchronizationContext.Current;
            //Control.CheckForIllegalCrossThreadCalls = false;
            app_path = Application.StartupPath;
            config_file_name = app_path + config_file_name;
        }

        private void WPSmart_Load(object sender, EventArgs e)
        {
            Logger.Error("【Simcpa Version】: " + __version);
            PrintLog("【Simcpa Version】: " + __version);
            /*
            PrinterSettings.InstalledPrinters.CopyTo(printerName, 0);
            foreach (string name in printerName)
            {
                this.textBox_sys_log.Text += name + ": " + WindowsPrinter.GetPrinterStatus(name) + "\r\n";
            }

            return;
            */

            //MessageBox.Show("pwd: " + app_path);
            //stringToPrint = this.textBox_sys_log.Text;
            /*
            PrinterSettings.InstalledPrinters.CopyTo(printerName, 0);
            printDocument1.PrinterSettings.PrinterName = printerName[4];
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            */

           
            this.timer_status.Start();
            this.label_service_running_status.Text = "服务停止，请点击“启动按钮”启动服务";


            this.listView_printer_status.Columns.Add("ID", 40, HorizontalAlignment.Left);
            this.listView_printer_status.Columns.Add("打印机名称", 150, HorizontalAlignment.Left);
            this.listView_printer_status.Columns.Add("类型", 100, HorizontalAlignment.Left);
            this.listView_printer_status.Columns.Add("任务队列", 120, HorizontalAlignment.Left);
            this.listView_printer_status.Columns.Add("失败队列", 120, HorizontalAlignment.Left);

            #region printer job list
            /*
            PrintServer myPrintServer = new PrintServer(); // Get all the printers installed on this PC
            PrintQueueCollection myPrintQueues = myPrintServer.GetPrintQueues();
            //String printQueueNames = "My Print Queues:\n\n";

            
            foreach (PrintQueue pq in myPrintQueues)
            {
                //this.textBox_sys_log.Text += pq.Name + GetPrinterStatus(pq.Name).ToString() + GetPrinterJobCount(pq.Name)+"\r\n";
                
                //PrintDocument printDocument = new PrintDocument();
                //printDocument.PrinterSettings.PrinterName = pq.Name;
                //bool online = printDocument.PrinterSettings.IsValid;
                //MessageBox.Show(pq.Name + ":" + test(pq.Name).ToString());

                switch (pq.QueueStatus)
                {
                    case PrintQueueStatus.Offline:
                        this.textBox_sys_log.Text += pq.Name + ": offline\r\n";
                        break;
                    case PrintQueueStatus.Busy:
                        this.textBox_sys_log.Text += pq.Name + ": Busy\r\n";
                        break;
                    case PrintQueueStatus.DoorOpen:
                        this.textBox_sys_log.Text += pq.Name + ": DoorOpen\r\n";
                        break;
                    case PrintQueueStatus.ManualFeed:
                        this.textBox_sys_log.Text += pq.Name + ": ManualFeed\r\n";
                        break;
                    case PrintQueueStatus.PaperJam:
                        this.textBox_sys_log.Text += pq.Name + ": PaperJam\r\n";
                        break;
                    case PrintQueueStatus.Waiting:
                        this.textBox_sys_log.Text += pq.Name + ": Waiting\r\n";
                        break;
                    case PrintQueueStatus.Printing:
                        this.textBox_sys_log.Text += pq.Name + ": Printing\r\n";
                        break;
                    case PrintQueueStatus.Paused:
                        this.textBox_sys_log.Text += pq.Name + ": Paused\r\n";
                        break;
                }
                
                printQueueNames += "\t" + pq.Name + "\n";
                this.textBox_sys_log.Text += "---------------------------------------------\r\n";
                this.textBox_sys_log.Text += pq.Name + ":" + pq.NumberOfJobs.ToString() + "\r\n";
                if (pq.NumberOfJobs == 0)
                {
                    continue;
                }
                try
                {
                    
                    var jobs = pq.GetPrintJobInfoCollection();
                    if (jobs == null)
                    {
                        break;
                    }
                    foreach (PrintSystemJobInfo job in pq.GetPrintJobInfoCollection())
                    {
                        this.textBox_sys_log.Text += job.JobName;
                    }
                }
                catch (System.Exception ex)
                {
                    return;
                }
                 
                
            }
            


            return;

        */
            #endregion

            if (true == LoadPrinterDevices())
            {
                //auto run
                //startTask();
            }
            GetServiceStatus();

        }


        private bool LoadPrinterDevices()
        {
            this.listView_printer_status.Items.Clear();
            setting = XMLParser.Read(config_file_name);
            if (setting == null)
            {
                MessageBox.Show("Load Config error !");
                return false;
            }
            else
            {
                setting.config_file_name = config_file_name;
                setting.db_file_name = app_path + "\\Data\\Simcpa.mdb";
                DeviceListCompare deviceCompare = new DeviceListCompare();
                ArrayList printerList = setting.printers;
                printerList.Sort(deviceCompare);
                foreach (DeviceInfo item in printerList)
                {
                    int count = GetPrinterJobCount(item.addr);
                    ListViewItem[] lvs = new ListViewItem[1];
                    lvs[0] = new ListViewItem(new string[] { item.id, item.name, item.type, count.ToString(), "0" });
                    this.listView_printer_status.Items.AddRange(lvs);
                }
            }
            this.listView_printer_status.Sort();
            return true;

        }
        private void button_choose_Click(object sender, EventArgs e)
        {

        }

        private void RPrintLog(object msg)
        {
            if (this.textBox_sys_log.Lines.Count() > 1000)
            {
                this.textBox_sys_log.Text = "";
            }
            DateTime dt = DateTime.Now;
            this.textBox_sys_log.Text += "[" + dt.ToLocalTime().ToString() + "]" + msg.ToString() + "\r\n";
            this.textBox_sys_log.SelectionStart = this.textBox_sys_log.Text.Length;
            this.textBox_sys_log.ScrollToCaret();

        }
        private void PrintLog(string msg)
        {
            m_SyncContext.Post(RPrintLog, msg);
            //this.textBox_sys_log.Invoke(null, new object[] { msg });

        }
        private void ROrderCounter(object order_id)
        {
            int count = int.Parse(this.label_order_counter.Text);
            count++;
            this.label_order_counter.Text = count.ToString();

            this.label_curr_order_id.Text = order_id.ToString();
        }
        private void OrderCounter(string order_id)
        {
            m_SyncContext.Post(ROrderCounter, order_id);
        }
        class JobStatus
        {
            public int printer_id;
            public bool status;
        }

        private void PrinterJobStatistics(int printer_id, bool succ = true)
        {
            JobStatus __obj = new JobStatus();
            __obj.printer_id = printer_id;
            __obj.status = succ;
            m_SyncContext.Post(RPrinterJobStatistics, __obj);
        }
        private void RPrinterJobStatistics(object obj)
        {
            JobStatus __obj = (JobStatus)obj;

            string str_printer_id = __obj.printer_id.ToString();
            //bool found = false;
            foreach (ListViewItem item in this.listView_printer_status.Items)
            {

                if (item.SubItems[0].Text == str_printer_id)
                {
                    if (true == __obj.status)
                    {
                        int count = int.Parse(item.SubItems[3].Text);
                        count++;
                        item.SubItems[3].Text = count.ToString();
                    }
                    else
                    {
                        int count = int.Parse(item.SubItems[4].Text);
                        count++;
                        item.SubItems[4].Text = count.ToString();
                    }

                    //found = true;
                    break;
                }
            }
        }



        private bool isRunning()
        {
            return is_running;
        }
        TaskWorker task = new TaskWorker();
        private void ThreadStartTask()
        {
            task.PrintLog = new TaskWorker.DelegatePrintLog(PrintLog);
            task.PrinterJobStatistics = new TaskWorker.DelegatePrinterJobStatistic(PrinterJobStatistics);
            task.OrderCounter = new TaskWorker.DelegateOrderCounter(OrderCounter);
            task.isRunning = new TaskWorker.DelegateIsRunning(isRunning);
            task.Run(setting);
            
        }
        private void startTask()
        {
            this.label_service_running_status.Text = "服务运行中。。。";
            is_running = true;
            Thread thread_task = new Thread(new ThreadStart(ThreadStartTask));
            thread_task.Start();

            this.button_start.Text = "停止";
        }
        private void stopTask()
        {
            is_running = false;
            task.Stop();
            this.button_start.Text = "启动测试";
            this.label_service_running_status.Text = "服务停止，请点击“启动按钮”启动服务";
            PrintLog("正在终止任务...");
        }
        private void button_start_Click(object sender, EventArgs e)
        {
            
            if (this.button_start.Text == "启动测试")
            {

                if (MessageBox.Show("这是测试服务，确定启动测试服务？", "趣吃饭-确认窗口",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
                {
                    return;
                }
                startTask();
            }
            else
            {
                stopTask();
            }
            
        }
        void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;
            //e.MarginBounds.Width=42;
            e.Graphics.MeasureString(stringToPrint, this.Font, e.MarginBounds.Size, StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);
            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black, e.MarginBounds, StringFormat.GenericTypographic);
            Image img = Image.FromFile(@"E:\Andrew\打印机\Projects\Quchifan\Simcpa\Look.jpg");
            e.Graphics.DrawImage(img, 0, 0);
            stringToPrint = stringToPrint.Substring(charactersOnPage);
            e.HasMorePages = (stringToPrint.Length > 0);
        }


        string stringToPrint = string.Empty;
        PrintDocument printDocument1 = new PrintDocument();
        string[] printerName = new string[PrinterSettings.InstalledPrinters.Count];
        private bool Download(string url, string localFile)
        { 
            try
            {
                 WebClient __web_client = new WebClient();
                __web_client.DownloadFile(url, localFile);
            }
            catch (System.Exception ex)
            {
                PrintLog("下载文件(" + url + ")失败：" + ex.Message);
                return false;
            }
            return true;
        }

        private void button_more_Click(object sender, EventArgs e)
        {
            //Download("http://182.92.76.190/Simcpa_Windows_Service.exe", "./Simcpa_Windows_Service.exe");
            //return;
            if (is_running == true)
            {
                stopTask();
            }

            /*
            printDocument1.DocumentName = "hello test";

            this.printDocument1.DefaultPageSettings.Margins.Bottom = 0;
            this.printDocument1.DefaultPageSettings.Margins.Top = 0;
            this.printDocument1.DefaultPageSettings.Margins.Left = 0;
            this.printDocument1.DefaultPageSettings.Margins.Right = 0;
            this.printDocument1.Print();
            */

            /*
              Util.GetImage thumb = new Util.GetImage(html, 800, 900, 800, 900);
            System.Drawing.Bitmap bp = thumb.GetBitmap();
            string path = Application.StartupPath + "\\aaa";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            string name = orderCode + "(" + DateTime.Now.ToString("yyyyMMdd") + ").jpg";
            imgPath = path + "\\" + name;
            bp.Save(imgPath);// GetImage(imgPath);
             */

            ServerConfig cfg = new ServerConfig(config_file_name);
            cfg.ShowDialog();
            PrintLog("配置完成更新，重新加载打印机列表中...");
            LoadPrinterDevices();

            PrintLog("【提醒】服务配置如果变化，请手动重启服务！");
            MessageBox.Show("【提醒】服务配置如果变化，请手动重启服务！");
          
        }
        private void button_setting_printer_Click(object sender, EventArgs e)
        {
            if (is_running == true)
            {
                stopTask();
            }
           
            PrinterConfig cfg = new PrinterConfig(config_file_name);
            cfg.ShowDialog();

            PrintLog("重新加载打印机列表中...");
            LoadPrinterDevices();

            PrintLog("【提醒】打印配置如果变化，请手动重启服务！");
            MessageBox.Show("【提醒】打印配置变化，请手动重启服务！");

        }

        private void notifyIcon_system_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.notifyIcon_system.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void WPSmart_Resize(object sender, EventArgs e)
        {

        }

        private void WPSmart_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) //判断是否最小化
            {
                this.ShowInTaskbar = false; //不显示在系统任务栏
                this.notifyIcon_system.Visible = true; //托盘图标可见
            }
        }

        private void WPSmart_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 取消关闭窗体
            //e.Cancel = true;
            // 将窗体变为最小化
            //this.WindowState = FormWindowState.Minimized;
            this.timer_status.Stop();
            is_running = false;
        }
        private void ExitMainForm()
        {
            if (MessageBox.Show("您确定要退出化验数据接收程序吗？", "确认退出",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                this.notifyIcon_system.Visible = false;
                this.Close();
                this.Dispose();
                Application.Exit();
            }
        }

        private void HideMainForm()
        {
            this.Hide();
        }

        private void ShowMainForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void contextMenuStrip_notify_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "退出")
            {
                ExitMainForm();
            }
            else
            {

            }
        }

        private void timer_status_Tick(object sender, EventArgs e)
        {
            //TODO
        }

        private void button_orders_list_Click(object sender, EventArgs e)
        {
//             byte[] test= new byte[4];
//             test[0] = 0x00;
//             test[1] = 0x00;
//             test[2] = 0x00;
//             test[3] = 0x22;
//             
//             int s = System.BitConverter.ToInt32(test.Reverse().ToArray(), 0); //test[0];
//             MessageBox.Show(s.ToString());
//             return;
            if (is_running == true)
            {
                stopTask();
            }
            JobWorker worker = new JobWorker(setting.db_file_name, setting);
            worker.Show();
        }

        
        #region 服务控制
        private ServiceController _controller;
        private void StopService(string serviceName)
        {
            try
            {
                this._controller = new ServiceController(serviceName);
                this._controller.Stop();
                this._controller.WaitForStatus(ServiceControllerStatus.Stopped);
                this._controller.Close();
            }
            catch (System.Exception ex)
            {
                PrintLog("停止服务异常：" + ex.Message);
            }
            
        }

        private void StartService(string serviceName)
        {
            try
            {
                this._controller = new ServiceController(serviceName);
                this._controller.Start();
                this._controller.WaitForStatus(ServiceControllerStatus.Running);
                this._controller.Close();
            }
            catch (System.Exception ex)
            {
                PrintLog("启动服务异常：" + ex.Message);
            }
            
        }

        private void ResetService(string serviceName)
        {
            StopService(serviceName);
            StartService(serviceName);
            
        }
        #endregion

        private void button_start_service_Click(object sender, EventArgs e)
        {
            StartService("SimcpaService");
            MessageBox.Show("服务启动完成");
            GetServiceStatus();
        }

        private void button_restart_service_Click(object sender, EventArgs e)
        {
            ResetService("SimcpaService");
            MessageBox.Show("服务重启完成");
            GetServiceStatus();
        }

        private void button_stop_service_Click(object sender, EventArgs e)
        {
            StopService("SimcpaService");
            MessageBox.Show("服务停止完成");
            GetServiceStatus();
        }
        private void doCommand(string cmd)
        {
            try
            {
                PrintLog("执行命令：" + cmd);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine("cd " + app_path);
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.WriteLine("exit");
                p.StandardInput.AutoFlush = true;
                string output = p.StandardOutput.ReadToEnd();
                PrintLog(output);
                //p.WaitForExit();
                p.Close();
            }
            catch (System.Exception ex)
            {
                PrintLog("执行命令出错： " + ex.Message);
            }
            

        }
        private void button_install_Click(object sender, EventArgs e)
        {
            doCommand("C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe SimcpaService.exe");
            MessageBox.Show("安装命令完成，请在日志中查看执行结果");
            GetServiceStatus();
        }

        private void button_uninstall_Click(object sender, EventArgs e)
        {
            doCommand("C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe /u SimcpaService.exe");
            MessageBox.Show("卸载命令完成，请在日志中查看执行结果");
            GetServiceStatus();
        }
      
        private void GetServiceStatus()
        {

            try
            {
                this._controller = new ServiceController("SimcpaService");
                ServiceControllerStatus status = this._controller.Status;
                this.textBox_sys_log.Text += "服务（SimcpaService）状态：" + status.ToString() + "\r\n";
                this._controller.Close();

                switch(status)
                {
                    case ServiceControllerStatus.PausePending:
                    case ServiceControllerStatus.Paused:
                        this.button_restart_service.Enabled = true;
                        this.button_start_service.Enabled = true;
                        this.button_stop_service.Enabled = false;
                        break;
                    case ServiceControllerStatus.Running:
                    case ServiceControllerStatus.StartPending:
                        this.button_restart_service.Enabled = true;
                        this.button_start_service.Enabled = false;
                        this.button_stop_service.Enabled = true;
                        break;
                    case ServiceControllerStatus.StopPending:
                    case ServiceControllerStatus.Stopped:
                        this.button_restart_service.Enabled = true;
                        this.button_start_service.Enabled = true;
                        this.button_stop_service.Enabled = false;
                        break;
                    default:
                        break;

                }
                this.button_uninstall.Enabled = true;
                this.button_install.Enabled = false;
                this.button_onekey_install_run_simcpa.Enabled = false;
            }
            catch (System.Exception ex)
            {
                this.textBox_sys_log.Text += "服务（SimcpaService）状态：未找到（未安装），可点击“安装”按钮进行服务安装\r\n";
                this.button_restart_service.Enabled = false;
                this.button_start_service.Enabled = false;
                this.button_stop_service.Enabled = false;
                this.button_uninstall.Enabled = false;
                this.button_install.Enabled = true;
                this.button_onekey_install_run_simcpa.Enabled = true;
            }
            
            try
            {
                this._controller = new ServiceController("SimcpaAutoUpdateService");
                ServiceControllerStatus status = this._controller.Status;
                this.textBox_sys_log.Text += "服务（SimcpaAutoUpdateService）状态：" + status.ToString() + "\r\n";
                this._controller.Close();
                this.button_onkey_install_run_update.Enabled = false;
                this.button_onekey_uninstall_update.Enabled = true;
                this.button_restart_autoupdate.Enabled = true;
            }
            catch (System.Exception ex)
            {
                this.textBox_sys_log.Text += "服务（SimcpaAutoUpdateService）状态：未找到（未安装）\r\n";
                this.button_onkey_install_run_update.Enabled = true;
                this.button_onekey_uninstall_update.Enabled = false;
                this.button_restart_autoupdate.Enabled = false;
            }
            
        }
        private int GetPrinterJobCount(string printerName)
        {
            // Get all the printers installed on this PC
            PrintServer myPrintServer = new PrintServer();
            PrintQueueCollection myPrintQueues = myPrintServer.GetPrintQueues();

            foreach (PrintQueue pq in myPrintQueues)
            {
                if (printerName == pq.Name)
                {
                    return pq.NumberOfJobs;
                }


            }
            return 0;
        }

        private void button_onekey_uninstall_update_Click(object sender, EventArgs e)
        {
            
            doCommand("C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe /u SimcpaAutoUpdate.exe");
            MessageBox.Show("卸载命令完成，请在日志中查看执行结果");
            GetServiceStatus();
        }

        private void button_onkey_install_run_update_Click(object sender, EventArgs e)
        {
            doCommand("C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe SimcpaAutoUpdate.exe");
            MessageBox.Show("安装命令完成，正在启动服务");
            StartService("SimcpaAutoUpdateService");
            MessageBox.Show("启动服务命令完成，请在日志中查看执行结果");
            GetServiceStatus();
        }

        private void button_restart_autoupdate_Click(object sender, EventArgs e)
        {
            ResetService("SimcpaAutoUpdateService");
            MessageBox.Show("服务重启完成");
            GetServiceStatus();
        }

        private void button_onekey_install_run_simcpa_Click(object sender, EventArgs e)
        {
            doCommand("C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe SimcpaService.exe");
            MessageBox.Show("安装命令完成，正在启动服务");
            StartService("SimcpaService");
            MessageBox.Show("启动服务命令完成，请在日志中查看执行结果");
            GetServiceStatus();
        }

    }
}
