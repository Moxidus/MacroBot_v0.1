using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockDelay : ContentBlock, ICode, IPrevCommand, INextCommand, IInputCommand
    {

        static ContentBlockDelay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockDelay), new FrameworkPropertyMetadata(typeof(ContentBlockDelay)));
        }

        public override void OnApplyTemplate()
        {
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
