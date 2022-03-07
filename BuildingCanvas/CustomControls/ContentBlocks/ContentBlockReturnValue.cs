using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockReturnValue: ContentBlock, ICode, IReturnCommand
    {

        static ContentBlockReturnValue()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockReturnValue), new FrameworkPropertyMetadata(typeof(ContentBlockReturnValue)));
        }


        public ContentBlockReturnValue() { }
        public ContentBlockReturnValue(SingleContent content)
        {

        }

        private TextBox textBoxVar;

        Brush DefaultBlockColor = new SolidColorBrush(Color.FromRgb(0, 255, 209));
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(0, 141, 135));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;
            
            textBoxVar = (TextBox)Template.FindName("PART_TextBox", this);

            base.OnApplyTemplate();
        }

        public string GetCode() => textBoxVar.Text;

        public new SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();

            content.ContentProperties = new object[1];
            content.ContentProperties[0] = textBoxVar.Text;

            return content;
        }
    }
}
