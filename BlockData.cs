using grabbableBlocks.CustomControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MacroBot_v0._1
{
    [Serializable]
    class BlockData
    {
        public AssetHolder[] savedImages { get; set; }
        public string[] variables { get; set; }
        public List<SingleBlock> Blocks { get; set; }
        public BlockData()
        {
            Blocks = new List<SingleBlock>();
        }


        [Serializable]
        public class AssetHolder
        {
            public AssetHolder()
            {

            }
            public AssetHolder(string assetBase64, string assetName, System.Drawing.Size assetSize)
            {
                AssetBase64 = assetBase64;
                AssetName = assetName;
                AssetSize = assetSize;
            }

            public string AssetBase64 { get; set; }
            public string AssetName { get; set; }
            public System.Drawing.Size AssetSize { get; set; }

        }

        [Serializable]
        public class SingleBlock
        {
            public SingleContent InsideContent { get; set; }
            public SingleBlock NextContent { get; set; }
            public SingleBlock Inputcontent { get; set; }
            public Point Pos { get; set; }
        }
        [Serializable]
        public class SingleContent
        {
            public string ContentType { get; set; }
            public object[] ContentProperties { get; set; }
            public SingleBlock[] BlockList { get; set; }
        }
    }
}
