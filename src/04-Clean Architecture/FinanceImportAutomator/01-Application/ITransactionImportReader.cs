using FinanceImportAutomator._02_Domain;
using FinanceImportAutomator._04_CrossCutting;
using System.Collections.Generic;

namespace FinanceImportAutomator._01_Application
{
    public interface ITransactionImportReader : IInteractor<string, IEnumerable<Transaction>> { }
}
