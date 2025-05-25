using HexagonalFinanceImportAutomator._01_Core.Domain;
using HexagonalFinanceImportAutomator._02_Ports;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Utilities;

namespace HexagonalFinanceImportAutomator._03_Adapters
{
    /// <summary>
    /// Adapter for transaction repository operations - implements ITransactionRepositoryPort
    /// This is a driven adapter (secondary adapter)
    /// </summary>
    public class TransactionRepositoryAdapter : DatabaseAdapterBase, ITransactionRepositoryPort
    {
        public TransactionRepositoryAdapter(DbConnection connection) : base(connection)
        {
        }

        public TransactionRepositoryAdapter(string connectionString) : base(connectionString)
        {
        }

        public void InsertTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            LogHelper.Log(LogType.Start, nameof(InsertTransaction));

            try
            {
                ExecuteWithConnection(connection =>
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO [dbo].[Transaction]
                                   ([Date]
                                   ,[Description]
                                   ,[Amount]
                                   ,[TransactionType]
                                   ,[Category]
                                   ,[AccountName])
                            VALUES
                                   (@Date
                                   ,@Description
                                   ,@Amount
                                   ,@TransactionType
                                   ,@Category
                                   ,@AccountName)";

                        command.Parameters.Add(CreateParameter(command, "@Date", transaction.Date));
                        command.Parameters.Add(CreateParameter(command, "@Amount", transaction.Amount));
                        command.Parameters.Add(CreateParameter(command, "@Description", transaction.Description));
                        command.Parameters.Add(CreateParameter(command, "@TransactionType", transaction.TransactionType));
                        command.Parameters.Add(CreateParameter(command, "@AccountName", transaction.AccountName));
                        command.Parameters.Add(CreateParameter(command, "@Category", transaction.Category));

                        command.ExecuteNonQuery();
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogType.Error, $"Error inserting transaction: {ex.Message}");
                throw;
            }
            finally
            {
                LogHelper.Log(LogType.End, nameof(InsertTransaction));
            }
        }

        public void BulkInsert(IEnumerable<Transaction> transactions)
        {
            if (transactions == null)
                throw new ArgumentNullException(nameof(transactions));

            var transactionList = transactions.ToList();
            if (!transactionList.Any())
                return;

            LogHelper.Log(LogType.Start, $"{nameof(BulkInsert)} - {transactionList.Count} transactions");

            try
            {
                ExecuteWithConnection(connection =>
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var transactionItem in transactionList)
                            {
                                using (var command = connection.CreateCommand())
                                {
                                    command.Transaction = transaction;
                                    command.CommandText = @"
                                        INSERT INTO [dbo].[Transaction]
                                               ([Date]
                                               ,[Description]
                                               ,[Amount]
                                               ,[TransactionType]
                                               ,[Category]
                                               ,[AccountName])
                                        VALUES
                                               (@Date
                                               ,@Description
                                               ,@Amount
                                               ,@TransactionType
                                               ,@Category
                                               ,@AccountName)";

                                    command.Parameters.Add(CreateParameter(command, "@Date", transactionItem.Date));
                                    command.Parameters.Add(CreateParameter(command, "@Amount", transactionItem.Amount));
                                    command.Parameters.Add(CreateParameter(command, "@Description", transactionItem.Description));
                                    command.Parameters.Add(CreateParameter(command, "@TransactionType", transactionItem.TransactionType));
                                    command.Parameters.Add(CreateParameter(command, "@AccountName", transactionItem.AccountName));
                                    command.Parameters.Add(CreateParameter(command, "@Category", transactionItem.Category));

                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogType.Error, $"Error bulk inserting transactions: {ex.Message}");
                throw;
            }
            finally
            {
                LogHelper.Log(LogType.End, $"{nameof(BulkInsert)} - {transactionList.Count} transactions");
            }
        }
    }
}
