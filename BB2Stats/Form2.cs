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
            team1.MouseDown += Form1_MouseDown;
            team1.MouseUp += Form1_MouseUp;
            team1.MouseMove += Form1_MouseMove;
            team2.MouseDown += Form1_MouseDown;
            team2.MouseUp += Form1_MouseUp;
            team2.MouseMove += Form1_MouseMove;
            checkBox1.Checked = true;
            this.panel1.Controls.Add(team1);
            this.panel2.Controls.Add(team2);
            team1.Show();
            team2.Show();
        }

        private bool IsFormBeingDragged = false;
        private Point OrigPosition;
        private Point MouseDownPosition;

        private void Form1_MouseDown(Object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                IsFormBeingDragged = true;
                MouseDownPosition = MousePosition;
                OrigPosition = this.Location;
                System.Console.WriteLine("Mouse Down");
            }
        }

        private void Form1_MouseUp(Object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                IsFormBeingDragged = false;
                System.Console.WriteLine("Mouse Up");
            }
        }

        private void Form1_MouseMove(Object sender, MouseEventArgs e)
        {
            if (IsFormBeingDragged) {
                Point temp = new Point();
                temp.X = OrigPosition.X + (MousePosition.X - MouseDownPosition.X);
                temp.Y = OrigPosition.Y + (MousePosition.Y - MouseDownPosition.Y);
                this.Location = temp;
                System.Console.WriteLine("Mouse Move");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                panel1.Enabled = true;
                panel2.Enabled = false;
            }
            else
            {
                panel2.Enabled = true;
                panel1.Enabled = false;
            }
        }
    }
}
