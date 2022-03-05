using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static MacroBot_v0._1.BlockData;

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

            result += GetBuildingBlockOrNull(DOValuePanel) == null? "\"Empty\"": GetBuildingBlockOrNull(DOValuePanel).GetCode();

            return result;
        }

        public new SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();
            content.BlockList = new SingleBlock[1];
            content.BlockList[0] = GetBuildingBlockOrNull(DOValuePanel).GetData();

            return content;
        }

    }
}
