using System.Collections.Generic;

namespace HexagonalFinanceImportAutomator._02_Ports
{
    /// <summary>
    /// Port for notification operations - defines the contract for notification handling
    /// </summary>
    public interface INotificationPort
    {
        void AddMessage(string message);
        IEnumerable<string> GetMessages();
        string GetMessagesAsString();
    }
}
