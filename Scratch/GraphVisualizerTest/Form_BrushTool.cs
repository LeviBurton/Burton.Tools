using GraphVisualizerTest.CustomControls;
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
    public partial class Form_BrushTool : Form
    {
        ComboBox_BrushSelector BrushSelector;

        public Form_BrushTool()
        {
            InitializeComponent();

            BrushSelector = new ComboBox_BrushSelector();
            BrushSelector.Items.Add(new ComboBox_BrushSelectorItem("Black"));
            BrushSelector.Items.Add(new ComboBox_BrushSelectorItem("Red"));
            BrushSelector.Items.Add(new ComboBox_BrushSelectorItem("Green"));
            BrushSelector.Items.Add(new ComboBox_BrushSelectorItem("Blue"));

            BrushSelector.Parent = this;
          
            Width = BrushSelector.Width + 60;
            Height = BrushSelector.Height + 60;

            BrushSelector.Show();
        }
    }
}
