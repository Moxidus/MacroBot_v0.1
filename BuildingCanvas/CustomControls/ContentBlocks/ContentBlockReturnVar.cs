using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
    }
}
