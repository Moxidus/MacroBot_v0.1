using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockPrint : ContentBlock, ICode, IPrevCommand, INextCommand, IInputCommand
    {

        static ContentBlockPrint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockPrint), new FrameworkPropertyMetadata(typeof(ContentBlockPrint)));
        }

        public string GetCode()
        {
            string result = "PRINT(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
