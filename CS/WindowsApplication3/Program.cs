// Developer Express Code Central Example:
// How to drag multiple columns in a grid at once
// 
// This example illustrates how to move or hide some columns in a grid at once. For
// this you should select a desirable region where particular column headers are
// located by holding the left mouse button. After this, columns that reside within
// this area will be selected and you can drag them.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E3385

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.XtraEditors;

namespace DXSample {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            WindowsFormsSettings.ForceDirectXPaint();
            SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}