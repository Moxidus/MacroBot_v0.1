using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockReturnNumber: ContentBlock, ICode, IReturnCommand
    {
        static ContentBlockReturnNumber()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockReturnNumber), new FrameworkPropertyMetadata(typeof(ContentBlockReturnNumber)));
        }


        public ContentBlockReturnNumber()
        {
            ContentType = ContentTypes.NumberContent;
        }
        public ContentBlockReturnNumber(SingleContent content)
        {
            ContentType = ContentTypes.NumberContent;
            SettingValue = content.ContentProperties[0].ToString();
        }

        string SettingValue;
        private TextBox textBoxVar;

        Brush DefaultBlockColor = new SolidColorBrush(Color.FromRgb(0, 255, 209));
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(0, 141, 135));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;
            
            textBoxVar = (TextBox)Template.FindName("PART_TextBox", this);
            textBoxVar.Text = SettingValue;

            base.OnApplyTemplate();
        }

        public string GetCode() => textBoxVar.Text;

        public override SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();

            content.ContentProperties = new object[1];
            content.ContentProperties[0] = textBoxVar.Text;

            return content;
        }
    }
}
