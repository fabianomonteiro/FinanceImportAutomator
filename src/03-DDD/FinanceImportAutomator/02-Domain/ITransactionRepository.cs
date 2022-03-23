using System.Collections.Generic;

namespace FinanceImportAutomator._02_Domain
{
    public interface ITransactionRepository
    {
        void InsertTransaction(Transaction transaction);

        void BulkInsert(IEnumerable<Transaction> transactions);
    }
}
