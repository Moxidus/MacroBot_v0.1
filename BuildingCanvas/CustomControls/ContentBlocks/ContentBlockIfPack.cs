﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace grabbableBlocks.CustomControls
{
    class ContentBlockIfPack : ContentBlock, ICode, IPrevCommand, INextCommand
    {

        static ContentBlockIfPack()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentBlockIfPack), new FrameworkPropertyMetadata(typeof(ContentBlockIfPack)));
        }



        public int IfElseNumber
        {
            get { return (int)GetValue(IfElseNumberProperty); }
            set {
                SetValue(IfElseNumberProperty, value);
            }
        }

        public Visibility ElseVisible
        {
            get { return (Visibility)GetValue(ElseVisibleProperty); }
            set
            {
                SetValue(ElseVisibleProperty, value);
            }
        }


        public static DependencyProperty IfElseNumberProperty = DependencyProperty.Register("IfElseNumber", typeof(int), typeof(ContentBlockIfPack));
        public static DependencyProperty ElseVisibleProperty = DependencyProperty.Register("ElseVisible", typeof(Visibility), typeof(ContentBlockIfPack));


        void CreteIfElse()
        {

            StackPanel groupStack = new StackPanel();
            groupStack.Orientation = Orientation.Vertical;
            groupStack.Margin = new Thickness(0);

            //add lable Else If
            StackPanel lablePanel = new StackPanel();
            lablePanel.Orientation = Orientation.Horizontal;
            lablePanel.Children.Add(new Label() { Content = "Else if" });
            StackPanel conditionHolder = new StackPanel()
            {
                MinHeight = 25,
                MinWidth = 60,
                Margin = new Thickness(0, 0, 0, 10),
                Orientation = Orientation.Horizontal,
                Background = CanvasColor,
                AllowDrop = true,
            };
            lablePanel.Children.Add(conditionHolder);

            StackPanel DoPanel = new StackPanel();
            DoPanel.Orientation = Orientation.Horizontal;
            DoPanel.Children.Add(new Label() { Content = "Do" });
            StackPanel contentHolder = new StackPanel()
            {
                MinHeight = 25,
                MinWidth = 60,
                Margin = new Thickness(0, 0, 0, 10),
                Orientation = Orientation.Horizontal,
                Background = CanvasColor,
                AllowDrop = true,
            };
            DoPanel.Children.Add(contentHolder);

            groupStack.Children.Add(lablePanel);
            groupStack.Children.Add(DoPanel);


            ifDoList.Add(groupStack);
        }


            List<StackPanel> ifDoList = new List<StackPanel>();
        public StackPanel DOValuePanel { get; private set; }
        public StackPanel ElseDoDataPanel { get; private set; }
        public StackPanel MainConPanel { get; private set; }
        public StackPanel IfsStack { get; private set; }
        public Button AddIEButton { get; private set; }
        public Button RemoveIEButton { get; private set; }
        public CheckBox ElseEnabled { get; private set; }

        public override void OnApplyTemplate()
        {

            DOValuePanel = (StackPanel)Template.FindName("PART_DoDataPanel", this);
            DOValuePanel.Drop += BlockBuildingCanvas.CanvasCommandDropEvent;
            MainConPanel = (StackPanel)Template.FindName("PART_MainConPanel", this);
            MainConPanel.Drop += BlockBuildingCanvas.CanvasInputDropEvent;
            ElseDoDataPanel = (StackPanel)Template.FindName("PART_ElseDoDataPanel", this);
            ElseDoDataPanel.Drop += BlockBuildingCanvas.CanvasCommandDropEvent;


            IfsStack = (StackPanel)Template.FindName("PART_IfsStack", this);

            AddIEButton = (Button)Template.FindName("PART_AddIEButton", this);
            AddIEButton.Click += AddIEButton_Click;
            RemoveIEButton = (Button)Template.FindName("PART_RemoveIEButton", this);
            RemoveIEButton.Click += RemoveIEButton_Click;
            ElseEnabled = (CheckBox)Template.FindName("PART_ElseEnabled", this);
            ElseEnabled.Click += ElseEnabled_Click; ;

            base.OnApplyTemplate();
        }

        private void ElseEnabled_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ElseEnabled.IsChecked)
                ElseVisible = Visibility.Visible;
            else
                ElseVisible = Visibility.Collapsed;
        }

        private void RemoveIEButton_Click(object sender, RoutedEventArgs e)
        {
            if (ifDoList.Count == 0)
                return;
            IfsStack.Children.Remove(ifDoList[ifDoList.Count - 1]);

            ifDoList.RemoveAt(ifDoList.Count - 1);


        }

        private void AddIEButton_Click(object sender, RoutedEventArgs e)
        {
            CreteIfElse();
            if (IfsStack != null)
            {

                for (int i = 0; i < ifDoList.Count; i++)
                {

                    if (IfsStack.Children.Contains(ifDoList[i]))
                        continue;

                    (ifDoList[i].Children[1] as StackPanel).Children[1].Drop += BlockBuildingCanvas.CanvasCommandDropEvent;
                    (ifDoList[i].Children[0] as StackPanel).Children[1].Drop += BlockBuildingCanvas.CanvasInputDropEvent;
                    IfsStack.Children.Insert(IfsStack.Children.Count - 1, ifDoList[i]);
                }
            }
        }

        public string GetCode()
        {
            string result = "IF ";

            if(MainConPanel.Children.Count > 0)
            {
                result += (MainConPanel.Children[0] as BuildingBlock).GetCode();
            }
            else
            {
                result += "FALSE";
            }

            result += " THEN\n";
            
            if(DOValuePanel.Children.Count > 0)
            {
                result += (DOValuePanel.Children[0] as BuildingBlock).GetCode() + "\n";
            }
            else
            {
                result += "\"Empty\"\n";
            }

            foreach (StackPanel pan in ifDoList)
            {
                result += "ELIF ";

                StackPanel conIfElsePanel = (pan.Children[0] as StackPanel).Children[1] as StackPanel;
                StackPanel doIfElsePanel = (pan.Children[1] as StackPanel).Children[1] as StackPanel;


                if (conIfElsePanel.Children.Count > 0)
                {
                    result += (conIfElsePanel.Children[0] as BuildingBlock).GetCode();
                }
                else
                {
                    result += "FALSE";
                }

                result += " THEN\n";

                if (doIfElsePanel.Children.Count > 0)
                {
                    result += (doIfElsePanel.Children[0] as BuildingBlock).GetCode() + "\n";
                }
                else
                {
                    result += "\"Empty\"\n";
                }


            }


            if(ElseVisible == Visibility.Visible)
            {
                result += "ELSE\n";
                if (ElseDoDataPanel.Children.Count > 0)
                {
                    result += (ElseDoDataPanel.Children[0] as BuildingBlock).GetCode() + "\n";
                }
                else
                {
                    result += "\"Empty\"\n";
                }
            }



            result += "END";

            return result;

        }
    }
}