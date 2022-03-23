using System;

namespace FinanceImportAutomator._02_Domain
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
            DateTime date
            , decimal amount
            , string description
            , string transactionType
            , string accountName)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("description");

            if (string.IsNullOrWhiteSpace(transactionType))
                throw new ArgumentNullException("transactionType");

            if (string.IsNullOrWhiteSpace(accountName))
                throw new ArgumentNullException("accountName");

            var transaction = new Transaction();

            transaction.Date = date;
            transaction.Amount = amount;
            transaction.Description = description;
            transaction.TransactionType = transactionType;
            transaction.AccountName = accountName;

            return transaction;
        }
    }
}
