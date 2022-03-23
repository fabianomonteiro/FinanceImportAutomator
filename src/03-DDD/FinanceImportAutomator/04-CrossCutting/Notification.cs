using System.Collections.Generic;
using System.Text;

namespace DDDFinanceImportAutomator._04_CrossCutting
{
    public interface INotification
    {
        IReadOnlyCollection<string> Messages { get; }

        void AddNotification(string message);

        string GetMessagesSepartedByLine();
    }

    public class Notification : INotification
    {
        public List<string> _messages;

        public IReadOnlyCollection<string> Messages => _messages;

        public Notification()
        {
            _messages = new List<string>();
        }

        public void AddNotification(string message)
        {
            _messages.Add(message);
        }

        public string GetMessagesSepartedByLine()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string message in _messages)
            {
                stringBuilder.AppendLine(message);
            }

            return stringBuilder.ToString();
        }
    }
}
