using CleanArchitectureFinanceImportAutomator._02_Domain;
using CleanArchitectureFinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CleanArchitectureFinanceImportAutomator._01_Application
{
    public interface IImportUseCase : IInteractor<string, VoidOutput> { }

    public class ImportUseCase : Interactor<string, VoidOutput>, IImportUseCase
    {
        private readonly IFileReader _fileReader;
        private readonly IGetCategoryByDescriptionQuery _getCategoryByDescriptionQuery;
        private readonly ISaveTransactionsCommand _saveTransactionsCommand;
        private readonly INotification _notification;

        public ImportUseCase(
            IFileReader fileReader
            , IGetCategoryByDescriptionQuery getCategoryByDescriptionQuery
            , ISaveTransactionsCommand saveTransactionsCommand
            , INotification notification)
        {
            _fileReader = fileReader;
            _getCategoryByDescriptionQuery = getCategoryByDescriptionQuery;
            _saveTransactionsCommand = saveTransactionsCommand;
            _notification = notification;
        }

        protected override VoidOutput ImplementExecute(string input)
        {
            var lines = _fileReader.Execute(input);

            if (lines == null)
            {
                _notification.AddNotification("No transactions were found to be imported");
                return VoidOutput.Empty;
            }

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

            foreach (var transaction in transactions)
            {
                var category = _getCategoryByDescriptionQuery.Execute(transaction.Description);

                transaction.Category = category;
            }

            _saveTransactionsCommand.Execute(transactions);

            if (transactions.Count() == 1)
                _notification.AddNotification($"{transactions.Count()} transactions imported successfully.");
            else if (transactions.Count() > 1)
                _notification.AddNotification($"{transactions.Count()} transactions imported successfully.");
            else
                _notification.AddNotification($"No imported transactions.");

            return VoidOutput.Empty;
        }
    }
}
