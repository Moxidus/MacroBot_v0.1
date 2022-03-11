using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockReturnVar : ContentBlock, ICode, IReturnCommand
    {
        static ContentBlockReturnVar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockReturnVar), new FrameworkPropertyMetadata(typeof(ContentBlockReturnVar)));
        }


        public ContentBlockReturnVar() { }
        public ContentBlockReturnVar(SingleContent content)
        {
            selectedItem = content.ContentProperties[0];
        }

        object selectedItem;
        private ComboBox comboxVar;

        Brush DefaultBlockColor = new SolidColorBrush(Colors.Lime);
        Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(32, 141, 0));
        public override void OnApplyTemplate()
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;

            comboxVar = (ComboBox)Template.FindName("PART_Combobox", this);
            comboxVar.ItemsSource = BlockBuildingCanvas.VariableList;
            if (selectedItem != null)
            {
                foreach (object item in comboxVar.Items)
                {
                    if (item.ToString() == selectedItem.ToString())
                    {
                        comboxVar.SelectedItem = item;
                    }
                }

            }

            base.OnApplyTemplate();
        }
        public string GetCode()
        {
            if (comboxVar.SelectedItem is null)
                return "UnselectedVarName";
            return comboxVar.SelectedItem.ToString();
        }
        public override SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();

            content.ContentProperties = new object[1];
            content.ContentProperties[0] = comboxVar.SelectedItem;

            return content;
        }
    }
}
