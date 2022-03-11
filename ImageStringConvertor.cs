using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MacroBot_v0._1
{
    static class ImageConvertor
    {
        static public string ImageToBase64String(Image<Bgra, byte> image)
        {
            string base64 = Convert.ToBase64String(image.Bytes);
            return base64;
        }
        static public byte[] ByteFromBase64String(string base64)
        {
            byte[] memory = Convert.FromBase64String(base64);
            return memory;
        }

        public static BitmapSource ToBitmapSource(Image<Bgra, byte> image)
        {
            using (System.Drawing.Bitmap source = image.ToBitmap())
            {
                IntPtr ptr = source.GetHbitmap();//could cause memmory leak... POG

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                return bs;
            }
        }

    }
}
