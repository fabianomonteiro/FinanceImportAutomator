using FinanceImportAutomator._01_Application;
using FinanceImportAutomator._03_Infra;
using FinanceImportAutomator._04_CrossCutting;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FinanceImportAutomator
{
    public partial class FormImport : Form
    {
        private readonly IImportUseCase _importUseCase;
        private readonly INotification _notification;

        public FormImport()
        {
            //Injeção de dependência
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FinanceImportAutomator;Integrated Security=True";
            var sqlConnection = new SqlConnection(connectionString);
            
            _notification = new Notification();

            _importUseCase = new ImportUseCase(
                new TransactionImportReader(_notification)
                , new GetCategoryByDescriptionQuery(sqlConnection)
                , new SaveTransactionsOneToOneCommand(new InsertTransactionsCommand(sqlConnection))
                , _notification
                );

            InitializeComponent();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            _importUseCase.Execute(textBoxFilePath.Text);

            if (_notification.Messages.Count > 0)
                MessageBox.Show(_notification.GetMessagesSepartedByLine());
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBoxFilePath.Text = openFileDialog.FileName;
        }
    }
}
