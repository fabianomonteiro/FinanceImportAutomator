using FinanceImportAutomator.LO;
using FinanceImportAutomator.VO;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Utilities;

namespace FinanceImportAutomator.BO
{
    public static class BusinessLogic
    {
        public static void ImportFile(string path)
        {
            LogHelper.LogStart(nameof(ImportFile));

            StreamReader streamReader = null;

            try
            {
                int linesImported = 0;
                int lineIndex = -1;
                string line;

                // Lê o arquivo
                streamReader = File.OpenText(path);

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

                    var transactionVO = CreateTransactionVO(line);
                    var category = DataAcessLayer.GetCategory(transactionVO.Description);

                    transactionVO.Category = category;
                    
                    DataAcessLayer.InsertTransaction(transactionVO);

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
                LogHelper.LogError(nameof(ImportFile), ex);

                MessageBox.Show($"An unexpected error occurred in the application. Error: {ex.Message}");
            }
            finally
            {
                streamReader?.Close();

                LogHelper.LogEnd(nameof(ImportFile));
            }
        }

        public static TransactionVO CreateTransactionVO(string line)
        {
            // Separa o conteúdo da linha por ponto e virgula
            var splitedLine = line.Split(';');
            var transactionVO = new TransactionVO();

            transactionVO.Date = DateTime.ParseExact(splitedLine[0], "MM/dd/yyyy", CultureInfo.InvariantCulture);
            transactionVO.Description = splitedLine[1];
            transactionVO.Amount = decimal.Parse(splitedLine[2], CultureInfo.InvariantCulture);
            transactionVO.TransactionType = splitedLine[3];
            transactionVO.AccountName = splitedLine[4];

            return transactionVO;
        }
    }
}
