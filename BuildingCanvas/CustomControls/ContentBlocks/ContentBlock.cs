using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlock : Control
    {
        
        public BuildingBlock BlockParent;
        public Brush CanvasColor
        {
            get { return (Brush)GetValue(CanvasColorProperty); }
            set { SetValue(CanvasColorProperty, value); }
        }
        public static DependencyProperty CanvasColorProperty = DependencyProperty.Register("CanvasColor", typeof(Brush), typeof(ContentBlock), null);
        static ContentBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlock), new FrameworkPropertyMetadata(typeof(ContentBlock)));
        }

        protected BuildingBlock GetBuildingBlockOrNull(StackPanel stack) => stack.Children.Count != 0 ? stack.Children[0] as BuildingBlock : null;

        public SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();
            return content;
        }
    }
}
