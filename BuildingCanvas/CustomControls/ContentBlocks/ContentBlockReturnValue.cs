using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockReturnValue: ContentBlock, ICode, IReturnCommand
    {

        static ContentBlockReturnValue()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockReturnValue), new FrameworkPropertyMetadata(typeof(ContentBlockReturnValue)));
        }

        private TextBox textBoxVar;

        public override void OnApplyTemplate()
        {

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
