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

        public enum ContentTypes
        {
            TextContent,
            BoolContent,
            ImageContent,
            NumberContent,
            ArrayContent,
            AllContent,
            NoContent
        }

        public ContentTypes[] AcceptedTypes = { ContentTypes.NoContent};
        public ContentTypes ContentType = ContentTypes.NoContent;

        public bool IsCompatible(BuildingBlock buildingBlock)
        {
            ContentBlock foreignContent = buildingBlock.MainContent as ContentBlock;

            if (AcceptedTypes[0] == ContentTypes.AllContent)
                return true;
            if (AcceptedTypes[0] == ContentTypes.NoContent)
                return false;
            if (AcceptedTypes[0] == foreignContent.ContentType)
                return true;
            if (foreignContent.ContentType == ContentTypes.AllContent)
                return true;
            return false;
        }

        static ContentBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlock), new FrameworkPropertyMetadata(typeof(ContentBlock)));
        }

        Brush DefaultBlockColor = new SolidColorBrush(Colors.White);
        Brush DefaultBorderColor = new SolidColorBrush(Colors.DarkGray);
        public override void OnApplyTemplate()
        {
            if(BlockParent.BlockColor == null)
                BlockParent.BlockColor = DefaultBlockColor;
            if (BlockParent.BorderColor == null)
                BlockParent.BorderColor = DefaultBorderColor;
            base.OnApplyTemplate();
        }
        protected BuildingBlock GetBuildingBlockOrNull(StackPanel stack) => stack.Children.Count != 0 ? stack.Children[0] as BuildingBlock : null;

        public virtual SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();
            return content;
        }
    }
}
