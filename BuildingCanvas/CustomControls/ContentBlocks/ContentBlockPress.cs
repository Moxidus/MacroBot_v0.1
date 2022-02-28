using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockPress : ContentBlock, ICode, INextCommand, IPrevCommand, IInputCommand
    {

        static ContentBlockPress()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockPress), new FrameworkPropertyMetadata(typeof(ContentBlockPress)));
        }

        public string GetCode()
        {
            string result = "PRESS(";
            result += BlockParent.GetInputData();
            result += ")\n";
            return result;

        }
    }
}
