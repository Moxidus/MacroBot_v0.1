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

namespace MacroBot_v0._1.Dialogs
{
    /// <summary>
    /// Interaction logic for WantToSaveDialog.xaml
    /// </summary>
    public partial class WantToSaveDialog : Window
    {
        public WantToSaveDialog()
        {
            InitializeComponent();
        }

        private bool anws;
        public bool SaveItDialog(Window par)
        {
            Owner = par;
            ShowDialog();
            return anws;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "Save it")
                anws = true;
            else
                anws = false;
            Close();
        }
    }
}
