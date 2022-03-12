using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MacroBot_v0._1.Dialogs
{
    /// <summary>
    /// Interaction logic for CodePlayer.xaml
    /// </summary>
    public partial class CodePlayer : Window
    {
        public static CodePlayer Player;

        string outputData = "";

        System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
        DispatcherTimer remainTimer = new DispatcherTimer();


        public CodePlayer(List<string> paths, string codePath)
        {
            InitializeComponent();

            string PathPara = "";
            if (paths.Count > 0)
                paths.ForEach(x => PathPara += x + " ");



            pProcess.StartInfo.FileName = @"External\MacroBotV0.1Language.exe";
            pProcess.StartInfo.Arguments = $"-f \"{codePath}\""; //argument

            if (PathPara != "")
                pProcess.StartInfo.Arguments += $" -a \"{PathPara}\""; //assets

            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows

            pProcess.OutputDataReceived += (sender, e) => {
                outputData += e.Data + "\n";
            };


            Application.Current.Dispatcher.Invoke(delegate {
                pProcess.Start();
                pProcess.BeginOutputReadLine();


                remainTimer.Tick += (sender, e) => { displayData(); };
                remainTimer.Interval = TimeSpan.FromMilliseconds(100);
                remainTimer.Start();
                
            });

        }



        public static void StartCode(List<string> paths, string codePath)
        {
            if (Player != null)
                Player.Close();
            Player = new CodePlayer(paths, codePath);
            Player.Show();
           
        }
        public void displayData()
        {
            if (pProcess.HasExited)
            {
                Thread.Sleep(300);//waits for a rest of the data to arive
                ConsoleOutput.Text = outputData;
                ConsoleOutput.ScrollToEnd();
                remainTimer.Stop();
                return;
            }

            ConsoleOutput.Text = outputData;
            ConsoleOutput.ScrollToEnd();
        }
        protected override void OnClosed(EventArgs e)
        {
            pProcess.Kill();
            pProcess.Dispose();
            base.OnClosed(e);
        }

        private void ExitProcess(object sender, RoutedEventArgs e)
        {
            pProcess.Kill();
            Close();
        }

        private void StopProcess(object sender, RoutedEventArgs e)
        {
            pProcess.Kill();
        }

        private void HideProcess(object sender, RoutedEventArgs e)
        {
            
            if ((sender as Button).Content.ToString() == "Hide")
            {
                Height = 25;
                (sender as Button).Content = "Show";
                return;
            }

            Height = 200;
            (sender as Button).Content = "Hide";
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                return;
            if (Keyboard.IsKeyDown(Key.P))//Stop Process
                StopProcess(null, null);
        }
    }
}
