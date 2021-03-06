using WeifenLuo.WinFormsUI.Docking;

namespace DrawingPlayground.Forms {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.canvasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.errorListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopRunningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licensesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vs2015LightTheme = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.showHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.dockPanel.Location = new System.Drawing.Point(0, 24);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.ShowAutoHideContentOnHover = false;
            this.dockPanel.Size = new System.Drawing.Size(911, 521);
            this.dockPanel.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.scriptToolStripMenuItem,
            this.panelToolStripMenuItem,
            this.runToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(911, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importToolStripMenuItem.Image")));
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.importToolStripMenuItem.Text = "&Import";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripMenuItem.Image")));
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exportToolStripMenuItem.Text = "&Export";
            // 
            // scriptToolStripMenuItem
            // 
            this.scriptToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetToolStripMenuItem});
            this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            this.scriptToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.scriptToolStripMenuItem.Text = "&Edit";
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("resetToolStripMenuItem.Image")));
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.resetToolStripMenuItem.Text = "&Reset Script";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // panelToolStripMenuItem
            // 
            this.panelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.codeEditorToolStripMenuItem,
            this.canvasToolStripMenuItem,
            this.errorListToolStripMenuItem,
            this.showConsoleToolStripMenuItem});
            this.panelToolStripMenuItem.Name = "panelToolStripMenuItem";
            this.panelToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.panelToolStripMenuItem.Text = "&View";
            // 
            // codeEditorToolStripMenuItem
            // 
            this.codeEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("codeEditorToolStripMenuItem.Image")));
            this.codeEditorToolStripMenuItem.Name = "codeEditorToolStripMenuItem";
            this.codeEditorToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.codeEditorToolStripMenuItem.Text = "&Code Editor";
            this.codeEditorToolStripMenuItem.Click += new System.EventHandler(this.codeEditorToolStripMenuItem_Click);
            // 
            // canvasToolStripMenuItem
            // 
            this.canvasToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("canvasToolStripMenuItem.Image")));
            this.canvasToolStripMenuItem.Name = "canvasToolStripMenuItem";
            this.canvasToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.canvasToolStripMenuItem.Text = "Can&vas";
            this.canvasToolStripMenuItem.Click += new System.EventHandler(this.canvasToolStripMenuItem_Click);
            // 
            // errorListToolStripMenuItem
            // 
            this.errorListToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("errorListToolStripMenuItem.Image")));
            this.errorListToolStripMenuItem.Name = "errorListToolStripMenuItem";
            this.errorListToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.errorListToolStripMenuItem.Text = "&Error List";
            this.errorListToolStripMenuItem.Click += new System.EventHandler(this.errorListToolStripMenuItem_Click);
            // 
            // showConsoleToolStripMenuItem
            // 
            this.showConsoleToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showConsoleToolStripMenuItem.Image")));
            this.showConsoleToolStripMenuItem.Name = "showConsoleToolStripMenuItem";
            this.showConsoleToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showConsoleToolStripMenuItem.Text = "C&onsole";
            this.showConsoleToolStripMenuItem.Click += new System.EventHandler(this.showConsoleToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runCodeToolStripMenuItem,
            this.stopRunningToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "&Run";
            // 
            // runCodeToolStripMenuItem
            // 
            this.runCodeToolStripMenuItem.Image = global::DrawingPlayground.Properties.Resources.RunImage;
            this.runCodeToolStripMenuItem.Name = "runCodeToolStripMenuItem";
            this.runCodeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runCodeToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.runCodeToolStripMenuItem.Text = "&Run code";
            this.runCodeToolStripMenuItem.Click += new System.EventHandler(this.RunCodeToolStripMenuItem_Click);
            // 
            // stopRunningToolStripMenuItem
            // 
            this.stopRunningToolStripMenuItem.Name = "stopRunningToolStripMenuItem";
            this.stopRunningToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.stopRunningToolStripMenuItem.Text = "&Stop running";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHelpToolStripMenuItem,
            this.gitHubToolStripMenuItem,
            this.toolStripSeparator2,
            this.licensesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // licensesToolStripMenuItem
            // 
            this.licensesToolStripMenuItem.Name = "licensesToolStripMenuItem";
            this.licensesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.licensesToolStripMenuItem.Text = "Licenses";
            this.licensesToolStripMenuItem.Click += new System.EventHandler(this.licensesToolStripMenuItem_Click);
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            this.gitHubToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gitHubToolStripMenuItem.Text = "GitHub";
            this.gitHubToolStripMenuItem.Click += new System.EventHandler(this.gitHubToolStripMenuItem_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 15000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.AddExtension = false;
            this.saveFileDialog.Filter = "JavaScript files|*.js";
            // 
            // showHelpToolStripMenuItem
            // 
            this.showHelpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showHelpToolStripMenuItem.Image")));
            this.showHelpToolStripMenuItem.Name = "showHelpToolStripMenuItem";
            this.showHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.showHelpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showHelpToolStripMenuItem.Text = "&Help";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(911, 545);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Drawing Playground";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DockPanel dockPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem panelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codeEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem canvasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem errorListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private VS2015LightTheme vs2015LightTheme;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runCodeToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem stopRunningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licensesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gitHubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

