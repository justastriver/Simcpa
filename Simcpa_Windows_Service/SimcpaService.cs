using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using ISimcpa;
using ISimcpa.Util;
using ISimcpa.Task;
using System.Threading;

namespace Simcpa_Windows_Service
{
    public partial class SimcpaService : ServiceBase
    {
        private string __version = "1.0.0.1";//每次修改需要更新内部程序版本号

        private string __config_file_name = "Config\\config.xml";
        private string __db_file_name = "Data\\Simcpa.mdb";
        private  ISimcpaWorker __simcpa_job = null;
        private string __app_path = "";

        public SimcpaService()
        {
            InitializeComponent();
        }
        private void StartJobThread()
        {
            __app_path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //AppDomain.CurrentDomain.BaseDirectory;
            __simcpa_job = new ISimcpaWorker(__app_path + __config_file_name, __app_path + __db_file_name);
            Logger.Debug("start service, app path: " + __app_path);
            __simcpa_job.Initialize();
            __simcpa_job.Start();

        }
        protected override void OnStart(string[] args)
        {
            Logger.Error("【SimcpaWindowsService Version】: " + __version);
            Thread job_thread = new Thread(new ThreadStart(StartJobThread));
            job_thread.Start();
            
        }

        protected override void OnStop()
        {
            __simcpa_job.Stop();
            Logger.Error("stop service");
        }
    }
}
