using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizerTest
{
    [Serializable]
    public class TileBrushManager
    {
        public List<TileBrush> Brushes = new List<TileBrush>();
        public string BrushesFile;
        public int NextValidID;

        public TileBrushManager()
        {
            NextValidID = 0;
            Brushes.Clear();
            BrushesFile = string.Empty;
        }

        public void LoadBrushes(string FileName)
        {
            // Load brushes from their own file
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(FileName,
                                      FileMode.Open,
                                      FileAccess.Read,
                                      FileShare.Read);

            var obj = (TileBrushManager)formatter.Deserialize(stream);
            NextValidID = obj.NextValidID;
            Brushes = obj.Brushes;
            stream.Close();

            BrushesFile = FileName;
        }

        public void SaveBrushes(string FileName)
        {
            // Save ourself to our own file.  
            // this means each manager will be its own database 
            // of brushes for now.  may need to change that in the future
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(FileName,
                                           FileMode.Create,
                                           FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();

            BrushesFile = FileName;
        }

        public int AddBrush(TileBrush NewBrush)
        {
            NewBrush.BrushID = NextValidID++;
            Brushes.Add(NewBrush);

            return NextValidID;
        }

        public TileBrush GetBrush(int BrushID)
        {
            return Brushes[BrushID];
        }

        public void ImportDirectory(string DirectoryName)
        {

        }
    }

    [Serializable]
    public class TileBrush
    {
        public int BrushID;
        public string Name;
        public Color Color;
        public int Width;
        public int Height;

        public TileBrush(string Name, Color Color, int Width, int Height)
        {
            this.Name = Name;
            this.Color = Color;
            this.Width = Width;
            this.Height = Height;
        }
    }

}



//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace GraphVisualizerTest
//{
//    public partial class TileBrushesWindow : Form
//    {
//        public TileBrushManager TileBrushManager;

//        public TileBrushesWindow(TileBrushManager InTileBrushManager)
//        {
//            if (InTileBrushManager == null)
//                throw new ArgumentException("InTileBrushManager cannot be null!");

//            TileBrushManager = InTileBrushManager;
//            ListView_Brushes = new ListView();

//            this.Load += new EventHandler(TileBrushesWindow_Load);
//        }

//        public void TileBrushesWindow_Load(object sender, EventArgs e)
//        {
//            foreach (var Brush in TileBrushManager.Brushes)
//            {
//                ListViewItem item = new ListViewItem();
//                item.Text = Brush.Name;

//                ListView_Brushes.Items.Add(item);
//            }

//        }
//    }
//}
