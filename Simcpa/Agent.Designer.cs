namespace Simcpa
{
    partial class Agent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Agent));
            this.button_start = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_sys_log = new System.Windows.Forms.TextBox();
            this.button_more = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listView_printer_status = new System.Windows.Forms.ListView();
            this.button_setting_printer = new System.Windows.Forms.Button();
            this.notifyIcon_system = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip_notify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_set = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_orders_list = new System.Windows.Forms.Button();
            this.label_curr_order_id = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_order_counter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_service_running_status = new System.Windows.Forms.Label();
            this.timer_status = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_start_service = new System.Windows.Forms.Button();
            this.button_restart_service = new System.Windows.Forms.Button();
            this.button_stop_service = new System.Windows.Forms.Button();
            this.button_uninstall = new System.Windows.Forms.Button();
            this.button_install = new System.Windows.Forms.Button();
            this.button_onekey_install_run_simcpa = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_onkey_install_run_update = new System.Windows.Forms.Button();
            this.button_onekey_uninstall_update = new System.Windows.Forms.Button();
            this.button_restart_autoupdate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip_notify.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(394, 45);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(68, 31);
            this.button_start.TabIndex = 3;
            this.button_start.Text = "启动测试";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_sys_log);
            this.groupBox1.Location = new System.Drawing.Point(12, 372);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(564, 186);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "系统日志";
            // 
            // textBox_sys_log
            // 
            this.textBox_sys_log.Location = new System.Drawing.Point(6, 20);
            this.textBox_sys_log.Multiline = true;
            this.textBox_sys_log.Name = "textBox_sys_log";
            this.textBox_sys_log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_sys_log.Size = new System.Drawing.Size(546, 160);
            this.textBox_sys_log.TabIndex = 0;
            // 
            // button_more
            // 
            this.button_more.Location = new System.Drawing.Point(394, 10);
            this.button_more.Name = "button_more";
            this.button_more.Size = new System.Drawing.Size(68, 31);
            this.button_more.TabIndex = 6;
            this.button_more.Text = "服务配置";
            this.button_more.UseVisualStyleBackColor = true;
            this.button_more.Click += new System.EventHandler(this.button_more_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listView_printer_status);
            this.groupBox3.Location = new System.Drawing.Point(12, 231);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(564, 129);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "打印机队列";
            // 
            // listView_printer_status
            // 
            this.listView_printer_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_printer_status.FullRowSelect = true;
            this.listView_printer_status.Location = new System.Drawing.Point(3, 17);
            this.listView_printer_status.MultiSelect = false;
            this.listView_printer_status.Name = "listView_printer_status";
            this.listView_printer_status.Size = new System.Drawing.Size(558, 109);
            this.listView_printer_status.TabIndex = 7;
            this.listView_printer_status.UseCompatibleStateImageBehavior = false;
            this.listView_printer_status.View = System.Windows.Forms.View.Details;
            // 
            // button_setting_printer
            // 
            this.button_setting_printer.Location = new System.Drawing.Point(484, 10);
            this.button_setting_printer.Name = "button_setting_printer";
            this.button_setting_printer.Size = new System.Drawing.Size(68, 31);
            this.button_setting_printer.TabIndex = 6;
            this.button_setting_printer.Text = "打印设置";
            this.button_setting_printer.UseVisualStyleBackColor = true;
            this.button_setting_printer.Click += new System.EventHandler(this.button_setting_printer_Click);
            // 
            // notifyIcon_system
            // 
            this.notifyIcon_system.BalloonTipText = "趣吃饭-智能打印客户端";
            this.notifyIcon_system.BalloonTipTitle = "趣吃饭";
            this.notifyIcon_system.ContextMenuStrip = this.contextMenuStrip_notify;
            this.notifyIcon_system.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon_system.Icon")));
            this.notifyIcon_system.Text = "趣吃饭-智能打印客户端";
            this.notifyIcon_system.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_system_MouseDoubleClick);
            // 
            // contextMenuStrip_notify
            // 
            this.contextMenuStrip_notify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_set,
            this.toolStripMenuItem_exit});
            this.contextMenuStrip_notify.Name = "contextMenuStrip_notify";
            this.contextMenuStrip_notify.Size = new System.Drawing.Size(95, 48);
            this.contextMenuStrip_notify.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_notify_ItemClicked);
            // 
            // toolStripMenuItem_set
            // 
            this.toolStripMenuItem_set.Name = "toolStripMenuItem_set";
            this.toolStripMenuItem_set.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem_set.Text = "设置";
            // 
            // toolStripMenuItem_exit
            // 
            this.toolStripMenuItem_exit.Name = "toolStripMenuItem_exit";
            this.toolStripMenuItem_exit.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem_exit.Text = "退出";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_orders_list);
            this.groupBox2.Controls.Add(this.label_curr_order_id);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label_order_counter);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label_service_running_status);
            this.groupBox2.Controls.Add(this.button_start);
            this.groupBox2.Controls.Add(this.button_setting_printer);
            this.groupBox2.Controls.Add(this.button_more);
            this.groupBox2.Location = new System.Drawing.Point(18, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(558, 82);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "系统设置";
            // 
            // button_orders_list
            // 
            this.button_orders_list.Location = new System.Drawing.Point(484, 44);
            this.button_orders_list.Name = "button_orders_list";
            this.button_orders_list.Size = new System.Drawing.Size(68, 30);
            this.button_orders_list.TabIndex = 11;
            this.button_orders_list.Text = "任务列表";
            this.button_orders_list.UseVisualStyleBackColor = true;
            this.button_orders_list.Click += new System.EventHandler(this.button_orders_list_Click);
            // 
            // label_curr_order_id
            // 
            this.label_curr_order_id.AutoSize = true;
            this.label_curr_order_id.Location = new System.Drawing.Point(143, 62);
            this.label_curr_order_id.Name = "label_curr_order_id";
            this.label_curr_order_id.Size = new System.Drawing.Size(11, 12);
            this.label_curr_order_id.TabIndex = 10;
            this.label_curr_order_id.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "当前订单号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "处理订单数:";
            // 
            // label_order_counter
            // 
            this.label_order_counter.AutoSize = true;
            this.label_order_counter.Location = new System.Drawing.Point(143, 39);
            this.label_order_counter.Name = "label_order_counter";
            this.label_order_counter.Size = new System.Drawing.Size(11, 12);
            this.label_order_counter.TabIndex = 8;
            this.label_order_counter.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "状态：";
            // 
            // label_service_running_status
            // 
            this.label_service_running_status.AutoSize = true;
            this.label_service_running_status.Location = new System.Drawing.Point(53, 20);
            this.label_service_running_status.Name = "label_service_running_status";
            this.label_service_running_status.Size = new System.Drawing.Size(53, 12);
            this.label_service_running_status.TabIndex = 7;
            this.label_service_running_status.Text = "运行正常";
            // 
            // timer_status
            // 
            this.timer_status.Interval = 1000;
            this.timer_status.Tick += new System.EventHandler(this.timer_status_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_onekey_install_run_simcpa);
            this.groupBox4.Controls.Add(this.button_start_service);
            this.groupBox4.Controls.Add(this.button_restart_service);
            this.groupBox4.Controls.Add(this.button_stop_service);
            this.groupBox4.Controls.Add(this.button_uninstall);
            this.groupBox4.Controls.Add(this.button_install);
            this.groupBox4.Location = new System.Drawing.Point(12, 97);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(564, 61);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "主服务(SimcpaService)控制";
            // 
            // button_start_service
            // 
            this.button_start_service.Location = new System.Drawing.Point(311, 23);
            this.button_start_service.Name = "button_start_service";
            this.button_start_service.Size = new System.Drawing.Size(75, 23);
            this.button_start_service.TabIndex = 0;
            this.button_start_service.Text = "启动";
            this.button_start_service.UseVisualStyleBackColor = true;
            this.button_start_service.Click += new System.EventHandler(this.button_start_service_Click);
            // 
            // button_restart_service
            // 
            this.button_restart_service.Location = new System.Drawing.Point(477, 23);
            this.button_restart_service.Name = "button_restart_service";
            this.button_restart_service.Size = new System.Drawing.Size(75, 23);
            this.button_restart_service.TabIndex = 0;
            this.button_restart_service.Text = "重启";
            this.button_restart_service.UseVisualStyleBackColor = true;
            this.button_restart_service.Click += new System.EventHandler(this.button_restart_service_Click);
            // 
            // button_stop_service
            // 
            this.button_stop_service.Location = new System.Drawing.Point(393, 23);
            this.button_stop_service.Name = "button_stop_service";
            this.button_stop_service.Size = new System.Drawing.Size(75, 23);
            this.button_stop_service.TabIndex = 0;
            this.button_stop_service.Text = "停止";
            this.button_stop_service.UseVisualStyleBackColor = true;
            this.button_stop_service.Click += new System.EventHandler(this.button_stop_service_Click);
            // 
            // button_uninstall
            // 
            this.button_uninstall.Location = new System.Drawing.Point(220, 23);
            this.button_uninstall.Name = "button_uninstall";
            this.button_uninstall.Size = new System.Drawing.Size(75, 23);
            this.button_uninstall.TabIndex = 0;
            this.button_uninstall.Text = "卸载";
            this.button_uninstall.UseVisualStyleBackColor = true;
            this.button_uninstall.Click += new System.EventHandler(this.button_uninstall_Click);
            // 
            // button_install
            // 
            this.button_install.Location = new System.Drawing.Point(139, 23);
            this.button_install.Name = "button_install";
            this.button_install.Size = new System.Drawing.Size(75, 23);
            this.button_install.TabIndex = 0;
            this.button_install.Text = "安装";
            this.button_install.UseVisualStyleBackColor = true;
            this.button_install.Click += new System.EventHandler(this.button_install_Click);
            // 
            // button_onekey_install_run_simcpa
            // 
            this.button_onekey_install_run_simcpa.Location = new System.Drawing.Point(14, 18);
            this.button_onekey_install_run_simcpa.Name = "button_onekey_install_run_simcpa";
            this.button_onekey_install_run_simcpa.Size = new System.Drawing.Size(106, 32);
            this.button_onekey_install_run_simcpa.TabIndex = 1;
            this.button_onekey_install_run_simcpa.Text = "一键安装启动";
            this.button_onekey_install_run_simcpa.UseVisualStyleBackColor = true;
            this.button_onekey_install_run_simcpa.Click += new System.EventHandler(this.button_onekey_install_run_simcpa_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_restart_autoupdate);
            this.groupBox5.Controls.Add(this.button_onekey_uninstall_update);
            this.groupBox5.Controls.Add(this.button_onkey_install_run_update);
            this.groupBox5.Location = new System.Drawing.Point(12, 165);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(564, 60);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "自动升级服务（SimcpaAutoUpdateService)控制";
            // 
            // button_onkey_install_run_update
            // 
            this.button_onkey_install_run_update.Location = new System.Drawing.Point(14, 20);
            this.button_onkey_install_run_update.Name = "button_onkey_install_run_update";
            this.button_onkey_install_run_update.Size = new System.Drawing.Size(106, 34);
            this.button_onkey_install_run_update.TabIndex = 0;
            this.button_onkey_install_run_update.Text = "一键安装启动";
            this.button_onkey_install_run_update.UseVisualStyleBackColor = true;
            this.button_onkey_install_run_update.Click += new System.EventHandler(this.button_onkey_install_run_update_Click);
            // 
            // button_onekey_uninstall_update
            // 
            this.button_onekey_uninstall_update.Location = new System.Drawing.Point(220, 26);
            this.button_onekey_uninstall_update.Name = "button_onekey_uninstall_update";
            this.button_onekey_uninstall_update.Size = new System.Drawing.Size(75, 26);
            this.button_onekey_uninstall_update.TabIndex = 1;
            this.button_onekey_uninstall_update.Text = "卸载";
            this.button_onekey_uninstall_update.UseVisualStyleBackColor = true;
            this.button_onekey_uninstall_update.Click += new System.EventHandler(this.button_onekey_uninstall_update_Click);
            // 
            // button_restart_autoupdate
            // 
            this.button_restart_autoupdate.Location = new System.Drawing.Point(477, 29);
            this.button_restart_autoupdate.Name = "button_restart_autoupdate";
            this.button_restart_autoupdate.Size = new System.Drawing.Size(65, 23);
            this.button_restart_autoupdate.TabIndex = 2;
            this.button_restart_autoupdate.Text = "重启";
            this.button_restart_autoupdate.UseVisualStyleBackColor = true;
            this.button_restart_autoupdate.Click += new System.EventHandler(this.button_restart_autoupdate_Click);
            // 
            // Agent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 560);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Agent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "趣吃饭-打印机客户端助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WPSmart_FormClosing);
            this.Load += new System.EventHandler(this.WPSmart_Load);
            this.SizeChanged += new System.EventHandler(this.WPSmart_SizeChanged);
            this.Resize += new System.EventHandler(this.WPSmart_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.contextMenuStrip_notify.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_sys_log;
        private System.Windows.Forms.Button button_more;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_setting_printer;
        private System.Windows.Forms.NotifyIcon notifyIcon_system;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_notify;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_set;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_exit;
        private System.Windows.Forms.ListView listView_printer_status;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_service_running_status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer_status;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_order_counter;
        private System.Windows.Forms.Label label_curr_order_id;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_orders_list;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_start_service;
        private System.Windows.Forms.Button button_restart_service;
        private System.Windows.Forms.Button button_stop_service;
        private System.Windows.Forms.Button button_uninstall;
        private System.Windows.Forms.Button button_install;
        private System.Windows.Forms.Button button_onekey_install_run_simcpa;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button_onekey_uninstall_update;
        private System.Windows.Forms.Button button_onkey_install_run_update;
        private System.Windows.Forms.Button button_restart_autoupdate;
    }
}