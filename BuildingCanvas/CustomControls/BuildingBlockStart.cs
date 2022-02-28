using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class BuildingBlockStart: BuildingBlock
    {

        public BuildingBlockStart()
        {
            this.SetResourceReference(StyleProperty, typeof(BuildingBlock));
            InputNobVis = Visibility.Collapsed;
            PrevNobVis = Visibility.Collapsed;
            NextNobVis = Visibility.Visible;
            ReturnNobVis = Visibility.Collapsed;
            
        }

        new public string GetCode()
        {
            if (nextCommandPanel.Children.Count == 0)
                return "";
            UIElement NextBlock = nextCommandPanel.Children[0];
            if (NextBlock is BuildingBlock)
                return (NextBlock as BuildingBlock).GetCode();
            return "";

        }

        public override void OnApplyTemplate()
        {
            MainContent = new Label() { Content = "Start", HorizontalAlignment = HorizontalAlignment.Left };
            BlockColor = new SolidColorBrush(Colors.White);
            BorderColor = new SolidColorBrush(Colors.Gray);
            base.OnApplyTemplate();
        }


    }
}
