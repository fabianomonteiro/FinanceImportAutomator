using FinanceImportAutomator._02_Domain;
using FinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Utilities;

namespace FinanceImportAutomator._01_Application
{
    public interface ITransactionService
    {
        void ImportTransactions(string path);

        IEnumerable<Transaction> ReadTransactionsToImport(string path);

        void CategorizeTransactions(IEnumerable<Transaction> transactions);

        void SaveTransactions(IEnumerable<Transaction> transactions);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ICategorizeRepository _categorizeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotification _notification;

        public TransactionService(
            ICategorizeRepository categorizeRepository
            , ITransactionRepository transactionRepository
            , INotification notification)
        {
            _categorizeRepository = categorizeRepository;   
            _transactionRepository = transactionRepository;
            _notification = notification;
        }

        public void ImportTransactions(string path)
        {
            LogHelper.LogStart(nameof(ImportTransactions));

            var transactions = ReadTransactionsToImport(path);

            CategorizeTransactions(transactions);
            SaveTransactions(transactions);

            LogHelper.LogEnd(nameof(ImportTransactions));
        }

        public IEnumerable<Transaction> ReadTransactionsToImport(string path)
        {
            LogHelper.LogStart(nameof(ReadTransactionsToImport));

            StreamReader streamReader = null;
            List<Transaction> transactions = new List<Transaction>();

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
                LogHelper.LogError(nameof(ReadTransactionsToImport), ex);

                _notification.AddNotification($"An unexpected error occurred in the application. Error: {ex.Message}");
            }
            finally
            {
                streamReader?.Close();

                LogHelper.LogEnd(nameof(ReadTransactionsToImport));
            }

            return transactions;
        }

        public void CategorizeTransactions(IEnumerable<Transaction> transactions)
        {
            LogHelper.LogStart(nameof(CategorizeTransactions));

            foreach (var transaction in transactions)
            {
                var category = _categorizeRepository.GetCategoryByDescription(transaction.Description);

                transaction.Category = category;
            }

            LogHelper.LogEnd(nameof(CategorizeTransactions));
        }

        public void SaveTransactions(IEnumerable<Transaction> transactions)
        {
            LogHelper.LogStart(nameof(SaveTransactions));

            foreach (var transaction in transactions)
            {
                _transactionRepository.InsertTransaction(transaction);
            }

            LogHelper.LogEnd(nameof(SaveTransactions));
        }
    }
}
