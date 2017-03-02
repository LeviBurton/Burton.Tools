namespace GraphVisualizerTest
{
    partial class Window_TileBrushManager
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
            this.ListView_Brushes = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // ListView_Brushes
            // 
            this.ListView_Brushes.Location = new System.Drawing.Point(13, 47);
            this.ListView_Brushes.Name = "ListView_Brushes";
            this.ListView_Brushes.Size = new System.Drawing.Size(257, 378);
            this.ListView_Brushes.TabIndex = 0;
            this.ListView_Brushes.UseCompatibleStateImageBehavior = false;
            this.ListView_Brushes.View = System.Windows.Forms.View.Tile;
            // 
            // Window_TileBrushManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 437);
            this.Controls.Add(this.ListView_Brushes);
            this.Name = "Window_TileBrushManager";
            this.Text = "Window_TileBrushManager";
            this.Load += new System.EventHandler(this.Window_TileBrushManager_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ListView_Brushes;
    }
}