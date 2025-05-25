namespace HexagonalFinanceImportAutomator._02_Ports
{
    /// <summary>
    /// Primary Port (Driver Port) - defines the contract for the main application use case
    /// This is the entry point for the hexagonal architecture
    /// </summary>
    public interface ITransactionImportPort
    {
        void ImportTransactions(string filePath);
    }
}
