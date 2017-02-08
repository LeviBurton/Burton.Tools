namespace GraphVisualizerTest
{
    partial class Form1
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
            this.SuspendLayout();
            // 
            // GridPanel
            // 
            this.GridPanel.Location = new System.Drawing.Point(12, 41);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.Size = new System.Drawing.Size(725, 708);
            this.GridPanel.TabIndex = 0;
            this.GridPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GridPanel_Paint);
            // 
            // SourceButton
            // 
            this.SourceButton.AutoSize = true;
            this.SourceButton.Checked = true;
            this.SourceButton.Location = new System.Drawing.Point(12, 13);
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
            this.TargetButton.Location = new System.Drawing.Point(77, 12);
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
            this.ObstacleButton.Location = new System.Drawing.Point(203, 12);
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
            this.WaterButton.Location = new System.Drawing.Point(276, 11);
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
            this.MudButton.Location = new System.Drawing.Point(336, 11);
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
            this.NormalButton.Location = new System.Drawing.Point(139, 12);
            this.NormalButton.Name = "NormalButton";
            this.NormalButton.Size = new System.Drawing.Size(58, 17);
            this.NormalButton.TabIndex = 6;
            this.NormalButton.Text = "Normal";
            this.NormalButton.UseVisualStyleBackColor = true;
            this.NormalButton.CheckedChanged += new System.EventHandler(this.NormalButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.NormalButton);
            this.Controls.Add(this.MudButton);
            this.Controls.Add(this.WaterButton);
            this.Controls.Add(this.ObstacleButton);
            this.Controls.Add(this.TargetButton);
            this.Controls.Add(this.SourceButton);
            this.Controls.Add(this.GridPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
    }
}

