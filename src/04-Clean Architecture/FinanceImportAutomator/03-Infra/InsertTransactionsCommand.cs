using FinanceImportAutomator._01_Application;
using FinanceImportAutomator._02_Domain;
using FinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace FinanceImportAutomator._03_Infra
{
    public interface IInsertTransactionCommand : IInteractor<Transaction, VoidOutput>
    {

    }

    public class InsertTransactionsCommand : DatabaseInteractor<Transaction, VoidOutput>, IInsertTransactionCommand
    {
        public InsertTransactionsCommand(DbConnection connection) : base(connection) { }

        protected override VoidOutput ImplementExecute(Transaction input)
        {
            Command.CommandText = @"
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

            Command.ExecuteNonQuery();

            return VoidOutput.Empty;
        }

        protected override void AddParameters(DbParameterCollection parameters)
        {
            parameters.Add(Command.CreateParameter("@Date", Input.Date));
            parameters.Add(Command.CreateParameter("@Amount", Input.Amount));
            parameters.Add(Command.CreateParameter("@Description", Input.Description));
            parameters.Add(Command.CreateParameter("@TransactionType", Input.TransactionType));
            parameters.Add(Command.CreateParameter("@AccountName", Input.AccountName));

            object category = (object)Input.Category ?? DBNull.Value;

            parameters.Add(Command.CreateParameter("@Category", category));
        }
    }
}
