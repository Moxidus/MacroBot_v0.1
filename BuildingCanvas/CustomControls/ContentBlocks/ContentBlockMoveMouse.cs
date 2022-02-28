using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockMouseMove : ContentBlock, ICode, IPrevCommand, INextCommand
    {

        static ContentBlockMouseMove()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockMouseMove), new FrameworkPropertyMetadata(typeof(ContentBlockMouseMove)));
        }

        public StackPanel XStack { get; private set; }
        public StackPanel YStack { get; private set; }
        public override void OnApplyTemplate()
        {
            XStack = (StackPanel)Template.FindName("PART_XStack", this);
            XStack.Drop += BlockBuildingCanvas.CanvasInputDropEvent;
            YStack = (StackPanel)Template.FindName("PART_YStack", this);
            YStack.Drop += BlockBuildingCanvas.CanvasInputDropEvent;


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


            result += ")\n";
            return result;

        }
    }
}
