using System.Collections.Generic;

namespace HexagonalFinanceImportAutomator._02_Ports
{
    /// <summary>
    /// Port for reading files - defines the contract for file operations
    /// </summary>
    public interface IFileReaderPort
    {
        IEnumerable<string> ReadLines(string filePath);
    }
}
