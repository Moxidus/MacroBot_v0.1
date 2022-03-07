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
    class ContentBlockDelay : ContentBlock, ICode, IPrevCommand, INextCommand, IInputCommand
    {

        static ContentBlockDelay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockDelay), new FrameworkPropertyMetadata(typeof(ContentBlockDelay)));
        }

        public ContentBlockDelay() { }
        public ContentBlockDelay(SingleContent content)
        {

        }

        Brush DefaultBlockColor = new SolidColorBrush(Color.FromRgb(255, 80, 255));
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 34, 170));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;
            base.OnApplyTemplate();
        }
        public string GetCode()
        {
            string result = "DELAY(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;
        }
    }
}
