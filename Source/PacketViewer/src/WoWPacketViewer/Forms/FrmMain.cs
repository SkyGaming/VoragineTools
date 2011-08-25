using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WoWPacketViewer.Properties;
using WowTools.Core;

namespace WoWPacketViewer
{
    public partial class FrmMain : Form
    {
        private FrmSearch searchForm;

        private PacketViewTab SelectedTab
        {
            get
            {
                try
                {
                    return (PacketViewTab)tabControl1.SelectedTab.Controls[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        public FrmMain()
        {
            InitializeComponent();

            if (Settings.Default.StartMaximized)
                WindowState = FormWindowState.Maximized;
        }

        private void OpenMenu_Click(object sender, EventArgs e)
        {
            if (_openDialog.ShowDialog() != DialogResult.OK)
                return;
            var file = _openDialog.FileName;

            OpenFile(file);

            if (Settings.Default.LastFiles == null)
            {
                Settings.Default.LastFiles = new StringCollection();
            }
            Settings.Default.LastFiles.Add(file);
        }

        private void OpenFile(string file)
        {
            _statusLabel.Text = "Loading...";

            CreateTab(file);

            _statusLabel.Text = String.Format("Done.");
        }

        private void CreateTab(string file)
        {
            var viewTab = new PacketViewTab(file);

            viewTab.SetColors(Settings.Default.PacketViewForeColor, Settings.Default.PacketViewBackColor,
                Settings.Default.HexViewForeColor, Settings.Default.HexViewBackColor,
                Settings.Default.ParsedViewForeColor, Settings.Default.ParsedViewBackColor);

            viewTab.Dock = DockStyle.Fill;

            var tabPage = new TabPage(viewTab.Text);
            tabPage.Controls.Add(viewTab);

            tabControl1.Controls.Add(tabPage);
            tabControl1.SelectedTab = tabPage;

            if (!tabControl1.Visible)
                tabControl1.Visible = true;
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;

            if (!SelectedTab.Loaded)
            {
                MessageBox.Show("You should load something first!");
                return;
            }

            _saveDialog.FileName = Path.GetFileName(_openDialog.FileName).Replace("bin", "txt");

            if (_saveDialog.ShowDialog() != DialogResult.OK)
                return;

            using (var stream = new StreamWriter(_saveDialog.OpenFile()))
            {
                foreach (var p in SelectedTab.Packets)
                {
                    stream.Write(p.HexLike());
                }
            }
        }

        private void ExitMenu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FindMenu_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;

            CreateSearchFormIfNeed();

            if (!searchForm.Visible)
                searchForm.Show(this);

            searchForm.Select();
        }

        private bool CreateSearchFormIfNeed()
        {
            if (searchForm == null || searchForm.IsDisposed)
            {
                searchForm = new FrmSearch();
                searchForm.CurrentTab = SelectedTab;
                searchForm.Show(this);
                return true;
            }
            return false;
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (SelectedTab == null)
                return;

            if (e.KeyCode != Keys.F3)
                return;

            if (!CreateSearchFormIfNeed())
                searchForm.FindNext();
        }

        private void saveAsParsedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;

            if (!SelectedTab.Loaded)
            {
                MessageBox.Show("You should load something first!");
                return;
            }

            if (_saveDialog.ShowDialog() != DialogResult.OK)
                return;

            using (var stream = new StreamWriter(_saveDialog.OpenFile()))
            {
                foreach (var p in SelectedTab.Packets)
                {
                    string parsed = ParserFactory.CreateParser(p).ToString();
                    if (String.IsNullOrEmpty(parsed))
                        continue;
                    stream.WriteLine(p.Name);
                    stream.WriteLine(parsed);
                }
            }
        }

        private void saveWardenAsTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;

            if (!SelectedTab.Loaded)
            {
                MessageBox.Show("You should load something first!");
                return;
            }

            _saveDialog.FileName = Path.GetFileName(_openDialog.FileName).Replace("bin", "txt");

            if (_saveDialog.ShowDialog() != DialogResult.OK)
                return;

            using (var stream = new StreamWriter(_saveDialog.OpenFile()))
            {
                foreach (var p in SelectedTab.Packets)
                {
                    if (p.Code != OpCodes.CMSG_WARDEN_DATA && p.Code != OpCodes.SMSG_WARDEN_DATA)
                        continue;
                    //stream.Write(Utility.HexLike(p));

                    var parsed = ParserFactory.CreateParser(p).ToString();
                    if (String.IsNullOrEmpty(parsed))
                        continue;
                    stream.Write(parsed);
                }
            }
        }

        private void reloadDefinitionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParserFactory.ReInit();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ParserFactory.Init();
            if (Settings.Default.AutoOpenLast && Settings.Default.LastFiles != null)
            {
                foreach (string filePath in Settings.Default.LastFiles)
                {
                    OpenFile(filePath);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!tabControl1.HasChildren)
            {
                tabControl1.Visible = false;
                return;
            }

            if (searchForm != null)
                searchForm.CurrentTab = SelectedTab;
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right && e.Button != MouseButtons.Middle)
                return;

            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                var r = tabControl1.GetTabRect(i);
                if (r.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Middle)
                    {
                        tabControl1.TabPages[i].Dispose();
                        return;
                    }
                    closeTabToolStripMenuItem.Tag = tabControl1.TabPages[i];
                    closeAllButThisToolStripMenuItem.Tag = tabControl1.TabPages[i];
                    contextMenuStrip1.Show(tabControl1, e.Location);
                    break;
                }
            }
        }

        private void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                var r = tabControl1.GetTabRect(i);
                if (r.Contains(e.Location))
                {
                    tabControl1.TabPages[i].Dispose();
                    break;
                }
            }
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((TabPage)((ToolStripMenuItem)sender).Tag).Dispose();
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while (tabControl1.HasChildren)
                tabControl1.TabPages[0].Dispose();
        }

        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var thisTab = ((ToolStripMenuItem)sender).Tag;

            int index = 0;
            while (tabControl1.TabPages.Count != 1)
            {
                if (tabControl1.TabPages[index] == thisTab)
                {
                    index++;
                    continue;
                }

                tabControl1.TabPages[index].Dispose();
            }
        }

        private void wardenDebugToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            WardenData.Enabled = wardenDebugToolStripMenuItem.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NYI!", Text);
        }

        private void foreColorStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;

            var name = (string)((ToolStripMenuItem)sender).Tag;

            var control = SelectedTab.GetControlByName(name);

            colorChooser.Color = control.ForeColor;

            if (colorChooser.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            control.ForeColor = colorChooser.Color;

            Settings.Default[name + "ForeColor"] = colorChooser.Color;
            Settings.Default.Save();
        }

        private void backColorStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;

            var name = (string)((ToolStripMenuItem)sender).Tag;

            var control = SelectedTab.GetControlByName(name);

            colorChooser.Color = control.BackColor;

            if (colorChooser.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            control.BackColor = colorChooser.Color;

            Settings.Default[name + "BackColor"] = colorChooser.Color;
            Settings.Default.Save();
        }

        private void reloadOpcodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpcodeDB.Reload();
            foreach(TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count > 0 && tab.Controls[0] is PacketViewTab)
                {
                    ((PacketViewTab)tab.Controls[0]).ClearCache();
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Settings.Default.LastFiles == null)
                return;
            // Save the list of files currently opened as tabs to the Settings file
            List<string> filesList = new List<string>();
            foreach (TabPage tab in tabControl1.TabPages)
            {
                if (tab.Controls.Count > 0)
                {
                    string fileName = tab.Controls[0].Text;
                    
                    foreach(string filePath in Settings.Default.LastFiles)
                    {
                        if(Path.GetFileName(filePath) == fileName)
                        {
                            filesList.Add(filePath);
                            break;
                        }
                    }
                }
            }
            Settings.Default.LastFiles.Clear();
            Settings.Default.LastFiles.AddRange(filesList.ToArray());
            Settings.Default.Save();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSettings SettingsForm = new FrmSettings();
            SettingsForm.ShowDialog(this);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;

            SelectedTab.RunParserCode();
        }

        private void saveParserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedTab == null)
                return;
            try
            {
                SelectedTab.SaveParserCode();
                statusStrip1.Items[0].Text = "Parser saved.";
            }
            catch (Exception ex)
            {
                statusStrip1.Items[0].Text = "Parser save error: " + ex.Message;
            }
            
        }

        private void loadSourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParserCompiler.Sources.Clear();
            string parsersDir = Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\src\\WoWPacketViewer\\Parsers\\";
            string parsersBinDir = Path.GetDirectoryName(Application.ExecutablePath) + "\\parsers\\";
            IEnumerable<string> files = new string[0];
            if(Directory.Exists(parsersDir))
                files = files.Concat(Directory.GetFiles(parsersDir, "*.cs", SearchOption.AllDirectories));
            // loads only from the top dir inside bin atm
            if(Directory.Exists(parsersBinDir))
                files = files.Concat(Directory.GetFiles(parsersBinDir, "*.cs", SearchOption.TopDirectoryOnly));
            var start = DateTime.Now;
            foreach (string parserFile in files)
            {
                string dir = Path.GetDirectoryName(parserFile);
                if(dir.EndsWith("\\bak") || dir.EndsWith("\\Enums"))
                    continue; // skip backups and enums
                ParserCompiler.LoadSources(parserFile);
            }
            var taken = DateTime.Now - start;
            statusStrip1.Items[0].Text = String.Format("{0} parsers loaded in {1} ms", 
                ParserCompiler.Sources.Count, (uint)taken.TotalMilliseconds);
        }
    }
}
