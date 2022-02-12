namespace net.r_eg.vsCE.UI.WForms
{
    partial class AboutFrm
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
            if(disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutFrm));
            this.pictureBoxSpace = new System.Windows.Forms.PictureBox();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelContact = new System.Windows.Forms.Label();
            this.groupBoxCopyright = new System.Windows.Forms.GroupBox();
            this.linkPage = new System.Windows.Forms.LinkLabel();
            this.groupBoxIncludes = new System.Windows.Forms.GroupBox();
            this.textBoxIncludes = new System.Windows.Forms.TextBox();
            this.groupBoxMixed = new System.Windows.Forms.GroupBox();
            this.labelVersionVal = new System.Windows.Forms.TextBox();
            this.linkLicense = new System.Windows.Forms.LinkLabel();
            this.labelLicense = new System.Windows.Forms.Label();
            this.labelVersion = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.linkLabelDonationHelp = new System.Windows.Forms.LinkLabel();
            this.linkVSSBE = new System.Windows.Forms.LinkLabel();
            this.groupBoxBasedOn = new System.Windows.Forms.GroupBox();
            this.btnDonate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpace)).BeginInit();
            this.groupBoxCopyright.SuspendLayout();
            this.groupBoxIncludes.SuspendLayout();
            this.groupBoxMixed.SuspendLayout();
            this.groupBoxBasedOn.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxSpace
            // 
            this.pictureBoxSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxSpace.BackColor = System.Drawing.Color.Black;
            this.pictureBoxSpace.Cursor = System.Windows.Forms.Cursors.PanNorth;
            this.pictureBoxSpace.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSpace.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxSpace.Name = "pictureBoxSpace";
            this.pictureBoxSpace.Size = new System.Drawing.Size(190, 326);
            this.pictureBoxSpace.TabIndex = 0;
            this.pictureBoxSpace.TabStop = false;
            this.pictureBoxSpace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpace_MouseDown);
            this.pictureBoxSpace.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpace_MouseUp);
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new System.Drawing.Point(6, 12);
            this.labelCopyright.Margin = new System.Windows.Forms.Padding(3);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(343, 13);
            this.labelCopyright.TabIndex = 1;
            this.labelCopyright.Text = "Copyright (c) 2015-2022  Denis Kuzmin <x-3F@outlook.com> github/3F";
            // 
            // labelContact
            // 
            this.labelContact.AutoSize = true;
            this.labelContact.Location = new System.Drawing.Point(6, 31);
            this.labelContact.Margin = new System.Windows.Forms.Padding(3);
            this.labelContact.Name = "labelContact";
            this.labelContact.Size = new System.Drawing.Size(47, 13);
            this.labelContact.TabIndex = 2;
            this.labelContact.Text = "Contact:";
            // 
            // groupBoxCopyright
            // 
            this.groupBoxCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCopyright.Controls.Add(this.linkPage);
            this.groupBoxCopyright.Controls.Add(this.labelContact);
            this.groupBoxCopyright.Controls.Add(this.labelCopyright);
            this.groupBoxCopyright.Location = new System.Drawing.Point(193, -4);
            this.groupBoxCopyright.Name = "groupBoxCopyright";
            this.groupBoxCopyright.Size = new System.Drawing.Size(405, 52);
            this.groupBoxCopyright.TabIndex = 4;
            this.groupBoxCopyright.TabStop = false;
            // 
            // linkPage
            // 
            this.linkPage.AutoSize = true;
            this.linkPage.Location = new System.Drawing.Point(53, 31);
            this.linkPage.Name = "linkPage";
            this.linkPage.Size = new System.Drawing.Size(76, 13);
            this.linkPage.TabIndex = 4;
            this.linkPage.TabStop = true;
            this.linkPage.Text = "github.com/3F";
            this.linkPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPage_LinkClicked);
            // 
            // groupBoxIncludes
            // 
            this.groupBoxIncludes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxIncludes.Controls.Add(this.textBoxIncludes);
            this.groupBoxIncludes.Location = new System.Drawing.Point(193, 213);
            this.groupBoxIncludes.Name = "groupBoxIncludes";
            this.groupBoxIncludes.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.groupBoxIncludes.Size = new System.Drawing.Size(405, 75);
            this.groupBoxIncludes.TabIndex = 5;
            this.groupBoxIncludes.TabStop = false;
            this.groupBoxIncludes.Text = "This product includes:";
            // 
            // textBoxIncludes
            // 
            this.textBoxIncludes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxIncludes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxIncludes.Location = new System.Drawing.Point(10, 18);
            this.textBoxIncludes.Multiline = true;
            this.textBoxIncludes.Name = "textBoxIncludes";
            this.textBoxIncludes.ReadOnly = true;
            this.textBoxIncludes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIncludes.Size = new System.Drawing.Size(390, 52);
            this.textBoxIncludes.TabIndex = 0;
            this.textBoxIncludes.Text = resources.GetString("textBoxIncludes.Text");
            this.textBoxIncludes.WordWrap = false;
            // 
            // groupBoxMixed
            // 
            this.groupBoxMixed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMixed.Controls.Add(this.labelVersionVal);
            this.groupBoxMixed.Controls.Add(this.linkLicense);
            this.groupBoxMixed.Controls.Add(this.labelLicense);
            this.groupBoxMixed.Controls.Add(this.labelVersion);
            this.groupBoxMixed.Controls.Add(this.richTextBox1);
            this.groupBoxMixed.Location = new System.Drawing.Point(193, 46);
            this.groupBoxMixed.Name = "groupBoxMixed";
            this.groupBoxMixed.Size = new System.Drawing.Size(405, 114);
            this.groupBoxMixed.TabIndex = 6;
            this.groupBoxMixed.TabStop = false;
            // 
            // labelVersionVal
            // 
            this.labelVersionVal.BackColor = System.Drawing.SystemColors.Control;
            this.labelVersionVal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelVersionVal.Location = new System.Drawing.Point(54, 12);
            this.labelVersionVal.Name = "labelVersionVal";
            this.labelVersionVal.ReadOnly = true;
            this.labelVersionVal.Size = new System.Drawing.Size(346, 13);
            this.labelVersionVal.TabIndex = 9;
            // 
            // linkLicense
            // 
            this.linkLicense.AutoSize = true;
            this.linkLicense.Location = new System.Drawing.Point(50, 30);
            this.linkLicense.Name = "linkLicense";
            this.linkLicense.Size = new System.Drawing.Size(73, 13);
            this.linkLicense.TabIndex = 8;
            this.linkLicense.TabStop = true;
            this.linkLicense.Text = "GNU LGPLv3";
            this.linkLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLicense_LinkClicked);
            // 
            // labelLicense
            // 
            this.labelLicense.AutoSize = true;
            this.labelLicense.Location = new System.Drawing.Point(6, 30);
            this.labelLicense.Margin = new System.Windows.Forms.Padding(3);
            this.labelLicense.Name = "labelLicense";
            this.labelLicense.Size = new System.Drawing.Size(47, 13);
            this.labelLicense.TabIndex = 1;
            this.labelLicense.Text = "License:";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(6, 12);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(3);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(45, 13);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Version:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(6, 49);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(399, 65);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(523, 293);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 29);
            this.buttonOk.TabIndex = 7;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // linkLabelDonationHelp
            // 
            this.linkLabelDonationHelp.AutoSize = true;
            this.linkLabelDonationHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelDonationHelp.Location = new System.Drawing.Point(360, 301);
            this.linkLabelDonationHelp.Name = "linkLabelDonationHelp";
            this.linkLabelDonationHelp.Size = new System.Drawing.Size(108, 13);
            this.linkLabelDonationHelp.TabIndex = 11;
            this.linkLabelDonationHelp.TabStop = true;
            this.linkLabelDonationHelp.Text = " Ko-fi / Patreaon / ... ";
            this.linkLabelDonationHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDonationHelp_LinkClicked);
            // 
            // linkVSSBE
            // 
            this.linkVSSBE.AutoSize = true;
            this.linkVSSBE.Location = new System.Drawing.Point(147, 18);
            this.linkVSSBE.Name = "linkVSSBE";
            this.linkVSSBE.Size = new System.Drawing.Size(107, 13);
            this.linkVSSBE.TabIndex = 13;
            this.linkVSSBE.TabStop = true;
            this.linkVSSBE.Text = "vsSolutionBuildEvent";
            this.linkVSSBE.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkVSSBE_LinkClicked);
            // 
            // groupBoxBasedOn
            // 
            this.groupBoxBasedOn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBasedOn.Controls.Add(this.linkVSSBE);
            this.groupBoxBasedOn.Location = new System.Drawing.Point(193, 166);
            this.groupBoxBasedOn.Name = "groupBoxBasedOn";
            this.groupBoxBasedOn.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.groupBoxBasedOn.Size = new System.Drawing.Size(405, 41);
            this.groupBoxBasedOn.TabIndex = 6;
            this.groupBoxBasedOn.TabStop = false;
            this.groupBoxBasedOn.Text = "Based on:";
            // 
            // btnDonate
            // 
            this.btnDonate.BackColor = System.Drawing.Color.FloralWhite;
            this.btnDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDonate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDonate.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDonate.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnDonate.Location = new System.Drawing.Point(221, 290);
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.Size = new System.Drawing.Size(133, 32);
            this.btnDonate.TabIndex = 12;
            this.btnDonate.Text = "☕";
            this.btnDonate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDonate.UseVisualStyleBackColor = false;
            this.btnDonate.Click += new System.EventHandler(this.btnDonate_Click);
            // 
            // AboutFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 324);
            this.Controls.Add(this.btnDonate);
            this.Controls.Add(this.groupBoxBasedOn);
            this.Controls.Add(this.linkLabelDonationHelp);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxCopyright);
            this.Controls.Add(this.groupBoxMixed);
            this.Controls.Add(this.groupBoxIncludes);
            this.Controls.Add(this.pictureBoxSpace);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AboutFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About - vsCommandEvent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AboutFrm_FormClosing);
            this.Load += new System.EventHandler(this.AboutFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpace)).EndInit();
            this.groupBoxCopyright.ResumeLayout(false);
            this.groupBoxCopyright.PerformLayout();
            this.groupBoxIncludes.ResumeLayout(false);
            this.groupBoxIncludes.PerformLayout();
            this.groupBoxMixed.ResumeLayout(false);
            this.groupBoxMixed.PerformLayout();
            this.groupBoxBasedOn.ResumeLayout(false);
            this.groupBoxBasedOn.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxSpace;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelContact;
        private System.Windows.Forms.GroupBox groupBoxCopyright;
        private System.Windows.Forms.GroupBox groupBoxIncludes;
        private System.Windows.Forms.TextBox textBoxIncludes;
        private System.Windows.Forms.GroupBox groupBoxMixed;
        private System.Windows.Forms.Label labelLicense;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.LinkLabel linkLicense;
        private System.Windows.Forms.LinkLabel linkPage;
        private System.Windows.Forms.TextBox labelVersionVal;
        private System.Windows.Forms.LinkLabel linkLabelDonationHelp;
        private System.Windows.Forms.LinkLabel linkVSSBE;
        private System.Windows.Forms.GroupBox groupBoxBasedOn;
        private System.Windows.Forms.Button btnDonate;
    }
}