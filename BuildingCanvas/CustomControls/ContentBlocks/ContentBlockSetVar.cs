using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {


                    base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
        
        public string GetCode() => $"VAR {comboxVar.SelectedItem} = {BlockParent.GetInputData()}";
    }
}
