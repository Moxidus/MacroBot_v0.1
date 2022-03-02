using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace MacroBot_v0._1
{
    class AssetItem
    {
        public AssetItem(Image<Bgr, byte> asset, string name)
        {
            this.asset = asset;
            this.name = name;
        }

        public readonly Image<Bgr, byte> asset;
        public string name;

        public override string ToString()
        {
            return name;
        }
    }
}
