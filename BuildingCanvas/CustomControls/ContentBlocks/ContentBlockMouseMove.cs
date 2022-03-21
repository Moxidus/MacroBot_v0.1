using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockMouseMove : ContentBlock, ICode, IPrevCommand, INextCommand
    {

        static ContentBlockMouseMove()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockMouseMove), new FrameworkPropertyMetadata(typeof(ContentBlockMouseMove)));
        }

        public ContentBlockMouseMove()
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.NumberContent };
        }
        public ContentBlockMouseMove(SingleContent content)
        {
            AcceptedTypes = new ContentTypes[] { ContentTypes.NumberContent };
            if (content.BlockList != null && content.BlockList[0] != null)
                XStackHolder = new BuildingBlock(content.BlockList[0]);
            if (content.BlockList != null && content.BlockList[1] != null)
                YStackHolder = new BuildingBlock(content.BlockList[1]);
        }

        BuildingBlock XStackHolder;
        BuildingBlock YStackHolder;
        public StackPanel XStack { get; private set; }
        public StackPanel YStack { get; private set; }

        Brush DefaultBlockColor = new SolidColorBrush(Color.FromRgb(255, 80, 255));
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 34, 170));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;

            XStack = (StackPanel)Template.FindName("PART_XStack", this);
            XStack.Drop += BlockBuildingCanvas.CanvasInputDropEvent;
            YStack = (StackPanel)Template.FindName("PART_YStack", this);
            YStack.Drop += BlockBuildingCanvas.CanvasInputDropEvent;

            if (XStackHolder != null)
                XStack.Children.Add(XStackHolder);
            if (YStackHolder != null)
                YStack.Children.Add(YStackHolder);


            base.OnApplyTemplate();
        }

        public string GetCode()
        {
            string result = "MOVE(";
            if (XStack.Children.Count > 0)
                result += (XStack.Children[0] as BuildingBlock).GetCode();
            else
                result += "0";
            result += ",";
            if (YStack.Children.Count > 0)
                result += (YStack.Children[0] as BuildingBlock).GetCode();
            else
                result += "0";


            result += ")";
            return result;

        }
        public override SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();

            content.BlockList = new SingleBlock[2];
            if (GetBuildingBlockOrNull(XStack) != null)
                content.BlockList[0] = GetBuildingBlockOrNull(XStack).GetData();
            if(GetBuildingBlockOrNull(YStack) != null)
                content.BlockList[1] = GetBuildingBlockOrNull(YStack).GetData();

            return content;
        }
    }
}
