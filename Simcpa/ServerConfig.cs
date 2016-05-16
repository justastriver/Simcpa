using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ISimcpa.Task;
using ISimcpa.Net;
using ISimcpa.Util;
using ISimcpa.Config;
using Microsoft.Win32;

namespace Simcpa
{
    public partial class ServerConfig : Form
    {
        private string config_file_name = "../../Config/config.xml";
        
        public ServerConfig(string config_file_name)
        {
            InitializeComponent();
            this.config_file_name = config_file_name;
        }

        private void button_start_service_Click(object sender, EventArgs e)
        {
        }
        private void button_printer_config_Click(object sender, EventArgs e)
        {
            
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            SystemConfig cfg = new SystemConfig();
            cfg.restaurant_id = this.textBox_id.Text;
            cfg.server_ip = this.textBox_ip.Text;
            cfg.server_port = this.textBox_port.Text;

            if (XMLParser.SaveSystemConfig(cfg, config_file_name) == true)
            {
                MessageBox.Show("Save Configure Ok !");
            }
            else
            {
                MessageBox.Show("Save Configure Failed !");
            }
            this.Close();
        }

        private void WPService_Load(object sender, EventArgs e)
        {
            Setting setting = XMLParser.Read(config_file_name);
            if (setting == null)
            {
                MessageBox.Show("Load Config error !");
            }
            else
            {
                this.textBox_id.Text = setting.system.restaurant_id;
                this.textBox_ip.Text = setting.system.server_ip;
                this.textBox_port.Text = setting.system.server_port;
            }
            Logger.Debug("Load ok !");
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox_autorun_EnabledChanged(object sender, EventArgs e)
        {
            /*
            if (this.checkBox_autorun.Checked)
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.SetValue("Simcpa", path);
                rk2.Close();
                rk.Close();
            }
            else
            {
                string path = Application.ExecutablePath;
                RegistryKey rk = Registry.LocalMachine;
                RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                rk2.DeleteValue("Simcpa", false);
                rk2.Close();
                rk.Close();
            }
             */ 
            
        }
    }
}
