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
using static MacroBot_v0._1.SaveAsQuestionWindow;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using MacroBot_v0._1.Dialogs;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string editingFilePath = "";
        private SaveType? SaveAsType;
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
            scriptCode.Document.Blocks.Clear();

            VarListBox.ItemsSource = BlockBuildingCanvas.VariableList;
        }


        /// <summary>
        /// Creates screenhot creted by croping tool and saves it to variable list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void InsertImage(object sender, RoutedEventArgs e)
        {
            Hide();//Hides main windows

            Application.Current.Dispatcher.Invoke(delegate {
                Thread.Sleep(300);//waits for window to hide

                AssetItem screenCrop = new AssetItem(SnipMaker.takeSnip(), "Image01");
                if (screenCrop.asset == null)
                    return;

                Show();

                screenCrop.name = changeNameDialog.GetName(this);


                AssetsList.Items.Add(screenCrop);
                BlockBuildingCanvas.VariableList.Add(screenCrop.ToString());
            });



        }
        public void createNew(object sender, RoutedEventArgs e)
        {
            WantToSaveDialog saveIt = new WantToSaveDialog();
            if (saveIt.SaveItDialog(this))
                SaveFile(sender, e);
            deleteImages();
            scriptCode.Document.Blocks.Clear();
            EditingFilePath = "";
        }

        private void deleteImages()
        {
            foreach (AssetItem item in AssetsList.Items)
            {
                item.asset.Dispose();
            }
            AssetsList.Items.Clear();
        }

        private void SaveAsDialog(object sender, RoutedEventArgs e)
        {
            SaveAsQuestionWindow DialogSaveAs = new SaveAsQuestionWindow();
            SaveAsType = DialogSaveAs.SaveAsQuestionDialog(this);
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            string jsonString;

            if (SaveAsType == SaveType.SaveBlock)
            {
                dlg.FileName = "MacroBlock"; // Default file name
                dlg.DefaultExt = ".mcrb"; // Default file extension
                dlg.Filter = "Macro script file (.mcrb)|*.mcrb"; // Filter files by extension

                jsonString = SaveFileAsBlock();
            }
            else if (SaveAsType == SaveType.SaveScript)
            {
                dlg.FileName = "MacroScript"; // Default file name
                dlg.DefaultExt = ".mcr"; // Default file extension
                dlg.Filter = "Macro script file (.mcr)|*.mcr"; // Filter files by extension

                jsonString = SaveFileAsText();
            }
            else
                return;

            // Shows save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                string filename = dlg.FileName;
                File.WriteAllText(filename, jsonString);
                EditingFilePath = filename;
            }
        }
        public string SaveFileAsBlock()
        {
            BlockData data = new BlockData();

            data.savedImages = new AssetHolder[AssetsList.Items.Count];
            for (int i = 0; i < AssetsList.Items.Count; i++)
            {
                AssetItem item = AssetsList.Items[i] as AssetItem;
                data.savedImages[i] = new AssetHolder(ImageConvertor.ImageToBase64String(item.asset), item.name, item.asset.Size);
            }

            data.variables = BlockBuildingCanvas.VariableList.ToArray<string>();

            foreach (BuildingBlock b in CustomCanvas.Children)
            {
                SingleBlock block = new SingleBlock();
                block = b.GetData();
                data.Blocks.Add(block);
            }

            return JsonSerializer.Serialize(data);
        }
        public string SaveFileAsText()
        {
            ScriptData data = new ScriptData();

            data.savedImages = new AssetHolder[AssetsList.Items.Count];
            for (int i = 0; i < AssetsList.Items.Count; i++)
            {
                AssetItem item = AssetsList.Items[i] as AssetItem;
                data.savedImages[i] = new AssetHolder(ImageConvertor.ImageToBase64String(item.asset), item.name, item.asset.Size);
            }

            data.script = new TextRange(scriptCode.Document.ContentStart, scriptCode.Document.ContentEnd).Text;// Gets the whole code in string

            return JsonSerializer.Serialize(data);

        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            createNew(sender, e);

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
            SaveAsType = SaveType.SaveScript;
            ScriptData data = new ScriptData();
            try
            {
                string jsonText = File.ReadAllText(pwd);

                data = JsonSerializer.Deserialize(jsonText, typeof(ScriptData)) as ScriptData;
            }
            catch (Exception ex)// This is here incase there is error with the file name
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            scriptCode.Document.Blocks.Clear();
            scriptCode.Document.Blocks.Add(new Paragraph(new Run(data.script)));


            AssetLoader(data.savedImages);
        }

        private void LoaderMCRB(string pwd)
        {
            SaveAsType = SaveType.SaveBlock;
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
            //TODO: load blocks in

            CustomCanvas.Children.Clear();

            foreach(SingleBlock block in data.Blocks)
            {
                callBlock(block, "");
                CustomCanvas.Children.Add(BuildingBlock.CreateBlockOrStart(block));
            }

            //Load variables
            BlockBuildingCanvas.VariableList.Clear();
            for(int i = 0; i < data.variables.Length; i++)
            {
                BlockBuildingCanvas.VariableList.Add(data.variables[i]);
            }


            AssetLoader(data.savedImages);
        }
        //Just debug class delete this
        private void callBlock(SingleBlock block, string offset)
        {
            
            if (block == null)
                return;
            Debug.Write(offset);
            Type tp = Type.GetType(block.InsideContent.ContentType, new bool());
            Debug.WriteLine(tp + $"x: {block.Pos.X} y: {block.Pos.Y}");//TODO: create special constructor for every type that is going to create it self with given parameters
            offset += "\t";
            callBlock(block.NextContent, offset);

        }

        private void AssetLoader(AssetHolder[] assets)
        {
            foreach (AssetHolder item in assets)
            {
                Image<Bgra, byte> img = new Image<Bgra, byte>(item.AssetSize);

                img.Bytes = ImageConvertor.ByteFromBase64String(item.AssetBase64);
                AssetItem newAsset = new AssetItem(img, item.AssetName);

                AssetsList.Items.Add(newAsset);
            }
        }

        private void StartProgram(object sender, RoutedEventArgs e)
        {
            SaveFile(sender, e);

            List<string> paths = saveAssetsToTemp();
            string codePath = saveCodeToTemp();

            string PathPara = "";
            if (paths.Count > 0)
                paths.ForEach(x => PathPara += x + " ");


            string output;
            using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = @"External\MacroBotV0.1Language.exe";
                pProcess.StartInfo.Arguments = $"-f \"{codePath}\""; //argument

                if (PathPara != "")
                    pProcess.StartInfo.Arguments += $" -a \"{PathPara}\""; //assets

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
            foreach (AssetItem item in AssetsList.Items)
            {
                string path = tempFileLoc + item.name + ".jpg";
                item.asset.Save(path);
                allAssetsPath.Add(path);
            }

            return allAssetsPath;
        }
        private string saveCodeToTemp()
        {
            string tempFileLoc = Path.GetTempPath();
            tempFileLoc += "MacroBotCode";

            string code = new TextRange(scriptCode.Document.ContentStart, scriptCode.Document.ContentEnd).Text;// Gets the whole code in string

            File.WriteAllText(tempFileLoc, code);
            return tempFileLoc;
        }

        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (SaveAsType == null || EditingFilePath == "")
            {
                SaveAsDialog(sender, e);
            }
            else if(SaveAsType == SaveType.SaveBlock)
                File.WriteAllText(EditingFilePath, SaveFileAsBlock());
            else
                File.WriteAllText(EditingFilePath, SaveFileAsText());

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
                CreateBlock(new ContentBlockReturnValue());
            else if (btn.Content.ToString() == "Variable select block")
                CreateBlock(new ContentBlockReturnVar());
            else if (btn.Content.ToString() == "Operation block")
                CreateBlock(new ContentBlockMathOperation());
            else if (btn.Content.ToString() == "Set variable block")
                CreateBlock(new ContentBlockSetVar());
            else if (btn.Content.ToString() == "Not block")
                CreateBlock(new ContentBlockNot());
            else if (btn.Content.ToString() == "Repeat block")
                CreateBlock(new ContentBlockWhile());
            else if (btn.Content.ToString() == "Condition block")
                CreateBlock(new ContentBlockIfPack() { ElseVisible = Visibility.Collapsed });
            else if (btn.Content.ToString() == "Start block")
                CustomCanvas.Children.Add(new BuildingBlockStart());
            else if (btn.Content.ToString() == "Start Function")
                CreateBlock(new ContentBlockStart());
            else if (btn.Content.ToString() == "Find Function")
                CreateBlock(new ContentBlockFind());
            else if (btn.Content.ToString() == "Press Function")
                CreateBlock(new ContentBlockPress());
            else if (btn.Content.ToString() == "Click Function")
                CreateBlock(new ContentBlockClick());
            else if (btn.Content.ToString() == "Print Function")
                CreateBlock(new ContentBlockPrint());
            else if (btn.Content.ToString() == "To String Function")
                CreateBlock(new ContentBlockToString());
            else if (btn.Content.ToString() == "Delay Function")
                CreateBlock(new ContentBlockDelay());
            else if (btn.Content.ToString() == "Mouse move Function")
                CreateBlock(new ContentBlockMouseMove());
            else if (btn.Content.ToString() == "Sin Function")
                CreateBlock(new ContentBlockSin());
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

        private void CreateBlock(ContentBlock content)
        {
            BuildingBlock block = new BuildingBlock()
            {
                MainContent = content,
            };
            Canvas.SetLeft(block, 50);
            Canvas.SetTop(block, 50);
            CustomCanvas.Children.Add(block);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (BlockCanvas.Visibility == Visibility.Collapsed)
                BlockCanvas.Visibility = Visibility.Visible;
            else
                BlockCanvas.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(VarListBox.SelectedIndex != -1)
            {
                foreach (AssetItem item in AssetsList.Items)
                {
                    Debug.WriteLine(VarListBox.SelectedItem.ToString() + " == " + item.name);
                    if(VarListBox.SelectedItem.ToString() == item.name)
                    {
                        ImageViewer.ShowImage(item.asset);
                        break;
                    }
                }
            }
        }
    }
}
