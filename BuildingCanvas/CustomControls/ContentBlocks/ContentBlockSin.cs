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
    class ContentBlockSin : ContentBlock, ICode, IReturnCommand, IInputCommand
    {

        static ContentBlockSin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockSin), new FrameworkPropertyMetadata(typeof(ContentBlockSin)));
        }


        public ContentBlockSin()
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.NumberContent };
            ContentType = ContentTypes.NumberContent;
        }
        public ContentBlockSin(SingleContent content)
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.NumberContent };
            ContentType = ContentTypes.NumberContent;
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
            string result = "SIN(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
