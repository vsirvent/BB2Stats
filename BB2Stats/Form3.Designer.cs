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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // publishMqtt
            // 
            this.publishMqtt.AutoSize = true;
            this.publishMqtt.Location = new System.Drawing.Point(169, 42);
            this.publishMqtt.Name = "publishMqtt";
            this.publishMqtt.Size = new System.Drawing.Size(71, 17);
            this.publishMqtt.TabIndex = 0;
            this.publishMqtt.Text = "PUBLISH";
            this.publishMqtt.UseVisualStyleBackColor = true;
            // 
            // subscribeMqtt
            // 
            this.subscribeMqtt.AutoSize = true;
            this.subscribeMqtt.Location = new System.Drawing.Point(169, 65);
            this.subscribeMqtt.Name = "subscribeMqtt";
            this.subscribeMqtt.Size = new System.Drawing.Size(93, 17);
            this.subscribeMqtt.TabIndex = 1;
            this.subscribeMqtt.Text = "SUSBSCRIBE";
            this.subscribeMqtt.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(48, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "1234";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
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
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(289, 100);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Session";
            // 
            // disabledMqtt
            // 
            this.disabledMqtt.AutoSize = true;
            this.disabledMqtt.Checked = true;
            this.disabledMqtt.Location = new System.Drawing.Point(169, 19);
            this.disabledMqtt.Name = "disabledMqtt";
            this.disabledMqtt.Size = new System.Drawing.Size(78, 17);
            this.disabledMqtt.TabIndex = 4;
            this.disabledMqtt.TabStop = true;
            this.disabledMqtt.Text = "DISABLED";
            this.disabledMqtt.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 37);
            this.button1.TabIndex = 5;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 166);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfigForm";
            this.Text = "Settings";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}