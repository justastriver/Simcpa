using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ISimcpa.DB;
using System.Collections;
using ISimcpa.Task;
using System.IO;
using ISimcpa.Config;
using ISimcpa.Util;

namespace Simcpa
{
    public partial class JobWorker : Form
    {
        private string db_file_name = "";
        Dictionary<string/*order id*/, OrderDetail/*order detail*/> __order_list = new Dictionary<string/*order id*/, OrderDetail/*order detail*/>();
        Dictionary<int/*printer id*/, DeviceInfo> printers = new Dictionary<int, DeviceInfo>();
        Setting __setting = new Setting();
        TaskWorker __worker = new TaskWorker();
        public JobWorker(string db_file_name, Setting setting) 
        {
            InitializeComponent();
            this.db_file_name = db_file_name;
            __setting = setting;
            foreach (DeviceInfo printer in setting.printers)
            {
                try
                {
                    int printer_id = int.Parse(printer.id);
                    printers.Add(printer_id, printer);
                }
                catch (System.Exception ex)
                {
                    Logger.Error("parse printer_id failed," + printer.id +", exc: " + ex.Message);
                }
            }
        }

        private void JobWorker_Load(object sender, EventArgs e)
        {
            
            //Storage.CreateDB(db_file_name);
            //Storage.CreateTable(db_file_name);
            //storage.GetTodayOrder(db_file_name);

            this.listView_order_list.Columns.Add("ID", 60, HorizontalAlignment.Left);
            this.listView_order_list.Columns.Add("订单号", 120, HorizontalAlignment.Left);
            this.listView_order_list.Columns.Add("日期", 160, HorizontalAlignment.Left);
            this.listView_order_list.Columns.Add("打印状态", 100, HorizontalAlignment.Left);

            this.button_print.Enabled = false;
            ArrayList array = Storage.GetTodayOrder(db_file_name);
            foreach (OrderDetail item in array)
            {
                ListViewItem[] lvs = new ListViewItem[1];
                string status = "已打印,已响应";
                if (item.status == "1")
                {
                    status = "已打印，未响应";
                }
                else if (item.status == "2")
                {
                    status = "未打印，已响应";
                }
                else if (item.status == "3")
                {
                    status = "未打印，未响应";
                }
                lvs[0] = new ListViewItem(new string[] { item.id, item.order_id, item.date, status });
                this.listView_order_list.Items.AddRange(lvs);

                if (!__order_list.ContainsKey(item.order_id))
                {
                    __order_list.Add(item.order_id, item);
                }
            }
            if (array.Count > 0)
            {
                this.button_clear.Enabled = true;
            }
            else
            {
                this.button_clear.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listView_order_list.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView_order_list.SelectedItems[0];
                string orderid = item.SubItems[1].Text;
                //MessageBox.Show(orderid);
                if (__order_list.ContainsKey(orderid))
                {
                    OrderDetail detail = __order_list[orderid];
                    byte[] bytes = Convert.FromBase64String(detail.message);
                    try
                    {
                        MemoryStream stream = new MemoryStream(bytes);
                        winprint.Message msg = ProtoBuf.Serializer.Deserialize<winprint.Message>(stream);
                        if (null != msg.data && null != msg.data.receipts)
                        {
                            Logger.Debug("接收到消息：（打印）请求，执行打印中，票据个数:" + msg.data.receipts.Count.ToString());
                            foreach (winprint.Receipt receipt in msg.data.receipts)
                            {
                                if (printers.ContainsKey(receipt.printer_id))
                                {
                                    DeviceInfo device = printers[receipt.printer_id];
                                    if (__worker.IDriverPrinter(device.addr, receipt, device.print_width))
                                    {
                                        MessageBox.Show("打印完成！");
                                    }
                                    else
                                    {
                                        MessageBox.Show("打印失败！");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("打印配置未找到！， id: " + receipt.printer_id);
                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show("解析Message失败");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("执行打印失败： " + ex.Message);
                    }
                    
                }
            }
        }

        private void listView_order_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_order_list.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView_order_list.SelectedItems[0];
                this.button_print.Enabled = true;
            }
            else
            {
                this.button_print.Enabled = false;
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定清除所有？", "趣吃饭-确认窗口",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }
            Storage.Clear(db_file_name);
           
            this.listView_order_list.Clear();
            this.button_clear.Enabled = false;
            MessageBox.Show("清除成功！");
        }
    }
}
