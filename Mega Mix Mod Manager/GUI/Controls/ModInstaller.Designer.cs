namespace Mega_Mix_Mod_Manager.GUI.Controls
{
    partial class ModInstaller
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GB_ModDetails = new System.Windows.Forms.GroupBox();
            this.CMS_RemoveDisable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CMS_Export = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TS_MergeMods = new System.Windows.Forms.ToolStripMenuItem();
            this.TS_ClearMods = new System.Windows.Forms.ToolStripMenuItem();
            this.B_ModUp = new System.Windows.Forms.Button();
            this.PB_InstallProgress = new System.Windows.Forms.ProgressBar();
            this.B_ModDown = new System.Windows.Forms.Button();
            this.B_OpenMod = new System.Windows.Forms.Button();
            this.B_InstallMod = new System.Windows.Forms.Button();
            this.RTB_ModDetails = new System.Windows.Forms.RichTextBox();
            this.PB_ModPreview = new System.Windows.Forms.PictureBox();
            this.GB_ModList = new System.Windows.Forms.GroupBox();
            this.TV_ModList = new System.Windows.Forms.TreeView();
            this.MB_Disable = new Mega_Mix_Mod_Manager.IO.MenuButton();
            this.MB_Export = new Mega_Mix_Mod_Manager.IO.MenuButton();
            this.GB_ModDetails.SuspendLayout();
            this.CMS_RemoveDisable.SuspendLayout();
            this.CMS_Export.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_ModPreview)).BeginInit();
            this.GB_ModList.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_ModDetails
            // 
            this.GB_ModDetails.Controls.Add(this.MB_Disable);
            this.GB_ModDetails.Controls.Add(this.MB_Export);
            this.GB_ModDetails.Controls.Add(this.B_ModUp);
            this.GB_ModDetails.Controls.Add(this.PB_InstallProgress);
            this.GB_ModDetails.Controls.Add(this.B_ModDown);
            this.GB_ModDetails.Controls.Add(this.B_OpenMod);
            this.GB_ModDetails.Controls.Add(this.B_InstallMod);
            this.GB_ModDetails.Controls.Add(this.RTB_ModDetails);
            this.GB_ModDetails.Controls.Add(this.PB_ModPreview);
            this.GB_ModDetails.Location = new System.Drawing.Point(220, 6);
            this.GB_ModDetails.Name = "GB_ModDetails";
            this.GB_ModDetails.Size = new System.Drawing.Size(347, 392);
            this.GB_ModDetails.TabIndex = 4;
            this.GB_ModDetails.TabStop = false;
            this.GB_ModDetails.Text = "Mod Details";
            // 
            // CMS_RemoveDisable
            // 
            this.CMS_RemoveDisable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.CMS_RemoveDisable.Name = "CMS_RemoveDisable";
            this.CMS_RemoveDisable.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.B_RemoveMod_Click);
            // 
            // CMS_Export
            // 
            this.CMS_Export.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TS_MergeMods,
            this.TS_ClearMods});
            this.CMS_Export.Name = "contextMenuStrip1";
            this.CMS_Export.Size = new System.Drawing.Size(155, 48);
            // 
            // TS_MergeMods
            // 
            this.TS_MergeMods.Name = "TS_MergeMods";
            this.TS_MergeMods.Size = new System.Drawing.Size(154, 22);
            this.TS_MergeMods.Text = "Remerge Mods";
            this.TS_MergeMods.Click += new System.EventHandler(this.TS_MergeMods_Click);
            // 
            // TS_ClearMods
            // 
            this.TS_ClearMods.Name = "TS_ClearMods";
            this.TS_ClearMods.Size = new System.Drawing.Size(154, 22);
            this.TS_ClearMods.Text = "Clear Mods";
            this.TS_ClearMods.Click += new System.EventHandler(this.B_ClearMods_Click);
            // 
            // B_ModUp
            // 
            this.B_ModUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_ModUp.Location = new System.Drawing.Point(6, 364);
            this.B_ModUp.Name = "B_ModUp";
            this.B_ModUp.Size = new System.Drawing.Size(28, 23);
            this.B_ModUp.TabIndex = 2;
            this.B_ModUp.Text = "🠝";
            this.B_ModUp.UseVisualStyleBackColor = true;
            this.B_ModUp.Click += new System.EventHandler(this.B_ModUp_Click);
            // 
            // PB_InstallProgress
            // 
            this.PB_InstallProgress.Location = new System.Drawing.Point(6, 338);
            this.PB_InstallProgress.Name = "PB_InstallProgress";
            this.PB_InstallProgress.Size = new System.Drawing.Size(335, 23);
            this.PB_InstallProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.PB_InstallProgress.TabIndex = 5;
            this.PB_InstallProgress.Visible = false;
            // 
            // B_ModDown
            // 
            this.B_ModDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_ModDown.Location = new System.Drawing.Point(36, 364);
            this.B_ModDown.Name = "B_ModDown";
            this.B_ModDown.Size = new System.Drawing.Size(28, 23);
            this.B_ModDown.TabIndex = 3;
            this.B_ModDown.Text = "🠟";
            this.B_ModDown.UseVisualStyleBackColor = true;
            this.B_ModDown.Click += new System.EventHandler(this.B_ModDown_Click);
            // 
            // B_OpenMod
            // 
            this.B_OpenMod.Location = new System.Drawing.Point(191, 364);
            this.B_OpenMod.Name = "B_OpenMod";
            this.B_OpenMod.Size = new System.Drawing.Size(82, 23);
            this.B_OpenMod.TabIndex = 4;
            this.B_OpenMod.Text = "Open Folder";
            this.B_OpenMod.UseVisualStyleBackColor = true;
            this.B_OpenMod.Click += new System.EventHandler(this.B_OpenExport_Click);
            // 
            // B_InstallMod
            // 
            this.B_InstallMod.Location = new System.Drawing.Point(64, 364);
            this.B_InstallMod.Name = "B_InstallMod";
            this.B_InstallMod.Size = new System.Drawing.Size(59, 23);
            this.B_InstallMod.TabIndex = 2;
            this.B_InstallMod.Text = "Install";
            this.B_InstallMod.UseVisualStyleBackColor = true;
            this.B_InstallMod.Click += new System.EventHandler(this.B_InstallMod_Click);
            // 
            // RTB_ModDetails
            // 
            this.RTB_ModDetails.BackColor = System.Drawing.SystemColors.Control;
            this.RTB_ModDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RTB_ModDetails.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.RTB_ModDetails.Location = new System.Drawing.Point(6, 216);
            this.RTB_ModDetails.Name = "RTB_ModDetails";
            this.RTB_ModDetails.ReadOnly = true;
            this.RTB_ModDetails.Size = new System.Drawing.Size(335, 141);
            this.RTB_ModDetails.TabIndex = 1;
            this.RTB_ModDetails.TabStop = false;
            this.RTB_ModDetails.Text = "";
            // 
            // PB_ModPreview
            // 
            this.PB_ModPreview.Image = global::Mega_Mix_Mod_Manager.Properties.Resources.Logo;
            this.PB_ModPreview.Location = new System.Drawing.Point(6, 19);
            this.PB_ModPreview.Name = "PB_ModPreview";
            this.PB_ModPreview.Size = new System.Drawing.Size(335, 191);
            this.PB_ModPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PB_ModPreview.TabIndex = 0;
            this.PB_ModPreview.TabStop = false;
            // 
            // GB_ModList
            // 
            this.GB_ModList.Controls.Add(this.TV_ModList);
            this.GB_ModList.Location = new System.Drawing.Point(6, 6);
            this.GB_ModList.Name = "GB_ModList";
            this.GB_ModList.Size = new System.Drawing.Size(208, 392);
            this.GB_ModList.TabIndex = 3;
            this.GB_ModList.TabStop = false;
            this.GB_ModList.Text = "Mod List";
            // 
            // TV_ModList
            // 
            this.TV_ModList.CheckBoxes = true;
            this.TV_ModList.Location = new System.Drawing.Point(6, 19);
            this.TV_ModList.Name = "TV_ModList";
            this.TV_ModList.ShowLines = false;
            this.TV_ModList.ShowRootLines = false;
            this.TV_ModList.Size = new System.Drawing.Size(196, 367);
            this.TV_ModList.TabIndex = 0;
            this.TV_ModList.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TV_ToggleMod_Click);
            this.TV_ModList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TV_ModList_AfterSelect);
            // 
            // MB_Disable
            // 
            this.MB_Disable.Location = new System.Drawing.Point(124, 364);
            this.MB_Disable.Menu = this.CMS_RemoveDisable;
            this.MB_Disable.Name = "MB_Disable";
            this.MB_Disable.Size = new System.Drawing.Size(65, 23);
            this.MB_Disable.TabIndex = 7;
            this.MB_Disable.Text = "Disable";
            this.MB_Disable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MB_Disable.UseVisualStyleBackColor = true;
            this.MB_Disable.Click += new System.EventHandler(this.B_ToggleMod_Click);
            // 
            // MB_Export
            // 
            this.MB_Export.Location = new System.Drawing.Point(274, 364);
            this.MB_Export.Menu = this.CMS_Export;
            this.MB_Export.Name = "MB_Export";
            this.MB_Export.Size = new System.Drawing.Size(67, 23);
            this.MB_Export.TabIndex = 6;
            this.MB_Export.Text = "Export     ";
            this.MB_Export.UseVisualStyleBackColor = true;
            this.MB_Export.Click += new System.EventHandler(this.B_ExportMods_Click);
            // 
            // ModInstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GB_ModDetails);
            this.Controls.Add(this.GB_ModList);
            this.Name = "ModInstaller";
            this.Size = new System.Drawing.Size(573, 404);
            this.GB_ModDetails.ResumeLayout(false);
            this.CMS_RemoveDisable.ResumeLayout(false);
            this.CMS_Export.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_ModPreview)).EndInit();
            this.GB_ModList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_ModDetails;
        private IO.MenuButton MB_Disable;
        private IO.MenuButton MB_Export;
        private System.Windows.Forms.Button B_ModUp;
        private System.Windows.Forms.ProgressBar PB_InstallProgress;
        private System.Windows.Forms.Button B_ModDown;
        private System.Windows.Forms.Button B_OpenMod;
        private System.Windows.Forms.Button B_InstallMod;
        private System.Windows.Forms.RichTextBox RTB_ModDetails;
        private System.Windows.Forms.PictureBox PB_ModPreview;
        private System.Windows.Forms.GroupBox GB_ModList;
        private System.Windows.Forms.TreeView TV_ModList;
        private System.Windows.Forms.ContextMenuStrip CMS_RemoveDisable;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip CMS_Export;
        private System.Windows.Forms.ToolStripMenuItem TS_MergeMods;
        private System.Windows.Forms.ToolStripMenuItem TS_ClearMods;
    }
}
