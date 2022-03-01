using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockWhile : ContentBlock, ICode, INextCommand, IPrevCommand, IInputCommand
    {
        static ContentBlockWhile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockWhile), new FrameworkPropertyMetadata(typeof(ContentBlockWhile)));
        }

        public StackPanel DOValuePanel { get; private set; }
        public override void OnApplyTemplate()
        {

            DOValuePanel = (StackPanel)Template.FindName("PART_DoDataPanel", this);
            DOValuePanel.Drop += BlockBuildingCanvas.CanvasCommandDropEvent;

            base.OnApplyTemplate();
        }

        public string GetCode()
        {
            string result = "WHILE ";
            result += BlockParent.GetInputData();
            result += " THEN \n";

            if (DOValuePanel.Children.Count > 0)
                result += (DOValuePanel.Children[0] as BuildingBlock).GetCode();
            else
                result += "\"Empty\"";

            result += "\nEND";

            return result;
        }

    }
}
