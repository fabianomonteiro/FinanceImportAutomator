using DDDFinanceImportAutomator._02_Domain;
using DDDFinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Utilities;

namespace DDDFinanceImportAutomator._01_Application
{
    public interface ITransactionService
    {
        void ImportTransactions(string path);

        void CategorizeTransactions(IEnumerable<Transaction> transactions);

        void SaveTransactions(IEnumerable<Transaction> transactions);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionInfraService _transactionInfraService; 
        private readonly ICategorizeRepository _categorizeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotification _notification;

        public TransactionService(
            ITransactionInfraService transactionInfraService
            , ICategorizeRepository categorizeRepository
            , ITransactionRepository transactionRepository
            , INotification notification)
        {
            _transactionInfraService = transactionInfraService;
            _categorizeRepository = categorizeRepository;   
            _transactionRepository = transactionRepository;
            _notification = notification;
        }

        public void ImportTransactions(string path)
        {
            LogHelper.LogStart(nameof(ImportTransactions));

            var lines = _transactionInfraService.ReadTransactionsToImport(path);
            var transactions = new List<Transaction>();

            foreach (var line in lines)
            {
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
            }

            CategorizeTransactions(transactions);
            SaveTransactions(transactions);

            if (transactions.Count() == 1)
                _notification.AddNotification($"{transactions.Count()} transactions imported successfully.");
            else if (transactions.Count() > 1)
                _notification.AddNotification($"{transactions.Count()} transactions imported successfully.");
            else
                _notification.AddNotification($"No imported transactions.");

            LogHelper.LogEnd(nameof(ImportTransactions));
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
