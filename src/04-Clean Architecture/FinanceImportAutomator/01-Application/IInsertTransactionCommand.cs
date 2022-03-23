using FinanceImportAutomator._02_Domain;
using FinanceImportAutomator._04_CrossCutting;

namespace FinanceImportAutomator._01_Application
{
    public interface IInsertTransactionCommand : IInteractor<Transaction, VoidOutput>
    {
    }
}
