namespace Simcpa
{
    partial class JobWorker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobWorker));
            this.button_print = new System.Windows.Forms.Button();
            this.listView_order_list = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_clear = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_print
            // 
            this.button_print.Location = new System.Drawing.Point(286, 415);
            this.button_print.Name = "button_print";
            this.button_print.Size = new System.Drawing.Size(104, 28);
            this.button_print.TabIndex = 0;
            this.button_print.Text = "打印";
            this.button_print.UseVisualStyleBackColor = true;
            this.button_print.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView_order_list
            // 
            this.listView_order_list.CheckBoxes = true;
            this.listView_order_list.FullRowSelect = true;
            this.listView_order_list.Location = new System.Drawing.Point(6, 26);
            this.listView_order_list.MultiSelect = false;
            this.listView_order_list.Name = "listView_order_list";
            this.listView_order_list.Size = new System.Drawing.Size(481, 348);
            this.listView_order_list.TabIndex = 2;
            this.listView_order_list.UseCompatibleStateImageBehavior = false;
            this.listView_order_list.View = System.Windows.Forms.View.Details;
            this.listView_order_list.SelectedIndexChanged += new System.EventHandler(this.listView_order_list_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 420);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择下面的订单列表，可以执行打印";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView_order_list);
            this.groupBox1.Location = new System.Drawing.Point(12, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(493, 380);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "订单列表";
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(396, 415);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(87, 28);
            this.button_clear.TabIndex = 5;
            this.button_clear.Text = "清空";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // JobWorker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 455);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_print);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JobWorker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "趣吃饭-打印订单列表";
            this.Load += new System.EventHandler(this.JobWorker_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_print;
        private System.Windows.Forms.ListView listView_order_list;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_clear;
    }
}