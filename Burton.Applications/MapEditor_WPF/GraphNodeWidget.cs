using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MapEditor_WPF
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class VisualGraphNode : ViewModelBase
    {
        public Point CenterPoint { get; set; }
        public Rect BoundingRect { get; set; }
        public string Name { get; set; }
        public double Left;
        public double Top;

        private double _CenterX;
        public double CenterX
        {
            get { return _CenterX; }
            set { _CenterX = value; OnPropertyChanged(); }
        }

        private double _CenterY;
        public double CenterY
        {
            get { return _CenterY; }
            set { _CenterY = value; OnPropertyChanged(); }
        }

        private double _Width;
        public double Width
        {
            get { return _Width; }
            set { _Width = value; OnPropertyChanged(); }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set { _Height = value; OnPropertyChanged(); }
        }

        private System.Windows.Media.Color _Color;
        public System.Windows.Media.Color Color
        {
            get { return _Color; }
            set { _Color = value; OnPropertyChanged(); }
        }
    }
}
