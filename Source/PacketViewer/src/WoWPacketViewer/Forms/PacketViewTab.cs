using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WoWPacketViewer.Properties;
using WowTools.Core;

namespace WoWPacketViewer
{
    public partial class PacketViewTab : UserControl, ISupportFind
    {
        private IPacketReader packetViewer;
        private List<Packet> packets;
        private Dictionary<int, ListViewItem> _listCache = new Dictionary<int, ListViewItem>();
        private bool _searchUp;
        private bool _ignoreCase;
        private string file;

        public PacketViewTab(string file)
        {
            InitializeComponent();

            Text = Path.GetFileName(file);

            packetViewer = PacketReaderFactory.Create(Path.GetExtension(file));
            packets = packetViewer.ReadPackets(file).ToList();
            OpcodeDB.Load(packetViewer.Build, GetConnectionString());

            PacketView.VirtualMode = true;
            PacketView.VirtualListSize = packets.Count;
            PacketView.EnsureVisible(0);
        }

        public void SetColors(Color listFore, Color listBack, Color hexFore, Color hexBack, Color parsedFore, Color parsedBack)
        {
            PacketView.ForeColor = listFore;
            PacketView.BackColor = listBack;
            HexView.ForeColor = hexFore;
            HexView.BackColor = hexBack;
            ParsedView.ForeColor = parsedFore;
            ParsedView.BackColor = parsedBack;
        }

        private int SelectedIndex
        {
            get
            {
                var sic = PacketView.SelectedIndices;
                return sic.Count > 0 ? sic[0] : -1;
            }
        }

        public bool Loaded
        {
            get { return packetViewer != null; }
        }

        public List<Packet> Packets { get { return packets; } }

        public string File
        {
            get { return file; }
            set { file = value; }
        }

        public ListViewEx PacketList
        {
            get { return PacketView; }
        }

        public uint Build
        {
            get { return packetViewer.Build; }
        }

        #region ISupportFind Members

        public void Search(string text, bool searchUp, bool ignoreCase)
        {
            if (!Loaded)
                return;

            _searchUp = searchUp;
            _ignoreCase = ignoreCase;

            int startIndex = SelectedIndex;
            if (startIndex < 0)
                startIndex = 0;

            var item = PacketView.FindItemWithText(text, true, startIndex, true);
            if (item != null)
            {
                PacketView.BeginUpdate();
                // hack to avoid redrawing bug
                PacketView.GridLines = !PacketView.GridLines;
                item.Selected = true;
                item.EnsureVisible();
                PacketView.GridLines = !PacketView.GridLines;
                PacketView.EndUpdate();
                return;
            }

            MessageBox.Show(string.Format("Can't find:'{0}'", text), "Packet Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void _list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex < 0)
                return;
            var packet = packets[SelectedIndex];

            HexView.Text = packet.HexLike();
            ParsedView.Text = ParserFactory.CreateParser(packet).ToString();
            ParserCode.Text = ParserCompiler.GetSource(packet.Code);
        }

        private void _list_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            // check to see if the requested item is currently in the cache
            if (_listCache.ContainsKey(e.ItemIndex))
            {
                // A cache hit, so get the ListViewItem from the cache instead of making a new one.
                e.Item = _listCache[e.ItemIndex];
            }
            else
            {
                // A cache miss, so create a new ListViewItem and pass it back.
                var item = CreateListViewItemByIndex(e.ItemIndex);
                e.Item = item;
                _listCache[e.ItemIndex] = item;
            }
        }

        private void _list_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
            var comparisonType = _ignoreCase
                                    ? StringComparison.InvariantCultureIgnoreCase
                                    : StringComparison.InvariantCulture;

            if (_searchUp)
            {
                for (var i = SelectedIndex - 1; i >= 0; --i)
                    if (SearchMatches(e, comparisonType, i))
                        break;
            }
            else
            {
                for (int i = SelectedIndex + 1; i < PacketView.Items.Count; ++i)
                    if (SearchMatches(e, comparisonType, i))
                        break;
            }
        }

        private bool SearchMatches(SearchForVirtualItemEventArgs e, StringComparison comparisonType, int i)
        {
            var op = packets[i].Name;
            if (op.IndexOf(e.Text, comparisonType) != -1)
            {
                e.Index = i;
                return true;
            }
            return false;
        }

        private void _list_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            // We've gotten a request to refresh the cache. First check if it's really necessary.
            if (_listCache.ContainsKey(e.StartIndex) && _listCache.ContainsKey(e.EndIndex))
            {
                // If the newly requested cache is a subset of the old cache, no need to rebuild everything, so do nothing.
                return;
            }

            // Now we need to rebuild the cache.
            int length = e.EndIndex - e.StartIndex + 1; // indexes are inclusive

            // Fill the cache with the appropriate ListViewItems.
            for (int i = 0; i < length; ++i)
            {
                // Skip already existing ListViewItemsItems
                if (_listCache.ContainsKey(e.StartIndex + i))
                    continue;

                // Add new ListViewItemsItem to the cache
                _listCache.Add(e.StartIndex + i, CreateListViewItemByIndex(e.StartIndex + i));
            }
        }

        private ListViewItem CreateListViewItemByIndex(int index)
        {
            var p = packets[index];
            uint startTick = packets[0].TicksCount;

            return p.Direction == Direction.Client
                ? new ListViewItem(new[]
                    {
                        p.UnixTime.AsUnixTime().ToString("H:mm:ss"),
                        (p.TicksCount - startTick).ToString(),
                        p.Name,
                        String.Empty,
                        p.Data.Length.ToString(),
                        ParserFactory.HasParser(p.Code) ? "+" : ""
                    })
                : new ListViewItem(new[]
                    {
                        p.UnixTime.AsUnixTime().ToString("H:mm:ss"),
                        (p.TicksCount - startTick).ToString(),
                        String.Empty,
                        p.Name, 
                        p.Data.Length.ToString(),
                        ParserFactory.HasParser(p.Code) ? "+" : ""
                    });
        }

        private Control ControlsLoop(string name, Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c.Name == name)
                    return c;
                else if (c.Controls.Count != 0)
                {
                    var res = ControlsLoop(name, c);
                    if (res != null)
                        return res;
                }
            }
            return null;
        }

        public Control GetControlByName(string name)
        {
            return ControlsLoop(name, this);
        }

        public void ClearCache()
        {
            _listCache.Clear();
            Invalidate(true);
            Update();
        }

        private void PacketViewTab_Load(object sender, EventArgs e)
        {
            PacketView.Focus();
        }

        private string GetConnectionString()
        {
            return  String.Format("Server={0};Port={1};Uid={2};Pwd={3};Database={4};character set=utf8;Connection Timeout=10",
                    Settings.Default.Host,
                    Settings.Default.Port,
                    Settings.Default.User,
                    Settings.Default.Pass,
                    Settings.Default.OpcodeDBName);
        }

        private void tsmChangeOpcode_Click(object sender, EventArgs e)
        {
            int itemIndex = 0;

            try
            {
                itemIndex = PacketView.SelectedIndices[0];
            }
            catch
            {
                return;
            }

            ListViewItem item = PacketView.Items[itemIndex];
            ListViewItem.ListViewSubItem subitem = item.SubItems[3];
            
            if (subitem.Text == "")
                subitem = item.SubItems[2];

            ChangeOpcode(subitem.Text);
        }

        private void ChangeOpcode(string name)
        {
            var form = new FrmChangeOpcode(name);
            if (form.ShowDialog() == DialogResult.OK)
                ClearCache();   // maybe do the same for other tabs?
        }

        private void PacketView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = PacketView.GetItemAt(e.Location.X, e.Location.Y);
            if (item == null)
                return;
            ListViewItem.ListViewSubItem subitem = item.GetSubItemAt(e.Location.X, e.Location.Y);
            if (subitem == null)
                return;
            var index = item.SubItems.IndexOf(subitem);
            if ((index != 2 && index != 3) || string.IsNullOrEmpty(subitem.Text))
                return; // only for clicks on the opcode

            ChangeOpcode(subitem.Text);
        }

        private void ParserCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t')
            {
                e.Handled = true;
            }
            else if (e.KeyChar == '\r')
            {
                string tab = GetTabulation(ParserCode);
                InsertText(ParserCode, Environment.NewLine + tab);
                e.Handled = true;
            }

        }

        private void ParserCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (e.Modifiers.HasFlag(Keys.Shift))
                    Untab(ParserCode);
                else
                    InsertText(ParserCode, "    ");

                e.Handled = true;
            }
        }

        public void RunParserCode()
        {
            if (SelectedIndex < 0)
                return;
            var packet = packets[SelectedIndex];
            try
            {
                ParserFactory.DefineParser(ParserCode.Text, packet.Code);
                ParsedView.Text = ParserFactory.CreateParser(packet).ToString();
            }
            catch (Exception ex)
            {
                ParsedView.Text = ex.Message;
            }
        }

        public void SaveParserCode()
        {
            // TODO: allow editing of parsers in existing files
            if (SelectedIndex < 0)
                return;
            var packet = packets[SelectedIndex];
            string source = ParserCode.Text;
            var opcode = packet.Code;
            ParserCompiler.Sources[opcode] = source;
            if (ParserCompiler.TryReplaceSource(source, opcode))
                return; // successfully replaced

            // try creating a new file
            string completeCode = ParserCompiler.AddImpliedCode(source, opcode);
            var className = ParserCompiler.GetClassName(packet.Code);
            string newFile = "parsers" + Path.DirectorySeparatorChar + className + ".cs";
            using (var s = new StreamWriter(newFile))
            {
                s.WriteLine(completeCode);
            }
            ParserCompiler.SourceFiles[opcode] = newFile;
        }

        private static void Untab(TextBox textBox)
        {
            var selectionIndex = textBox.SelectionStart;
            string text = textBox.Text.Substring(0, selectionIndex);
            int lineStart = text.LastIndexOf('\n') + 1;
            int i = lineStart;
            while (i < text.Length && text[i] == ' ')
            {
                i++;
            }
            int spacesRemoved = Math.Min(i - lineStart, 4);
            textBox.Text = textBox.Text.Remove(lineStart, spacesRemoved);
            textBox.SelectionStart = selectionIndex - spacesRemoved;
        }

        private static string GetTabulation(TextBox textBox)
        {
            var selectionIndex = textBox.SelectionStart;
            string text = textBox.Text.Substring(0, selectionIndex);
            int lineStart = text.LastIndexOf('\n') + 1;
            int i = lineStart;
            while(i < text.Length && text[i] == ' ')
            {
                i++;
            }
            return new string(' ', i - lineStart);
        }

        private static void InsertText(TextBox textBox, string insertText)
        {
            var selectionIndex = textBox.SelectionStart;
            textBox.Text = textBox.Text.Insert(selectionIndex, insertText);
            textBox.SelectionStart = selectionIndex + insertText.Length;
        }
    }
}
