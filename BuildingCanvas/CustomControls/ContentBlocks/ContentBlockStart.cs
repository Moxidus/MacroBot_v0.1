using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockStart : ContentBlock, ICode, IInputCommand, INextCommand, IPrevCommand
    {
        static ContentBlockStart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockStart), new FrameworkPropertyMetadata(typeof(ContentBlockStart)));
        }


        public ContentBlockStart()
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.TextContent };
        }
        public ContentBlockStart(SingleContent content)
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.TextContent };
        }


        Brush DefaultBlockColor = new SolidColorBrush(Color.FromRgb(255, 80, 255));
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 34, 170));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;
        }
        public string GetCode()
        {
            string result = "START(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
 