using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockReturnVar : ContentBlock, ICode, IReturnCommand
    {
        static ContentBlockReturnVar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockReturnVar), new FrameworkPropertyMetadata(typeof(ContentBlockReturnVar)));
        }

        private ComboBox comboxVar;

        public override void OnApplyTemplate()
        {

            comboxVar = (ComboBox)Template.FindName("PART_Combobox", this);
            
            comboxVar.ItemsSource = BlockBuildingCanvas.VariableList;

            base.OnApplyTemplate();
        }
        public string GetCode()
        {
            if (comboxVar.SelectedItem is null)
                return "UnselectedVarName";
            return comboxVar.SelectedItem.ToString();
        }
        public new SingleContent GetData()
        {
            SingleContent content = new SingleContent();
            content.ContentType = GetType().ToString();

            content.ContentProperties = new object[1];
            content.ContentProperties[1] = comboxVar.SelectedItem;

            return content;
        }
    }
}
