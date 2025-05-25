namespace HexagonalFinanceImportAutomator._02_Ports
{
    /// <summary>
    /// Port for categorize repository operations - defines the contract for category data access
    /// </summary>
    public interface ICategorizeRepositoryPort
    {
        string GetCategoryByDescription(string description);
    }
}
