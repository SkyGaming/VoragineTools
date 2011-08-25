namespace WoWPacketViewer
{
    partial class FrmSettings
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
            this.edtHost = new System.Windows.Forms.TextBox();
            this.edtPort = new System.Windows.Forms.TextBox();
            this.edtUser = new System.Windows.Forms.TextBox();
            this.edtPass = new System.Windows.Forms.TextBox();
            this.edtOpcodeDBName = new System.Windows.Forms.TextBox();
            this.BtnStave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // edtHost
            // 
            this.edtHost.Location = new System.Drawing.Point(84, 12);
            this.edtHost.Name = "edtHost";
            this.edtHost.Size = new System.Drawing.Size(119, 20);
            this.edtHost.TabIndex = 0;
            // 
            // edtPort
            // 
            this.edtPort.Location = new System.Drawing.Point(84, 38);
            this.edtPort.Name = "edtPort";
            this.edtPort.Size = new System.Drawing.Size(119, 20);
            this.edtPort.TabIndex = 1;
            // 
            // edtUser
            // 
            this.edtUser.Location = new System.Drawing.Point(84, 64);
            this.edtUser.Name = "edtUser";
            this.edtUser.Size = new System.Drawing.Size(119, 20);
            this.edtUser.TabIndex = 2;
            // 
            // edtPass
            // 
            this.edtPass.Location = new System.Drawing.Point(84, 90);
            this.edtPass.Name = "edtPass";
            this.edtPass.Size = new System.Drawing.Size(119, 20);
            this.edtPass.TabIndex = 3;
            // 
            // edtOpcodeDBName
            // 
            this.edtOpcodeDBName.Location = new System.Drawing.Point(84, 116);
            this.edtOpcodeDBName.Name = "edtOpcodeDBName";
            this.edtOpcodeDBName.Size = new System.Drawing.Size(119, 20);
            this.edtOpcodeDBName.TabIndex = 4;
            // 
            // BtnStave
            // 
            this.BtnStave.Location = new System.Drawing.Point(68, 142);
            this.BtnStave.Name = "BtnStave";
            this.BtnStave.Size = new System.Drawing.Size(75, 23);
            this.BtnStave.TabIndex = 5;
            this.BtnStave.Text = "Save";
            this.BtnStave.UseVisualStyleBackColor = true;
            this.BtnStave.Click += new System.EventHandler(this.BtnStave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Host";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "User";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "OpecodeDB";
            // 
            // FrmSettings
            // 
            this.AcceptButton = this.BtnStave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 170);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnStave);
            this.Controls.Add(this.edtOpcodeDBName);
            this.Controls.Add(this.edtPass);
            this.Controls.Add(this.edtUser);
            this.Controls.Add(this.edtPort);
            this.Controls.Add(this.edtHost);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSettings_FormClosing);
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox edtHost;
        private System.Windows.Forms.TextBox edtPort;
        private System.Windows.Forms.TextBox edtUser;
        private System.Windows.Forms.TextBox edtPass;
        private System.Windows.Forms.TextBox edtOpcodeDBName;
        private System.Windows.Forms.Button BtnStave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}