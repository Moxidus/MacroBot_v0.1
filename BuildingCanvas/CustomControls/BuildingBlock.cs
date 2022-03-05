using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using static MacroBot_v0._1.BlockData;

namespace grabbableBlocks.CustomControls
{
    [ContentProperty("MainContent")]
    class BuildingBlock : Control, ICode
    {
        public object MainContent
        {
            get { return GetValue(MainContentProperty); }
            set { SetValue(MainContentProperty, value); }
        }

        public Brush BlockColor
        {
            get { return (Brush)GetValue(BlockColorProperty); }
            set { SetValue(BlockColorProperty, value); }
        }
        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public Brush CanvasColor
        {
            get { return (Brush)GetValue(CanvasColorProperty); }
            set { SetValue(CanvasColorProperty, value); }
        }

        public static readonly DependencyProperty MainContentProperty = DependencyProperty.Register("MainContent", typeof(object), typeof(BuildingBlock), null);
        public static DependencyProperty BlockColorProperty = DependencyProperty.Register("BlockColor", typeof(Brush), typeof(BuildingBlock), null);
        public static DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Brush), typeof(BuildingBlock), null);
        public static DependencyProperty CanvasColorProperty = DependencyProperty.Register("CanvasColor", typeof(Brush), typeof(BuildingBlock), null);



        public Visibility InputNobVis
        {
            get { return (Visibility)GetValue(InputNobVisProperty); }
            set { SetValue(InputNobVisProperty, value); }
        }
        public Visibility NextNobVis
        {
            get { return (Visibility)GetValue(NextNobVisProperty); }
            set { SetValue(NextNobVisProperty, value); }
        }
        public Visibility PrevNobVis
        {
            get { return (Visibility)GetValue(PrevNobVisProperty); }
            set { SetValue(PrevNobVisProperty, value); }
        }
        public Visibility ReturnNobVis
        {
            get { return (Visibility)GetValue(ReturnNobVisProperty); }
            set { SetValue(ReturnNobVisProperty, value); }
        }

        public static DependencyProperty InputNobVisProperty = DependencyProperty.Register("InputNobVis", typeof(Visibility), typeof(BuildingBlock), null);
        public static DependencyProperty NextNobVisProperty = DependencyProperty.Register("NextNobVis", typeof(Visibility), typeof(BuildingBlock), null);
        public static DependencyProperty PrevNobVisProperty = DependencyProperty.Register("PrevNobVis", typeof(Visibility), typeof(BuildingBlock), null);
        public static DependencyProperty ReturnNobVisProperty = DependencyProperty.Register("ReturnNobVis", typeof(Visibility), typeof(BuildingBlock), null);
        static BuildingBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BuildingBlock), new FrameworkPropertyMetadata(typeof(BuildingBlock)));
        }


      


        protected StackPanel nextCommandPanel;
        protected StackPanel inputDataPanel;
        protected StackPanel contentPanel;

        private Rectangle backgroundRectangle;
        public Grid InputGRID;

        public MouseButtonEventHandler Block_MouseLeftPressed { get; private set; }
        public DragEventHandler NextCommand_Drop { get; private set; }
        public DragEventHandler Input_Drop { get; private set; }
        public MouseEventHandler Stack_mouseEnter { get; private set; }
        public MouseEventHandler Stack_mouseLeave { get; private set; }

        public string GetInputData()
        {
            if (inputDataPanel.Children.Count == 0)
                return "FALSE";
            return ((inputDataPanel.Children[0] as BuildingBlock).MainContent as ICode).GetCode();
        }


        public override void OnApplyTemplate()
        {
            nextCommandPanel = (StackPanel)Template.FindName("PART_NextCommandPanel", this);
            inputDataPanel = (StackPanel)Template.FindName("PART_InputDataPanel", this);
            contentPanel = (StackPanel)Template.FindName("PART_ContentPanel", this);
            backgroundRectangle = (Rectangle)Template.FindName("PART_BackgroundRectangle", this);
            InputGRID = (Grid)Template.FindName("PART_InputGRID", this);




            if (contentPanel != null)
            {
                contentPanel.MouseLeftButtonDown += Block_MouseLeftPressed;
                contentPanel.MouseUp += BlockBuildingCanvas.CanvasBlockBuilding_Delete;
            }
            if (backgroundRectangle != null)
            {
                backgroundRectangle.MouseLeftButtonDown += Block_MouseLeftPressed;
                backgroundRectangle.MouseUp += BlockBuildingCanvas.CanvasBlockBuilding_Delete;
            }
            
            if (inputDataPanel != null)
            {
                inputDataPanel.Drop += Input_Drop;
                inputDataPanel.DragEnter += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragEnter;
                inputDataPanel.DragLeave += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragLeave;
            }
            if (nextCommandPanel != null)
            {
                nextCommandPanel.Drop += NextCommand_Drop;
                nextCommandPanel.DragEnter += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragEnter;
                nextCommandPanel.DragLeave += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragLeave;
            }



            if (Stack_mouseEnter != null)
            {
                nextCommandPanel.MouseEnter += Stack_mouseEnter;
                inputDataPanel.MouseEnter += Stack_mouseEnter;

            }
            if (Stack_mouseLeave != null)
            {
                nextCommandPanel.MouseLeave += Stack_mouseLeave;
                inputDataPanel.MouseLeave += Stack_mouseLeave;
            }


            base.OnApplyTemplate();
        }


        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {


            if (this is BuildingBlockStart == false)
            {
                InputNobVis = Visibility.Collapsed;
                NextNobVis = Visibility.Collapsed;
                PrevNobVis = Visibility.Collapsed;
                ReturnNobVis = Visibility.Collapsed;

                if (MainContent is IInputCommand)
                    InputNobVis = Visibility.Visible;
                if (MainContent is INextCommand)
                    NextNobVis = Visibility.Visible;
                if (MainContent is IReturnCommand)
                    ReturnNobVis = Visibility.Visible;
                if (MainContent is IPrevCommand)
                    PrevNobVis = Visibility.Visible;
            }

            if (MainContent is ContentBlock)
            {
                (MainContent as ContentBlock).BlockParent = this;
                (MainContent as ContentBlock).CanvasColor = CanvasColor;
            }

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        public void SetMouseEvents(MouseButtonEventHandler block_mouseLeftPressed, DragEventHandler nextCommand_drop, DragEventHandler input_drop, MouseEventHandler stack_mouseEnter, MouseEventHandler stack_mouseLeave)
        {
            this.Block_MouseLeftPressed = block_mouseLeftPressed;
            this.NextCommand_Drop = nextCommand_drop;
            this.Input_Drop = input_drop;
            this.Stack_mouseEnter = stack_mouseEnter;
            this.Stack_mouseLeave = stack_mouseLeave;
        }

        public string GetCode()
        {
            string result = "";
            if(MainContent is ICode)
            {
                result += (MainContent as ICode).GetCode();
            }
            if (nextCommandPanel.Children.Count == 0)
                return result;

            UIElement NextBlock = nextCommandPanel.Children[0];
            if (NextBlock is BuildingBlock)
                return result + "\n" + (NextBlock as BuildingBlock).GetCode();
            return result;
        }

        public SingleBlock GetData()
        {
            SingleBlock block = new SingleBlock();

            if (inputDataPanel.Children.Count != 0)
                block.Inputcontent = (inputDataPanel.Children[0] as BuildingBlock).GetData();

            if (nextCommandPanel.Children.Count != 0)
                block.NextContent = (nextCommandPanel.Children[0] as BuildingBlock).GetData();

            if (inputDataPanel.Children.Count != 0)
                block.InsideContent = (MainContent as ContentBlock).GetData();

            block.Pos = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            return block;
        }


    }
}
