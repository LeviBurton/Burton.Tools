using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizerTest
{
    [Serializable]
    public class TileImage
    {
        // We never actually delete anything, we just assign it this value
        public static int InvalidID = -1;

        // The TileImage unique ID
        public int ID;

        // It's path -- will typically be relative to the executable.
        public string Path;

        // We don't want to save our image data -- we load that at runtime.
        [NonSerialized]
        public Image Image;
    }

    // A simple database style pattern for managing "tile images"
    // Simply assigns our image a valid ID and is responsible for mainting this integrity via serialization.
    [Serializable]
    public class TileImageManager
    {
        // We are responsible for handing out valid IDs.
        public int NextValidID;

        // Our list serialized list of TileImages we maintain.
        public List<TileImage> TileImages;

        public TileImageManager()
        {
            NextValidID = 0;
            TileImages = new List<TileImage>();
        }

        public void RemoveTileImage(int TileImageID)
        {
            if (TileImages[TileImageID].ID == TileImageID)
            {
                TileImages[TileImageID].ID = TileImage.InvalidID;
            }
        }

        public TileImage GetTileImage(int TileImageID)
        {
            TileImage TheTileImage = null;

            if (TileImages[TileImageID].ID == TileImageID)
            {
                TheTileImage = TileImages[TileImageID];
            }

            return TheTileImage;
        }

        public int AddTileImage(TileImage NewTileImage)
        {
            NewTileImage.ID = NextValidID++;
            TileImages.Insert(NewTileImage.ID, NewTileImage);
            return NewTileImage.ID;
        }

        // Import a folder into the database -- assigns new IDs to all the 
        // files found using the Filter.  bAddChildren tells it to recurse.
        public void ImportFolder(string Folder, string Filter, bool bAddChildren)
        {
            foreach (string File in Directory.GetFiles(Folder, Filter))
            {
                var NewTileImage = new TileImage();
                NewTileImage.Path = File;
                NewTileImage.Image = Image.FromFile(File);
                AddTileImage(NewTileImage);
            }

            if (bAddChildren)
            {
                foreach (string ChildDir in Directory.GetDirectories(Folder))
                {
                    ImportFolder(ChildDir, Filter, bAddChildren);
                }
            }
        }

        #region Serialization
        public void Save()
        {
            using (Stream OutStream = File.Open("tileimagemanager.dat", FileMode.Create))
            {
                var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                BinaryFormatter.Serialize(OutStream, NextValidID);
                BinaryFormatter.Serialize(OutStream, TileImages);
            }
        }

        public void Load()
        {
            using (Stream InStream = File.Open("tileimagemanager.dat", FileMode.Open))
            {
                var BinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                NextValidID = (int)BinaryFormatter.Deserialize(InStream);
                TileImages = (List<TileImage>)BinaryFormatter.Deserialize(InStream);
            }

            LoadImageData();
        }

        public void LoadImageData()
        {
            foreach (var TileImage in TileImages)
            {
                if (TileImages[TileImage.ID].ID != TileImage.InvalidID)
                {
                    TileImage.Image = Image.FromFile(TileImage.Path);
                }
            }
        }

        #endregion

    }
}
