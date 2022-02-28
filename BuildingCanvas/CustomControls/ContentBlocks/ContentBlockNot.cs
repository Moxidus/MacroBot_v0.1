using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockNot : ContentBlock, ICode, IReturnCommand, IInputCommand
    {
        static ContentBlockNot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockNot), new FrameworkPropertyMetadata(typeof(ContentBlockNot)));
        }

        public string GetCode() => "NOT " + BlockParent.GetInputData();
    }
}
