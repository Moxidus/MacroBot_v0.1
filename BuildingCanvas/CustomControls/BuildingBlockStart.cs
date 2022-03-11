using MacroBot_v0._1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

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

        public override SingleBlock GetData()   
        {
            SingleBlock block = new SingleBlock();
            block.InsideContent = new SingleContent();
            block.InsideContent.ContentType = typeof(BuildingBlockStart).ToString();

            if (nextCommandPanel.Children.Count != 0)
                block.NextContent = (nextCommandPanel.Children[0] as BuildingBlock).GetData();

            block.Pos = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            return block;
        }


    }
}
