using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Zipper
{
    public partial class ArchivePopup : Form
    {
        public ArchivePopup()
        {
            InitializeComponent();
            if (Properties.Settings.Default.Dark == true)
            {
                UseImmersiveDarkMode(this.Handle, IsWindows10OrGreater());
                this.BackColor = Color.FromArgb(25, 25, 25);
                button2.BackColor = Color.FromArgb(88, 88, 88);
                button2.ForeColor = Color.FromArgb(255, 255, 255);
                button3.BackColor = Color.FromArgb(88, 88, 88);
                button3.ForeColor = Color.FromArgb(255, 255, 255);
                checkBox1.ForeColor = Color.FromArgb(255, 255, 255);
                label1.ForeColor = Color.FromArgb(255, 255, 255);
                label3.ForeColor = Color.FromArgb(255, 255, 255);
                textBox1.BackColor = Color.FromArgb(48, 48, 48);
                textBox2.BackColor = Color.FromArgb(48, 48, 48);
                textBox1.ForeColor = Color.FromArgb(255, 255, 255);
                textBox2.ForeColor = Color.FromArgb(255, 255, 255);
                progressBar1.BackColor = Color.FromArgb(48, 48, 48);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = saveFileDialog1.FileName;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.ReadOnly = false;
            }
            else
                textBox1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    progressBar1.Maximum = openFileDialog1.FileNames.Length;
                    using (var archive = ZipArchive.Create())
                    {
                        foreach (String file in openFileDialog1.FileNames)
                        {
                            FileInfo fi = new FileInfo(file);
                            archive.AddEntry(fi.Name, file);
                            progressBar1.PerformStep();
                        }
                        archive.SaveTo(textBox2.Text, CompressionType.Deflate);
                        MessageBox.Show("Archive created.");
                        this.Hide();
                    }
                }
                catch
                {
                    MessageBox.Show("The archival has failed.", "Archive Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmSetWindowAttribute(handle, (int)attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }

            return false;
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }
    }
   
    }
