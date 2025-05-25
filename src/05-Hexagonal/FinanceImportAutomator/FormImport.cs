using HexagonalFinanceImportAutomator._01_Core.Services;
using HexagonalFinanceImportAutomator._02_Ports;
using HexagonalFinanceImportAutomator._03_Adapters;
using HexagonalFinanceImportAutomator._04_CrossCutting;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace HexagonalFinanceImportAutomator
{
    /// <summary>
    /// Primary Adapter (Driver Adapter) - Windows Forms UI that drives the application
    /// This adapter translates UI events into calls to the primary port
    /// </summary>
    public partial class FormImport : Form
    {
        private readonly ITransactionImportPort _transactionImportPort;
        private readonly INotificationPort _notificationPort;

        public FormImport()
        {
            // Dependency Injection - Hexagonal Architecture Assembly
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FinanceImportAutomator;Integrated Security=True";
            
            // Create notification adapter (shared across all adapters)
            _notificationPort = new NotificationAdapter();

            // Create secondary adapters (driven adapters)
            var fileReaderAdapter = new FileReaderAdapter(_notificationPort);
            var categorizeRepositoryAdapter = new CategorizeRepositoryAdapter(connectionString);
            var transactionRepositoryAdapter = new TransactionRepositoryAdapter(connectionString);

            // Create core service implementing primary port
            _transactionImportPort = new TransactionService(
                fileReaderAdapter,
                categorizeRepositoryAdapter,
                transactionRepositoryAdapter,
                _notificationPort);

            InitializeComponent();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            try
            {
                // Call the primary port - this is the entry point to the hexagon
                _transactionImportPort.ImportTransactions(textBoxFilePath.Text);

                // Display any messages from the notification port
                var messages = _notificationPort.GetMessages();
                if (messages.Any())
                {
                    MessageBox.Show(_notificationPort.GetMessagesAsString(), "Import Result", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFilePath.Text = openFileDialog.FileName;
            }
        }
    }
}
