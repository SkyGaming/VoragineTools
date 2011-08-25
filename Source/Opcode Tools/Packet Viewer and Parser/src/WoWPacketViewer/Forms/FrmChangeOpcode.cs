using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WowTools.Core;

namespace WoWPacketViewer
{
    public partial class FrmChangeOpcode : Form
    {
        public FrmChangeOpcode(string opcode)
        {
            InitializeComponent();

            BuildBox.Text = OpcodeDB.BuildLoaded.ToString();
            OpCodes enumVal;
            if (!OpCodes.TryParse(opcode, true, out enumVal) || !Enum.IsDefined(typeof(OpCodes), enumVal))
            {
                uint value;
                if (uint.TryParse(opcode, out value))
                {
                    OpcodeNumber.Text = opcode;
                    OpcodeNumber.TabIndex = 1;
                }
            }
            foreach(var pair in OpcodeDB.EnumToNumber)
            {
                OpcodeList.Items.Add(pair.Key + " (" + pair.Value + ")");
                if (pair.Key == enumVal)
                {
                    OpcodeList.SelectedIndex = OpcodeList.Items.Count - 1;
                    OpcodeList.TabIndex = 1;
                    OpcodeNumber.Text = pair.Value.ToString();
                }
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (WriteUpdate())
            {
                Close();
                DialogResult = DialogResult.OK;
            }
        }

        private bool WriteUpdate()
        {
            string name = ((String)OpcodeList.SelectedItem);
            // cut off the old number
            name = name.Substring(0, name.IndexOf(' '));
            uint number;
            if (!uint.TryParse(OpcodeNumber.Text, out number) || number > 0xFFFF)
                return false;

            var filename = Properties.Settings.Default.OpcodesUpdatesPath;
            if(string.IsNullOrEmpty(filename))
                filename = "OpcodesUpdates" + BuildBox.Text + ".sql";

            bool newfile = !File.Exists(filename);
            using(var w = new StreamWriter(filename, true))
            {
                if(newfile)
                    w.WriteLine("SET @ver={0};", BuildBox.Text);

                string query = OpcodeDB.UpdateOpcode(name, number);
                if (String.IsNullOrEmpty(query))
                    return false;
                
                w.WriteLine(query);
            }
            

            return true;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }

        private void OpcodeNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // only digits allowed
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
