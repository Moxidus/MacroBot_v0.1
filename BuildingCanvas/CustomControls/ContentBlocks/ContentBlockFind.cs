using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockFind : ContentBlock, ICode, IInputCommand, INextCommand, IPrevCommand
    {

        static ContentBlockFind()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockFind), new FrameworkPropertyMetadata(typeof(ContentBlockFind)));
        }

        public string GetCode()
        {
            string result = "FIND(";
            result += BlockParent.GetInputData();
            result += ")\n";
            return result;

        }
    }
}
 