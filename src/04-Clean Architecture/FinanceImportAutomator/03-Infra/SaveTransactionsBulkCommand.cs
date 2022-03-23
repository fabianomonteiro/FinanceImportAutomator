using FinanceImportAutomator._01_Application;
using FinanceImportAutomator._02_Domain;
using FinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace FinanceImportAutomator._03_Infra
{
    public class SaveTransactionsBulkCommand : 
        DatabaseInteractor<IEnumerable<Transaction>, VoidOutput>, 
        ISaveTransactionsCommand
    {
        public SaveTransactionsBulkCommand(DbConnection connection) : base(connection) { }

        protected override VoidOutput ImplementExecute(IEnumerable<Transaction> input)
        {
            throw new NotImplementedException();
        }
    }
}
