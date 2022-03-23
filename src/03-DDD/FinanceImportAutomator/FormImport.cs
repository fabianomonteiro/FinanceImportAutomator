using DDDFinanceImportAutomator._01_Application;
using DDDFinanceImportAutomator._03_Infra;
using DDDFinanceImportAutomator._04_CrossCutting;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DDDFinanceImportAutomator
{
    public partial class FormImport : Form
    {
        private readonly ITransactionService _transactionService;
        private readonly INotification _notification;

        public FormImport()
        {
            //Injeção de dependências
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DDDFinanceImportAutomator;Integrated Security=True";
            var sqlConnection = new SqlConnection(connectionString);

            _notification = new Notification();

            _transactionService = new TransactionService(
                new TransactionInfraService(_notification)
                , new CategorizeRepository(sqlConnection)
                , new TransactionRepository(sqlConnection)
                , _notification);

            InitializeComponent();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            _transactionService.ImportTransactions(textBoxFilePath.Text);

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
