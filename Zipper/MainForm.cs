using SharpCompress.Archives;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Zipper
{

    //Zipper
    //2020-2022 Lako
    
    //MAJOR TODO - Work with strange ways of handling directories.
    //Like archives that don't explicitly mention directorys (ex. folder/file.ext)
    //Or archives that are all over the place.

    public partial class MainForm : Form
    {
        public int progress;
        public int maxprogress;
        private ProgressForm progressBarForm = new ProgressForm();
        public string fileName;
        public string dirName;

        public MainForm()
        {
            InitializeComponent();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

            menuStrip1.ForeColor = Color.FromArgb(49, 49, 49); //To keep consistentcy with Windows' titlebar.

            this.listView1.SizeChanged += new EventHandler(ListView_SizeChanged);
            this.menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            this.menuStrip1.Renderer = new ToolStripProfessionalRenderer(new ZipCols());

            foreach (ColumnHeader header in listView1.Columns)
            {
                if (header.Text == "Modified")
                {
                    header.Width = -2;
                }
            }


            //Reading user settings.

            switch (Properties.Settings.Default.ListStyle)
            {
                case "List":
                    listView1.View = View.List;
                    break;

                case "Details":
                    listView1.View = View.Details;
                    break;

                case "Tile":
                    listView1.View = View.Tile;
                    break;

                case "SmallIcon":
                    listView1.View = View.SmallIcon;
                    break;

                case "LargeIcon":
                    listView1.View = View.LargeIcon;
                    break;

            }

            if (Properties.Settings.Default.CheckToggles == true)
            {
                try
                {
                    listView1.CheckBoxes = true;
                }
                catch //If the current view does not support checkboxes.
                {
                    listView1.CheckBoxes = false;
                    Properties.Settings.Default.CheckToggles = false;
                    Properties.Settings.Default.Save();
                }
            }

            if (Properties.Settings.Default.Dark == true)
            {
                UseImmersiveDarkMode(this.Handle, IsWindows10OrGreater());
                this.BackColor = Color.FromArgb(00, 00, 00);
                listView1.BackColor = Color.FromArgb(25, 25, 25);
                listView1.ForeColor = Color.FromArgb(255, 255, 255);
                listView1.OwnerDraw = true;
                menuStrip1.BackColor = Color.FromArgb(00, 00, 00);
                menuStrip1.ForeColor = Color.FromArgb(255, 255, 255);

            }
        }

        private void ListView_SizeChanged(object sender, EventArgs e)
        {
            foreach (ColumnHeader header in listView1.Columns)
            {
                if (header.Text == "Modified")
                {
                    header.Width = -2;
                }
            }
        }

        public void openFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    dirName = "";
                    fileName = openFileDialog1.FileName;
                    var archive = ArchiveFactory.Open(fileName);
                    this.Text = "Zipper - " + openFileDialog1.FileName;
                    listView1.Items.Clear();
                    Properties.Settings.Default.Save();
                    foreach (var arcentry in archive.Entries)
                    {
                        //Do something with this
                        //(dirName == "" && Path.GetFileName(Path.GetDirectoryName(entry.Key)) == @"\" && entry.IsDirectory)

                        Debug.WriteLine(arcentry.Key);

                        if (arcentry.Key == dirName + Path.GetFileName(arcentry.Key) ||
                            arcentry.Key == dirName + "/" + Path.GetDirectoryName(arcentry.Key) + "/" ||
                            arcentry.Key == dirName + "/" + Path.GetFileName(arcentry.Key) ||
                            arcentry.Key == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) ||
                            arcentry.Key == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) + "/" ||
                            (Path.GetDirectoryName(arcentry.Key) == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) && arcentry.IsDirectory) ||
                            (Path.GetDirectoryName(arcentry.Key) == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) + "/" && arcentry.IsDirectory)
                           )
                        {
                            ListViewItem lvi = new ListViewItem(arcentry.Key, 1);
                            lvi.SubItems.Add(arcentry.Size.ToString("N0"));
                            lvi.SubItems.Add(arcentry.LastModifiedTime.ToString());
                            listView1.Items.Add(lvi);
                        }
                    }
                }
                catch
                {
                    OpenMessageBox("Invalid Archive", "The selected archive is not a valid archive.");
                }
            }
        }

        public void openFileFromCMD(String file)
        {
            try
            {
                var archive = ArchiveFactory.Open(file);
                this.Text = "Zipper - " + file;
                fileName = file;
                listView1.Items.Clear();
                foreach (var entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        ListViewItem lvi = new ListViewItem(entry.Key);
                        lvi.SubItems.Add(entry.Size.ToString("N0"));
                        lvi.SubItems.Add(entry.LastModifiedTime.ToString());
                        listView1.Items.Add(lvi);
                    }
                }
            }
            catch
            {
                OpenMessageBox("Invalid Archive", "The selected archive is not a valid archive.");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0) //If no archive is open or if there's nothing to extract.
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                    var archive = ArchiveFactory.Open(fileName);
                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        maxprogress = archive.Entries.Count();
                        progress = 0;
                        progressBarForm.maxp(maxprogress);
                        progressBarForm.Show();
                        foreach (var entry in archive.Entries)
                        {
                            entry.WriteToDirectory(folderBrowserDialog1.SelectedPath, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                            progress += 1;
                        }
                        progressBarForm.Hide();
                        OpenMessageBox("Extraction", "The archive has been successfully extracted.");
                    }
                }
            }
            else if (listView1.SelectedItems.Count == 0) //If no items are selected, Zipper will automatically extract all files.
            {
                var archive = ArchiveFactory.Open(fileName);
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    maxprogress = archive.Entries.Count();
                    progress = 0;
                    progressBarForm.maxp(maxprogress);
                    progressBarForm.StartPosition = FormStartPosition.CenterScreen;
                    progressBarForm.Show();
                    foreach (var entry in archive.Entries)
                    {
                        entry.WriteToDirectory(folderBrowserDialog1.SelectedPath, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                        progressBarForm.Step();
                    }
                    progressBarForm.Hide();
                    OpenMessageBox("Extraction", "The archive has been successfully extracted.");
                }

            }
            else
            {
                var archive = ArchiveFactory.Open(fileName);
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    maxprogress = listView1.SelectedItems.Count;
                    progress = 0;
                    progressBarForm.maxp(maxprogress);
                    progressBarForm.StartPosition = FormStartPosition.CenterScreen;
                    progressBarForm.Show();
                    foreach (var entry in archive.Entries) //Will extract each selected file.
                    {
                        foreach (ListViewItem item in listView1.SelectedItems)
                        {
                            if (item.Text.Equals(entry.Key) || dirName + item.Text == entry.Key)
                            {
                                entry.WriteToDirectory(folderBrowserDialog1.SelectedPath, new ExtractionOptions()
                                {
                                    ExtractFullPath = true,
                                    Overwrite = true
                                });
                                progressBarForm.Step();
                            }
                        }
                    }
                    progressBarForm.Hide();
                    OpenMessageBox("Extraction", "The archive has been successfully extracted.");
                }
            }
        }

        private void archiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArchivePopup archive = new ArchivePopup();
            archive.StartPosition = FormStartPosition.CenterParent;
            archive.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.StartPosition = FormStartPosition.CenterParent;
            options.ShowDialog();
        }

        private void OpenMessageBox(string title, string description)
        {
            using (var form = new MessageBoxForm(title, description))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            List<string> selection = new List<string>();
            var archive = ArchiveFactory.Open(fileName);
            Directory.CreateDirectory(Path.GetTempPath() + @"\ZipperTMP\");
            foreach (var entry in archive.Entries)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    Debug.WriteLine(dirName);
                    if (item.Text.Equals(entry.Key) || dirName + item.Text == entry.Key || item.Text.Equals("./"))
                    {
                        if (!entry.IsDirectory && !item.Text.Equals("./"))
                        {
                            entry.WriteToDirectory(Path.GetTempPath() + @"\ZipperTMP\", new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                            selection.Add(Path.GetTempPath() + @"\ZipperTMP\" + entry.Key);
                            Process tempExtractProcess = new Process
                            {
                                StartInfo = new ProcessStartInfo(Path.GetTempPath() + @"\ZipperTMP\" + entry.Key)
                                {
                                    UseShellExecute = true
                                }
                            };
                            tempExtractProcess.Start();
                        }
                        else
                        {
                            listView1.Items.Clear();
                            if (item.Text.Equals("./"))
                            {
                                Debug.WriteLine(dirName);
                                string preDirName = dirName;
                                if (dirName.EndsWith("/"))
                                {
                                    dirName = dirName.Replace(Path.GetFileName(Path.GetDirectoryName(dirName)) + "/", "");
                                }else if (dirName.EndsWith(@"\"))
                                {
                                    dirName = dirName.Replace(Path.GetFileName(Path.GetDirectoryName(dirName)) + @"\", "");
                                }
                                
                                if(preDirName + @".." == dirName)
                                {
                                    dirName = "";
                                }
                                Debug.WriteLine(dirName);
                            }
                            else
                            {
                                dirName = entry.Key;
                            }
                            if (dirName != "")
                            {
                                ListViewItem lvi = new ListViewItem("./", 1);
                                lvi.SubItems.Add("0");
                                lvi.SubItems.Add("");
                                listView1.Items.Add(lvi);
                            }
                            foreach (var arcentry in archive.Entries)
                            {
                                if (arcentry.Key == dirName + Path.GetFileName(arcentry.Key) ||
                                    arcentry.Key == dirName + "/" + Path.GetDirectoryName(arcentry.Key) + "/" ||
                                    arcentry.Key == dirName + "/" + Path.GetFileName(arcentry.Key) ||
                                    arcentry.Key == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) ||
                                    arcentry.Key == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) + "/" ||
                                    arcentry.Key == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) + @"\" ||
                                    arcentry.Key == dirName + @"\" + Path.GetDirectoryName(arcentry.Key) + @"\" ||
                                    arcentry.Key == dirName + @"\" + Path.GetFileName(arcentry.Key) ||
                                    (Path.GetDirectoryName(arcentry.Key) == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) && arcentry.IsDirectory) ||
                                    (Path.GetDirectoryName(arcentry.Key) == dirName + Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) + "/" && arcentry.IsDirectory)
                                   )
                                {
                                    if (arcentry.IsDirectory)
                                    {
                                        //Debug.WriteLine(arcentry.Key);
                                        if (arcentry.Key != dirName)
                                        {
                                            if (arcentry.Key.EndsWith("/"))
                                            {
                                                ListViewItem lvi = new ListViewItem(Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) + "/", 1);
                                                lvi.SubItems.Add(arcentry.Size.ToString("N0"));
                                                lvi.SubItems.Add(arcentry.LastModifiedTime.ToString());
                                                listView1.Items.Add(lvi);
                                            }
                                            else if (arcentry.Key.EndsWith(@"\"))
                                            {
                                                ListViewItem lvi = new ListViewItem(Path.GetFileName(Path.GetDirectoryName(arcentry.Key)) + @"\", 1);
                                                lvi.SubItems.Add(arcentry.Size.ToString("N0"));
                                                lvi.SubItems.Add(arcentry.LastModifiedTime.ToString());
                                                listView1.Items.Add(lvi);
                                            }
                                            else
                                            {
                                                ListViewItem lvi = new ListViewItem(Path.GetFileName(Path.GetDirectoryName(arcentry.Key)), 1);
                                                lvi.SubItems.Add(arcentry.Size.ToString("N0"));
                                                lvi.SubItems.Add(arcentry.LastModifiedTime.ToString());
                                                listView1.Items.Add(lvi);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ListViewItem lvi = new ListViewItem(Path.GetFileName(arcentry.Key), 1);
                                        lvi.SubItems.Add(arcentry.Size.ToString("N0"));
                                        lvi.SubItems.Add(arcentry.LastModifiedTime.ToString());
                                        listView1.Items.Add(lvi);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Directory.Exists(Path.GetTempPath() + @"\ZipperTMP\"))
            {
                Directory.Delete(Path.GetTempPath() + @"\ZipperTMP\", true);
            }
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawColumnHeader(object sender,
                                        DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                e.Graphics.FillRectangle(new SolidBrush(listView1.BackColor), Bounds);
                e.Graphics.DrawString(e.Header.Text, listView1.Font, Brushes.White, e.Bounds, sf);
            }
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            List<string> selection = new List<string>();
            var archive = ArchiveFactory.Open(fileName);
            Directory.CreateDirectory(Path.GetTempPath() + @"\ZipperTMP\");
            foreach (var entry in archive.Entries)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    if (item.Text.Equals(entry.Key) || dirName + item.Text == (entry.Key))
                    {
                        entry.WriteToDirectory(Path.GetTempPath() + @"\ZipperTMP\", new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                        selection.Add(Path.GetTempPath() + @"\ZipperTMP\" + entry.Key);
                    }

                }
            }
            DataObject data = new DataObject(DataFormats.FileDrop, selection.ToArray());
            DoDragDrop(data, DragDropEffects.Copy);
            Directory.Delete(Path.GetTempPath() + @"\ZipperTMP\", true);
        }

        //This code I found helps enable the Windows 10 dark titlebar.
        //It's basically in every form.

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

        private class ZipRenderer : ToolStripProfessionalRenderer
        {
            public ZipRenderer() : base(new ZipCols()) { }
        }

        private class ZipCols : ProfessionalColorTable
        {
            public override Color MenuItemBorder
            {
                get 
                {
                    if (Properties.Settings.Default.Dark == true) { 
                        return Color.FromArgb(126, 126, 126); 
                    }
                    else return Color.FromArgb(165, 165, 165);
                }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get 
                {
                    if (Properties.Settings.Default.Dark == true)
                    {
                        return Color.FromArgb(51, 51, 51);
                    }
                    else return Color.FromArgb(223, 223, 223);
                }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get
                {
                    if (Properties.Settings.Default.Dark == true)
                    {
                        return Color.FromArgb(51, 51, 51);
                    }
                    else return Color.FromArgb(223, 223, 223);
                }
            }
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Dark == true)
            {
                menuStrip1.BackColor = Color.FromArgb(43, 43, 43);
                menuStrip1.ForeColor = Color.FromArgb(127, 127, 127);
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Dark == true)
            {
                menuStrip1.BackColor = Color.FromArgb(00, 00, 00);
                menuStrip1.ForeColor = Color.FromArgb(255, 255, 255);
            }
        }
    }
}
