using grabbableBlocks.CustomControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using static MacroBot_v0._1.BlockData;

namespace MacroBot_v0._1
{
    [Serializable]
    class ScriptData
    {
        public AssetHolder[] savedImages { get; set; }
        public string script { get; set; }
        public ScriptData()
        {
        }
    }
}
