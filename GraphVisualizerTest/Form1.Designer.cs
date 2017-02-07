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
            this.SuspendLayout();
            // 
            // GridPanel
            // 
            this.GridPanel.Location = new System.Drawing.Point(16, 50);
            this.GridPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GridPanel.Name = "GridPanel";
            this.GridPanel.Size = new System.Drawing.Size(967, 857);
            this.GridPanel.TabIndex = 0;
            this.GridPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.GridPanel_Paint);
          
            // 
            // SourceButton
            // 
            this.SourceButton.AutoSize = true;
            this.SourceButton.Checked = true;
            this.SourceButton.Location = new System.Drawing.Point(16, 16);
            this.SourceButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SourceButton.Name = "SourceButton";
            this.SourceButton.Size = new System.Drawing.Size(74, 21);
            this.SourceButton.TabIndex = 1;
            this.SourceButton.TabStop = true;
            this.SourceButton.Text = "Source";
            this.SourceButton.UseVisualStyleBackColor = true;
            this.SourceButton.CheckedChanged += new System.EventHandler(this.SourceButton_CheckedChanged);
            // 
            // TargetButton
            // 
            this.TargetButton.AutoSize = true;
            this.TargetButton.Location = new System.Drawing.Point(103, 15);
            this.TargetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TargetButton.Name = "TargetButton";
            this.TargetButton.Size = new System.Drawing.Size(71, 21);
            this.TargetButton.TabIndex = 2;
            this.TargetButton.Text = "Target";
            this.TargetButton.UseVisualStyleBackColor = true;
            this.TargetButton.CheckedChanged += new System.EventHandler(this.TargetButton_CheckedChanged);
            // 
            // ObstacleButton
            // 
            this.ObstacleButton.AutoSize = true;
            this.ObstacleButton.Location = new System.Drawing.Point(185, 16);
            this.ObstacleButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ObstacleButton.Name = "ObstacleButton";
            this.ObstacleButton.Size = new System.Drawing.Size(85, 21);
            this.ObstacleButton.TabIndex = 3;
            this.ObstacleButton.Text = "Obstacle";
            this.ObstacleButton.UseVisualStyleBackColor = true;
            this.ObstacleButton.CheckedChanged += new System.EventHandler(this.ObstacleButton_CheckedChanged);
            // 
            // WaterButton
            // 
            this.WaterButton.AutoSize = true;
            this.WaterButton.Location = new System.Drawing.Point(283, 15);
            this.WaterButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.WaterButton.Name = "WaterButton";
            this.WaterButton.Size = new System.Drawing.Size(67, 21);
            this.WaterButton.TabIndex = 4;
            this.WaterButton.Text = "Water";
            this.WaterButton.UseVisualStyleBackColor = true;
            this.WaterButton.CheckedChanged += new System.EventHandler(this.WaterButton_CheckedChanged);
            // 
            // MudButton
            // 
            this.MudButton.AutoSize = true;
            this.MudButton.Location = new System.Drawing.Point(363, 15);
            this.MudButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MudButton.Name = "MudButton";
            this.MudButton.Size = new System.Drawing.Size(56, 21);
            this.MudButton.TabIndex = 5;
            this.MudButton.Text = "Mud";
            this.MudButton.UseVisualStyleBackColor = true;
            this.MudButton.CheckedChanged += new System.EventHandler(this.MudButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 937);
            this.Controls.Add(this.MudButton);
            this.Controls.Add(this.WaterButton);
            this.Controls.Add(this.ObstacleButton);
            this.Controls.Add(this.TargetButton);
            this.Controls.Add(this.SourceButton);
            this.Controls.Add(this.GridPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
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
    }
}

