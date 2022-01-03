using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace MacroBot_v0._1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern void SHChangeNotify(
        int wEventId,
        uint uFlags,
        IntPtr dwItem1,
        IntPtr dwItem2);


        private void Application_Startup(object sender, StartupEventArgs e)
        {
           //if (!IsAssociated())
            //    Associate();
            MainWindow mainWindow = new MainWindow(e.Args);
            mainWindow.Show();
        }

        private static bool IsAssociated()
        {
            return Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.mcr", false) != null;
        }

        private static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.mcr");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Application\\MacroBot_v0.1.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.mcr");


            FileReg.CreateSubKey("DefaultIcon").SetValue("", @"C:\Users\domku\Downloads\path31.ico");
            FileReg.CreateSubKey("PerceivedType").SetValue("", "Text");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + " %1");
            AppReg.CreateSubKey("shell\\edit\\command").SetValue("", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + " %1");
            AppReg.CreateSubKey("DefaultIcon").SetValue("", @"C:\Users\domku\Downloads\path31.ico");

            AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\MacroBot_v0.1.exe");
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

    }
}
