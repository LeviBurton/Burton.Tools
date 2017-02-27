namespace GraphVisualizerTest
{
    partial class TileBrushesWindow
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Test");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Test 2");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("asdasdasd");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("344333333d");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("34444");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("asdasdasdasdasd");
            this.TileBrushListView = new System.Windows.Forms.ListView();
            this.TileBrushImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // TileBrushListView
            // 
            listViewItem3.ToolTipText = "a";
            this.TileBrushListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.TileBrushListView.Location = new System.Drawing.Point(12, 50);
            this.TileBrushListView.Name = "TileBrushListView";
            this.TileBrushListView.Size = new System.Drawing.Size(368, 332);
            this.TileBrushListView.TabIndex = 0;
            this.TileBrushListView.TileSize = new System.Drawing.Size(100, 36);
            this.TileBrushListView.UseCompatibleStateImageBehavior = false;
            // 
            // TileBrushImageList
            // 
            this.TileBrushImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.TileBrushImageList.ImageSize = new System.Drawing.Size(64, 64);
            this.TileBrushImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TileBrushesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 394);
            this.Controls.Add(this.TileBrushListView);
            this.Name = "TileBrushesWindow";
            this.Text = "TileBrushesWindow";
            this.Load += new System.EventHandler(this.TileBrushesWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView TileBrushListView;
        private System.Windows.Forms.ImageList TileBrushImageList;
    }
}