using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zipper
{
    public partial class MessageBoxForm : Form
    {
        public MessageBoxForm(string title, string description)
        {
            InitializeComponent();
            if (Properties.Settings.Default.Dark == true)
            {
                this.BackColor = Color.FromArgb(48, 48, 48);
                TitleLabel.ForeColor = Color.FromArgb(255, 255, 255);
                DescriptionLabel.ForeColor = Color.FromArgb(255, 255, 255);
                CancelButton.BackColor = Color.FromArgb(88, 88, 88);
                CancelButton.ForeColor = Color.FromArgb(255, 255, 255);
                UseImmersiveDarkMode(this.Handle, IsWindows10OrGreater());
            }
            TitleLabel.Text = title;
            DescriptionLabel.Text = description;
        }

        private void ActionButton_Click(object sender, EventArgs e)
        {
            this.Hide();
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
