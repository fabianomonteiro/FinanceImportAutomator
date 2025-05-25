using HexagonalFinanceImportAutomator._02_Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexagonalFinanceImportAutomator._04_CrossCutting
{
    /// <summary>
    /// Notification adapter - implements INotificationPort for message handling
    /// This is a driven adapter (secondary adapter)
    /// </summary>
    public class NotificationAdapter : INotificationPort
    {
        private readonly List<string> _messages;

        public NotificationAdapter()
        {
            _messages = new List<string>();
        }

        public void AddMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _messages.Add(message);
            }
        }

        public IEnumerable<string> GetMessages()
        {
            return _messages.AsReadOnly();
        }

        public string GetMessagesAsString()
        {
            if (!_messages.Any())
                return string.Empty;

            var stringBuilder = new StringBuilder();
            foreach (var message in _messages)
            {
                stringBuilder.AppendLine(message);
            }
            return stringBuilder.ToString().TrimEnd();
        }
    }
}
