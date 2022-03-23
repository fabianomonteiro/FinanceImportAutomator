using FinanceImportAutomator._01_Application;
using FinanceImportAutomator._03_Infra;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FinanceImportAutomator
{
    public partial class FormImport : System.Windows.Forms.Form
    {
        private readonly IImportUseCase _importUseCase;

        public FormImport()
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FinanceImportAutomator;Integrated Security=True";
            var sqlConnection = new SqlConnection(connectionString);

            _importUseCase = new ImportUseCase(
                new GetCategoryQuery(sqlConnection)
                , new InsertTransactionCommand(sqlConnection)
                );

            InitializeComponent();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            _importUseCase.Execute(textBoxFilePath.Text);

            // String de conexão com o banco de dados
            

            // Valida se o arquivo foi selecionado
            if (string.IsNullOrWhiteSpace(textBoxFilePath.Text))
            {
                MessageBox.Show("Import file is required.");
                return;
            }

            var connection = new SqlConnection(connectionString);
            StreamReader streamReader = null;

            try
            {
                int linesImported = 0;
                int lineIndex = -1;
                string line;

                // Lê o arquivo
                streamReader = File.OpenText(textBoxFilePath.Text);

                // Percorre o arquivo linha a linha
                while ((line = streamReader.ReadLine()) != null)
                {
                    lineIndex++;

                    // Valida se o conteúdo do arquivo está em formato correto
                    if (!line.Contains(';'))
                    {
                        MessageBox.Show("Incorrect format file.");
                        return;
                    }

                    // Pular cabeçalho
                    if (lineIndex == 0)
                        continue;

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    // Separa o conteúdo da linha por ponto e virgula
                    var splitedLine = line.Split(';');
                    var command = connection.CreateCommand();
                    var date = splitedLine[0];
                    var description = splitedLine[1];
                    var amount = splitedLine[2];
                    var transactionType = splitedLine[3];
                    var accountName = splitedLine[4];

                    command.CommandText = $@"
                            SELECT Category FROM Categorize WHERE @Description LIKE '%' + Description + '%'";

                    command.Parameters.Add(new SqlParameter("@Description", description));

                    var category = command.ExecuteScalar();

                    if (category == null)
                        category = DBNull.Value;

                    command.CommandText = @"
                            INSERT INTO [dbo].[Transaction]
                                   ([Date]
                                   ,[Description]
                                   ,[Amount]
                                   ,[TransactionType]
                                   ,[Category]
                                   ,[AccountName])
                            VALUES
                                   (@Date
                                   ,@Description
                                   ,@Amount
                                   ,@TransactionType
                                   ,@Category
                                   ,@AccountName)";

                    command.Parameters.Add(new SqlParameter("@Date", date));
                    command.Parameters.Add(new SqlParameter("@Amount", amount));
                    command.Parameters.Add(new SqlParameter("@TransactionType", transactionType));
                    command.Parameters.Add(new SqlParameter("@Category", category));
                    command.Parameters.Add(new SqlParameter("@AccountName", accountName));

                    command.ExecuteNonQuery();

                    linesImported++;
                }

                if (linesImported == 1)
                    MessageBox.Show($"{linesImported} line imported successfully.");
                else if (linesImported > 1)
                    MessageBox.Show($"{linesImported} lines imported successfully.");
                else
                    MessageBox.Show($"No imported lines.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred in the application. Error: {ex.Message}");
            }
            finally
            {
                streamReader?.Close();

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBoxFilePath.Text = openFileDialog.FileName;
        }
    }
}
