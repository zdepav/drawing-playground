#nullable enable
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Jint;

namespace DrawingPlayground.Forms {

    internal static class Program {

        [STAThread]
        private static void Main() {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

    }

}
