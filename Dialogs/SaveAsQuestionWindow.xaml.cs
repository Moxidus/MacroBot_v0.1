using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for SaveAsQuestionWindow.xaml
    /// </summary>
    public partial class SaveAsQuestionWindow : Window
    {
        public SaveAsQuestionWindow()
        {
            InitializeComponent();
        }

        private SaveType? type;
        public enum SaveType {
            SaveBlock, SaveScript
        }

        public SaveType? SaveAsQuestionDialog(Window par)
        {
            Owner = par;
            ShowDialog();
            return type;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "Save as Block")
                type = SaveType.SaveBlock;
            else
                type = SaveType.SaveScript;
            Close();
        }
    }
}
