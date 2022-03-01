using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

        public override void OnApplyTemplate()
        {

            comboxOperator = (ComboBox)Template.FindName("PART_Combobox", this);
            LeftValuePanel = (StackPanel)Template.FindName("PART_LeftDataPanel", this);
            RightValuePanel = (StackPanel)Template.FindName("PART_RightDataPanel", this);

            LeftValuePanel.Drop += BlockBuildingCanvas.CanvasInputDropEvent;
            RightValuePanel.Drop += BlockBuildingCanvas.CanvasInputDropEvent;

            base.OnApplyTemplate();
        }

        public string GetCode()
        {
            string leftCode;

            if(LeftValuePanel.Children.Count > 0)
            {
                BuildingBlock leftBlock = LeftValuePanel.Children[0] as BuildingBlock;
                leftCode = (leftBlock.MainContent as ICode).GetCode();
            }
            else
            {
                leftCode = "FALSE";
            }

            string rightCode;

            if (RightValuePanel.Children.Count > 0)
            {
                BuildingBlock RightBlock = RightValuePanel.Children[0] as BuildingBlock;
                rightCode = (RightBlock.MainContent as ICode).GetCode();
            }
            else
            {
                rightCode = "FALSE";
            }

            string operation = "+";
            ComboBoxItem item = comboxOperator.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                operation = item.Content.ToString();
            }

            return $"{leftCode} {operation} {rightCode}";


        }

    }
}
