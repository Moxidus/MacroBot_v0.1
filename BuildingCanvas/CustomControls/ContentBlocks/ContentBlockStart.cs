using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockStart : ContentBlock, ICode, IInputCommand, INextCommand, IPrevCommand
    {

        static ContentBlockStart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockStart), new FrameworkPropertyMetadata(typeof(ContentBlockStart)));
        }

        public string GetCode()
        {
            string result = "START(";
            result += BlockParent.GetInputData();
            result += ")\n";
            return result;

        }
    }
}
 