namespace net.r_eg.vsCE.UI.WForms
{
    partial class ComponentsFrm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainerComponents = new System.Windows.Forms.SplitContainer();
            this.groupBoxComponentsList = new System.Windows.Forms.GroupBox();
            this.dgvComponents = new net.r_eg.vsCE.UI.WForms.Components.DataGridViewExt();
            this.dgvComponentsIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvComponentsEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvComponentsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvComponentsClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvComponentsDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuComponents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemCompDoc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCompNew = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCompNew = new System.Windows.Forms.Button();
            this.groupBoxComponentsMembers = new System.Windows.Forms.GroupBox();
            this.dgvComponentInfo = new net.r_eg.vsCE.UI.WForms.Components.DataGridViewExt();
            this.dgvCompInfoType = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvCompInfoName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCompInfoSignature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCompInfoDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerComponents)).BeginInit();
            this.splitContainerComponents.Panel1.SuspendLayout();
            this.splitContainerComponents.Panel2.SuspendLayout();
            this.splitContainerComponents.SuspendLayout();
            this.groupBoxComponentsList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).BeginInit();
            this.contextMenuComponents.SuspendLayout();
            this.groupBoxComponentsMembers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerComponents
            // 
            this.splitContainerComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerComponents.Location = new System.Drawing.Point(0, 0);
            this.splitContainerComponents.Name = "splitContainerComponents";
            this.splitContainerComponents.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerComponents.Panel1
            // 
            this.splitContainerComponents.Panel1.Controls.Add(this.groupBoxComponentsList);
            this.splitContainerComponents.Panel1.Controls.Add(this.btnSave);
            this.splitContainerComponents.Panel1.Controls.Add(this.btnCompNew);
            // 
            // splitContainerComponents.Panel2
            // 
            this.splitContainerComponents.Panel2.Controls.Add(this.groupBoxComponentsMembers);
            this.splitContainerComponents.Size = new System.Drawing.Size(776, 381);
            this.splitContainerComponents.SplitterDistance = 193;
            this.splitContainerComponents.TabIndex = 88;
            // 
            // groupBoxComponentsList
            // 
            this.groupBoxComponentsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxComponentsList.Controls.Add(this.dgvComponents);
            this.groupBoxComponentsList.Location = new System.Drawing.Point(0, 0);
            this.groupBoxComponentsList.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxComponentsList.Name = "groupBoxComponentsList";
            this.groupBoxComponentsList.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.groupBoxComponentsList.Size = new System.Drawing.Size(695, 193);
            this.groupBoxComponentsList.TabIndex = 87;
            this.groupBoxComponentsList.TabStop = false;
            this.groupBoxComponentsList.Text = "Available components:";
            // 
            // dgvComponents
            // 
            this.dgvComponents.AllowUserToAddRows = false;
            this.dgvComponents.AllowUserToDeleteRows = false;
            this.dgvComponents.AllowUserToResizeRows = false;
            this.dgvComponents.AlwaysSelected = true;
            this.dgvComponents.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvComponents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvComponentsIcon,
            this.dgvComponentsEnabled,
            this.dgvComponentsName,
            this.dgvComponentsClass,
            this.dgvComponentsDescription});
            this.dgvComponents.ContextMenuStrip = this.contextMenuComponents;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvComponents.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvComponents.DragDropSortable = false;
            this.dgvComponents.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvComponents.Location = new System.Drawing.Point(0, 14);
            this.dgvComponents.Margin = new System.Windows.Forms.Padding(0);
            this.dgvComponents.MultiSelect = false;
            this.dgvComponents.Name = "dgvComponents";
            this.dgvComponents.NumberingForRowsHeader = false;
            this.dgvComponents.RowHeadersVisible = false;
            this.dgvComponents.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            this.dgvComponents.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvComponents.RowTemplate.Height = 17;
            this.dgvComponents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvComponents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComponents.Size = new System.Drawing.Size(695, 179);
            this.dgvComponents.TabIndex = 84;
            this.dgvComponents.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComponents_CellContentClick);
            this.dgvComponents.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComponents_CellDoubleClick);
            this.dgvComponents.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComponents_CellValueChanged);
            this.dgvComponents.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComponents_RowEnter);
            // 
            // dgvComponentsIcon
            // 
            this.dgvComponentsIcon.FillWeight = 23F;
            this.dgvComponentsIcon.HeaderText = "";
            this.dgvComponentsIcon.MinimumWidth = 16;
            this.dgvComponentsIcon.Name = "dgvComponentsIcon";
            this.dgvComponentsIcon.ReadOnly = true;
            this.dgvComponentsIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvComponentsIcon.Width = 23;
            // 
            // dgvComponentsEnabled
            // 
            this.dgvComponentsEnabled.FalseValue = "False";
            this.dgvComponentsEnabled.FillWeight = 60F;
            this.dgvComponentsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dgvComponentsEnabled.HeaderText = "Enabled";
            this.dgvComponentsEnabled.IndeterminateValue = "False";
            this.dgvComponentsEnabled.MinimumWidth = 16;
            this.dgvComponentsEnabled.Name = "dgvComponentsEnabled";
            this.dgvComponentsEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvComponentsEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvComponentsEnabled.TrueValue = "True";
            this.dgvComponentsEnabled.Width = 52;
            // 
            // dgvComponentsName
            // 
            this.dgvComponentsName.FillWeight = 130F;
            this.dgvComponentsName.HeaderText = "Name";
            this.dgvComponentsName.MinimumWidth = 100;
            this.dgvComponentsName.Name = "dgvComponentsName";
            this.dgvComponentsName.ReadOnly = true;
            this.dgvComponentsName.Width = 130;
            // 
            // dgvComponentsClass
            // 
            this.dgvComponentsClass.FillWeight = 170F;
            this.dgvComponentsClass.HeaderText = "Class";
            this.dgvComponentsClass.MinimumWidth = 100;
            this.dgvComponentsClass.Name = "dgvComponentsClass";
            this.dgvComponentsClass.ReadOnly = true;
            this.dgvComponentsClass.Width = 170;
            // 
            // dgvComponentsDescription
            // 
            this.dgvComponentsDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvComponentsDescription.HeaderText = "Description";
            this.dgvComponentsDescription.MinimumWidth = 30;
            this.dgvComponentsDescription.Name = "dgvComponentsDescription";
            this.dgvComponentsDescription.ReadOnly = true;
            // 
            // contextMenuComponents
            // 
            this.contextMenuComponents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCompDoc,
            this.toolStripSeparator1,
            this.menuItemCompNew});
            this.contextMenuComponents.Name = "contextMenuComponents";
            this.contextMenuComponents.Size = new System.Drawing.Size(199, 54);
            // 
            // menuItemCompDoc
            // 
            this.menuItemCompDoc.Name = "menuItemCompDoc";
            this.menuItemCompDoc.Size = new System.Drawing.Size(198, 22);
            this.menuItemCompDoc.Text = "About component";
            this.menuItemCompDoc.Click += new System.EventHandler(this.menuItemCompDoc_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // menuItemCompNew
            // 
            this.menuItemCompNew.Name = "menuItemCompNew";
            this.menuItemCompNew.Size = new System.Drawing.Size(198, 22);
            this.menuItemCompNew.Text = "Create new component";
            this.menuItemCompNew.Click += new System.EventHandler(this.menuItemCompNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(698, 29);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 90;
            this.btnSave.Text = "Apply";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCompNew
            // 
            this.btnCompNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompNew.Location = new System.Drawing.Point(698, 0);
            this.btnCompNew.Name = "btnCompNew";
            this.btnCompNew.Size = new System.Drawing.Size(75, 23);
            this.btnCompNew.TabIndex = 86;
            this.btnCompNew.Text = "Create new";
            this.btnCompNew.UseVisualStyleBackColor = true;
            this.btnCompNew.Click += new System.EventHandler(this.btnCompNew_Click);
            // 
            // groupBoxComponentsMembers
            // 
            this.groupBoxComponentsMembers.Controls.Add(this.dgvComponentInfo);
            this.groupBoxComponentsMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxComponentsMembers.Location = new System.Drawing.Point(0, 0);
            this.groupBoxComponentsMembers.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxComponentsMembers.Name = "groupBoxComponentsMembers";
            this.groupBoxComponentsMembers.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.groupBoxComponentsMembers.Size = new System.Drawing.Size(776, 184);
            this.groupBoxComponentsMembers.TabIndex = 86;
            this.groupBoxComponentsMembers.TabStop = false;
            this.groupBoxComponentsMembers.Text = "Members of components:";
            // 
            // dgvComponentInfo
            // 
            this.dgvComponentInfo.AllowUserToAddRows = false;
            this.dgvComponentInfo.AllowUserToDeleteRows = false;
            this.dgvComponentInfo.AllowUserToResizeRows = false;
            this.dgvComponentInfo.AlwaysSelected = false;
            this.dgvComponentInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvComponentInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvComponentInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComponentInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCompInfoType,
            this.dgvCompInfoName,
            this.dgvCompInfoSignature,
            this.dgvCompInfoDescription});
            this.dgvComponentInfo.ContextMenuStrip = this.contextMenuComponents;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvComponentInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvComponentInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvComponentInfo.DragDropSortable = false;
            this.dgvComponentInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvComponentInfo.Location = new System.Drawing.Point(0, 14);
            this.dgvComponentInfo.Margin = new System.Windows.Forms.Padding(0);
            this.dgvComponentInfo.MultiSelect = false;
            this.dgvComponentInfo.Name = "dgvComponentInfo";
            this.dgvComponentInfo.NumberingForRowsHeader = false;
            this.dgvComponentInfo.ReadOnly = true;
            this.dgvComponentInfo.RowHeadersVisible = false;
            this.dgvComponentInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(252)))), ((int)(((byte)(248)))));
            this.dgvComponentInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvComponentInfo.RowTemplate.Height = 17;
            this.dgvComponentInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvComponentInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComponentInfo.Size = new System.Drawing.Size(776, 170);
            this.dgvComponentInfo.TabIndex = 85;
            // 
            // dgvCompInfoType
            // 
            this.dgvCompInfoType.FillWeight = 38F;
            this.dgvCompInfoType.HeaderText = "Type";
            this.dgvCompInfoType.MinimumWidth = 16;
            this.dgvCompInfoType.Name = "dgvCompInfoType";
            this.dgvCompInfoType.ReadOnly = true;
            this.dgvCompInfoType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCompInfoType.Width = 38;
            // 
            // dgvCompInfoName
            // 
            this.dgvCompInfoName.FillWeight = 140F;
            this.dgvCompInfoName.HeaderText = "Name";
            this.dgvCompInfoName.MinimumWidth = 100;
            this.dgvCompInfoName.Name = "dgvCompInfoName";
            this.dgvCompInfoName.ReadOnly = true;
            this.dgvCompInfoName.Width = 140;
            // 
            // dgvCompInfoSignature
            // 
            this.dgvCompInfoSignature.FillWeight = 200F;
            this.dgvCompInfoSignature.HeaderText = "Signature";
            this.dgvCompInfoSignature.MinimumWidth = 180;
            this.dgvCompInfoSignature.Name = "dgvCompInfoSignature";
            this.dgvCompInfoSignature.ReadOnly = true;
            this.dgvCompInfoSignature.Width = 200;
            // 
            // dgvCompInfoDescription
            // 
            this.dgvCompInfoDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCompInfoDescription.HeaderText = "Description";
            this.dgvCompInfoDescription.MinimumWidth = 30;
            this.dgvCompInfoDescription.Name = "dgvCompInfoDescription";
            this.dgvCompInfoDescription.ReadOnly = true;
            // 
            // ComponentsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 381);
            this.Controls.Add(this.splitContainerComponents);
            this.Name = "ComponentsFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Components of SBE-Scripts core";
            this.Load += new System.EventHandler(this.ComponentsFrm_Load);
            this.splitContainerComponents.Panel1.ResumeLayout(false);
            this.splitContainerComponents.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerComponents)).EndInit();
            this.splitContainerComponents.ResumeLayout(false);
            this.groupBoxComponentsList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponents)).EndInit();
            this.contextMenuComponents.ResumeLayout(false);
            this.groupBoxComponentsMembers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerComponents;
        private System.Windows.Forms.GroupBox groupBoxComponentsList;
        private Components.DataGridViewExt dgvComponents;
        private System.Windows.Forms.DataGridViewImageColumn dgvComponentsIcon;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvComponentsEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvComponentsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvComponentsClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvComponentsDescription;
        private System.Windows.Forms.Button btnCompNew;
        private System.Windows.Forms.GroupBox groupBoxComponentsMembers;
        private Components.DataGridViewExt dgvComponentInfo;
        private System.Windows.Forms.DataGridViewImageColumn dgvCompInfoType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCompInfoName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCompInfoSignature;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCompInfoDescription;
        private System.Windows.Forms.ContextMenuStrip contextMenuComponents;
        private System.Windows.Forms.ToolStripMenuItem menuItemCompDoc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemCompNew;
        private System.Windows.Forms.Button btnSave;
    }
}