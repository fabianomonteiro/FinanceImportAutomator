using DDDFinanceImportAutomator._02_Domain;
using System.Collections.Generic;

namespace DDDFinanceImportAutomator._01_Application
{
    public interface ITransactionInfraService
    {
        IEnumerable<Transaction> ReadTransactionsToImport(string path);
    }
}
