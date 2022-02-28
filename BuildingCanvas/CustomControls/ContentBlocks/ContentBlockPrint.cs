using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockPrint : ContentBlock, ICode, IPrevCommand, INextCommand
    {

        static ContentBlockPrint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockPrint), new FrameworkPropertyMetadata(typeof(ContentBlockPrint)));
        }

        public StackPanel TextToPrintStack { get; private set; }
        public override void OnApplyTemplate()
        {
            TextToPrintStack = (StackPanel)Template.FindName("PART_TextToPrintStack", this);
            TextToPrintStack.Drop += BlockBuildingCanvas.CanvasInputDropEvent;


            base.OnApplyTemplate();
        }

        public string GetCode()
        {
            string result = "PRINT(";
            result += (TextToPrintStack.Children[0] as BuildingBlock).GetCode();


            result += ")\n";
            return result;

        }
    }
}
