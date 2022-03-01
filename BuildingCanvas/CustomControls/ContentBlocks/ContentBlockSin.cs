using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockSin : ContentBlock, ICode, IReturnCommand, IInputCommand
    {

        static ContentBlockSin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockSin), new FrameworkPropertyMetadata(typeof(ContentBlockSin)));
        }

        public string GetCode()
        {
            string result = "SIN(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
