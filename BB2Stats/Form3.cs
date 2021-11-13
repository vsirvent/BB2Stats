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
        public ConfigForm()
        {
            InitializeComponent();
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
            this.Hide();
        }
    }
}
