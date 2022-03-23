using System.Collections.Generic;

namespace DDDFinanceImportAutomator._02_Domain
{
    public interface ITransactionRepository
    {
        void InsertTransaction(Transaction transaction);

        void BulkInsert(IEnumerable<Transaction> transactions);
    }
}
