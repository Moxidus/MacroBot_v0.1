using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Emgu.CV.Structure;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Controls;

using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace MacroBot_v0._1
{
    //It has memmory leak somewhere
    static class SnipMaker
    {
        static public Image<Bgr, byte> takeSnip()
        {
            Image<Bgr, byte> bmp = SnippingTool.Snip();
            if (bmp != null)
            {
                return (bmp);
            }
            return null;
        }
    }

    public partial class SnippingTool : Window, IDisposable
    {
        public static Image<Bgr, byte> Snip()
        {
            int screenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int screenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);

                using (var snipper = new SnippingTool(bmp))
                {
                    if (snipper.ShowDialog() == true)
                    {
                        Bitmap bitmapImage = new Bitmap(snipper.Image);

                        Rectangle rectangle = new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height);//System.Drawing
                        BitmapData bmpData = bitmapImage.LockBits(rectangle, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);//System.Drawing.Imaging

                        Image<Bgr, byte> outputImage = new Image<Bgr, byte>(bitmapImage.Width, bitmapImage.Height, bmpData.Stride, bmpData.Scan0);

                        bitmapImage.Dispose();
                        snipper.Close();
                        return outputImage;
                    }
                }
                return null;
            }
        }

        public SnippingTool(System.Drawing.Image screenShot)
        {
            background = new System.Drawing.Bitmap(screenShot);

            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                background.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            var brush = new ImageBrush(bitmapSource);


            canvas = new Canvas();
            System.Windows.Media.Brush brh = new VisualBrush();
            brh.Opacity = 0;
            canvas.Background = brh;
            this.Content = canvas;


            rectangl = new System.Windows.Shapes.Rectangle();

            rectangl.Width = 0;
            rectangl.Height = 0;
            rectangl.Stroke = new SolidColorBrush(Colors.Red);
            rectangl.StrokeThickness = 2;
            Canvas.SetLeft(rectangl, 0);
            Canvas.SetTop(rectangl, 0);

            canvas.Children.Add(rectangl);


            this.Background = brush;
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.KeyDown += keyDownEvent;

        }
        Canvas canvas;
        System.Windows.Shapes.Rectangle rectangl;
        private Bitmap background;
        public System.Drawing.Image Image { get; set; }

        private Rectangle rcSelect = new Rectangle();
        private System.Windows.Point pntStart;
        private bool disposedValue;

        protected void OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            // Start the snip on mouse down
            if (e.LeftButton != MouseButtonState.Pressed) return;
            pntStart = e.GetPosition(this);
            rcSelect = new Rectangle(new System.Drawing.Point((int)pntStart.X, (int)pntStart.Y), new System.Drawing.Size(0, 0));
            this.OnPaint();
        }
        protected void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Modify the selection on mouse move
            if (e.LeftButton != MouseButtonState.Pressed) return;
            System.Windows.Point pos = e.GetPosition(this);
            int x1 = Math.Min((int)pos.X, (int)pntStart.X);
            int y1 = Math.Min((int)pos.Y, (int)pntStart.Y);
            int x2 = Math.Max((int)pos.X, (int)pntStart.X);
            int y2 = Math.Max((int)pos.Y, (int)pntStart.Y);
            rcSelect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            this.OnPaint();
        }
        protected void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Complete the snip on mouse-up
            if (rcSelect.Width <= 0 || rcSelect.Height <= 0) return;

            Image = new Bitmap(rcSelect.Width, rcSelect.Height);
            using (Graphics gr = Graphics.FromImage(Image))
            {
                gr.DrawImage(background, new Rectangle(0, 0, Image.Width, Image.Height), rcSelect, GraphicsUnit.Pixel);
            }
            DialogResult = true;
        }
        protected void OnPaint()
        {
            rectangl.Width = rcSelect.Width;
            rectangl.Height = rcSelect.Height;
            Canvas.SetLeft(rectangl, rcSelect.X);
            Canvas.SetTop(rectangl, rcSelect.Y);

        }
        protected void keyDownEvent(object sender, KeyEventArgs e)
        {
            // Allow canceling the snip with the Escape key
            if (e.Key == Key.Escape) this.DialogResult = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    rectangl = null;
                    this.Image = null;
                    disposedValue = true;
                    background.Dispose();
                    canvas.Children.Clear();
                    Background = null;
                }
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }


}
