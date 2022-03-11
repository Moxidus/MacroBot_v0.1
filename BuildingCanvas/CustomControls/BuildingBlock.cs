using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
        public static DependencyProperty BlockColorProperty = DependencyProperty.Register("BlockColor", typeof(Brush), typeof(BuildingBlock), null /*new PropertyMetadata(Brushes.White)*/);
        public static DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Brush), typeof(BuildingBlock), null /*new PropertyMetadata(Brushes.DarkGray)*/);
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

        public BuildingBlock()
        {
            CanvasColor = BlockBuildingCanvas.CanvasColor;
            Foreground = new SolidColorBrush(Colors.White);
        }


        public BuildingBlock(SingleBlock meData) {


            Foreground = new SolidColorBrush(Colors.White);
            CanvasColor = BlockBuildingCanvas.CanvasColor;

            Canvas.SetLeft(this, meData.Pos.X);
            Canvas.SetTop(this, meData.Pos.Y);

            Type type = Type.GetType(meData.InsideContent.ContentType);
            ConstructorInfo ctor = type.GetConstructor(new Type[] { typeof(SingleContent) });
            object instance = ctor.Invoke(new object[]{ meData.InsideContent });
            MainContent = instance;

            if(meData.NextContent != null)
                NextChild = new BuildingBlock(meData.NextContent);
            if(meData.Inputcontent != null)
                InputChild = new BuildingBlock(meData.Inputcontent);

        }

        public static BuildingBlock CreateBlockOrStart(SingleBlock meData)
        {
            if (Type.GetType(meData.InsideContent.ContentType) != typeof(BuildingBlockStart))
                return new BuildingBlock(meData);

            BuildingBlockStart startBlock = new BuildingBlockStart();

            Canvas.SetLeft(startBlock, meData.Pos.X);
            Canvas.SetTop(startBlock, meData.Pos.Y);
            
            if (meData.NextContent != null)
                startBlock.NextChild = new BuildingBlock(meData.NextContent);

            return startBlock;

        }


        public UIElement NextChild;//is public because BBStart needs it
        UIElement InputChild;
        protected StackPanel nextCommandPanel;
        protected StackPanel inputDataPanel;
        protected StackPanel contentPanel;

        private Rectangle backgroundRectangle;
        public Grid InputGRID;

        private Border inputNob;

        public string GetInputData()
        {
            if (inputDataPanel.Children.Count == 0)
                return "FALSE";
            return ((inputDataPanel.Children[0] as BuildingBlock).MainContent as ICode).GetCode();
        }

        public override void OnApplyTemplate()
        {
            nextCommandPanel = Template.FindName("PART_NextCommandPanel", this) as StackPanel;
            if(NextChild != null)
                nextCommandPanel.Children.Add(NextChild);

            inputDataPanel = Template.FindName("PART_InputDataPanel", this) as StackPanel;
            if (InputChild != null)
                inputDataPanel.Children.Add(InputChild);

            contentPanel = Template.FindName("PART_ContentPanel", this) as StackPanel;
            backgroundRectangle = Template.FindName("PART_BackgroundRectangle", this) as Rectangle;
            InputGRID = Template.FindName("PART_InputGRID", this) as Grid;

            inputNob = Template.FindName("PART_InputNob", this) as Border;


            
            
            if (contentPanel != null)
            {
                contentPanel.MouseLeftButtonDown += BlockBuildingCanvas.CanvasBlockBuilding_MouseMove;
                contentPanel.MouseUp += BlockBuildingCanvas.CanvasBlockBuilding_Delete;
            }
            
            if (backgroundRectangle != null)
            {
                backgroundRectangle.MouseLeftButtonDown += BlockBuildingCanvas.CanvasBlockBuilding_MouseMove;
                backgroundRectangle.MouseUp += BlockBuildingCanvas.CanvasBlockBuilding_Delete;
            }
            
            if (inputDataPanel != null)
            {
                inputDataPanel.Drop += BlockBuildingCanvas.CanvasInputDropEvent;
                inputDataPanel.DragEnter += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragEnter;
                inputDataPanel.DragLeave += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragLeave;
            }
            if (nextCommandPanel != null)
            {
                nextCommandPanel.Drop += BlockBuildingCanvas.CanvasCommandDropEvent;
                nextCommandPanel.DragEnter += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragEnter;
                nextCommandPanel.DragLeave += BlockBuildingCanvas.CanvasBlockBuildingCanvas_DragLeave;
            }
                        
            nextCommandPanel.MouseEnter += BlockBuildingCanvas.CanvasBlockBuilding_MouseEnter;
            inputDataPanel.MouseEnter += BlockBuildingCanvas.CanvasBlockBuilding_MouseEnter;
                        
            nextCommandPanel.MouseLeave += BlockBuildingCanvas.CanvasBlockBuilding_MouseLeave;
            inputDataPanel.MouseLeave += BlockBuildingCanvas.CanvasBlockBuilding_MouseLeave;

            inputDataPanel.LayoutUpdated += InputDataPanel_LayoutUpdated;

            base.OnApplyTemplate();
        }

        private void InputDataPanel_LayoutUpdated(object sender, EventArgs e)
        {
            if (inputDataPanel != null && inputNob != null && inputDataPanel.Children.Count > 0)
            {
                inputNob.Background = (inputDataPanel.Children[0] as BuildingBlock).BlockColor;
                (inputDataPanel.Children[0] as BuildingBlock).ReturnNobVis = Visibility.Collapsed;
            }
            else if (inputDataPanel != null && inputNob != null && inputDataPanel.Children.Count == 0)
            {
                inputNob.Background = CanvasColor;
            }
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

        public virtual SingleBlock GetData()
        {
            SingleBlock block = new SingleBlock();

            if (inputDataPanel.Children.Count != 0)
                block.Inputcontent = (inputDataPanel.Children[0] as BuildingBlock).GetData();

            if (nextCommandPanel.Children.Count != 0)
                block.NextContent = (nextCommandPanel.Children[0] as BuildingBlock).GetData();

            block.InsideContent = (MainContent as ContentBlock).GetData();

            block.Pos = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));

            return block;
        }


    }
}
