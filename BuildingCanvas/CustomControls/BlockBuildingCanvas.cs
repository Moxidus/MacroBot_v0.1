using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace grabbableBlocks.CustomControls
{

    class BlockBuildingCanvas : Canvas
    {
        private BuildingBlock DraggedObject;

        public static ObservableCollection<string> VariableList = new ObservableCollection<string>();

        public static DragEventHandler CanvasInputDropEvent;
        public static DragEventHandler CanvasCommandDropEvent;
        public static DragEventHandler CanvasBlockBuildingCanvas_DragEnter;
        public static DragEventHandler CanvasBlockBuildingCanvas_DragLeave;
        public static MouseButtonEventHandler CanvasBlockBuilding_Delete;

        protected override void OnInitialized(EventArgs e)
        {
            VariableList.Add("PI");
            VariableList.Add("TRUE");
            VariableList.Add("FALSE");
            VariableList.Add("NULL");


            DragOver += Canvas_DragOver;
            CanvasInputDropEvent = Input_Drop;
            CanvasCommandDropEvent = NextCommand_Drop;
            CanvasBlockBuildingCanvas_DragEnter = BlockBuildingCanvas_DragEnter;
            CanvasBlockBuildingCanvas_DragLeave = BlockBuildingCanvas_DragLeave;
            CanvasBlockBuilding_Delete = Block_MouseClick;
            base.OnInitialized(e);
        }


        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);


            if (visualAdded is BuildingBlock)
            {
                (visualAdded as BuildingBlock).SetMouseEvents(Block_MouseMove, NextCommand_Drop, Input_Drop, Stack_MouseEnter, Stack_MouseLeave);
                (visualAdded as BuildingBlock).CanvasColor = Background;
            }

            if(visualAdded is Rectangle)
            {
                (visualAdded as Rectangle).DragEnter += BlockBuildingCanvas_DragEnter;
                (visualAdded as Rectangle).DragLeave += BlockBuildingCanvas_DragLeave;
                (visualAdded as Rectangle).Drop += TrashBin_Drop;
            }

        }
        double offsetX, offsetY;
        private void Block_MouseMove(object sender, MouseEventArgs e)
        {
            DraggedObject = ((sender as FrameworkElement).Parent as Grid).TemplatedParent as BuildingBlock;
            offsetX = e.GetPosition(DraggedObject).X;
            offsetY = e.GetPosition(DraggedObject).Y;
            DragDrop.DoDragDrop(DraggedObject, DraggedObject, DragDropEffects.Move);
            //after dragg
            SetZIndex(DraggedObject, 0);
            DraggedObject.Opacity = 1;
            DraggedObject.IsHitTestVisible = true;
        }

        private void Block_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Middle)
            {
                BuildingBlock tobeDel = ((sender as FrameworkElement).Parent as Grid).TemplatedParent as BuildingBlock;
                (tobeDel.Parent as BlockBuildingCanvas).Children.Remove(tobeDel);
            }
        }
        private void Stack_MouseEnter(object sender, MouseEventArgs e)
        {
            StackPanel stackP = sender as StackPanel;
            if (stackP.Children.Count > 0)
                return;
            stackP.Background = new SolidColorBrush(Color.FromArgb(255, 255, 144, 144));

        }
        private void Stack_MouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel stackP = sender as StackPanel;
            stackP.Background = new SolidColorBrush(Colors.Transparent);

        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPos = e.GetPosition((IInputElement)sender);


            SetZIndex(DraggedObject, 1);
            DraggedObject.Opacity = 0.55;
            DraggedObject.IsHitTestVisible = false;

           SetLeft(DraggedObject, dropPos.X - offsetX);
           SetTop(DraggedObject, dropPos.Y - offsetY);

            if (DraggedObject != null && DraggedObject.Parent != null && this != DraggedObject.Parent)
            {
                Panel oldParent = (Panel)DraggedObject.Parent;
                oldParent.Children.Remove(DraggedObject);
                if (oldParent is StackPanel)
                {
                    if (oldParent.Children.Count == 0)
                        oldParent.AllowDrop = true;
                }

                this.Children.Add(DraggedObject);

            }

            if(DraggedObject.MainContent is IReturnCommand)
                DraggedObject.ReturnNobVis = Visibility.Visible;

        }

        private void NextCommand_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (DraggedObject.MainContent is INextCommand == false)
                return;
            StackPanel newParent = (StackPanel)sender;
            Panel oldParent = (Panel)DraggedObject.Parent;



            if (DraggedObject is BuildingBlockStart || DraggedObject == newParent.TemplatedParent)
                return;

            if (newParent.TemplatedParent is ContentBlock && DraggedObject == (newParent.TemplatedParent as ContentBlock).BlockParent)
                return;



            if(newParent.Children.Count == 0)
            {
                oldParent.Children.Remove(DraggedObject);
                newParent.Children.Add(DraggedObject);
            }
        }

        private void Input_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (DraggedObject.MainContent is IReturnCommand == false)
                return;
            StackPanel newParent = (StackPanel)sender;
            Panel oldParent = (Panel)DraggedObject.Parent;

            if ( DraggedObject is BuildingBlockStart || DraggedObject == newParent.TemplatedParent)
                return;

            if (newParent.TemplatedParent is ContentBlock && DraggedObject == (newParent.TemplatedParent as ContentBlock).BlockParent)
                return;


            if (newParent.Children.Count == 0)
            {
                oldParent.Children.Remove(DraggedObject);
                DraggedObject.ReturnNobVis = Visibility.Collapsed;
                newParent.Children.Add(DraggedObject);
            }

        }


        //Trash Bin Events
        private void BlockBuildingCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (sender is StackPanel)
            {
                StackPanel stackP = sender as StackPanel;
                if (stackP.Children.Count > 0)
                    return;
                stackP.Background = new SolidColorBrush(Color.FromArgb(255, 255, 144, 144));
                return;
            }
            (sender as Rectangle).Fill = new SolidColorBrush(Colors.Red);
        }
        private void BlockBuildingCanvas_DragLeave(object sender, DragEventArgs e)
        {
            if (sender is StackPanel)
            {
                StackPanel stackP = sender as StackPanel;
                stackP.Background = new SolidColorBrush(Colors.Transparent);
                return;
            }
            (sender as Rectangle).Fill = new SolidColorBrush(Colors.PaleVioletRed);
        }

        private void TrashBin_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            this.Children.Remove(DraggedObject);
            (sender as Rectangle).Fill = new SolidColorBrush(Colors.PaleVioletRed);
        }

    }
}
