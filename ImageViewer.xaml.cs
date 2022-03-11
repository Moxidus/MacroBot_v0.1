using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        public ImageViewer()
        {
            InitializeComponent();
            ImageWindow.SnapsToDevicePixels = true;
        }

        public static void ShowImage(Image<Bgra, byte> imageToShow)
        {
            ImageViewer viewer = new ImageViewer();
            viewer.ImageWindow.Source = ImageConvertor.ToBitmapSource(imageToShow);
            viewer.Show();
        }
    }
}
