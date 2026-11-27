using System;
using System.Windows.Forms;
using AntiqueShopLibrary; 

namespace WinFormsApp2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            
            Application.Run(new Authorization());
        }
    }
}