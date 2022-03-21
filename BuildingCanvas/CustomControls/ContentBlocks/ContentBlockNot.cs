using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockNot : ContentBlock, ICode, IReturnCommand, IInputCommand
    {
        static ContentBlockNot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockNot), new FrameworkPropertyMetadata(typeof(ContentBlockNot)));
        }


        public ContentBlockNot()
        {
            AcceptedTypes = new ContentTypes[]{ ContentTypes.BoolContent };
            ContentType = ContentTypes.BoolContent;
        }
        public ContentBlockNot(SingleContent content)
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.BoolContent };
            ContentType = ContentTypes.BoolContent;
        }

        Brush DefaultBlockColor = new SolidColorBrush(Color.FromRgb(255, 80, 255));
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 34, 170));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;
            base.OnApplyTemplate();
        }
        public string GetCode() => "NOT " + BlockParent.GetInputData();
    }
}
