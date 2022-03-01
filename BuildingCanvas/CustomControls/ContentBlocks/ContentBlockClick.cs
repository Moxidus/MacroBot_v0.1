using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockClick : ContentBlock, ICode, INextCommand, IPrevCommand
    {

        static ContentBlockClick()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockClick), new FrameworkPropertyMetadata(typeof(ContentBlockClick)));
        }

        public string GetCode()
        {
            string result = "CLICK()";
            return result;

        }
    }
}
