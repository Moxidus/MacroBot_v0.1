using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string[] Args)
        {
            InitializeComponent();

            //MessageBox.Show("DEBUG LMAO:" + thing);

            if(Args.Length != 0)
                OpenFile(Args[0].ToString());
        }

        public void OpenFile(string path)
        {
            try
            {
                string scriptText = File.ReadAllText(path);

                scriptCode.Document.Blocks.Clear();
                scriptCode.Document.Blocks.Add(new Paragraph(new Run(scriptText)));
            }
            catch (Exception ex)// This is here incase there is error with the file name
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void SaveFile(object sender, RoutedEventArgs e)
        {
            string scriptText = new TextRange(scriptCode.Document.ContentStart, scriptCode.Document.ContentEnd).Text;// Gets the whole code in string


            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "MacroScript"; // Default file name
            dlg.DefaultExt = ".mcr"; // Default file extension
            dlg.Filter = "Macro script file (.mcr)|*.mcr"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                string filename = dlg.FileName;
                File.WriteAllText(filename, scriptText);
            }

        }
    }
}
