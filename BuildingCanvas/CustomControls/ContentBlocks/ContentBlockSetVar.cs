using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockSetVar : ContentBlock, ICode, INextCommand, IPrevCommand, IInputCommand
    {
        static ContentBlockSetVar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockSetVar), new FrameworkPropertyMetadata(typeof(ContentBlockSetVar)));
        }

        private ComboBox comboxVar;

        public override void OnApplyTemplate()
        {

            comboxVar = (ComboBox)Template.FindName("PART_Combobox", this); ;
            comboxVar.ItemsSource = BlockBuildingCanvas.VariableList;

            base.OnApplyTemplate();
        }

        public string GetCode() => $"VAR {comboxVar.SelectedItem} = {BlockParent.GetInputData()}";

        public new SingleContent GetData()
        {
            SingleContent content = new SingleContent();

            content.ContentType = GetType().ToString();
            content.ContentProperties = new object[1];
            content.ContentProperties[0] = comboxVar.SelectedItem;

            return content;
        }

    }
}
