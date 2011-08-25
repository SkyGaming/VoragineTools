namespace WoWPacketViewer
{
    partial class FrmChangeOpcode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OpcodeList = new System.Windows.Forms.ListBox();
            this.BuildBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OpcodeNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OkBtn.Location = new System.Drawing.Point(62, 229);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "Save";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(160, 229);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OpcodeList
            // 
            this.OpcodeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpcodeList.FormattingEnabled = true;
            this.OpcodeList.Location = new System.Drawing.Point(12, 49);
            this.OpcodeList.Name = "OpcodeList";
            this.OpcodeList.Size = new System.Drawing.Size(273, 173);
            this.OpcodeList.TabIndex = 4;
            // 
            // BuildBox
            // 
            this.BuildBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BuildBox.Location = new System.Drawing.Point(219, 12);
            this.BuildBox.Name = "BuildBox";
            this.BuildBox.ReadOnly = true;
            this.BuildBox.Size = new System.Drawing.Size(45, 20);
            this.BuildBox.TabIndex = 3;
            this.BuildBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Number (dec)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(183, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Build";
            // 
            // OpcodeNumber
            // 
            this.OpcodeNumber.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.OpcodeNumber.Location = new System.Drawing.Point(117, 12);
            this.OpcodeNumber.Name = "OpcodeNumber";
            this.OpcodeNumber.Size = new System.Drawing.Size(45, 20);
            this.OpcodeNumber.TabIndex = 4;
            this.OpcodeNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OpcodeNumber_KeyPress);
            // 
            // FrmChangeOpcode
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(297, 280);
            this.Controls.Add(this.OpcodeNumber);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BuildBox);
            this.Controls.Add(this.OpcodeList);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Name = "FrmChangeOpcode";
            this.Text = "Change opcode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ListBox OpcodeList;
        private System.Windows.Forms.TextBox BuildBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox OpcodeNumber;
    }
}