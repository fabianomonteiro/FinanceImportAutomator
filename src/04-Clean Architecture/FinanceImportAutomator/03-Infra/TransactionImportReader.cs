using FinanceImportAutomator._01_Application;
using FinanceImportAutomator._02_Domain;
using FinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FinanceImportAutomator._03_Infra
{
    public class TransactionImportReader : Interactor<string, IEnumerable<Transaction>>, ITransactionImportReader
    {
        public string Path => Input;

        private readonly INotification _notification;

        public TransactionImportReader(INotification notification)
        {
            _notification = notification;
        }

        protected override IEnumerable<Transaction> ImplementExecute(string input)
        {
            StreamReader streamReader = null;
            List<Transaction> transactions = new List<Transaction>();

            try
            {
                int linesImported = 0;
                int lineIndex = -1;
                string line;

                // Lê o arquivo
                streamReader = File.OpenText(Path);

                // Percorre o arquivo linha a linha
                while ((line = streamReader.ReadLine()) != null)
                {
                    lineIndex++;

                    // Valida se o conteúdo do arquivo está em formato correto
                    if (!line.Contains(';'))
                    {
                        _notification.AddNotification("Incorrect format file.");
                        return transactions;
                    }

                    // Pular cabeçalho
                    if (lineIndex == 0)
                        continue;

                    var splitedLine = line.Split(';');
                    var date = DateTime.ParseExact(splitedLine[0], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    var amount = decimal.Parse(splitedLine[2], CultureInfo.InvariantCulture);
                    var description = splitedLine[1];
                    var transactionType = splitedLine[3];
                    var accountName = splitedLine[4];

                    var transaction = Transaction.New(
                        date
                        , amount
                        , description
                        , transactionType
                        , accountName);

                    transactions.Add(transaction);

                    linesImported++;
                }

                if (linesImported == 1)
                    _notification.AddNotification($"{linesImported} line imported successfully.");
                else if (linesImported > 1)
                    _notification.AddNotification($"{linesImported} lines imported successfully.");
                else
                    _notification.AddNotification($"No imported lines.");
            }
            catch (Exception ex)
            {
                _notification.AddNotification($"An unexpected error occurred in the application. Error: {ex.Message}");
            }
            finally
            {
                streamReader?.Close();
            }

            return transactions;
        }
    }
}
