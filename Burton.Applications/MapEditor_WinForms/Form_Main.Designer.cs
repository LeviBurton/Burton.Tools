namespace GraphVisualizerTest
{
    partial class Form_Main
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
            this.GridPanel = new System.Windows.Forms.Panel();
            this.SourceButton = new System.Windows.Forms.RadioButton();
            this.TargetButton = new System.Windows.Forms.RadioButton();
            this.ObstacleButton = new System.Windows.Forms.RadioButton();
            this.WaterButton = new System.Windows.Forms.RadioButton();
            this.MudButton = new System.Windows.Forms.RadioButton();
            this.NormalButton = new System.Windows.Forms.RadioButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_SaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_View_Brushes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_View_BrushManager = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GridPanel
            // 
            this.GridPanel.AutoSize = true;
            this.GridPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridPanel.Location = new System.Drawing.Point(12, 50);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.Size = new System.Drawing.Size(726, 699);
            this.GridPanel.TabIndex = 0;
            this.GridPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GridPanel_Paint);
            // 
            // SourceButton
            // 
            this.SourceButton.AutoSize = true;
            this.SourceButton.Checked = true;
            this.SourceButton.Location = new System.Drawing.Point(12, 26);
            this.SourceButton.Name = "SourceButton";
            this.SourceButton.Size = new System.Drawing.Size(59, 17);
            this.SourceButton.TabIndex = 1;
            this.SourceButton.TabStop = true;
            this.SourceButton.Text = "Source";
            this.SourceButton.UseVisualStyleBackColor = true;
            this.SourceButton.CheckedChanged += new System.EventHandler(this.SourceButton_CheckedChanged);
            // 
            // TargetButton
            // 
            this.TargetButton.AutoSize = true;
            this.TargetButton.Location = new System.Drawing.Point(77, 25);
            this.TargetButton.Name = "TargetButton";
            this.TargetButton.Size = new System.Drawing.Size(56, 17);
            this.TargetButton.TabIndex = 2;
            this.TargetButton.Text = "Target";
            this.TargetButton.UseVisualStyleBackColor = true;
            this.TargetButton.CheckedChanged += new System.EventHandler(this.TargetButton_CheckedChanged);
            // 
            // ObstacleButton
            // 
            this.ObstacleButton.AutoSize = true;
            this.ObstacleButton.Location = new System.Drawing.Point(203, 25);
            this.ObstacleButton.Name = "ObstacleButton";
            this.ObstacleButton.Size = new System.Drawing.Size(67, 17);
            this.ObstacleButton.TabIndex = 3;
            this.ObstacleButton.Text = "Obstacle";
            this.ObstacleButton.UseVisualStyleBackColor = true;
            this.ObstacleButton.CheckedChanged += new System.EventHandler(this.ObstacleButton_CheckedChanged);
            // 
            // WaterButton
            // 
            this.WaterButton.AutoSize = true;
            this.WaterButton.Location = new System.Drawing.Point(276, 24);
            this.WaterButton.Name = "WaterButton";
            this.WaterButton.Size = new System.Drawing.Size(54, 17);
            this.WaterButton.TabIndex = 4;
            this.WaterButton.Text = "Water";
            this.WaterButton.UseVisualStyleBackColor = true;
            this.WaterButton.CheckedChanged += new System.EventHandler(this.WaterButton_CheckedChanged);
            // 
            // MudButton
            // 
            this.MudButton.AutoSize = true;
            this.MudButton.Location = new System.Drawing.Point(336, 24);
            this.MudButton.Name = "MudButton";
            this.MudButton.Size = new System.Drawing.Size(46, 17);
            this.MudButton.TabIndex = 5;
            this.MudButton.Text = "Mud";
            this.MudButton.UseVisualStyleBackColor = true;
            this.MudButton.CheckedChanged += new System.EventHandler(this.MudButton_CheckedChanged);
            // 
            // NormalButton
            // 
            this.NormalButton.AutoSize = true;
            this.NormalButton.Location = new System.Drawing.Point(139, 25);
            this.NormalButton.Name = "NormalButton";
            this.NormalButton.Size = new System.Drawing.Size(58, 17);
            this.NormalButton.TabIndex = 6;
            this.NormalButton.Text = "Normal";
            this.NormalButton.UseVisualStyleBackColor = true;
            this.NormalButton.CheckedChanged += new System.EventHandler(this.NormalButton_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(938, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_OpenFile,
            this.MenuItem_SaveFile,
            this.MenuItem_Exit});
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(37, 20);
            this.FileMenu.Text = "File";
            // 
            // MenuItem_OpenFile
            // 
            this.MenuItem_OpenFile.Name = "MenuItem_OpenFile";
            this.MenuItem_OpenFile.Size = new System.Drawing.Size(103, 22);
            this.MenuItem_OpenFile.Text = "Open";
            this.MenuItem_OpenFile.Click += new System.EventHandler(this.MenuItem_OpenFile_Click);
            // 
            // MenuItem_SaveFile
            // 
            this.MenuItem_SaveFile.Name = "MenuItem_SaveFile";
            this.MenuItem_SaveFile.Size = new System.Drawing.Size(103, 22);
            this.MenuItem_SaveFile.Text = "Save";
            this.MenuItem_SaveFile.Click += new System.EventHandler(this.MenuItem_SaveFile_Click);
            // 
            // MenuItem_Exit
            // 
            this.MenuItem_Exit.Name = "MenuItem_Exit";
            this.MenuItem_Exit.Size = new System.Drawing.Size(103, 22);
            this.MenuItem_Exit.Text = "Exit";
            this.MenuItem_Exit.Click += new System.EventHandler(this.MenuItem_Exit_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_View_Brushes,
            this.MenuItem_View_BrushManager});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // MenuItem_View_Brushes
            // 
            this.MenuItem_View_Brushes.Name = "MenuItem_View_Brushes";
            this.MenuItem_View_Brushes.Size = new System.Drawing.Size(154, 22);
            this.MenuItem_View_Brushes.Text = "Brushes";
            this.MenuItem_View_Brushes.Click += new System.EventHandler(this.MenuItem_View_Brushes_Click);
            // 
            // MenuItem_View_BrushManager
            // 
            this.MenuItem_View_BrushManager.Name = "MenuItem_View_BrushManager";
            this.MenuItem_View_BrushManager.Size = new System.Drawing.Size(154, 22);
            this.MenuItem_View_BrushManager.Text = "Brush Manager";
            this.MenuItem_View_BrushManager.Click += new System.EventHandler(this.MenuItem_View_BrushManager_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 761);
            this.Controls.Add(this.NormalButton);
            this.Controls.Add(this.MudButton);
            this.Controls.Add(this.WaterButton);
            this.Controls.Add(this.ObstacleButton);
            this.Controls.Add(this.TargetButton);
            this.Controls.Add(this.SourceButton);
            this.Controls.Add(this.GridPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_Main";
            this.Text = "Form_Main";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel GridPanel;
        private System.Windows.Forms.RadioButton SourceButton;
        private System.Windows.Forms.RadioButton TargetButton;
        private System.Windows.Forms.RadioButton ObstacleButton;
        private System.Windows.Forms.RadioButton WaterButton;
        private System.Windows.Forms.RadioButton MudButton;
        private System.Windows.Forms.RadioButton NormalButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_OpenFile;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_SaveFile;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Exit;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_View_Brushes;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_View_BrushManager;
    }
}

