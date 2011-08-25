using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WoWPacketViewer.Properties;

namespace WoWPacketViewer
{
    public partial class FrmSettings : Form
    {
        bool canClose = false;

        public FrmSettings()
        {
            InitializeComponent();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            edtHost.Text = Settings.Default.Host;
            edtPort.Text = Settings.Default.Port;
            edtUser.Text = Settings.Default.User;
            edtPass.Text = Settings.Default.Pass;
            edtOpcodeDBName.Text = Settings.Default.OpcodeDBName;
        }

        private void BtnStave_Click(object sender, EventArgs e)
        {
            canClose = true;
            Settings.Default.Host = edtHost.Text;
            Settings.Default.Port = edtPort.Text;
            Settings.Default.User = edtUser.Text;
            Settings.Default.Pass = edtPass.Text;
            Settings.Default.OpcodeDBName = edtOpcodeDBName.Text;
            Settings.Default.Save();
            Settings.Default.Reload();
            this.Close();
        }

        private void FrmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canClose)
                BtnStave.PerformClick();
        }
    }
}
