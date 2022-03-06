using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using Emgu.CV;
using Emgu.CV.Structure;

using grabbableBlocks.CustomControls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Animation;


using System.Text.Json;
using System.Text.Json.Serialization;
using static MacroBot_v0._1.BlockData;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string editingFilePath = "";
        private bool blockEditingEnabled = false;
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

            VarListBox.ItemsSource = BlockBuildingCanvas.VariableList;
        }



        public void InsertImage(object sender, RoutedEventArgs e)
        {
            Hide();
            
            Application.Current.Dispatcher.Invoke(delegate {
                Thread.Sleep(300);//waits for window to hide

                AssetItem screenCrop = new AssetItem(SnipMaker.takeSnip(), "Image01");
                if (screenCrop.asset == null)
                    return;

                changeNameDialog nameDialog = new changeNameDialog();
                screenCrop.name = nameDialog.GetName();

                AssetsList.Items.Add(screenCrop);
                BlockBuildingCanvas.VariableList.Add(screenCrop.ToString());
            });



            Show();
        }
        public void createNew(object sender, RoutedEventArgs e)
        {
            SaveFile(sender, e);
            scriptCode.Document.Blocks.Clear();
            editingFilePath = "";
        }

        public void SaveFileAsBlock(object sender, RoutedEventArgs e)
        {
            BlockData data = new BlockData();

            data.savedImages = new AssetHolder[AssetsList.Items.Count];
            for (int i = 0; i < AssetsList.Items.Count; i++)
            {
                AssetItem item = AssetsList.Items[i] as AssetItem;
                data.savedImages[i] =new AssetHolder(item.asset.Bytes, item.name, item.asset.Size);
            }


            foreach (BuildingBlock b in CustomCanvas.Children)
            {
                SingleBlock block = new SingleBlock();
                block = b.GetData();
                data.Blocks.Add(block);
            }

            string jsonString = JsonSerializer.Serialize(data);//TODO: wide images wont serialize
                      

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "MacroBlock"; // Default file name
            dlg.DefaultExt = ".mcrb"; // Default file extension
            dlg.Filter = "Macro script file (.mcrb)|*.mcrb"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                string filename = dlg.FileName;
                File.WriteAllText(filename, jsonString);
                EditingFilePath = filename;
            }
        }

        public void SaveFileAsText(object sender, RoutedEventArgs e)
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
            openFile.Filter = "Macro script file |*.mcr;*.mcrb|All files |*.*"; // Filter files by extension

            Nullable<bool> results = openFile.ShowDialog();

            if (results != true)
                return;



            string path = openFile.FileName;
            EditingFilePath = path;

            if (Path.GetExtension(path) == ".mcrb")
                LoaderMCRB(path);
            else
                LoaderMCR(path);                     
        }

        private void LoaderMCR(string pwd)
        {
            try
            {
                string scriptText = File.ReadAllText(pwd);

                scriptCode.Document.Blocks.Clear();
                scriptCode.Document.Blocks.Add(new Paragraph(new Run(scriptText)));
            }
            catch (Exception ex)// This is here incase there is error with the file name
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoaderMCRB(string pwd)
        {
            BlockData data = new BlockData();
            try
            {
                string jsonText = File.ReadAllText(pwd);

                data = JsonSerializer.Deserialize(jsonText, typeof(BlockData)) as BlockData;
            }
            catch (Exception ex)// This is here incase there is error with the file name
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AssetsList.Items.Clear();
            foreach (AssetHolder item in data.savedImages)
            {
                Image<Bgr, byte> img = new Image<Bgr, byte>(item.AssetSize);
                img.Bytes = item.AssetBytes;
                AssetItem newAsset = new AssetItem(img, item.AssetName);

                AssetsList.Items.Add(newAsset);
                BlockBuildingCanvas.VariableList.Add(newAsset.ToString());
            }
        }

        private void StartProgram(object sender, RoutedEventArgs e)
        {
            SaveFile(sender, e);

            List<string> paths = saveAssetsToTemp();
            string PathPara = "";
            if (paths.Count > 0)
                paths.ForEach(x => PathPara += x + " ");


            string output;
            using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = @"External\MacroBotV0.1Language.exe";
                pProcess.StartInfo.Arguments = $"-f \"{EditingFilePath}\""; //argument

                if(PathPara != "")
                    pProcess.StartInfo.Arguments += $"-a \"{PathPara}\""; //assets

                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                pProcess.StartInfo.CreateNoWindow = false; //not diplay a windows
                pProcess.Start();
                output = pProcess.StandardOutput.ReadToEnd(); //The output result
                pProcess.WaitForExit();
            }
            MessageBox.Show(output);
        }

        private List<string> saveAssetsToTemp()
        {
            List<string> allAssetsPath = new List<string>();
            string tempFileLoc = Path.GetTempPath();
            foreach(AssetItem item in AssetsList.Items)
            {
                string path = tempFileLoc + item.name + ".jpg";
                item.asset.Save(path);
                allAssetsPath.Add(path);
            }

            return allAssetsPath;
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (EditingFilePath == "")
            {
                if (blockEditingEnabled)
                    SaveFileAsBlock(sender, e);
                else
                    SaveFileAsText(sender, e);
                return;
            }

            string scriptText = new TextRange(scriptCode.Document.ContentStart, scriptCode.Document.ContentEnd).Text;// Gets the whole code in string

            string filename = EditingFilePath;
            File.WriteAllText(filename, scriptText);
            EditingFilePath = filename;

        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Button).RenderTransform = new ScaleTransform(1.05, 1.05);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Button).RenderTransform = new ScaleTransform(1, 1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Debug.WriteLine("Clicked: " + btn.Content);

            if (btn.Content.ToString() == "Insert variable block")
                CreateBlock(new ContentBlockReturnValue(), Color.FromRgb(0, 255, 209), Color.FromRgb(0, 141, 135));
            else if (btn.Content.ToString() == "Variable select block")
                CreateBlock(new ContentBlockReturnVar(), Colors.Lime, Color.FromRgb(32, 141, 0));
            else if (btn.Content.ToString() == "Operation block")
                CreateBlock(new ContentBlockMathOperation(), Color.FromRgb(209, 0, 255), Color.FromRgb(90, 0, 141));
            else if (btn.Content.ToString() == "Set variable block")
                CreateBlock(new ContentBlockSetVar(), Colors.Red, Colors.DarkRed);
            else if (btn.Content.ToString() == "Not block")
                CreateBlock(new ContentBlockNot(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Repeat block")
                CreateBlock(new ContentBlockWhile(), Colors.Yellow, Color.FromRgb(156, 162, 25));
            else if (btn.Content.ToString() == "Condition block")
                CreateBlock(new ContentBlockIfPack() { ElseVisible = Visibility.Collapsed }, Color.FromRgb(151, 255, 0), Color.FromRgb(112, 162, 25));
            else if (btn.Content.ToString() == "Start block")
                CustomCanvas.Children.Add(new BuildingBlockStart());
            else if (btn.Content.ToString() == "Start Function")
                CreateBlock(new ContentBlockStart(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Find Function")
                CreateBlock(new ContentBlockFind(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Press Function")
                CreateBlock(new ContentBlockPress(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Click Function")
                CreateBlock(new ContentBlockClick(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Print Function")
                CreateBlock(new ContentBlockPrint(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "To String Function")
                CreateBlock(new ContentBlockToString(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Delay Function")
                CreateBlock(new ContentBlockDelay(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Mouse move Function")
                CreateBlock(new ContentBlockMouseMove(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Sin Function")
                CreateBlock(new ContentBlockSin(), Color.FromRgb(255, 80, 255), Color.FromRgb(170, 34, 170));
            else if (btn.Content.ToString() == "Generate code")
            {
                scriptCode.Document.Blocks.Clear();
                foreach (UIElement startCode in CustomCanvas.Children)
                {
                    if (startCode.GetType() == typeof(BuildingBlockStart))
                    {
                        string code = (startCode as BuildingBlockStart).GetCode();
                        Debug.WriteLine(code);
                        scriptCode.Document.Blocks.Add(new Paragraph(new Run(code)));

                    }
                }

            }
            else if (btn.Content.ToString() == "Add variable")
            {
                if (VarNameTextBox.Text == "" || VarNameTextBox.Text.Contains(' ') || VarListBox.Items.Contains(VarNameTextBox.Text)) return;
                BlockBuildingCanvas.VariableList.Add(VarNameTextBox.Text);
                VarNameTextBox.Text = "";
            }
            else if (btn.Content.ToString() == "Delete variable")
            {
                BlockBuildingCanvas.VariableList.Remove(VarListBox.SelectedItem.ToString());
            }

        }

        private void CreateBlock(ContentBlock content, Color backColor, Color BorderColor)
        {
            BuildingBlock block = new BuildingBlock()
            {
                MainContent = content,
                BlockColor = new SolidColorBrush(backColor),
                BorderColor = new SolidColorBrush(BorderColor),
                Foreground = new SolidColorBrush(Colors.White)
            };
            CustomCanvas.Children.Add(block);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (BlockCanvas.Visibility == Visibility.Collapsed)
            {
                blockEditingEnabled = true;
                BlockCanvas.Visibility = Visibility.Visible;
            }
            else
                BlockCanvas.Visibility = Visibility.Collapsed;
        }
    }
}
