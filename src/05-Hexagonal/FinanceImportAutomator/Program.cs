using System;
using System.Windows.Forms;

namespace HexagonalFinanceImportAutomator
{
    /// <summary>
    /// Entry point for the Hexagonal Architecture implementation
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormImport());
        }
    }
}
