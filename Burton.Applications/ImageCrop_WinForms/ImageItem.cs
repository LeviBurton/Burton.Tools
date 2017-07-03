using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageCrop_WinForms
{
    public class ImageItem
    {
        public string FilePath { get; set; }
        public string BasePath { get; set; }
        public string ImageName { get; set; }

        public Rectangle CropRectangle;
        public Image SourceImage = null;
        public Image CroppedImage = null;

        // x: 63
        // y: 110
        // width: 440
        // height: 678

        public ImageItem(string FilePath)
        {
            this.FilePath = FilePath;
            this.SourceImage = Image.FromFile(FilePath);
            this.ImageName = Path.GetFileName(FilePath);
            this.BasePath = Path.GetPathRoot(FilePath);
        }
        
        public void CropImage(Rectangle CropRectangle)
        {
            using (Bitmap ImageCopy = new Bitmap(SourceImage))
            {
                CroppedImage = ImageCopy.Clone(CropRectangle, ImageCopy.PixelFormat);
            }
        }
    }
}
