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
    /// Interaction logic for changeNameDialog.xaml
    /// </summary>
    public partial class changeNameDialog : Window
    {
        public changeNameDialog()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Opens dialog that user can enter variable name to
        /// </summary>
        /// <returns>Returns variable name in form of string</returns>
        public static string GetName(Window owner)
        {
            changeNameDialog nameDialog = new changeNameDialog();
            nameDialog.Owner = owner;
            nameDialog.VarNameTextBox.Focus();
            if(nameDialog.ShowDialog() == true)
            {
                return nameDialog.VarNameTextBox.Text;
            }
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void VarNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            DialogResult = true;
            Close();
        }
    }
}
