namespace Simcpa
{
    partial class PrinterConfig
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterConfig));
            this.button_print = new System.Windows.Forms.Button();
            this.listView_Devices = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_printer_type = new System.Windows.Forms.ComboBox();
            this.radioButton_usb = new System.Windows.Forms.RadioButton();
            this.radioButton_ethernet = new System.Windows.Forms.RadioButton();
            this.radioButton_wifi = new System.Windows.Forms.RadioButton();
            this.radioButton_com = new System.Windows.Forms.RadioButton();
            this.radioButton_Driver = new System.Windows.Forms.RadioButton();
            this.textBox_print_id = new System.Windows.Forms.TextBox();
            this.textBox_printName = new System.Windows.Forms.TextBox();
            this.textBox_addr = new System.Windows.Forms.TextBox();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_test_print = new System.Windows.Forms.Button();
            this.button_new = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_save = new System.Windows.Forms.Button();
            this.checkBox_selectAll = new System.Windows.Forms.CheckBox();
            this.button_delete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog_Config = new System.Windows.Forms.OpenFileDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_interface_type_auto = new System.Windows.Forms.TextBox();
            this.button_test_print_auto = new System.Windows.Forms.Button();
            this.button_scan = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_printer_list = new System.Windows.Forms.ComboBox();
            this.button_add_auto = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_print
            // 
            this.button_print.Location = new System.Drawing.Point(268, 16);
            this.button_print.Name = "button_print";
            this.button_print.Size = new System.Drawing.Size(64, 23);
            this.button_print.TabIndex = 1;
            this.button_print.Text = "打印测试";
            this.button_print.UseVisualStyleBackColor = true;
            this.button_print.Click += new System.EventHandler(this.button_print_Click);
            // 
            // listView_Devices
            // 
            this.listView_Devices.CheckBoxes = true;
            this.listView_Devices.FullRowSelect = true;
            this.listView_Devices.GridLines = true;
            this.listView_Devices.LabelEdit = true;
            this.listView_Devices.Location = new System.Drawing.Point(6, 26);
            this.listView_Devices.MultiSelect = false;
            this.listView_Devices.Name = "listView_Devices";
            this.listView_Devices.Size = new System.Drawing.Size(653, 179);
            this.listView_Devices.TabIndex = 3;
            this.listView_Devices.UseCompatibleStateImageBehavior = false;
            this.listView_Devices.View = System.Windows.Forms.View.Details;
            this.listView_Devices.SelectedIndexChanged += new System.EventHandler(this.listView_Devices_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_printer_type);
            this.groupBox1.Controls.Add(this.radioButton_usb);
            this.groupBox1.Controls.Add(this.radioButton_ethernet);
            this.groupBox1.Controls.Add(this.radioButton_wifi);
            this.groupBox1.Controls.Add(this.radioButton_com);
            this.groupBox1.Controls.Add(this.radioButton_Driver);
            this.groupBox1.Controls.Add(this.textBox_print_id);
            this.groupBox1.Controls.Add(this.textBox_printName);
            this.groupBox1.Controls.Add(this.textBox_addr);
            this.groupBox1.Controls.Add(this.textBox_desc);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button_test_print);
            this.groupBox1.Controls.Add(this.button_new);
            this.groupBox1.Location = new System.Drawing.Point(27, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(665, 203);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "手动配置打印机";
            // 
            // comboBox_printer_type
            // 
            this.comboBox_printer_type.FormattingEnabled = true;
            this.comboBox_printer_type.Items.AddRange(new object[] {
            "58mm",
            "80mm"});
            this.comboBox_printer_type.Location = new System.Drawing.Point(421, 91);
            this.comboBox_printer_type.Name = "comboBox_printer_type";
            this.comboBox_printer_type.Size = new System.Drawing.Size(149, 20);
            this.comboBox_printer_type.TabIndex = 6;
            this.comboBox_printer_type.Text = "58mm";
            // 
            // radioButton_usb
            // 
            this.radioButton_usb.AutoSize = true;
            this.radioButton_usb.Location = new System.Drawing.Point(411, 28);
            this.radioButton_usb.Name = "radioButton_usb";
            this.radioButton_usb.Size = new System.Drawing.Size(41, 16);
            this.radioButton_usb.TabIndex = 5;
            this.radioButton_usb.Text = "USB";
            this.radioButton_usb.UseVisualStyleBackColor = true;
            // 
            // radioButton_ethernet
            // 
            this.radioButton_ethernet.AutoSize = true;
            this.radioButton_ethernet.Location = new System.Drawing.Point(298, 28);
            this.radioButton_ethernet.Name = "radioButton_ethernet";
            this.radioButton_ethernet.Size = new System.Drawing.Size(71, 16);
            this.radioButton_ethernet.TabIndex = 5;
            this.radioButton_ethernet.Text = "Ethernet";
            this.radioButton_ethernet.UseVisualStyleBackColor = true;
            // 
            // radioButton_wifi
            // 
            this.radioButton_wifi.AutoSize = true;
            this.radioButton_wifi.Location = new System.Drawing.Point(181, 28);
            this.radioButton_wifi.Name = "radioButton_wifi";
            this.radioButton_wifi.Size = new System.Drawing.Size(47, 16);
            this.radioButton_wifi.TabIndex = 5;
            this.radioButton_wifi.Text = "Wifi";
            this.radioButton_wifi.UseVisualStyleBackColor = true;
            // 
            // radioButton_com
            // 
            this.radioButton_com.AutoSize = true;
            this.radioButton_com.Location = new System.Drawing.Point(520, 28);
            this.radioButton_com.Name = "radioButton_com";
            this.radioButton_com.Size = new System.Drawing.Size(41, 16);
            this.radioButton_com.TabIndex = 4;
            this.radioButton_com.Text = "COM";
            this.radioButton_com.UseVisualStyleBackColor = true;
            // 
            // radioButton_Driver
            // 
            this.radioButton_Driver.AutoSize = true;
            this.radioButton_Driver.Checked = true;
            this.radioButton_Driver.Location = new System.Drawing.Point(69, 28);
            this.radioButton_Driver.Name = "radioButton_Driver";
            this.radioButton_Driver.Size = new System.Drawing.Size(59, 16);
            this.radioButton_Driver.TabIndex = 4;
            this.radioButton_Driver.TabStop = true;
            this.radioButton_Driver.Text = "Driver";
            this.radioButton_Driver.UseVisualStyleBackColor = true;
            // 
            // textBox_print_id
            // 
            this.textBox_print_id.Location = new System.Drawing.Point(69, 59);
            this.textBox_print_id.Name = "textBox_print_id";
            this.textBox_print_id.Size = new System.Drawing.Size(149, 21);
            this.textBox_print_id.TabIndex = 3;
            // 
            // textBox_printName
            // 
            this.textBox_printName.Location = new System.Drawing.Point(421, 54);
            this.textBox_printName.Name = "textBox_printName";
            this.textBox_printName.Size = new System.Drawing.Size(149, 21);
            this.textBox_printName.TabIndex = 3;
            // 
            // textBox_addr
            // 
            this.textBox_addr.Location = new System.Drawing.Point(69, 93);
            this.textBox_addr.Name = "textBox_addr";
            this.textBox_addr.Size = new System.Drawing.Size(149, 21);
            this.textBox_addr.TabIndex = 3;
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(69, 144);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(416, 21);
            this.textBox_desc.TabIndex = 3;
            this.textBox_desc.Text = "input description of printer...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(401, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "wifi or ethernet please input IP address, driver input driver name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(373, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "纸宽:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(372, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "名称:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "设备地址:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "描述:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Type:";
            // 
            // button_test_print
            // 
            this.button_test_print.Location = new System.Drawing.Point(332, 174);
            this.button_test_print.Name = "button_test_print";
            this.button_test_print.Size = new System.Drawing.Size(75, 23);
            this.button_test_print.TabIndex = 1;
            this.button_test_print.Text = "测试打印";
            this.button_test_print.UseVisualStyleBackColor = true;
            this.button_test_print.Click += new System.EventHandler(this.button_test_print_Click);
            // 
            // button_new
            // 
            this.button_new.Location = new System.Drawing.Point(449, 174);
            this.button_new.Name = "button_new";
            this.button_new.Size = new System.Drawing.Size(75, 23);
            this.button_new.TabIndex = 1;
            this.button_new.Text = "添加";
            this.button_new.UseVisualStyleBackColor = true;
            this.button_new.Click += new System.EventHandler(this.button_new_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.listView_Devices);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(27, 280);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(665, 256);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "当前配置打印机列表";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_save);
            this.groupBox3.Controls.Add(this.checkBox_selectAll);
            this.groupBox3.Controls.Add(this.button_print);
            this.groupBox3.Controls.Add(this.button_delete);
            this.groupBox3.Location = new System.Drawing.Point(68, 205);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(591, 43);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(352, 17);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 4;
            this.button_save.Text = "保存";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // checkBox_selectAll
            // 
            this.checkBox_selectAll.AutoSize = true;
            this.checkBox_selectAll.Location = new System.Drawing.Point(34, 20);
            this.checkBox_selectAll.Name = "checkBox_selectAll";
            this.checkBox_selectAll.Size = new System.Drawing.Size(48, 16);
            this.checkBox_selectAll.TabIndex = 3;
            this.checkBox_selectAll.Text = "全选";
            this.checkBox_selectAll.UseVisualStyleBackColor = true;
            this.checkBox_selectAll.CheckedChanged += new System.EventHandler(this.checkBox_selectAll_CheckedChanged);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(173, 15);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(75, 23);
            this.button_delete.TabIndex = 1;
            this.button_delete.Text = "删除";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "批量操作:";
            // 
            // openFileDialog_Config
            // 
            this.openFileDialog_Config.FileName = "config.xml";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.textBox_interface_type_auto);
            this.groupBox4.Controls.Add(this.button_test_print_auto);
            this.groupBox4.Controls.Add(this.button_scan);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.comboBox_printer_list);
            this.groupBox4.Controls.Add(this.button_add_auto);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(27, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(665, 53);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "自动扫描打印机（只支持驱动和USB自动扫描）";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(419, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 13;
            this.label11.Text = "mm";
            // 
            // textBox_interface_type_auto
            // 
            this.textBox_interface_type_auto.Location = new System.Drawing.Point(375, 19);
            this.textBox_interface_type_auto.Name = "textBox_interface_type_auto";
            this.textBox_interface_type_auto.Size = new System.Drawing.Size(38, 21);
            this.textBox_interface_type_auto.TabIndex = 12;
            this.textBox_interface_type_auto.Text = "58";
            // 
            // button_test_print_auto
            // 
            this.button_test_print_auto.Location = new System.Drawing.Point(520, 18);
            this.button_test_print_auto.Name = "button_test_print_auto";
            this.button_test_print_auto.Size = new System.Drawing.Size(62, 23);
            this.button_test_print_auto.TabIndex = 11;
            this.button_test_print_auto.Text = "测试打印";
            this.button_test_print_auto.UseVisualStyleBackColor = true;
            this.button_test_print_auto.Click += new System.EventHandler(this.button_test_print_auto_Click);
            // 
            // button_scan
            // 
            this.button_scan.Location = new System.Drawing.Point(456, 18);
            this.button_scan.Name = "button_scan";
            this.button_scan.Size = new System.Drawing.Size(59, 23);
            this.button_scan.TabIndex = 10;
            this.button_scan.Text = "扫描";
            this.button_scan.UseVisualStyleBackColor = true;
            this.button_scan.Click += new System.EventHandler(this.button_scan_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 12);
            this.label10.TabIndex = 9;
            this.label10.Text = "打印机列表:";
            // 
            // comboBox_printer_list
            // 
            this.comboBox_printer_list.FormattingEnabled = true;
            this.comboBox_printer_list.Location = new System.Drawing.Point(102, 20);
            this.comboBox_printer_list.Name = "comboBox_printer_list";
            this.comboBox_printer_list.Size = new System.Drawing.Size(206, 20);
            this.comboBox_printer_list.TabIndex = 8;
            // 
            // button_add_auto
            // 
            this.button_add_auto.Location = new System.Drawing.Point(585, 18);
            this.button_add_auto.Name = "button_add_auto";
            this.button_add_auto.Size = new System.Drawing.Size(60, 23);
            this.button_add_auto.TabIndex = 1;
            this.button_add_auto.Text = "添加";
            this.button_add_auto.UseVisualStyleBackColor = true;
            this.button_add_auto.Click += new System.EventHandler(this.button_new_auto_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(334, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "纸宽:";
            // 
            // PrinterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 548);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrinterConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "趣吃饭-打印机高级配置";
            this.Load += new System.EventHandler(this.PrinterConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_print;
        private System.Windows.Forms.ListView listView_Devices;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_new;
        private System.Windows.Forms.RadioButton radioButton_wifi;
        private System.Windows.Forms.RadioButton radioButton_Driver;
        private System.Windows.Forms.TextBox textBox_addr;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton_usb;
        private System.Windows.Forms.RadioButton radioButton_ethernet;
        private System.Windows.Forms.RadioButton radioButton_com;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_selectAll;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_test_print;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.ComboBox comboBox_printer_type;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_printName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.OpenFileDialog openFileDialog_Config;
        private System.Windows.Forms.TextBox textBox_print_id;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_scan;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox_printer_list;
        private System.Windows.Forms.Button button_test_print_auto;
        private System.Windows.Forms.Button button_add_auto;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_interface_type_auto;
        private System.Windows.Forms.Label label8;
    }
}

