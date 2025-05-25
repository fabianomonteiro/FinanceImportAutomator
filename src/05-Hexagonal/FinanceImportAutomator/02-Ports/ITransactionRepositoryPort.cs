using HexagonalFinanceImportAutomator._01_Core.Domain;
using System.Collections.Generic;

namespace HexagonalFinanceImportAutomator._02_Ports
{
    /// <summary>
    /// Port for transaction repository operations - defines the contract for transaction data access
    /// </summary>
    public interface ITransactionRepositoryPort
    {
        void InsertTransaction(Transaction transaction);
        void BulkInsert(IEnumerable<Transaction> transactions);
    }
}
