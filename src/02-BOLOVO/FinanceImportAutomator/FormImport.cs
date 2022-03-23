using BOLOVOFinanceImportAutomator.BO;
using System;
using System.Windows.Forms;

namespace BOLOVOFinanceImportAutomator
{
    public partial class FormImport : System.Windows.Forms.Form
    {
        public FormImport()
        {
            InitializeComponent();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            // Valida se o arquivo foi selecionado
            if (string.IsNullOrWhiteSpace(textBoxFilePath.Text))
            {
                MessageBox.Show("Import file is required.");
                return;
            }

            BusinessLogic.ImportFile(textBoxFilePath.Text);
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBoxFilePath.Text = openFileDialog.FileName;
        }
    }
}
