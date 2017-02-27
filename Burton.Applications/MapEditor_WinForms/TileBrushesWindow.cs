using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisualizerTest
{
    public partial class TileBrushesWindow : Form
    {
        public TileBrushManager TileBrushManager;

        public TileBrushesWindow(TileBrushManager InTileBrushManager)
        {
            if (InTileBrushManager == null)
                throw new ArgumentException("InTileBrushManager cannot be null!");

            TileBrushManager = InTileBrushManager;
            TileBrushImageList = new ImageList();
        }

        private void TileBrushesWindow_Load(object sender, EventArgs e)
        {
          
            TileBrushListView.Items.Clear();
            TileBrushListView.LargeImageList = TileBrushImageList;

            foreach (var Brush in TileBrushManager.Brushes)
            {
                ListViewItem item = new ListViewItem();
                item.Text = Brush.Name;
                item.ImageIndex = Brush.BrushID;
                TileBrushListView.Items.Add(item);
            }

        }
    }
}
