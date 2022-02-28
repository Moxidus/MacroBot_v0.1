using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
    }
}
