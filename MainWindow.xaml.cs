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

using Emgu.CV;
using Emgu.CV.Structure;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string editingFilePath = "";
        public string EditingFilePath
        {
            get
            {
                return editingFilePath;
            }
            set
            {
                editingFilePath = value;

                if (editingFilePath == "")
                    this.Title = "Macro Bot";
                else
                    this.Title = "Macro Bot" + " - " + editingFilePath;
            }
        }

        public MainWindow(string[] Args)
        {
            InitializeComponent();
            scriptCode.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scriptCode.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scriptCode.Document.PageWidth = 1000;
        }



        public void InsertImage(object sender, RoutedEventArgs e)
        {
            AssetsList.Items.Add(new AssetItem(SnipMaker.takeSnip(), "Image01"));
        }

        public void createNew(object sender, RoutedEventArgs e)
        {
            SaveFile(sender, e);
            scriptCode.Document.Blocks.Clear();
            editingFilePath = "";
        }

        public void SaveFileAs(object sender, RoutedEventArgs e)
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
                EditingFilePath = filename;
            }

        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.DefaultExt = ".mcr"; // Default file extension
            openFile.Filter = "Macro script file (.mcr)|*.mcr"; // Filter files by extension

            Nullable<bool> results = openFile.ShowDialog();

            if (results != true)
                return;

            string path = openFile.FileName;
            EditingFilePath = path;
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

        private void StartProgram(object sender, RoutedEventArgs e)
        {
            SaveFile(sender, e);

            string output;
            using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = @"External\MacroBotV0.1Language.exe";
                pProcess.StartInfo.Arguments = $"-f \"{EditingFilePath}\""; //argument
                pProcess.StartInfo.UseShellExecute = false;
                //pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                pProcess.StartInfo.CreateNoWindow = false; //not diplay a windows
                pProcess.Start();
                //output = pProcess.StandardOutput.ReadToEnd(); //The output result
                pProcess.WaitForExit();
            }

            //MessageBox.Show(output);
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (EditingFilePath == "")
            {
                SaveFileAs(sender, e);
                return;
            }

            string scriptText = new TextRange(scriptCode.Document.ContentStart, scriptCode.Document.ContentEnd).Text;// Gets the whole code in string

                string filename = EditingFilePath;
                File.WriteAllText(filename, scriptText);
                EditingFilePath = filename;

        }
    }
}
