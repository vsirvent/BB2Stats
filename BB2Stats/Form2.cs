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
    public partial class BBStatsMain : Form
    {
        public BBStatsMain()
        {
            InitializeComponent();
            BB2StatsForm team1 = new BB2StatsForm();
            BB2StatsForm team2 = new BB2StatsForm();
            team1.TopLevel = false;
            team2.TopLevel = false;
            this.panel1.Controls.Add(team1);
            this.panel2.Controls.Add(team2);
            team1.Show();
            team2.Show();
        }
    }
}
