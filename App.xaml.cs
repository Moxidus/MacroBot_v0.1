using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(e.Args);
            mainWindow.Show();
        }

        private static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.mcr");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Application\\MacroBot_v0.1.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\CurrentVersion\\Explorer\\FileExts\\.mcr");


        }


    }
}
