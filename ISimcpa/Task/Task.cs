using System;
using System.Collections.Generic;
using System.Text;
using ISimcpa.Net;
using System.Threading;
using System.IO;
using ISimcpa.Util;
using System.Data;
using System.Collections;
using winprint;
using ISimcpa.Config;
using System.Management;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Printing;
using ISimcpa.DB;
using System.Net;
using System.Windows.Forms;

namespace ISimcpa.Task
{
    public class TaskWorker
    {

        #region 变量定义
        private string __version = "1.0.0.3";//每次修改需要更新内部程序版本号
        private Client client = null;
        Dictionary<int/*printer id*/, DeviceInfo> printers = new Dictionary<int, DeviceInfo>();
        LoadPOSDll PosPrint = new LoadPOSDll();
        private IntPtr Gp_IntPtr;
        bool is_connected = false;
        Setting setting = new Setting();
        private libUsbContorl.UsbOperation NewUsb = new libUsbContorl.UsbOperation();

        Dictionary<int/*printer id*/, int/*job count*/> printer_job_statistic = new Dictionary<int/*printer id*/, int/*job count*/>();
        public delegate void DelegatePrintLog(string msg);
        public DelegatePrintLog PrintLog;

        public delegate void DelegatePrinterJobStatistic(int printer_id, bool succ = true);
        public DelegatePrinterJobStatistic PrinterJobStatistics;
        
        public delegate void DelegateOrderCounter(string current_order_id);
        public DelegateOrderCounter OrderCounter;

        public delegate bool DelegateIsRunning();
        public DelegateIsRunning isRunning;
        private string lastError = "";

        private Dictionary<string/*orderid + printid*/, bool/*is printed*/> orders_had_printed = new Dictionary<string/*orderid + printid*/, bool/*is printed*/>();
        
        #endregion

        #region 远程更新配置
        /// <summary>
        /// 远程更新配置
        /// </summary>
        /// <param name="syscfg"></param>
        /// <returns></returns>
        public bool UpdateConfigure(SystemConfig syscfg)
        {
            try
            {
                XMLParser.SaveSystemConfig(syscfg, setting.config_file_name);
            }
            catch (System.Exception ex)
            {
                Logger.Debug("update configure failed !" + ex.Message);
                return false;
            }
          
            return true;
        }
        #endregion

        #region 停止任务

        /// <summary>
        /// 停止任务
        /// </summary>
        public void Stop()
        {
           
            try
            {
                is_connected = false;
                PrintLog("任务线程终止完成！");
            }
            catch (System.Exception ex)
            {
                PrintLog("stop server exception: " + ex.Message);
            }
            try
            {
                if (null != client)
                {
                    client.Close();
                    client = null;
                }
            }
            catch (System.Exception ex)
            {
                PrintLog("stop server2 exception: " + ex.Message);
            }
            
        }
        #endregion

        #region 任务启动入口
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="set">系统设置(服务器IP,服务器端口号,打印机列表等）</param>
        /// <returns></returns>
        public bool Run(Setting set)
        {
            //MessageBox.Show("hello");
            PrintLog("【ISimcpa Version】: " + __version);
            PrintLog("任务启动，正在创建与服务器的连接，请稍后...");
            setting = set;
            ArrayList printerList = setting.printers;
            foreach (DeviceInfo printer in printerList)
            {
                try
                {
                    int printer_id = int.Parse(printer.id);
                    printers.Add(printer_id, printer);
                    printer_job_statistic.Add(printer_id, 0);
                }
                catch (System.Exception ex)
                {
                    Logger.Error("parse printer_id failed," + printer.id +", exc: " + ex.Message);
                }
            }
            //尝试创建数据库
            Storage.CreateDB(set.db_file_name);
            Storage.CreateTable(set.db_file_name);

            //启动心跳
            Thread thread_heartbeat = new Thread(new ThreadStart(StartHeartbeatThread));
             thread_heartbeat.Start();
            //启动作业操作线程
            Thread thread_handle_task = new Thread(new ThreadStart(threadHandleTask));
            thread_handle_task.Start();

            if (false == tryConnect())
            {
                Logger.Debug("trying to connect...");
                return false;
            }
            PrintLog("任务启动成功");
            Logger.Debug("Load ok !");
            return true;
        }
        #endregion 

        #region 同步打印机配置到服务端(每次与服务器连接成功时执行)
        /// <summary>
        /// 同步打印机配置信息
        /// </summary>
        void SyncPrinterConfig()
        {
            try
            {

                if (is_connected == true)
                {
                    PrintLog("正在同步打印机配置信息");
                    Logger.Debug("正在同步打印机配置信息");
                    winprint.Header resheader = new winprint.Header
                    {
                        cmd = winprint.Header.Command.PRINTERINFO,
                        restaurant_id = int.Parse(setting.system.restaurant_id),
                        seq = 12345678,
                    };
                    winprint.Data data = new winprint.Data
                    {

                    };
                    //List<winprint.Printer> printer_list = new List<winprint.Printer>();
                    foreach (DeviceInfo printer_cfg_item in setting.printers)
                    {
                        winprint.Printer.InterfaceType itype = new winprint.Printer.InterfaceType();
                        if (printer_cfg_item.type == "ethernet")
                        {
                            itype = Printer.InterfaceType.ETH;
                        }
                        if (printer_cfg_item.type == "wifi")
                        {
                            itype = Printer.InterfaceType.WIFI;
                        }
                        if (printer_cfg_item.type == "usb")
                        {
                            itype = Printer.InterfaceType.USB;
                        }
                        winprint.Printer printer = new winprint.Printer
                        {
                            type = winprint.Printer.PrinterType.TYPE58,
                            id = int.Parse(printer_cfg_item.id),
                            status = GetPrinterStatus(printer_cfg_item.addr) == true ?
                                    winprint.Printer.Status.ONLINE : winprint.Printer.Status.OFFLINE,
                            itype = itype,
                            desc = printer_cfg_item.desc,
                        };
                        data.printers.Add(printer);

                    }
                    winprint.Message msg = new winprint.Message
                    {
                        header = resheader,
                        data = data,
                    };

                    if (false == send2Server(msg))
                    {
                        PrintLog("同步打印机配置信息失败，发送到服务器数据失败!");
                        Logger.Debug("同步打印机配置信息完成，发送到服务器数据失败!");
                    }
                    else
                    {
                        PrintLog("同步打印机配置信息完成!");
                        Logger.Debug("同步打印机配置信息完成");
                    }

                    /*
                    string a = "";
                    for (int i = 0; i < len; ++i)
                    {
                        a += "0x" + send_buffer[i].ToString("X2") + ",";
                    }
                    */

                }
            }
            catch (System.Exception ex)
            {
                Logger.Error("Sync Printer Exception: " + ex.Message);
            }
            
        }
        #endregion

        #region 心跳处理
        /// <summary>
        /// 心跳
        /// </summary>
        void StartHeartbeatThread()
        {
            int sequence = 0;
            int print_count = 0;
            while (isRunning() == true)
            {
                if (is_connected == false)
                {
                    Thread.Sleep(setting.system.heartbeat_interval);
                    continue;
                }
                try
                {

                    winprint.Header resheader = new winprint.Header
                    {
                        cmd = winprint.Header.Command.HEARTBEAT,
                        restaurant_id = int.Parse(setting.system.restaurant_id),
                        seq = sequence,

                    };

                    winprint.Data heartbeat_data = new winprint.Data
                    {

                    };
                    foreach (DeviceInfo printer in setting.printers)
                    {
                        winprint.HeartBeat heartbeat = new winprint.HeartBeat
                        {
                            printer_id = int.Parse(printer.id),
                            status = GetPrinterStatus(printer.addr) == true ?
                                    winprint.HeartBeat.Status.ONLINE : winprint.HeartBeat.Status.OFFLINE,
                            queue = GetPrinterJobCount(printer.addr)
                        };
                        heartbeat_data.heartbeat.Add(heartbeat);
                    }
                    winprint.Message msg = new winprint.Message
                    {
                        header = resheader,
                        data = heartbeat_data
                    };
                    send2Server(msg);
                    sequence++;
                    if (sequence >= 100000)
                    {
                        sequence = 0;
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Error("发送心跳数据失败，ex: " + ex.Message);
                }
                Thread.Sleep(setting.system.heartbeat_interval);

                print_count++;
                if (print_count >= 120) //10分钟打印一次
                {
                    print_count = 0;
                    Logger.Error("处理与服务器连接的心跳！");
                }
            }

            //结束客户端服务
            //Stop();
        }
        #endregion 

        #region 发送数据（失败重试）
        /// <summary>
        /// 发送数据到服务器，具有5次重试
        /// </summary>
        /// <param name="msg">数据内容</param>
        /// <returns></returns>
        private bool send2Server(winprint.Message msg)
        {
           /* return true;*/
            bool ret = false;
            int try_count = 0;
            while (try_count < 20 && true == isRunning())
            {
                try
                {
                    MemoryStream resp_stream = new MemoryStream();
                    ProtoBuf.Serializer.Serialize<winprint.Message>(resp_stream, msg);
                    int len = (int)resp_stream.Length;
                    List<byte> send_data = new List<byte>();
                    byte[] len_data = new byte[4];
                    len_data = System.BitConverter.GetBytes(len);
//                     if (BitConverter.IsLittleEndian)
//                     {
//                         Array.Reverse(len_data);
//                     }
                    send_data.AddRange(len_data);
                    send_data.AddRange(resp_stream.GetBuffer());

                    if (true == client.Write(send_data.ToArray(), len + 4)) 
                    {
                        ret = true;
                        break;
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.Error("send2Server 出错重试次数 " + try_count.ToString() + " ： " + ex.Message);
                    ret = false;
                }
                try_count++;
                Thread.Sleep(1000);
            }
            return ret;
        }
        #endregion

        #region 处理打印机状态变化TODO
        /// <summary>
        /// 处理打印机状态变化
        /// </summary>
        void HandlePrinterStatus()
        {

        }
        #endregion

        #region 处理服务器连接
        /// <summary>
        /// 处理连接，断线后重连等机制, 重试10m一次
        /// </summary>
        /// <returns></returns>
        private bool tryConnect()
        {
            while (is_connected == false && isRunning() == true)
            {
                //PrintLog("开始尝试连接服务器");
                try
                {
                    //先关闭网络，再尝试连接
                    
                    if (null == client)
                    {
                        IPAddress ip;
                        if (IPAddress.TryParse(setting.system.server_ip, out ip))
                        {
                            //right
                            PrintLog("使用IP解析方式：" + setting.system.server_ip);
                        }
                        else
                        {
                            //is host name 
                            IPHostEntry hostinfo = Dns.GetHostEntry(setting.system.server_ip);
                            ip = hostinfo.AddressList[0];
                            PrintLog("使用域名解析方式：" + setting.system.server_ip);
                        }
                        client = new Client(ip.ToString(), int.Parse(setting.system.server_port));
                        client.OnReceive += new ReceiveEventHandler(client_OnReceive);
                        client.OnServerClosed += new SocketEventHandler(client_OnServerClosed);
                    }
                    else
                    {
                        try
                        {
                            client.Close();
                            client = null;
                        }
                        catch (System.Exception ex)
                        {
                            Logger.Error("on server closed , exception: " + ex.Message);
                        }
                    }
                    client.Connect();
                    is_connected = true;
                    //try to sync printer configuration
                    PrintLog("连接服务器成功，正在同步本地打印机数据...");
                    SyncPrinterConfig();
                    
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Warn("连接服务器失败，异常信息: " + ex.Message);
                    PrintLog("连接服务器失败：" + ex.Message);
                    is_connected = false;
                }
                Thread.Sleep(10000);
                
            }
            
            return true;
        }
        #endregion

        #region 处理具体打印任务（打印格式等）
        
        class PrinterFormat
        {
            public static uint CENTER_58MM = 90;
            public static uint CENTER_80MM = 140;
        }
        private static string PLUS_LINE_58MM = "-----------------------------";
        private static string PLUS_LINE_58MM_PRINT_NUM = "------------";
        private static string PLUS_LINE_58MM_TITLE = "菜品                    数量";
        private static string PLUS_LINE_80MM = "--------------------------------------------";
        private static string PLUS_LINE_80MM_PRINT_NUM = "-------------------";
        private static string PLUS_LINE_80MM_TITLE = "菜品                                 数量";
        private int PAPER_WIDTH_58MM = 440;
        private int PAPER_WIDTH_80MM = 620;
        // Font Type Size (W * H) 
        // Standard ASCII 12 * 24 / 13 * 24 
        // Compressed ASCII 9 * 17 
        // 标准宋体 24 * 24 

        /// <summary>
        /// 处理打印机打印任务
        /// </summary>
        /// <param name="receipt">小票内容</param>
        /// <returns></returns>
        private bool HandlePrint(winprint.Receipt receipt, PrintWidth width, int page_num)
        {
            try
            {

                LoadPOSDll.POS_SetLineSpacing(30);

                #region 执行字段校验
                //check kernel fields
                if (receipt.head == null ||
                    receipt.head.title == null ||
                    receipt.body == null ||
                    receipt.body.foods == null ||
                    receipt.body.foods.Count == 0 ||
                    receipt.order_id <= 0 ||
                    receipt.tail == null)
                {
                    Logger.Debug("关键字段为空，不执行打印");
                    lastError = "miss some import fields";
                    return false;
                }
                #endregion

                #region 打印头部信息
                int left = 0;

                if (width == PrintWidth.MM58)
                {
                    if (page_num > 0)
                    {
                        LoadPOSDll.POS_S_TextOut(PLUS_LINE_58MM_PRINT_NUM + "第 " + page_num + " 联" + PLUS_LINE_58MM_PRINT_NUM,
                            0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                        LoadPOSDll.POS_FeedLines(4);
                    }

                    left = (PAPER_WIDTH_58MM - receipt.head.title.Length * 48) / 2;
                    if (left <0)
                    {
                        left = 10;
                    }

                    LoadPOSDll.POS_S_TextOut(receipt.head.title, (uint)left,
                                                        2, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                    
                    if (receipt.head.subtitle != null && receipt.head.subtitle != "")
                    {
                        LoadPOSDll.POS_FeedLines(4);
                        left = (PAPER_WIDTH_58MM - receipt.head.subtitle.Length * 30) / 2;
                        if (left < 0)
                        {
                            left = 10;
                        }
                        LoadPOSDll.POS_S_TextOut(receipt.head.subtitle, (uint)left, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                    }
                }
                else
                {
                    if (page_num > 0)
                    {
                        LoadPOSDll.POS_S_TextOut(PLUS_LINE_80MM_PRINT_NUM + "第 " + page_num + " 联" + PLUS_LINE_80MM_PRINT_NUM,
                            0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                        LoadPOSDll.POS_FeedLines(2);
                    }
                    left = (PAPER_WIDTH_80MM - receipt.head.title.Length * 48) / 2;
                    if (left < 0)
                    {
                        left = 10;
                    }
                    LoadPOSDll.POS_S_TextOut(receipt.head.title, PrinterFormat.CENTER_80MM,
                                                        2, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                    if (receipt.head.subtitle != null && receipt.head.subtitle != "")
                    {
                        LoadPOSDll.POS_FeedLines(4);
                        left = (PAPER_WIDTH_80MM - receipt.head.subtitle.Length * 24) / 2;
                        if (left < 0)
                        {
                            left = 10;
                        }
                        LoadPOSDll.POS_S_TextOut(receipt.head.subtitle, (uint)left, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                    }
                }

                LoadPOSDll.POS_FeedLines(5);

                #region 正餐信息头
                if (receipt.head.bookhead !=null)
                {
                    if (receipt.head.bookhead.arrival_at != null && receipt.head.bookhead.arrival_at != "")
                    {
                        LoadPOSDll.POS_S_TextOut("到店时间: ", 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_S_TextOut(receipt.head.bookhead.arrival_at, 120, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_FeedLines(3);
                    }
                    if (receipt.head.bookhead.people != null && receipt.head.bookhead.people != "")
                    {
                        LoadPOSDll.POS_S_TextOut("就餐人数: ", 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_S_TextOut(receipt.head.bookhead.people, 120, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_FeedLines(3);
                    }
                    if (receipt.head.bookhead.phone != null && receipt.head.bookhead.phone != "")
                    {
                        LoadPOSDll.POS_S_TextOut("顾客电话: ", 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_S_TextOut(receipt.head.bookhead.phone, 120, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_FeedLines(3);
                    }
                }
                #endregion

                //LoadPOSDll.POS_SetLineSpacing(10);
                if (receipt.head.table_card_no != null && receipt.head.table_card_no != "")
                {

                    LoadPOSDll.POS_S_TextOut("桌牌号: ", 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_S_TextOut(receipt.head.table_card_no, 90, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);

                }
                if (receipt.head.way != null && receipt.head.way != "")
                {
                    LoadPOSDll.POS_S_TextOut("就餐方式: ", 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_S_TextOut(receipt.head.way, 120, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);
                }

                if (receipt.head.password != null && receipt.head.password != "")
                {
                    LoadPOSDll.POS_S_TextOut("密码: ", 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_S_TextOut(receipt.head.password, 60, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);
                }
                if (receipt.head.note != null && receipt.head.note != "")
                {
                    LoadPOSDll.POS_S_TextOut("备注: ", 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_S_TextOut(receipt.head.note, 60, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);
                }

                foreach (string moreitem in receipt.head.moreitems)
                {
                    LoadPOSDll.POS_S_TextOut(moreitem, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                #endregion

                #region 打印菜品信息
                if (width == PrintWidth.MM58)
                {
                    LoadPOSDll.POS_S_TextOut(PLUS_LINE_58MM, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(1);
                    LoadPOSDll.POS_S_TextOut(PLUS_LINE_58MM_TITLE, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }
                else
                {
                    LoadPOSDll.POS_S_TextOut(PLUS_LINE_80MM, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(1);
                    LoadPOSDll.POS_S_TextOut(PLUS_LINE_80MM_TITLE, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }

                LoadPOSDll.POS_FeedLines(3);
                uint width_of_food = 300;
                if (width == PrintWidth.MM80)
                {
                    width_of_food = 500;
                }
                foreach (winprint.Foods food in receipt.body.foods)
                {
                    if(food.name != null && food.name != "" && food.num > 0) 
                    {
                        LoadPOSDll.POS_S_TextOut(food.name, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_BOLD);
                        LoadPOSDll.POS_S_TextOut(food.num.ToString(), width_of_food, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_BOLD);
                        LoadPOSDll.POS_FeedLine();
                    }
                    
                    foreach (winprint.SlaveFoods slave_food in food.slavefoods)
                    {
                        if(slave_food.name != null && slave_food.name != "" && slave_food.num > 0) 
                        {
                            LoadPOSDll.POS_S_TextOut("  " + slave_food.name,
                                                    0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                            LoadPOSDll.POS_S_TextOut(slave_food.num.ToString(),
                                                    width_of_food, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                            LoadPOSDll.POS_FeedLines(2);
                        }
                    }
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (width == PrintWidth.MM58)
                {
                    LoadPOSDll.POS_S_TextOut(PLUS_LINE_58MM, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }
                else
                {
                    LoadPOSDll.POS_S_TextOut(PLUS_LINE_80MM, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }

                #endregion

                #region 打印尾部信息
                LoadPOSDll.POS_FeedLines(2);
                if (receipt.tail.orderno != null && receipt.tail.orderno != "")
                {
                    LoadPOSDll.POS_S_TextOut("订单号:" + receipt.tail.orderno, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (receipt.tail.restaurant_name != null && receipt.tail.restaurant_name != "")
                {
                    LoadPOSDll.POS_S_TextOut("餐厅名称: " + receipt.tail.restaurant_name, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (receipt.tail.phone != null && receipt.tail.phone != "")
                {
                     LoadPOSDll.POS_S_TextOut("联系电话: " + receipt.tail.phone, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (receipt.tail.time != null && receipt.tail.time != "")
                {
                    LoadPOSDll.POS_S_TextOut("日期: " + receipt.tail.time, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(10);
                }
                
                LoadPOSDll.POS_CutPaper(LoadPOSDll.POS_CUT_MODE_FULL, 0);
                LoadPOSDll.POS_Reset();//清理缓存
                Logger.Debug("really print over: " + receipt.tail.orderno);
                Thread.Sleep(200);//控制打印节奏，防止不必要的异常
                #endregion

            }
            catch (System.Exception ex)
            {
                Logger.Error("执行打印失败： " + ex.Message);
                lastError = "do print exception: " + ex.Message;
            }
            return true;
        }

        /// <summary>
        /// 处理打印机打印任务(PL模式打印)
        /// </summary>
        /// <param name="receipt">小票内容</param>
        /// <returns></returns>
        private bool HandlePrintPL(winprint.Receipt receipt, PrintWidth width, int page_num)
        {
            try
            {
                LoadPOSDll.POS_SetMode(LoadPOSDll.POS_PRINT_MODE_PAGE);
                LoadPOSDll.POS_SetLineSpacing(30);

                #region 执行字段校验
                //check kernel fields
                if (receipt.head == null ||
                    receipt.head.title == null ||
                    receipt.body == null ||
                    receipt.body.foods == null ||
                    receipt.body.foods.Count == 0 ||
                    receipt.order_id <= 0 ||
                    receipt.tail == null)
                {
                    Logger.Debug("关键字段为空，不执行打印");
                    lastError = "miss some import fields";
                    return false;
                }
                #endregion

                #region 打印头部信息
                int left = 0;

                if (width == PrintWidth.MM58)
                {
                    if (page_num > 0)
                    {
                        LoadPOSDll.POS_PL_TextOut(PLUS_LINE_58MM_PRINT_NUM + "第 " + page_num + " 联" + PLUS_LINE_58MM_PRINT_NUM,0,
                            0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                        LoadPOSDll.POS_FeedLines(4);
                    }

                    left = (PAPER_WIDTH_58MM - receipt.head.title.Length * 48) / 2;
                    if (left < 0)
                    {
                        left = 80;
                    }

                    LoadPOSDll.POS_PL_TextOut(receipt.head.title, (uint)left,0,
                                                        2, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);

                    if (receipt.head.subtitle != null && receipt.head.subtitle != "")
                    {
                        LoadPOSDll.POS_FeedLines(4);
                        left = (PAPER_WIDTH_58MM - receipt.head.subtitle.Length * 30) / 2;
                        if (left < 0)
                        {
                            left = 80;
                        }
                        LoadPOSDll.POS_PL_TextOut(receipt.head.subtitle, (uint)left, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                    }
                }
                else
                {
                    if (page_num > 0)
                    {
                        LoadPOSDll.POS_PL_TextOut(PLUS_LINE_80MM_PRINT_NUM + "第 " + page_num + " 联" + PLUS_LINE_80MM_PRINT_NUM,
                            0,0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                        LoadPOSDll.POS_FeedLines(2);
                    }
                    left = (PAPER_WIDTH_80MM - receipt.head.title.Length * 48) / 2;
                    if (left < 0)
                    {
                        left = 120;
                    }
                    LoadPOSDll.POS_PL_TextOut(receipt.head.title, PrinterFormat.CENTER_80MM,0,
                                                        2, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                    if (receipt.head.subtitle != null && receipt.head.subtitle != "")
                    {
                        LoadPOSDll.POS_FeedLines(4);
                        left = (PAPER_WIDTH_80MM - receipt.head.subtitle.Length * 24) / 2;
                        if (left < 0)
                        {
                            left = 120;
                        }
                        LoadPOSDll.POS_PL_TextOut(receipt.head.subtitle, (uint)left, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_SMOOTH);
                    }
                }

                LoadPOSDll.POS_FeedLines(5);

                #region 正餐信息头
                if (receipt.head.bookhead != null)
                {
                    if (receipt.head.bookhead.arrival_at != null && receipt.head.bookhead.arrival_at != "")
                    {
                        LoadPOSDll.POS_PL_TextOut("到店时间: ", 0, 0,1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_PL_TextOut(receipt.head.bookhead.arrival_at, 120,0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_FeedLines(3);
                    }
                    if (receipt.head.bookhead.people != null && receipt.head.bookhead.people != "")
                    {
                        LoadPOSDll.POS_PL_TextOut("就餐人数: ", 0,0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_PL_TextOut(receipt.head.bookhead.people, 120,0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_FeedLines(3);
                    }
                    if (receipt.head.bookhead.phone != null && receipt.head.bookhead.phone != "")
                    {
                        LoadPOSDll.POS_PL_TextOut("顾客电话: ", 0,0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_PL_TextOut(receipt.head.bookhead.phone, 120, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                        LoadPOSDll.POS_FeedLines(3);
                    }
                }
                #endregion

                //LoadPOSDll.POS_SetLineSpacing(10);
                if (receipt.head.table_card_no != null && receipt.head.table_card_no != "")
                {

                    LoadPOSDll.POS_PL_TextOut("桌牌号: ", 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_PL_TextOut(receipt.head.table_card_no, 90, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);

                }
                if (receipt.head.way != null && receipt.head.way != "")
                {
                    LoadPOSDll.POS_PL_TextOut("就餐方式: ", 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_PL_TextOut(receipt.head.way, 120, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);
                }

                if (receipt.head.password != null && receipt.head.password != "")
                {
                    LoadPOSDll.POS_PL_TextOut("密码: ", 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_PL_TextOut(receipt.head.password, 60, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);
                }
                if (receipt.head.note != null && receipt.head.note != "")
                {
                    LoadPOSDll.POS_PL_TextOut("备注: ", 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_PL_TextOut(receipt.head.note, 60, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(3);
                }

                foreach (string moreitem in receipt.head.moreitems)
                {
                    LoadPOSDll.POS_PL_TextOut(moreitem, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                #endregion

                #region 打印菜品信息
                if (width == PrintWidth.MM58)
                {
                    LoadPOSDll.POS_PL_TextOut(PLUS_LINE_58MM, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(1);
                    LoadPOSDll.POS_PL_TextOut(PLUS_LINE_58MM_TITLE, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }
                else
                {
                    LoadPOSDll.POS_PL_TextOut(PLUS_LINE_80MM, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(1);
                    LoadPOSDll.POS_PL_TextOut(PLUS_LINE_80MM_TITLE, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }

                LoadPOSDll.POS_FeedLines(3);
                uint width_of_food = 300;
                if (width == PrintWidth.MM80)
                {
                    width_of_food = 500;
                }
                foreach (winprint.Foods food in receipt.body.foods)
                {
                    if (food.name != null && food.name != "" && food.num > 0)
                    {
                        LoadPOSDll.POS_PL_TextOut(food.name, 0, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_BOLD);
                        LoadPOSDll.POS_PL_TextOut(food.num.ToString(), width_of_food, 0, 1, 2, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_BOLD);
                        LoadPOSDll.POS_FeedLine();
                    }
                    foreach (winprint.SlaveFoods slave_food in food.slavefoods)
                    {
                        if (slave_food.name != null && slave_food.name != "" && slave_food.num > 0)
                        {
                            LoadPOSDll.POS_PL_TextOut("  " + slave_food.name,
                                0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                            LoadPOSDll.POS_PL_TextOut(slave_food.num.ToString(),
                                width_of_food, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                            LoadPOSDll.POS_FeedLines(2);
                        }
                    }
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (width == PrintWidth.MM58)
                {
                    LoadPOSDll.POS_PL_TextOut(PLUS_LINE_58MM, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }
                else
                {
                    LoadPOSDll.POS_PL_TextOut(PLUS_LINE_80MM, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                }

                #endregion

                #region 打印尾部信息
                LoadPOSDll.POS_FeedLines(2);
                if (receipt.tail.orderno != null && receipt.tail.orderno != "")
                {
                    LoadPOSDll.POS_PL_TextOut("订单号:" + receipt.tail.orderno, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (receipt.tail.restaurant_name != null && receipt.tail.restaurant_name != "")
                {
                    LoadPOSDll.POS_PL_TextOut("餐厅名称: " + receipt.tail.restaurant_name, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (receipt.tail.phone != null && receipt.tail.phone != "")
                {
                    LoadPOSDll.POS_PL_TextOut("联系电话: " + receipt.tail.phone, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(2);
                }
                if (receipt.tail.time != null && receipt.tail.time != "")
                {
                    LoadPOSDll.POS_PL_TextOut("日期: " + receipt.tail.time, 0, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_CHINESE, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                    LoadPOSDll.POS_FeedLines(10);
                }

                LoadPOSDll.POS_CutPaper(LoadPOSDll.POS_CUT_MODE_FULL, 50);
                LoadPOSDll.POS_PL_Print();
                LoadPOSDll.POS_PL_Clear();

                Logger.Debug("really print over: " + receipt.tail.orderno);
                
                Thread.Sleep(200);//控制打印节奏，防止不必要的异常

                #endregion

            }
            catch (System.Exception ex)
            {
                Logger.Error("执行PL打印失败： " + ex.Message);
                lastError = "do print exception: " + ex.Message;
            }
            return true;
        }
        
        #endregion

        #region 执行TCP（wifi或者网口打印机）打印任务
        /// <summary>
        /// TCP Printer
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="receipt"></param>
        /// <returns></returns>
        private bool TCPPrinter(string ip, winprint.Receipt receipt, PrintWidth width)
        {
            if (PosPrint.OpenNetPort(ip))//当参数nParam的值为POS_OPEN_NETPORT时，表示打开指定的网络接口，如“192.168.10.251”表示网络接口IP地址，打印时参考
            {
                Gp_IntPtr = PosPrint.POS_IntPtr;
            }
            else
            {
                return false;
            }
            bool ret = false;
            if (receipt.print_num <= 1)
            {
                ret = HandlePrint(receipt, width, 0);
            }
            else
            {
                for (int i = 0; i < receipt.print_num; i++)
                {
                    ret = HandlePrint(receipt, width, i + 1);
                }
            }
            return ret;

        }

        #endregion

        #region 执行驱动打印任务
        /// <summary>
        /// 对外接口
        /// </summary>
        /// <param name="driver_name"></param>
        /// <param name="receipt"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool IDriverPrinter(string driver_name, winprint.Receipt receipt, PrintWidth width)
        {
            bool ret = false;
            try
            {
                if (PosPrint.OpenPrinter(driver_name))//当参数nParam的值为POS_OPEN_NETPORT时，表示打开指定的网络接口，如“192.168.10.251”表示网络接口IP地址，打印时参考
                {
                    Gp_IntPtr = PosPrint.POS_IntPtr;
                }
                else
                {
                    lastError = "open printer failed";
                    return false;
                }

                if (LoadPOSDll.POS_StartDoc())
                {

                    if (receipt.print_num <= 1)
                    {

                        ret = HandlePrintPL(receipt, width, 0);

                    }
                    else
                    {
                        for (int i = 0; i < receipt.print_num; i++)
                        {
                            ret = HandlePrintPL(receipt, width, i + 1);
                        }
                    }

                    LoadPOSDll.POS_EndDoc();
                }
                else
                {
                    return ret;
                }
            }
            catch (System.Exception ex)
            {
                Logger.Debug("执行打印失败，" + ex.Message);
                lastError = "start doc failed when printing, exception: " + ex.Message;
            }
            return ret;

        }
        /// <summary>
        /// Driver Printer
        /// </summary>
        /// <param name="driver_name"></param>
        /// <param name="receipt"></param>
        /// <returns></returns>
        private bool DriverPrinter(string driver_name, winprint.Receipt receipt, PrintWidth width)
        {
            //POS_COM_DTR_DSR 0x00 流控制为DTR/DST  
            //POS_COM_RTS_CTS 0x01 流控制为RTS/CTS 
            //POS_COM_XON_XOFF 0x02 流控制为XON/OFF 
            //POS_COM_NO_HANDSHAKE 0x03 无握手 
            //POS_OPEN_PARALLEL_PORT 0x12 打开并口通讯端口 
            //POS_OPEN_BYUSB_PORT 0x13 打开USB通讯端口 
            //POS_OPEN_PRINTNAME 0X14 打开打印机驱动程序 
            //POS_OPEN_NETPORT 0x15 打开网络接口 
            bool ret = false;
            
            
            int job_count = GetPrinterJobCount(driver_name);
            if (job_count > 1) //允许有1个作业正在执行
            {

                Logger.Debug("打印机(" + driver_name + ")队列有未消化的作业，作业数目：" + job_count.ToString() + ", orderid: " + receipt.order_id.ToString());
                PrintLog("打印机(" + driver_name + ")队列有未消化的作业，作业数目：" + job_count.ToString() + ", orderid: " + receipt.order_id.ToString());
                lastError = "printer(" + driver_name + ") has more than 1 jobs (" + job_count.ToString() + ")" + ", orderid: " + receipt.order_id.ToString();
                return ret;
            }
            Logger.Debug("打印机(" + driver_name + ")队列，作业数目：" + job_count.ToString());

            try
            {
                if (PosPrint.OpenPrinter(driver_name))//当参数nParam的值为POS_OPEN_NETPORT时，表示打开指定的网络接口，如“192.168.10.251”表示网络接口IP地址，打印时参考
                {
                    Gp_IntPtr = PosPrint.POS_IntPtr;
                }
                else
                {
                    lastError = "open printer failed";
                    return false;
                }

                if (LoadPOSDll.POS_StartDoc())
                {

                    if (receipt.print_num <= 1)
                    {
                        
                        ret = HandlePrintPL(receipt, width, 0);
                        
                    }
                    else
                    {
                        for (int i = 0; i < receipt.print_num; i++)
                        {
                            ret = HandlePrintPL(receipt, width, i + 1);
                        }
                    }

                    LoadPOSDll.POS_EndDoc();
                }
                else
                {
                    return ret;
                }
            }
            catch (System.Exception ex)
            {
                Logger.Debug("执行打印失败，" + ex.Message);
                lastError = "start doc failed when printing, exception: " + ex.Message;
            }
            return ret;
        }
        #endregion

        #region 执行USB打印机任务,打印格式TODO
        private void SendData2USB(byte[] str)
        {
            NewUsb.SendData2USB(str, str.Length);
        }
        private void SendData2USB(string str)
        {
            byte[] by_SendData = System.Text.Encoding.Default.GetBytes(str);
            SendData2USB(by_SendData);
        }

        
        /// <summary>
        /// USB Printer
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="receipt"></param>
        /// <returns></returns>
        private bool USBPrinter(string ip, winprint.Receipt receipt, PrintWidth width)
        {
            NewUsb.FindUSBPrinter();
            if (NewUsb.USBPortCount == 0)
            {
                return false;
            }
            for (int i = 0; i < NewUsb.USBPortCount; i++)
            {

                if (NewUsb.LinkUSB(i))
                {
                    byte[] shiftsize = { 0x1d, 0x57, 0xd0, 0x01 };//偏移量
                    byte[] KanjiMode = { 0x1c, 0x26 };//汉字模式

                    SendData2USB(shiftsize);
                    SendData2USB(KanjiMode);

                    #region 打印信息

                    byte[] SendData = { 0x1b, 0x61, 0x01, 0x1b, 0x21, 0x30, 0x1c, 0x57, 0x01 };
                    byte[] enddata = { 0x0a };//换行
                    byte[] cutpaper = { 0x1b, 0x69 };//USB切纸

                    SendData2USB(SendData);

                    SendData2USB(receipt.head.title);
                    SendData2USB(enddata);
                    SendData2USB(receipt.head.subtitle);
                    SendData2USB(enddata);
                    SendData2USB("就餐方式：" + receipt.head.way);
                    SendData2USB(enddata);
                    SendData2USB("密码：" + receipt.head.way);
                    SendData2USB(enddata);

                    SendData2USB("密码：" + receipt.head.way);
                    SendData2USB(enddata);

                    string width_of_food = "\t\t  ";
                    if (width == PrintWidth.MM80)
                    {
                        width_of_food = "\t\t\t\t";
                    }

                    foreach (winprint.Foods food in receipt.body.foods)
                    {

                        SendData2USB(food.name + width_of_food + food.num);
                        SendData2USB(enddata);
                        foreach (winprint.SlaveFoods slave_food in food.slavefoods)
                        {
                            SendData2USB(slave_food.name + width_of_food + slave_food.num);
                            SendData2USB(enddata);
                        }
                    }

                    SendData2USB("订单号:" + receipt.tail.orderno);
                    SendData2USB(enddata);
                    SendData2USB("餐厅名称: " + receipt.tail.restaurant_name);
                    SendData2USB(enddata);
                    SendData2USB("联系电话: " + receipt.tail.phone);
                    SendData2USB(enddata);
                    SendData2USB("日期: " + receipt.tail.time);
                    SendData2USB(enddata);

                    SendData2USB(new byte[] { 0x0a, 0x0a });
                    SendData2USB(new byte[] { 0x1b, 0x61, 0x00, 0x1b, 0x21, 0x00, 0x1c, 0x57, 0x00 });

                    #endregion

                    #region 字体打印
                    /*
                   
                    SendData2USB(KanjiMode);
                    SendData = new byte[16];
                    int linecount = 3;
                    byte bit = 0xa1, Zone = 0xa1;
                    for (i = 0; i < 16; i += 2)
                    {
                        SendData[i] = Zone;
                        SendData[i + 1] = bit;
                        bit++;
                    }
                    SendData2USB(enddata);
                    SendData2USB(SendData);

                    Zone = 0xb0;
                    bit = 0xa1;
                    for (i = 0; i < linecount; i++)
                    {
                        for (int j = 0; j < 16; j += 2)
                        {
                            SendData[j] = Zone;
                            SendData[j + 1] = bit;
                            Zone++;
                        }
                        bit++;
                        SendData2USB(enddata);
                        SendData2USB(SendData);
                    }
                    SendData2USB(enddata);
                    SendData2USB(enddata);
                    SendData2USB(cutpaper);

                    
                    */
                    #endregion

                    NewUsb.CloseUSBPort();
                }
            }
            return true;
        }
        #endregion

        #region 处理服务器打印请求线程
        private static object lockObject = new Object();
        Queue<winprint.Message> __task_queue = new Queue<winprint.Message>();
        private void threadHandleTask()
        {
            while (isRunning())
            {

                winprint.Message msg = null;
                int cnt = 0;
                lock (lockObject)
                {
                    cnt = __task_queue.Count;
                    if (cnt > 0)
                    {
                        msg = __task_queue.Peek();
                    }
                }
                if (msg != null)
                {
                    try
                    {
                        bool ret = doPrintTask(msg);
                        //成功
                        if (true == ret)
                        {
                            lock (lockObject)
                            {
                                __task_queue.Dequeue();
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Error("handle task failed, exception: " + ex.Message);
                    }


                }
                if (cnt <= 1)
                {
                    Thread.Sleep(1000);
                }
                
             }
        }
        /// <summary>
        /// 执行打印
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool doPrintTask(winprint.Message msg)
        {
            List<int> err_printer_id = new List<int>();
            long order_id = 0;
            string succ_info = "grate, it's successful !";
            if (null != msg.data && null != msg.data.receipts)
            {
                //PrintLog("接收到消息：（打印）请求，执行打印中，票据个数:" + msg.data.receipts.Count.ToString());

                foreach(winprint.Receipt receipt in msg.data.receipts)
                { 
                    //do print
                    
                    int printer_id = receipt.printer_id;
                    printer_job_statistic[printer_id]++;
                    PrinterJobStatistics(printer_id);
                    string key = receipt.order_id.ToString() + "-" + printer_id.ToString();
                    if (printers.ContainsKey(printer_id))
                    {
                        if (receipt.is_force_print <= 0)//非强制打印得正常模式，检查如果已经打印则直接返回
                        {
                            //Logger.Debug("非强制打印，检查订单号是否存在");
                            if (orders_had_printed.ContainsKey(key))
                            {
                                //已经打印，直接回返
                                Logger.Debug("重复id 无需打印：" + receipt.order_id + ", order no: " + receipt.tail.orderno + ", printerid:" + receipt.printer_id.ToString());
                                lastError = "ignore, order id is repeated: " + receipt.order_id + ", printerid:" + receipt.printer_id.ToString();
                                succ_info = "ignore, order id is repeated: " + receipt.order_id + ", printerid:" + receipt.printer_id.ToString();
                                continue;
                            }
                        }
                        else
                        {
                            Logger.Debug("强制打印订单：" + receipt.order_id);
                        }
                        DeviceInfo printer = printers[printer_id];
                        string type = printer.type.ToLower();
                        Logger.Debug(type + " printer gonna print");
                        bool ret = false;
                        if (type == "wifi" || type == "ethernet")
                        {
                            ret =  TCPPrinter(printer.addr, receipt, printer.print_width);
                        } 
                        else if(type == "usb")
                        {
                            ret = USBPrinter(printer.addr, receipt, printer.print_width);
                        }
                        else if (type == "driver")
                        {
                            ret  = DriverPrinter(printer.addr, receipt, printer.print_width);
                        }
                        if (false == ret)
                        {
                            Logger.Error("order id: " + receipt.order_id.ToString() + ", order no: " + receipt.tail.orderno + ", printer id: " + printer_id + " print failed");
                            err_printer_id.Add(printer_id);
                            PrinterJobStatistics(printer_id,false);
                        }
                        else
                        {
                            Logger.Debug("order id: " + receipt.order_id.ToString() + ", order no: " + receipt.tail.orderno + ", printer id: " + printer_id + " print OK");
                            if (orders_had_printed.Count > 100000)
                            {
                                orders_had_printed.Clear();
                            }
                            if (false == orders_had_printed.ContainsKey(key))//如果之前没有打印成功
                            {
                                orders_had_printed.Add(key, true);//[order_id] = true;//insert order
                            }
                                     
                        }
                        if (order_id == 0)
                            order_id = receipt.order_id;
                    }
                    else
                    {
                        Logger.Error("打印机未找到, order: " + order_id + ", order no: " + receipt.tail.orderno + ", printer id: " + printer_id);
                        err_printer_id.Add(printer_id);
                        PrinterJobStatistics(printer_id, false);
                        continue;
                    }
                    PrintLog("打印完成，订单号： " + order_id.ToString() + ", order no: " + receipt.tail.orderno);
                    OrderCounter(order_id.ToString());
                    
                }
                
            }else 
            {
                err_printer_id.Add(0);//如果ID=0，则表示都错误了！
            }
            //do print response
            winprint.Header resheader = new winprint.Header
            {
                cmd = winprint.Header.Command.PRINTERTASK,
                restaurant_id = msg.header.restaurant_id,
                seq = msg.header.seq,
            };
            winprint.Result.Code rescode = new winprint.Result.Code();
            string resp_message = "";
            int save_db_status = 0;
            if (0 == err_printer_id.Count)
            {
                rescode = winprint.Result.Code.SUCC;
                resp_message = succ_info;

               
            }
            else
            {
                rescode = winprint.Result.Code.ERR;
                resp_message = "Error Print(ids): ";
                foreach (int id in err_printer_id)
                {
                    resp_message += id.ToString();
                    resp_message += ",";
                }
                resp_message += "detail (" + lastError + ")";     
                save_db_status = 2;
            }
            winprint.Result resresult = new winprint.Result
            {
                code = rescode,
                message = resp_message,
            };
            winprint.Data resdata = new winprint.Data
            {
                result = resresult,
            };
            winprint.Message response = new winprint.Message
            {
                header = resheader,
                data = resdata,
            };
            
            if (false == send2Server(response))
            {
                //TODO
                Logger.Error("发送数据失败");
                if (save_db_status == 2)
                {
                    save_db_status = 3;//打印失败，响应服务器失败
                }else 
                    save_db_status = 1;//打印成功，响应服务器失败
            }
            
            counter++;
            
            PrintLog("total counter : " + counter.ToString() + ", status: " + save_db_status.ToString());
            if (order_id > 0)
            {
                //保存信息到数据库
                save2DB(order_id.ToString(), save_db_status, msg);
            }  
            return true;
                    
        }
        private int counter = 0;
        #endregion

        #region 保存数据到DB
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="orderid">订单id</param>
        /// <param name="status">0：打印成功且响应服务器成功，1：打印成功，响应服务器失败，2：打印失败，响应服务器成功，3：打印失败，响应服务器失败</param>
        /// <param name="msg">消息体</param>
        private void save2DB(string orderid, int status, winprint.Message msg)
        {
            try
            {
                MemoryStream resp_stream = new MemoryStream();
                ProtoBuf.Serializer.Serialize<winprint.Message>(resp_stream, msg);
                
                byte[] bytedata64 = resp_stream.GetBuffer();
                string text = Convert.ToBase64String(bytedata64);
                Storage.InsertOrder(orderid,status, text,setting.db_file_name);
            }
            catch (System.Exception ex)
            {
                Logger.Debug("save 2 db failed: " + ex.Message);
            }
        }
        #endregion

        #region 处理服务器接收消息事件
        /// <summary>
        /// 接收到服务端消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void client_OnReceive(object sender, ReceiveEventArgs e)
        void client_OnReceive(object sender, Stream stream)
        {
            try
            {
                winprint.Message msg = null;
                //Stream stream = e.ReadStream();
                //Stream stream = e.ReadStream2();
                //stream.Length
                try
                {
                    msg = ProtoBuf.Serializer.Deserialize<winprint.Message>(stream);
                }
                catch (System.Exception ex)
                {
                    Logger.Error("反序列化失败：" + ex.Message);
                    return;
                }
                
                if(null == msg || msg.header == null )
                {
                    Logger.Error("反序列化后message is null");
                    return;
                }
                //Logger.Debug("recv message restid: " + msg.header.restaurant_id);
                switch (msg.header.cmd)
                {
                    case winprint.Header.Command.HEARTBEAT:
                        {
                            //Console.Out.WriteLine("Heartbeat message");
                            //Logger.Debug("recv heartbeat response message");
                            //PrintLog("接收到消息：（心跳）回复");
                        }
                       
                	break;
                    case winprint.Header.Command.PRINTERTASK:
                    #region 执行打印任务
                    {
                        try
                        {
                            lock (lockObject)
                            {
                                __task_queue.Enqueue(msg);
                            }
                            //doPrintTask(msg);
                            Logger.Debug("打印任务完成成功");
                        }
                        catch (System.Exception ex)
                        {
                            Logger.Debug("do Print task failed :　" + ex.Message);
                        }
                        
                    }
                    #endregion
                    break;
                    case winprint.Header.Command.PRINTERINFO:
                    {
                        PrintLog("接收到消息：（同步打印机）回复");
                        Console.Out.WriteLine("printer info message");
                        Logger.Debug("printer info received response");
                    }
                    
                    break;
                    case winprint.Header.Command.NOTIFY:
                    break;
                    case winprint.Header.Command.UPDATECONFIG:
                    {
                        #region 更新配置
                        bool succ = false;
                        if (msg.data.servercfg != null)
                        {
                            PrintLog("begin to update server configure");

                            setting.system.server_ip = msg.data.servercfg.host;
                            setting.system.server_port = msg.data.servercfg.port.ToString();
                            setting.system.restaurant_id = msg.data.servercfg.restaurant_id.ToString();
                            setting.system.heartbeat_interval = msg.data.servercfg.heartbeat_interval;

                            UpdateConfigure(setting.system);
                            succ = true;
                        }
                        winprint.Header resheader = new winprint.Header
                        {
                            cmd = winprint.Header.Command.UPDATECONFIG,
                            restaurant_id = int.Parse(setting.system.restaurant_id),
                            seq = msg.header.seq

                        };
                        winprint.Result.Code rescode = new winprint.Result.Code();
                        string resp_message = "";
                        if (true == succ)
                        {
                            rescode = winprint.Result.Code.SUCC;
                            resp_message = "grate, it's successful !";
                        }
                        else
                        {
                            rescode = winprint.Result.Code.ERR;
                            resp_message = "server configure is null";
                            
                        }
                        winprint.Result resresult = new winprint.Result
                        {
                            code = rescode,
                            message = resp_message,
                        };
                        winprint.Data resdata = new winprint.Data
                        {
                            result = resresult,
                        };
                        winprint.Message response = new winprint.Message
                        {
                            header = resheader,
                            data = resdata,
                        };

                        if (false == send2Server(response))
                        {
                            //TODO
                            Logger.Error("发送到服务器的重置配置失败");
                        }


                        if (true == succ)
                        {
                            Thread.Sleep(5000);
                            //会重新连接
                            PrintLog("重新加载配置，连接中。。。");
                            Stop();
                            PrintLog("停止服务完成，正在重新连接。。。");
                            tryConnect();
                        }
                        #endregion 
                    }
                    break;
                    default:
                    PrintLog("Unknown Command :" + msg.header.cmd.ToString());
                    break;

                }
                 
            }
            catch (System.Exception ex)
            {
                Logger.Error("处理请求出错: " + ex.Message);  
                PrintLog("处理请求出错：" + ex.Message);
            }
        }
        #endregion

        #region 服务器掉线事件处理
        /// <summary>
        /// 服务器掉线，重连
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_OnServerClosed(object sender, SocketEventArgs e)
        {
            //throw new NotImplementedException();
            PrintLog("服务器连接断开，正在重连中...");
            is_connected = false;
            
            tryConnect();
        }
        #endregion

        #region 获取打印机状态
        
        /// <summary>
        /// 获取打印机状态(驱动模式，使用windows自带API获取)
        /// </summary>
        /// <param name="PrinterDevice"></param>
        /// <returns></returns>
        private bool GetPrinterStatus(string printerName)
        {
            int status = 0;
            string path = @"win32_printer.DeviceId='" + printerName + "'";
            ManagementObject printer = new ManagementObject(path);
            if (printer == null)
            {
                Logger.Error("没有找到打印机：" + printerName);
                PrintLog("没有找到打印机：" + printerName);
                return false;
            }
            try
            {
                printer.Get();
                status = Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
                if ((status == 1) || //Other
                    (status == 2) || //Unknown
                    (status == 7) || //Offline
                    (status == 9) || //error
                    (status == 11))  //Not Available
                {
                    //PrintLog(printerName + ": 离线");
                    return false;
                }
                status = Int32.Parse(printer["DetectedErrorState"].ToString());
                if (status != 2) //No error
                {
                    //PrintLog(printerName + ": 在线正常");
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.Error("获取打印机状态出错：" + ex.Message);
            }
            //PrintLog(printerName + ": 离线（异常）");
            return false;
        }
        #region 获取打印机队列
        /// <summary>
        /// 获取打印机队列个数
        /// </summary>
        /// <param name="printerName"></param>
        /// <returns></returns>
        private int GetPrinterJobCount(string printerName)
        {
           // Get all the printers installed on this PC
            try
            {
                PrintServer myPrintServer = new PrintServer();
                PrintQueueCollection myPrintQueues = myPrintServer.GetPrintQueues();

                foreach (PrintQueue pq in myPrintQueues)
                {
                    if (printerName == pq.Name)
                    {
                        return pq.NumberOfJobs;
                    }
                    #region JOB的状态，本期先不考虑
                    /*
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
                */
                    #endregion


                }
            }
            catch (System.Exception ex)
            {
                Logger.Error("获取打印机任务数出错：" + ex.Message);
            }
            return 0;

        }
        #endregion
        #endregion

    }
}
