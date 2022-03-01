using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockToString : ContentBlock, ICode, IReturnCommand, IInputCommand
    {

        static ContentBlockToString()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockToString), new FrameworkPropertyMetadata(typeof(ContentBlockToString)));
        }

        public string GetCode()
        {
            string result = "TO_STRING(";
            result += BlockParent.GetInputData();
            result += ")";
            return result;

        }
    }
}
