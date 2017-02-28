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
    public partial class Window_TileBrushManager : Form
    {
        public TileBrushManager TileBrushManager;

        public Window_TileBrushManager(TileBrushManager TileBrushManager)
        {
            InitializeComponent();

            if (TileBrushManager == null)
                throw new ArgumentException("InTileBrushManager cannot be null!");

            this.TileBrushManager = TileBrushManager;
        }

        private void Window_TileBrushManager_Load(object sender, EventArgs e)
        {
            foreach (var Brush in TileBrushManager.Brushes)
            {
                ListViewItem Item = new ListViewItem();
                Item.BackColor = Brush.Color;
                Item.Name = Brush.Name;
                Item.Text = string.Format("{0} - {1}", Brush.BrushID, Brush.Name);
                ListView_Brushes.Items.Add(Item);
            }
        }
    }
}


