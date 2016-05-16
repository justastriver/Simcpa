using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ISimcpa.Task;
using ISimcpa.Util;
using ISimcpa.Config;

namespace ISimcpa
{
    public class ISimcpaWorker
    {
        private string _config_file_name = "";
        private TaskWorker __worker = new TaskWorker();
        private Setting __setting = new Setting();
        private bool __task_working_status = false;
        private string __last_error_message;
        private string _db_file_name;

        public string LastError
        {
            get { return __last_error_message; }
            set { __last_error_message = value; }
        }

        public ISimcpaWorker(string config_file_name, string db_file_name)
        {
            this._config_file_name = config_file_name;
            this._db_file_name = db_file_name;
        }
        private void PrintLog(string msg)
        {
            Logger.Debug(msg);
        }
        public bool Initialize()
        {
            //load configure
            __setting = XMLParser.Read(_config_file_name);
            if (__setting == null)
            {
                Logger.Error("加载配置失败：" + _config_file_name);
                return false;
            }
            __setting.config_file_name = _config_file_name;
            __setting.db_file_name = this._db_file_name;
            Logger.Debug("加载配置完成： " + _config_file_name);
            return true;
        }
        public bool Start()
        {
            if (__setting == null)
            {
                Logger.Error("加载配置失败：" + _config_file_name);
                return false;
            }

            
            if (true == __task_working_status)
            {
                Stop();
                Thread.Sleep(1000);
            }
            try
            {
                __task_working_status = true;
                __worker.PrintLog = new TaskWorker.DelegatePrintLog(PrintLog);
                __worker.PrinterJobStatistics = new TaskWorker.DelegatePrinterJobStatistic(PrinterJobStatistic);
                __worker.OrderCounter = new TaskWorker.DelegateOrderCounter(OrderCounter);
                __worker.isRunning = new TaskWorker.DelegateIsRunning(isRunning);
                __worker.Run(__setting);
            }
            catch (System.Exception ex)
            {
                __last_error_message = ex.Message;
                return false;
            }
           
            return true;
        }
        private void PrinterJobStatistic(int printer_id, bool succ = true)
        {

        }
        private void OrderCounter(string current_order_id)
        {
            
        }
        private bool isRunning()
        {
            return __task_working_status;
        }
        public bool Stop()
        {
            if (true == __task_working_status)//double check
            {
                __task_working_status = false;
                __worker.Stop();
            }
            return true;
        }
    }
}
