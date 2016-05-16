using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using ISimcpa.Task;
using ISimcpa.Net;
using ISimcpa.Util;
using ISimcpa.Config;
using System.Printing;
using System.Drawing.Printing;
using ISimcpa;
using System.Management;

namespace Simcpa
{
    

    public partial class PrinterConfig : Form
    {
        private string config_file_name = "../../Config/config.xml";
        private IntPtr Gp_IntPtr;                  
        LoadPOSDll PosPrint = new LoadPOSDll();
        private libUsbContorl.UsbOperation NewUsb = new libUsbContorl.UsbOperation();
        private List<String> fPrinters = new List<String>();

        public PrinterConfig(string config_file_name)
        {
            InitializeComponent();
            //Thread thread_job = new Thread(new ThreadStart(StartJobThread));
            //thread_job.Start();
            load_devices();
            this.config_file_name = config_file_name;
        }
        
        private void load_devices()
        {
            
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }


        private void button_start_service_Click(object sender, EventArgs e)
        {
            
            
        }

        private void listView_Devices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView_Devices.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView_Devices.SelectedItems[0];
                this.textBox_print_id.Text = item.SubItems[0].Text;
                this.textBox_printName.Text = item.SubItems[1].Text;
                if(item.SubItems[2].Text == "58mm") 
                {
                    this.comboBox_printer_type.Text = "58mm";
                }else 
                {
                    this.comboBox_printer_type.Text = "80mm";
                }
                
                string type = item.SubItems[3].Text.ToLower();
                if (type == "wifi")
                {
                    this.radioButton_wifi.Checked = true;
                }
                else if (type == "ethernet") 
                {
                    this.radioButton_ethernet.Checked = true;
                }
                else if (type == "usb")
                {
                    this.radioButton_usb.Checked = true;
                }
                else if (type == "com")
                {
                    this.radioButton_com.Checked = true;
                }
                else if (type == "driver")
                {
                    this.radioButton_Driver.Checked = true;
                }
                
                
                this.textBox_addr.Text = item.SubItems[4].Text;
                this.textBox_desc.Text = item.SubItems[5].Text;

                this.button_new.Text = "更新";
            }
            else
            {
                this.button_new.Text = "新增";
                this.textBox_print_id.Text = "";
                this.textBox_printName.Text = "";
                this.textBox_addr.Text = "";
                this.textBox_desc.Text = "";
            }
        }

        private void PrinterConfig_Load(object sender, EventArgs e)
        {
            ScanPrinter();
            //ID  
            this.listView_Devices.Columns.Add("ID",50,HorizontalAlignment.Left);
            this.listView_Devices.Columns.Add("名称", 80, HorizontalAlignment.Left);
            this.listView_Devices.Columns.Add("打印宽", 60, HorizontalAlignment.Left);
            this.listView_Devices.Columns.Add("接口类型", 60, HorizontalAlignment.Left);
            this.listView_Devices.Columns.Add("描述符", 120, HorizontalAlignment.Left);
            this.listView_Devices.Columns.Add("备注", 350, HorizontalAlignment.Left);
            /*
            ListViewItem lvi = new ListViewItem();
            lvi.ImageIndex = 1;     //通过与imageList绑定，显示imageList中第i项图标  
            lvi.Text = "1";
            lvi.SubItems.Add("WIFI");
            lvi.SubItems.Add("192.168.1.100");
            lvi.SubItems.Add("group 1 for WIFI"); ;
            
            this.listView_Devices.Items.Add(lvi);  
             * */
            
            ArrayList printerList = XMLParser.Read(config_file_name).printers;
            DeviceListCompare deviceCompare = new DeviceListCompare();
            printerList.Sort(deviceCompare);
            foreach (DeviceInfo item in printerList)
            {
                ListViewItem[] lvs = new ListViewItem[1];
                string print_width = "58mm";
                if (item.print_width == PrintWidth.MM80)
                    print_width = "80mm";
                lvs[0] = new ListViewItem(new string[] { item.id,item.name, print_width, item.type, item.addr, item.desc });
                this.listView_Devices.Items.AddRange(lvs);
            }
            /*
            lvs[0] = new ListViewItem(new string[] { "1", "WIFI", "192.168.1.100", "group 1 for WIFI" });
            lvs[1] = new ListViewItem(new string[] { "2", "Ethernet", "192.168.1.100", "group 1 for Ethernet" });
            lvs[2] = new ListViewItem(new string[] { "3", "Driver", "Gprinter 58130 seria", "group 1 for driver" });

            this.listView_Devices.Items.AddRange(lvs);
            */
            
        }

        private void button_new_Click(object sender, EventArgs e)
        {
            string id = "";
            string name = "";
            string type = "";
            string print_width = "58mm";
            string addr = "";
            string desc = "";
            if (this.radioButton_ethernet.Checked)
            {
                type = "Ethernet";
               
            }
            if (this.radioButton_usb.Checked)
            {
                type = "USB";
            }
            if (this.radioButton_wifi.Checked)
            {
                type = "WIFI";
            }
            if (this.radioButton_com.Checked)
            {
                type = "COM";
            }
            if (this.radioButton_Driver.Checked)
            {
                type = "Driver";
            }
            id = this.textBox_print_id.Text;
            name = this.textBox_printName.Text;
            addr = this.textBox_addr.Text;
            desc = this.textBox_desc.Text;
            print_width = this.comboBox_printer_type.Text;

            if (id == "" || name == "" || addr == "" || desc == "")
            {
                MessageBox.Show("some field missed");
                return;
            }
            ListViewItem[] lvs = new ListViewItem[1];
            lvs[0] = new ListViewItem(new string[] {id, name, print_width, type, addr, desc });
            if (this.listView_Devices.SelectedItems.Count > 0) //update
            {
                int index = this.listView_Devices.SelectedItems[0].Index;
                this.listView_Devices.Items[index] = lvs[0];
            }
            else // new
            {
                this.listView_Devices.Items.AddRange(lvs);

            }
            
            //clear 
            this.textBox_addr.Text = "";
            this.textBox_print_id.Text = "";
            this.textBox_printName.Text = "";
            this.textBox_desc.Text = "";

        }


        private void button_new_auto_Click(object sender, EventArgs e)
        {
            string id = "";
            string name = "";
            string type = "";
            string print_width = this.textBox_interface_type_auto.Text + "mm";
            string addr = this.comboBox_printer_list.Text;
            string desc = "";
            
            if ("" == addr || "" == print_width)
            {
                MessageBox.Show("请选择打印机（可先进行扫描）");
                return;

            }
            if (addr[0] == '*')
            {
                type = "USB";
            }
            else
            {
                type = "Driver";
            }
            int max_id = 0;
            foreach (ListViewItem item in this.listView_Devices.Items)
            {
                int curr = int.Parse(item.SubItems[0].Text);
                if (curr > max_id)
                {
                    max_id = curr;
                }
            }
            max_id++;
            id = max_id.ToString();
            name = "auto_printer";
            desc = "this is auto created description";
           
            ListViewItem[] lvs = new ListViewItem[1];
            lvs[0] = new ListViewItem(new string[] { id, name, print_width, type, addr, desc }); 
            this.listView_Devices.Items.AddRange(lvs);

        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Delete Printer ", "Confirm Message", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }
            if (this.checkBox_selectAll.Checked)
            {
                this.listView_Devices.Items.Clear();
            }
            else
            {
                foreach (ListViewItem lvi in this.listView_Devices.CheckedItems) 
                {
                    this.listView_Devices.Items.RemoveAt(lvi.Index); // 按索引移除 
                    //listView1.Items.Remove(lvi);   //按项移除 
                }
            }
            //this.listView_Devices.EndUpdate();

        }

        private bool doTestPrint()
        {
            #region USB Printer
            if (this.radioButton_usb.Checked)
            {

                return USBPrinter();

            }
            #endregion

            #region TCP wifi/ethernet Printer
            if (this.radioButton_wifi.Checked || this.radioButton_ethernet.Checked)
            {
                string ip = this.textBox_addr.Text;
                return TCPPrinter(ip);

            }
            #endregion

            #region Driver
            if (this.radioButton_Driver.Checked)
            {
                string driver_name = this.textBox_addr.Text;
                return DriverPrinter(driver_name);
            }
            #endregion
            return true;
        }
        private void SendData2USB(byte[] str)
        {
            NewUsb.SendData2USB(str, str.Length);
        }
        private void SendData2USB(string str)
        {
            byte[] by_SendData = System.Text.Encoding.Default.GetBytes(str);
            SendData2USB(by_SendData);
        }
        private void button_print_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem printer in this.listView_Devices.CheckedItems)
            {
                string type = printer.SubItems[3].Text.ToLower();
                string addr = printer.SubItems[4].Text;
                if (type == "wifi" || type == "ethernet")
                {
                    TCPPrinter(addr);
                }else if (type == "usb")
                {
                    USBPrinter();
                }else if (type == "driver")
                {
                    DriverPrinter(addr);
                }
            }
            MessageBox.Show("print finished");
        }
        
        private void button_test_print_Click(object sender, EventArgs e)
        {
            
            if (doTestPrint())
            {
                MessageBox.Show("print ok");
            }
            else
            {
                MessageBox.Show("print failed");
            }
        }


        //test for printer
        private bool TCPPrinter(string ip)
        {
            if (PosPrint.OpenNetPort(ip))//当参数nParam的值为POS_OPEN_NETPORT时，表示打开指定的网络接口，如“192.168.10.251”表示网络接口IP地址，打印时参考
            {
                Gp_IntPtr = PosPrint.POS_IntPtr;
            }
            else
            {
                return false;
            }
            IntPtr ret = LoadPOSDll.POS_PreDownloadBmpToRAM("Look.bmp", 0);
            ret = LoadPOSDll.POS_S_PrintBmpInRAM(0, 50, 330);

            LoadPOSDll.POS_SetMode(LoadPOSDll.POS_PRINT_MODE_PAGE);
            LoadPOSDll.POS_SetLineSpacing(20);
            LoadPOSDll.POS_PL_TextOut("趣吃饭", 90,0, 2, 2, LoadPOSDll.POS_FONT_TYPE_STANDARD, LoadPOSDll.POS_FONT_STYLE_BOLD);
            LoadPOSDll.POS_FeedLines(5);
            LoadPOSDll.POS_PL_TextOut("测试打印", 100,0, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD, LoadPOSDll.POS_FONT_STYLE_BOLD);
            LoadPOSDll.POS_FeedLines(6);
            LoadPOSDll.POS_PL_TextOut("网络IP：" + ip, 1, 0, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD, LoadPOSDll.POS_FONT_STYLE_NORMAL);
            LoadPOSDll.POS_FeedLines(15);
            LoadPOSDll.POS_CutPaper(LoadPOSDll.POS_CUT_MODE_FULL, 0);
            
            LoadPOSDll.POS_PL_Print();
            LoadPOSDll.POS_PL_Clear();
            
            return true;
        }
        private bool DriverPrinter(string driver_name)
        {
            doTestDriverPrint(driver_name);
            return true;
        }
        private bool USBPrinter()
        {
            NewUsb.FindUSBPrinter();
            if (NewUsb.USBPortCount == 0)
            {
                return false;
            }
            
            for (int i = 0; i < NewUsb.USBPortCount; i++)
            {
                Logger.Debug("usb port: " + i.ToString());
                if (NewUsb.LinkUSB(i))
                {
                    byte[] shiftsize = { 0x1d, 0x57, 0xd0, 0x01 };//偏移量
                    byte[] KanjiMode = { 0x1c, 0x26 };//汉字模式

                    SendData2USB(shiftsize);
                    SendData2USB(KanjiMode);

                    #region 打印信息测试
                    string strPrintwidth = "48毫米";
                    string strPrintDensity = "384点/行";
                    string strPrintSpeed = "90毫米/秒";
                    string strPrintLiftTime = "50公里";
                    string strPowerSupply = "DC 12V/4A";
                    string strSerialInfo = "有";
                    string strParInfo = "无";
                    string strUSBInfo = "USB2.0协议";
                    string strWirelessInfo = "无";

                    byte[] SendData = { 0x1b, 0x61, 0x01, 0x1b, 0x21, 0x30, 0x1c, 0x57, 0x01 };
                    byte[] enddata = { 0x0a };//换行

                    SendData2USB(SendData);

                    string strSendData = "联机测试";
                    SendData2USB(strSendData);

                    SendData2USB(new byte[] { 0x0a, 0x0a });
                    SendData2USB(new byte[] { 0x1b, 0x61, 0x00, 0x1b, 0x21, 0x00, 0x1c, 0x57, 0x00 });

                    SendData2USB("技术指标：");
                    SendData2USB(enddata);
                    SendData2USB("*打印宽度" + strPrintwidth);
                    SendData2USB(enddata);
                    SendData2USB("*打印速度" + strPrintSpeed);
                    SendData2USB(enddata);
                    SendData2USB("*打印浓度" + strPrintDensity);
                    SendData2USB(enddata);
                    SendData2USB("*使用寿命" + strPrintLiftTime);
                    SendData2USB(enddata);
                    SendData2USB("*电源要求" + strPowerSupply);
                    SendData2USB(enddata);
                    SendData2USB("*打印宽度" + strPrintwidth);
                    SendData2USB(enddata);
                    SendData2USB("*串行接口" + strSerialInfo);
                    SendData2USB(enddata);
                    SendData2USB("*并行接口" + strParInfo);
                    SendData2USB(enddata);
                    SendData2USB("*USB接口" + strUSBInfo);
                    SendData2USB(enddata);
                    SendData2USB("*无线接口" + strWirelessInfo);
                    SendData2USB(enddata);
                    SendData2USB("*网络接口" + strWirelessInfo);
                    SendData2USB(enddata);
                    SendData2USB(enddata);
                    #endregion

                    #region 字体打印测试
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
                    #endregion

                    NewUsb.CloseUSBPort();
                }
            }
            return true;
        }

        private void checkBox_selectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool is_checked = false;
            if (this.checkBox_selectAll.Checked)
            {
                is_checked = true;
            }
            foreach (ListViewItem item in this.listView_Devices.Items)
            {
                
                item.Checked = is_checked;
                
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            ArrayList printerList = new ArrayList();
            foreach (ListViewItem item in this.listView_Devices.Items)
            {
                DeviceInfo info = new DeviceInfo();
                info.id = item.SubItems[0].Text;
                info.name = item.SubItems[1].Text;
                string print_width = item.SubItems[2].Text;
                if (print_width.IndexOf("80") >=0)
                {
                    info.print_width = PrintWidth.MM80;
                }
                else 
                {
                    info.print_width = PrintWidth.MM58;
                }
                
                info.type = item.SubItems[3].Text;
                info.addr = item.SubItems[4].Text;
                info.desc = item.SubItems[5].Text;
                
                printerList.Add(info);

            }
            if (XMLParser.SavePrinterConfig(printerList, config_file_name))
            {
                MessageBox.Show("Save Ok !");
            }
            else MessageBox.Show("Save falied !");

            this.Close();
        }

        private void button_open_config_file_Click(object sender, EventArgs e)
        {
            this.openFileDialog_Config.ShowDialog();
        }
        private void ScanPrinter()
        {
            this.comboBox_printer_list.ResetText();
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                    this.comboBox_printer_list.Items.Add(fPrinterName);
                }
            }
            NewUsb.FindUSBPrinter();
            if (NewUsb.USBPortCount > 0)
            {
                for (int i = 0; i < NewUsb.USBPortCount; i++)
                {
                    this.comboBox_printer_list.Items.Add("*USB-" + (i + 1).ToString());
                }
            }
            if (this.comboBox_printer_list.Items.Count > 0)
            {
                this.comboBox_printer_list.SelectedText = this.comboBox_printer_list.Items[0].ToString();
                
            }
            
        }
        private void button_scan_Click(object sender, EventArgs e)
        {
            this.comboBox_printer_list.ResetText();
            ScanPrinter();
            
        }
        private bool doTestDriverPrint(string addr)
        {

            if (PosPrint.OpenPrinter(addr))
            {
                Gp_IntPtr = PosPrint.POS_IntPtr;

            }
            else
            {
                return false;
            }

            if (LoadPOSDll.POS_StartDoc())
            {

                LoadPOSDll.POS_SetLineSpacing(20);
                LoadPOSDll.POS_S_TextOut("趣吃饭", 90, 2, 2, LoadPOSDll.POS_FONT_TYPE_STANDARD, LoadPOSDll.POS_FONT_STYLE_BOLD);
                LoadPOSDll.POS_FeedLines(5);
                LoadPOSDll.POS_S_TextOut("测试打印", 100, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD, LoadPOSDll.POS_FONT_STYLE_BOLD);
                LoadPOSDll.POS_FeedLines(6);
                LoadPOSDll.POS_S_TextOut("驱动：" + addr, 1, 1, 1, LoadPOSDll.POS_FONT_TYPE_STANDARD, LoadPOSDll.POS_FONT_STYLE_NORMAL);
                LoadPOSDll.POS_FeedLines(15);
                LoadPOSDll.POS_CutPaper(LoadPOSDll.POS_CUT_MODE_FULL, 0);


                LoadPOSDll.POS_EndDoc();
            }
            else
            {
                return false;
            }
            return true;


        }
        private bool doTestUSBPrint(int port)
        {
            
            if (NewUsb.LinkUSB(port))
            {

                byte[] shiftsize = { 0x1d, 0x57, 0xd0, 0x01 };//偏移量
                byte[] KanjiMode = { 0x1c, 0x26 };//汉字模式

                SendData2USB(shiftsize);
                SendData2USB(KanjiMode);

                #region 打印信息测试

                byte[] SendData = { 0x1b, 0x61, 0x01, 0x1b, 0x21, 0x30, 0x1c, 0x57, 0x01 };
                byte[] enddata = { 0x0a };//换行
                byte[] cutpaper = { 0x1b, 0x69 };//

                SendData2USB(SendData);

                string strSendData = "趣吃饭";
                SendData2USB(strSendData);
                SendData2USB(enddata);
                strSendData = "测试打印";
                SendData2USB(strSendData);
                SendData2USB(enddata);

                SendData2USB(new byte[] { 0x0a, 0x0a });
                SendData2USB(new byte[] { 0x1b, 0x61, 0x00, 0x1b, 0x21, 0x00, 0x1c, 0x57, 0x00 });

                SendData2USB("USB打印机。。。");
                SendData2USB(enddata);
                SendData2USB(enddata);

                #endregion

                SendData2USB(enddata);
                SendData2USB(enddata);
                SendData2USB(cutpaper);


                NewUsb.CloseUSBPort();
            }
            else
            {
                return false;
            }
            return true;
        }
        private void button_test_print_auto_Click(object sender, EventArgs e)
        {
            string addr = this.comboBox_printer_list.Text;

            if (addr == "")
            {
                MessageBox.Show("请选择打印机（可先进行扫描）");
                return;
            }
            bool ret = false;
            if (addr[0] == '*')//USB
            {
                int port = int.Parse(addr.Substring(5)) - 1;
                ret = doTestUSBPrint(port);
            }
            else
            {

                ret = doTestDriverPrint(addr);
            }
            if (true == ret)
            {
                MessageBox.Show("打印完成！");
            }
            else
            {
                MessageBox.Show("打印失败，请检查打印机配置！");
            }
            
        }
    }

   
}
