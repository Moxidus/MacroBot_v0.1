using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockSetVar : ContentBlock, ICode, INextCommand, IPrevCommand, IInputCommand
    {
        static ContentBlockSetVar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockSetVar), new FrameworkPropertyMetadata(typeof(ContentBlockSetVar)));
        }

        public ContentBlockSetVar() { }
        public ContentBlockSetVar(SingleContent content)
        {
            selectedItem = content.ContentProperties[0];
        }

        object selectedItem;
        private ComboBox comboxVar;

        Brush DefaultBlockColor = new SolidColorBrush(Colors.Red);
        Brush DefaultBorderColor = new SolidColorBrush(Colors.DarkRed);
        public override void OnApplyTemplate()//Combobox doesnt have any items at ApplyTemplate
        {
            BlockParent.BlockColor = DefaultBlockColor;
            BlockParent.BorderColor = DefaultBorderColor;

            comboxVar = (ComboBox)Template.FindName("PART_Combobox", this);
            comboxVar.ItemsSource = BlockBuildingCanvas.VariableList;
            if(selectedItem != null)
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

        public string GetCode() => $"VAR {comboxVar.SelectedItem} = {BlockParent.GetInputData()}";

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
