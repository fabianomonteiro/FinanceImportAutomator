using DDDFinanceImportAutomator._02_Domain;
using DDDFinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        private readonly ITransactionReaderService _transactionReaderService; 
        private readonly ICategorizeRepository _categorizeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly INotification _notification;

        public TransactionService(
            ITransactionReaderService transactionReaderService
            , ICategorizeRepository categorizeRepository
            , ITransactionRepository transactionRepository
            , INotification notification)
        {
            _transactionReaderService = transactionReaderService;
            _categorizeRepository = categorizeRepository;   
            _transactionRepository = transactionRepository;
            _notification = notification;
        }

        public void ImportTransactions(string path)
        {
            LogHelper.LogStart(nameof(ImportTransactions));

            var transactions = _transactionReaderService.ReadTransactionsToImport(path);

            CategorizeTransactions(transactions);
            SaveTransactions(transactions);

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
