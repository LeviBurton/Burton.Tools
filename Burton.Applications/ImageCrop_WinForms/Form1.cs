using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageCrop_WinForms
{
    public partial class Form1 : Form
    {
        public List<ImageItem> ImageItems = new List<ImageItem>();
        public BindingSource BindingImageItemsDataSource = new BindingSource();

        public string OutputDirectory;
        public Rectangle CropRectangle;

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CropRectangle = new Rectangle(63, 110, 440, 678);
            this.RectangleNumeric_X.Value = CropRectangle.X;
            this.RectangleNumeric_Y.Value = CropRectangle.Y;
            this.RectangleNumeric_Width.Value = CropRectangle.Width;
            this.RectangleNumeric_Height.Value = CropRectangle.Height;

            BindingImageItemsDataSource.DataSource = ImageItems;
            ImageList.DataSource = BindingImageItemsDataSource;
            ImageList.DisplayMember = "ImageName";
            ImageList.ValueMember = "FilePath";
        }

        private void ImageList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void ImageList_DragDrop(object sender, DragEventArgs e)
        {
            string[] DroppedFilePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
           
            foreach (var DroppedFilePath in DroppedFilePaths)
            {
                ImageItem NewImageItem = new ImageItem(DroppedFilePath);

                if (!ImageItems.Select(x => x.FilePath).Contains(DroppedFilePath))
                {
                    ImageItems.Add(NewImageItem);
                }
            }
            BindingImageItemsDataSource.ResetBindings(false);
        }

        private void Button_BrowseForOutputDir_Click(object sender, EventArgs e)
        {
            var Dialog = new System.Windows.Forms.FolderBrowserDialog();
            // Set the help text description for the FolderBrowserDialog.
            Dialog.Description =
                "Select the directory that you want to use as the default.";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            Dialog.ShowNewFolderButton = false;

            // Default to the My Documents folder.
           Dialog.RootFolder = Environment.SpecialFolder.Personal;

            DialogResult result = Dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.OutputDirectory = Dialog.SelectedPath;
                this.OutputDirectoryTextBox.Text = this.OutputDirectory;
            }
        }

        private void CropButton_Click(object sender, EventArgs e)
        {
            foreach (var ImageItem in ImageItems)
            {
                ImageItem.CropImage(CropRectangle);
                ImageItem.CroppedImage.Save(Path.Combine(OutputDirectory, ImageItem.ImageName), System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void RectangleNumeric_X_ValueChanged(object sender, EventArgs e)
        {
            CropRectangle.X = (int)this.RectangleNumeric_X.Value;
        }

        private void RectangleNumeric_Y_ValueChanged(object sender, EventArgs e)
        {
            CropRectangle.Y = (int)this.RectangleNumeric_Y.Value;
        }

        private void RectangleNumeric_Width_ValueChanged(object sender, EventArgs e)
        {
            CropRectangle.Width = (int)this.RectangleNumeric_Width.Value;
        }

        private void RectangleNumeric_Height_ValueChanged(object sender, EventArgs e)
        {
            CropRectangle.Height = (int)this.RectangleNumeric_Height.Value;
        }

        private void OutputDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            OutputDirectory = OutputDirectoryTextBox.Text;
        }
    }
}
