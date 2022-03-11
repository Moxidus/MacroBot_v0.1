using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockWhile : ContentBlock, ICode, INextCommand, IPrevCommand, IInputCommand
    {
        static ContentBlockWhile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockWhile), new FrameworkPropertyMetadata(typeof(ContentBlockWhile)));
        }

        public ContentBlockWhile() { }
        public ContentBlockWhile(SingleContent content)
        {
            if(content.BlockList != null)
                DOValuePanelHolder = new BuildingBlock(content.BlockList[0]);
        }
        
        BuildingBlock DOValuePanelHolder;
        public StackPanel DOValuePanel { get; private set; }

        Brush DefaultBlockColor = new SolidColorBrush(Colors.Yellow);
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(156, 162, 25));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;
            
            DOValuePanel = (StackPanel)Template.FindName("PART_DoDataPanel", this);
            if(DOValuePanelHolder != null)
                DOValuePanel.Children.Add(DOValuePanelHolder);
            DOValuePanel.Drop += BlockBuildingCanvas.CanvasCommandDropEvent;

            base.OnApplyTemplate();
        }

        public string GetCode()
        {
            string result = "WHILE ";
            result += BlockParent.GetInputData();
            result += " THEN \n";

            result += GetBuildingBlockOrNull(DOValuePanel) == null? "\"Empty\"": GetBuildingBlockOrNull(DOValuePanel).GetCode();

            result += "\nEND";

            return result;
        }

        public override SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();
            content.BlockList = new SingleBlock[1];
            if(GetBuildingBlockOrNull(DOValuePanel) != null)
                content.BlockList[0] = GetBuildingBlockOrNull(DOValuePanel).GetData();

            return content;
        }

    }
}
