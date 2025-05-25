using System;

namespace HexagonalFinanceImportAutomator._01_Core.Domain
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string TransactionType { get; set; }
        public string AccountName { get; set; }
        public string Category { get; set; }

        protected Transaction() { }

        public static Transaction New(
            DateTime date,
            decimal amount,
            string description,
            string transactionType,
            string accountName)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description));

            if (string.IsNullOrWhiteSpace(transactionType))
                throw new ArgumentNullException(nameof(transactionType));

            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentNullException(nameof(accountName));

            return new Transaction
            {
                Date = date,
                Amount = amount,
                Description = description,
                TransactionType = transactionType,
                AccountName = accountName
            };
        }

        public void SetCategory(string category)
        {
            Category = category;
        }
    }
}
