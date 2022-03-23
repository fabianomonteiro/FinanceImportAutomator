using FinanceImportAutomator._04_CrossCutting;

namespace FinanceImportAutomator._01_Application
{
    public interface IImportUseCase : IInteractor<string, VoidOutput> { }

    public class ImportUseCase : Interactor<string, VoidOutput>, IImportUseCase
    {
        private readonly ITransactionImportReader _transactionImportReader;
        private readonly IGetCategoryByDescriptionQuery _getCategoryByDescriptionQuery;
        private readonly ISaveTransactionsCommand _saveTransactionsCommand;
        private readonly INotification _notification;

        public ImportUseCase(
            ITransactionImportReader transactionImportReader
            , IGetCategoryByDescriptionQuery getCategoryByDescriptionQuery
            , ISaveTransactionsCommand saveTransactionsCommand
            , INotification notification)
        {
            _transactionImportReader = transactionImportReader;
            _getCategoryByDescriptionQuery = getCategoryByDescriptionQuery;
            _saveTransactionsCommand = saveTransactionsCommand;
            _notification = notification;
        }

        protected override VoidOutput ImplementExecute(string input)
        {
            var transactionsToImport = _transactionImportReader.Execute(input);

            if (transactionsToImport == null)
            {
                _notification.AddNotification("No transactions were found to be imported");
                return VoidOutput.Empty;
            }

            foreach (var transaction in transactionsToImport)
            {
                var category = _getCategoryByDescriptionQuery.Execute(transaction.Description);

                transaction.Category = category;
            }

            _saveTransactionsCommand.Execute(transactionsToImport);

            return VoidOutput.Empty;
        }
    }
}
