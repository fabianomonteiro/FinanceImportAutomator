using DDDFinanceImportAutomator._02_Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Utilities;

namespace DDDFinanceImportAutomator._03_Infra
{
    public class TransactionRepository : RepositoryBase, ITransactionRepository
    {
        public TransactionRepository(DbConnection connection) : base(connection) { }

        public void InsertTransaction(Transaction transaction)
        {
            LogHelper.LogStart(nameof(InsertTransaction));

            Connection.Open();

            try
            {
                var command = Connection.CreateCommand();

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

                command.Parameters.Add(command.CreateParameter("@Date", transaction.Date));
                command.Parameters.Add(command.CreateParameter("@Amount", transaction.Amount));
                command.Parameters.Add(command.CreateParameter("@Description", transaction.Description));
                command.Parameters.Add(command.CreateParameter("@TransactionType", transaction.TransactionType));
                command.Parameters.Add(command.CreateParameter("@AccountName", transaction.AccountName));

                object category = (object)transaction.Category ?? DBNull.Value;

                command.Parameters.Add(command.CreateParameter("@Category", category));

                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                LogHelper.LogError(nameof(InsertTransaction), ex);
            }
            finally
            {
                Connection.Close();

                LogHelper.LogEnd(nameof(InsertTransaction));
            }
        }

        public void BulkInsert(IEnumerable<Transaction> transactions)
        {
            throw new NotImplementedException();
        }
    }
}
