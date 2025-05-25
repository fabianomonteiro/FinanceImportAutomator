using HexagonalFinanceImportAutomator._01_Core.Domain;
using HexagonalFinanceImportAutomator._02_Ports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Utilities;

namespace HexagonalFinanceImportAutomator._01_Core.Services
{
    /// <summary>
    /// Core service for transaction import operations - implements the primary port
    /// This contains the pure business logic without external dependencies
    /// </summary>
    public class TransactionService : ITransactionImportPort
    {
        private readonly IFileReaderPort _fileReaderPort;
        private readonly ICategorizeRepositoryPort _categorizeRepositoryPort;
        private readonly ITransactionRepositoryPort _transactionRepositoryPort;
        private readonly INotificationPort _notificationPort;

        public TransactionService(
            IFileReaderPort fileReaderPort,
            ICategorizeRepositoryPort categorizeRepositoryPort,
            ITransactionRepositoryPort transactionRepositoryPort,
            INotificationPort notificationPort)
        {
            _fileReaderPort = fileReaderPort ?? throw new ArgumentNullException(nameof(fileReaderPort));
            _categorizeRepositoryPort = categorizeRepositoryPort ?? throw new ArgumentNullException(nameof(categorizeRepositoryPort));
            _transactionRepositoryPort = transactionRepositoryPort ?? throw new ArgumentNullException(nameof(transactionRepositoryPort));
            _notificationPort = notificationPort ?? throw new ArgumentNullException(nameof(notificationPort));
        }

        public void ImportTransactions(string filePath)
        {
            LogHelper.Log(LogType.Start, "Import init");

            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    _notificationPort.AddMessage("Import file is required.");
                    return;
                }

                var lines = _fileReaderPort.ReadLines(filePath);
                
                if (!lines.Any())
                {
                    _notificationPort.AddMessage("No data found in the file.");
                    return;
                }

                var transactions = ProcessLines(lines);
                
                if (transactions.Any())
                {
                    _transactionRepositoryPort.BulkInsert(transactions);
                    
                    var count = transactions.Count();
                    if (count == 1)
                        _notificationPort.AddMessage($"{count} line imported successfully.");
                    else
                        _notificationPort.AddMessage($"{count} lines imported successfully.");
                }
                else
                {
                    _notificationPort.AddMessage("No imported lines.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogType.Error, $"Error: {ex.Message}");
                _notificationPort.AddMessage($"An unexpected error occurred in the application. Error: {ex.Message}");
            }
            finally
            {
                LogHelper.Log(LogType.End, "Import finish");
            }
        }

        private IEnumerable<Transaction> ProcessLines(IEnumerable<string> lines)
        {
            var transactions = new List<Transaction>();
            var lineIndex = -1;

            foreach (var line in lines)
            {
                lineIndex++;

                // Skip header
                if (lineIndex == 0)
                    continue;

                var transaction = ProcessLine(line);
                if (transaction != null)
                {
                    var category = _categorizeRepositoryPort.GetCategoryByDescription(transaction.Description);
                    if (!string.IsNullOrEmpty(category))
                    {
                        transaction.SetCategory(category);
                    }
                    
                    transactions.Add(transaction);
                }
            }

            return transactions;
        }

        private Transaction ProcessLine(string line)
        {
            try
            {
                var splitedLine = line.Split(';');
                
                if (splitedLine.Length < 5)
                    return null;

                var dateString = splitedLine[0];
                var description = splitedLine[1];
                var amountString = splitedLine[2];
                var transactionType = splitedLine[3];
                var accountName = splitedLine[4];

                if (!DateTime.TryParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    return null;

                if (!decimal.TryParse(amountString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var amount))
                    return null;

                return Transaction.New(date, amount, description, transactionType, accountName);
            }
            catch
            {
                return null;
            }
        }
    }
}
