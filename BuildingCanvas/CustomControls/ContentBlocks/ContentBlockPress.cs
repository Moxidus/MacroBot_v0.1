using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockPress : ContentBlock, ICode, INextCommand, IPrevCommand, IInputCommand
    {

        static ContentBlockPress()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockPress), new FrameworkPropertyMetadata(typeof(ContentBlockPress)));
        }


        public ContentBlockPress() { }
        public ContentBlockPress(SingleContent content)
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
            string result = "PRESS(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
