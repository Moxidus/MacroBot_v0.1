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
    class ContentBlockFind : ContentBlock, ICode, IInputCommand, INextCommand, IPrevCommand, IReturnCommand
    {

        static ContentBlockFind()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockFind), new FrameworkPropertyMetadata(typeof(ContentBlockFind)));
        }

        public ContentBlockFind() { }
        public ContentBlockFind(SingleContent content)
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
            string result = "FIND(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
 