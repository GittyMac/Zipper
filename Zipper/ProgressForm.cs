using SharpCompress.Archives;
using SharpCompress.Common;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Zipper
{
    public partial class ProgressForm : Form
    {
        public int maxprogress;
        public int progress;
        public ProgressForm()
        {
            InitializeComponent();
            if (Properties.Settings.Default.Dark == true)
            {
                UseImmersiveDarkMode(this.Handle, IsWindows10OrGreater());
                this.BackColor = Color.FromArgb(32, 32, 32);
                label1.ForeColor = Color.FromArgb(255, 255, 255);
            }
        }

        public void Step()
        {
            //MessageBox.Show(progressBar1.Maximum.ToString() + progressBar1.Minimum.ToString() + progressBar1.Value.ToString());
            progressBar1.PerformStep();
        }

        public void maxp(int progress)
        {
            progressBar1.Maximum = progress;
            progressBar1.Value = 0;
        }

        public void extractFromCMD(string File)
        {
            this.Show();
            var archive = ArchiveFactory.Open(File);
            maxprogress = archive.Entries.Count();
            maxp(maxprogress);
            foreach (var entry in archive.Entries)
            {
                entry.WriteToDirectory(Environment.CurrentDirectory, new ExtractionOptions()
                {
                    ExtractFullPath = true,
                    Overwrite = true
                });
                Step();
            }
            this.Close();
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
