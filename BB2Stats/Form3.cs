using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BB2Stats
{
    public partial class ConfigForm : Form
    {
        private Sniffer sniffer;
        public ConfigForm(Sniffer sniffer)
        {
            InitializeComponent();
            this.sniffer = sniffer;

            List<String> eth_list = sniffer.GetDevices();
            comboBox1.Items.Clear();
            foreach (String eth in eth_list)
            {
                comboBox1.Items.Add(eth);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        public bool IsPublishActive()
        {
            return publishMqtt.Checked;
        }

        public bool IsSubscribeActive()
        {
            return subscribeMqtt.Checked;
        }

        public string getSessionId()
        {
            return textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (snifferActive.Checked)
            {
                if (comboBox1.Text != "")
                {
                    sniffer.SetDevice(comboBox1.Text);
                    sniffer.Run();
                }
            }
            else
            {
                sniffer.Stop();
            }
            this.Hide();
        }

        private void snifferActive_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = snifferActive.Checked;
        }
    }
}
