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
    class ContentBlockToString : ContentBlock, ICode, IReturnCommand, IInputCommand
    {
        static ContentBlockToString()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockToString), new FrameworkPropertyMetadata(typeof(ContentBlockToString)));
        }


        public ContentBlockToString()
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.AllContent };
            ContentType = ContentTypes.TextContent;
        }
        public ContentBlockToString(SingleContent content)
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.AllContent };
            ContentType = ContentTypes.TextContent;
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
            string result = "TO_STRING(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
