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
        public List<SingleBlock> Blocks = new List<SingleBlock>();
        public BlockData()
        {
            
        }

        [Serializable]
        public class SingleBlock
        {
            public SingleContent InsideContent;
            public SingleBlock NextContent;
            public SingleBlock Inputcontent;
            public Point Pos;
        }
        [Serializable]
        public class SingleContent
        {
            public string ContentType;
            public object[] ContentProperties;
            public SingleBlock[] BlockList;
        }
    }
}
