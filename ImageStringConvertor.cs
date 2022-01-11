using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace MacroBot_v0._1
{
    static class ImageStringConvertor
    {
        static public string ImageToBase64String(Image<Bgr, byte> image)
        {
            string base64 = Convert.ToBase64String(image.Bytes);
            return base64;
        }
        static public byte[] ByteFromBase64String(string base64)
        {
            byte[] memory = Convert.FromBase64String(base64);
            return memory;
        }
    }
}
