using System;
using System.Drawing;
using System.Windows.Forms;

namespace Zipper
{
    public partial class OptionsForm : Form
    {
        bool currentDark = Properties.Settings.Default.Dark;

        public OptionsForm()
        {
            InitializeComponent();
            if (Properties.Settings.Default.Dark == true)
            {
                this.BackColor = Color.FromArgb(48, 48, 48);
                label1.ForeColor = Color.FromArgb(255, 255, 255);
                checkBox1.ForeColor = Color.FromArgb(255, 255, 255);
                checkBox2.ForeColor = Color.FromArgb(255, 255, 255);
                button2.BackColor = Color.FromArgb(88, 88, 88);
                button2.ForeColor = Color.FromArgb(255, 255, 255);
                comboBox1.BackColor = Color.FromArgb(88, 88, 88);
                comboBox1.ForeColor = Color.FromArgb(255, 255, 255);
            }
            checkBox1.Checked = Properties.Settings.Default.Dark;
            checkBox2.Checked = Properties.Settings.Default.CheckToggles;
            comboBox1.SelectedItem = Properties.Settings.Default.ListStyle;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MainForm form1 = new MainForm();
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.Dark = true;
            }
            else
                Properties.Settings.Default.Dark = false;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(currentDark != Properties.Settings.Default.Dark)
            {
                Application.Restart();
            }
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Properties.Settings.Default.CheckToggles = true;
            }
            else
                Properties.Settings.Default.CheckToggles = false;
            Properties.Settings.Default.Save();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ListStyle = comboBox1.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
