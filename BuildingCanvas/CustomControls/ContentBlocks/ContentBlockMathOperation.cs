using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockMathOperation : ContentBlock, ICode, IReturnCommand
    {
        static ContentBlockMathOperation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockMathOperation), new FrameworkPropertyMetadata(typeof(ContentBlockMathOperation)));
        }

        private ComboBox comboxOperator;

        private StackPanel LeftValuePanel;
        private StackPanel RightValuePanel;


        public ContentBlockMathOperation() { }
        public ContentBlockMathOperation(SingleContent content)
        {
            selectedItem = content.ContentProperties[0];
            if (content.BlockList != null)
                LeftValuePanelHolder = new BuildingBlock(content.BlockList[0]);
            if (content.BlockList != null)
                RightValuePanelkHolder = new BuildingBlock(content.BlockList[1]);
        }

        object selectedItem;
        BuildingBlock LeftValuePanelHolder;
        BuildingBlock RightValuePanelkHolder;

        Brush DefaultBlockColor = new SolidColorBrush(Color.FromRgb(209, 0, 255));
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(90, 0, 141));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;

            comboxOperator = (ComboBox)Template.FindName("PART_Combobox", this);
            LeftValuePanel = (StackPanel)Template.FindName("PART_LeftDataPanel", this);
            RightValuePanel = (StackPanel)Template.FindName("PART_RightDataPanel", this);

            //setting values from save file
            if (LeftValuePanelHolder != null)
                LeftValuePanel.Children.Add(LeftValuePanelHolder);
            if (RightValuePanelkHolder != null)
                RightValuePanel.Children.Add(RightValuePanelkHolder);

            foreach (object item in comboxOperator.Items)
            {
                if (item.ToString() == selectedItem.ToString())
                {
                    comboxOperator.SelectedItem = item;
                }
            }


            LeftValuePanel.Drop += BlockBuildingCanvas.CanvasInputDropEvent;
            RightValuePanel.Drop += BlockBuildingCanvas.CanvasInputDropEvent;

            base.OnApplyTemplate();
        }

        public string GetCode()
        {
            string leftCode = GetBuildingBlockOrNull(LeftValuePanel) == null ? "FALSE" : GetBuildingBlockOrNull(LeftValuePanel).GetCode();

            string rightCode = GetBuildingBlockOrNull(RightValuePanel) == null ? "FALSE" : GetBuildingBlockOrNull(RightValuePanel).GetCode();

            string operation = "+";
            ComboBoxItem item = comboxOperator.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                operation = item.Content.ToString();
            }

            return $"{leftCode} {operation} {rightCode}";
        }

        public override SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();

            content.ContentProperties = new object[1];
            content.ContentProperties[0] = comboxOperator.SelectedItem == null ? "+" : comboxOperator.SelectedItem.ToString();

            content.BlockList = new SingleBlock[2];
            content.BlockList[0] = GetBuildingBlockOrNull(LeftValuePanel).GetData();
            content.BlockList[1] = GetBuildingBlockOrNull(RightValuePanel).GetData();

            return content;
        }

    }
}
