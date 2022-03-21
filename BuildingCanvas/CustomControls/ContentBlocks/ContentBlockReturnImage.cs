using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockReturnImage : ContentBlock, ICode, IReturnCommand
    {
        static ContentBlockReturnImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockReturnImage), new FrameworkPropertyMetadata(typeof(ContentBlockReturnImage)));
        }


        public ContentBlockReturnImage()
        {
            ContentType = ContentTypes.ImageContent;
        }
        public ContentBlockReturnImage(SingleContent content)
        {
            ContentType = ContentTypes.ImageContent;
            selectedItem = content.ContentProperties[0];
        }

        object selectedItem;
        private ComboBox comboxImage;

        Brush DefaultBlockColor = new SolidColorBrush(Colors.Lime);
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(32, 141, 0));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;

            comboxImage = (ComboBox)Template.FindName("PART_Combobox", this);
            comboxImage.ItemsSource = BlockBuildingCanvas.ImageList;
            if (selectedItem != null)
            {
                foreach (object item in comboxImage.Items)
                {
                    if (item.ToString() == selectedItem.ToString())
                    {
                        comboxImage.SelectedItem = item;
                    }
                }

            }

            base.OnApplyTemplate();
        }
        public string GetCode()
        {
            if (comboxImage.SelectedItem is null)
                return "UnselectedImageName";
            return comboxImage.SelectedItem.ToString();
        }
        public override SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();

            content.ContentProperties = new object[1];
            content.ContentProperties[0] = comboxImage.SelectedItem;

            return content;
        }
    }
}
