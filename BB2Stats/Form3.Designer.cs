namespace BB2Stats
{
    partial class ConfigForm
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
            this.publishMqtt = new System.Windows.Forms.RadioButton();
            this.subscribeMqtt = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.disabledMqtt = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.snifferActive = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // publishMqtt
            // 
            this.publishMqtt.AutoSize = true;
            this.publishMqtt.Location = new System.Drawing.Point(225, 52);
            this.publishMqtt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.publishMqtt.Name = "publishMqtt";
            this.publishMqtt.Size = new System.Drawing.Size(85, 20);
            this.publishMqtt.TabIndex = 0;
            this.publishMqtt.Text = "PUBLISH";
            this.publishMqtt.UseVisualStyleBackColor = true;
            // 
            // subscribeMqtt
            // 
            this.subscribeMqtt.AutoSize = true;
            this.subscribeMqtt.Location = new System.Drawing.Point(225, 80);
            this.subscribeMqtt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.subscribeMqtt.Name = "subscribeMqtt";
            this.subscribeMqtt.Size = new System.Drawing.Size(114, 20);
            this.subscribeMqtt.TabIndex = 1;
            this.subscribeMqtt.Text = "SUSBSCRIBE";
            this.subscribeMqtt.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(64, 37);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "1234";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "ID:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.disabledMqtt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.publishMqtt);
            this.groupBox1.Controls.Add(this.subscribeMqtt);
            this.groupBox1.Location = new System.Drawing.Point(17, 16);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(650, 123);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Session";
            // 
            // disabledMqtt
            // 
            this.disabledMqtt.AutoSize = true;
            this.disabledMqtt.Checked = true;
            this.disabledMqtt.Location = new System.Drawing.Point(225, 23);
            this.disabledMqtt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.disabledMqtt.Name = "disabledMqtt";
            this.disabledMqtt.Size = new System.Drawing.Size(94, 20);
            this.disabledMqtt.TabIndex = 4;
            this.disabledMqtt.TabStop = true;
            this.disabledMqtt.Text = "DISABLED";
            this.disabledMqtt.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(204, 277);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 46);
            this.button1.TabIndex = 5;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.snifferActive);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(17, 146);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(650, 123);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sniffer";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(69, 65);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(573, 24);
            this.comboBox1.TabIndex = 2;
            // 
            // snifferActive
            // 
            this.snifferActive.AutoSize = true;
            this.snifferActive.Location = new System.Drawing.Point(11, 37);
            this.snifferActive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.snifferActive.Name = "snifferActive";
            this.snifferActive.Size = new System.Drawing.Size(66, 20);
            this.snifferActive.TabIndex = 1;
            this.snifferActive.Text = "Active";
            this.snifferActive.UseVisualStyleBackColor = true;
            this.snifferActive.CheckedChanged += new System.EventHandler(this.snifferActive_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Device:";
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 340);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ConfigForm";
            this.Text = "Settings";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton publishMqtt;
        private System.Windows.Forms.RadioButton subscribeMqtt;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton disabledMqtt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox snifferActive;
        private System.Windows.Forms.Label label2;
    }
}