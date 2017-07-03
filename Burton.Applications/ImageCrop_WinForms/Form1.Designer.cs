namespace ImageCrop_WinForms
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
            this.ImageList = new System.Windows.Forms.ListBox();
            this.RectangleNumeric_X = new System.Windows.Forms.NumericUpDown();
            this.RectangleNumeric_Y = new System.Windows.Forms.NumericUpDown();
            this.RectangleNumeric_Width = new System.Windows.Forms.NumericUpDown();
            this.RectangleNumeric_Height = new System.Windows.Forms.NumericUpDown();
            this.CropRectangleGroup = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CropButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Button_BrowseForOutputDir = new System.Windows.Forms.Button();
            this.OutputDirectoryTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_X)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_Width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_Height)).BeginInit();
            this.CropRectangleGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImageList
            // 
            this.ImageList.AllowDrop = true;
            this.ImageList.FormattingEnabled = true;
            this.ImageList.Location = new System.Drawing.Point(12, 12);
            this.ImageList.Name = "ImageList";
            this.ImageList.Size = new System.Drawing.Size(200, 394);
            this.ImageList.TabIndex = 0;
            this.ImageList.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImageList_DragDrop);
            this.ImageList.DragEnter += new System.Windows.Forms.DragEventHandler(this.ImageList_DragEnter);
            // 
            // RectangleNumeric_X
            // 
            this.RectangleNumeric_X.Location = new System.Drawing.Point(0, 45);
            this.RectangleNumeric_X.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RectangleNumeric_X.Name = "RectangleNumeric_X";
            this.RectangleNumeric_X.Size = new System.Drawing.Size(57, 20);
            this.RectangleNumeric_X.TabIndex = 1;
            this.RectangleNumeric_X.ValueChanged += new System.EventHandler(this.RectangleNumeric_X_ValueChanged);
            // 
            // RectangleNumeric_Y
            // 
            this.RectangleNumeric_Y.Location = new System.Drawing.Point(63, 45);
            this.RectangleNumeric_Y.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RectangleNumeric_Y.Name = "RectangleNumeric_Y";
            this.RectangleNumeric_Y.Size = new System.Drawing.Size(57, 20);
            this.RectangleNumeric_Y.TabIndex = 2;
            this.RectangleNumeric_Y.ValueChanged += new System.EventHandler(this.RectangleNumeric_Y_ValueChanged);
            // 
            // RectangleNumeric_Width
            // 
            this.RectangleNumeric_Width.Location = new System.Drawing.Point(126, 45);
            this.RectangleNumeric_Width.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RectangleNumeric_Width.Name = "RectangleNumeric_Width";
            this.RectangleNumeric_Width.Size = new System.Drawing.Size(57, 20);
            this.RectangleNumeric_Width.TabIndex = 3;
            this.RectangleNumeric_Width.ValueChanged += new System.EventHandler(this.RectangleNumeric_Width_ValueChanged);
            // 
            // RectangleNumeric_Height
            // 
            this.RectangleNumeric_Height.Location = new System.Drawing.Point(189, 45);
            this.RectangleNumeric_Height.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.RectangleNumeric_Height.Name = "RectangleNumeric_Height";
            this.RectangleNumeric_Height.Size = new System.Drawing.Size(57, 20);
            this.RectangleNumeric_Height.TabIndex = 4;
            this.RectangleNumeric_Height.ValueChanged += new System.EventHandler(this.RectangleNumeric_Height_ValueChanged);
            // 
            // CropRectangleGroup
            // 
            this.CropRectangleGroup.Controls.Add(this.label4);
            this.CropRectangleGroup.Controls.Add(this.label3);
            this.CropRectangleGroup.Controls.Add(this.label2);
            this.CropRectangleGroup.Controls.Add(this.label1);
            this.CropRectangleGroup.Controls.Add(this.RectangleNumeric_X);
            this.CropRectangleGroup.Controls.Add(this.RectangleNumeric_Height);
            this.CropRectangleGroup.Controls.Add(this.RectangleNumeric_Y);
            this.CropRectangleGroup.Controls.Add(this.RectangleNumeric_Width);
            this.CropRectangleGroup.Location = new System.Drawing.Point(221, 12);
            this.CropRectangleGroup.Name = "CropRectangleGroup";
            this.CropRectangleGroup.Size = new System.Drawing.Size(257, 71);
            this.CropRectangleGroup.TabIndex = 5;
            this.CropRectangleGroup.TabStop = false;
            this.CropRectangleGroup.Text = "Crop Rectangle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(186, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Height";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Width";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "X";
            // 
            // CropButton
            // 
            this.CropButton.Location = new System.Drawing.Point(81, 17);
            this.CropButton.Name = "CropButton";
            this.CropButton.Size = new System.Drawing.Size(75, 23);
            this.CropButton.TabIndex = 6;
            this.CropButton.Text = "Crop";
            this.CropButton.UseVisualStyleBackColor = true;
            this.CropButton.Click += new System.EventHandler(this.CropButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Button_BrowseForOutputDir);
            this.groupBox1.Controls.Add(this.CropButton);
            this.groupBox1.Controls.Add(this.OutputDirectoryTextBox);
            this.groupBox1.Location = new System.Drawing.Point(221, 89);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(442, 75);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output";
            // 
            // Button_BrowseForOutputDir
            // 
            this.Button_BrowseForOutputDir.Location = new System.Drawing.Point(0, 17);
            this.Button_BrowseForOutputDir.Name = "Button_BrowseForOutputDir";
            this.Button_BrowseForOutputDir.Size = new System.Drawing.Size(75, 23);
            this.Button_BrowseForOutputDir.TabIndex = 1;
            this.Button_BrowseForOutputDir.Text = "Browse";
            this.Button_BrowseForOutputDir.UseVisualStyleBackColor = true;
            this.Button_BrowseForOutputDir.Click += new System.EventHandler(this.Button_BrowseForOutputDir_Click);
            // 
            // OutputDirectoryTextBox
            // 
            this.OutputDirectoryTextBox.Location = new System.Drawing.Point(0, 46);
            this.OutputDirectoryTextBox.Name = "OutputDirectoryTextBox";
            this.OutputDirectoryTextBox.Size = new System.Drawing.Size(286, 20);
            this.OutputDirectoryTextBox.TabIndex = 0;
            this.OutputDirectoryTextBox.TextChanged += new System.EventHandler(this.OutputDirectoryTextBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 417);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CropRectangleGroup);
            this.Controls.Add(this.ImageList);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_X)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_Width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RectangleNumeric_Height)).EndInit();
            this.CropRectangleGroup.ResumeLayout(false);
            this.CropRectangleGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ImageList;
        private System.Windows.Forms.NumericUpDown RectangleNumeric_X;
        private System.Windows.Forms.NumericUpDown RectangleNumeric_Y;
        private System.Windows.Forms.NumericUpDown RectangleNumeric_Width;
        private System.Windows.Forms.NumericUpDown RectangleNumeric_Height;
        private System.Windows.Forms.GroupBox CropRectangleGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CropButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Button_BrowseForOutputDir;
        private System.Windows.Forms.TextBox OutputDirectoryTextBox;
    }
}

