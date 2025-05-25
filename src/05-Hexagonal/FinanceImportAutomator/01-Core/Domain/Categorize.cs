namespace HexagonalFinanceImportAutomator._01_Core.Domain
{
    public class Categorize
    {
        public string Description { get; set; }
        public string Category { get; set; }

        public Categorize(string description, string category)
        {
            Description = description;
            Category = category;
        }
    }
}
