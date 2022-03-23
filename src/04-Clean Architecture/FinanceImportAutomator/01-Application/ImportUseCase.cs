using FinanceImportAutomator._04_CrossCutting;
using System;

namespace FinanceImportAutomator._01_Application
{
    public interface IImportUseCase : IInteractor<string, VoidOutput>
    {

    }

    public class ImportUseCase : Interactor<string, VoidOutput>, IImportUseCase
    {
        private readonly IGetCategoryQuery _getCategoryQuery;
        private readonly IInsertTransactionCommand _insertTransactionCommand;

        public ImportUseCase(
            IGetCategoryQuery getCategoryQuery
            , IInsertTransactionCommand insertTransactionCommand)
        {
            _getCategoryQuery = getCategoryQuery;
            _insertTransactionCommand = insertTransactionCommand;
        }

        protected override VoidOutput ImplementExecute(string input)
        {
            throw new NotImplementedException();
        }
    }
}
