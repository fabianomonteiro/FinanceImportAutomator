using FinanceImportAutomator._01_Application;
using FinanceImportAutomator._02_Domain;
using FinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace FinanceImportAutomator._03_Infra
{
    public class SaveTransactionsOneToOneCommand : 
        Interactor<IEnumerable<Transaction>, VoidOutput>, 
        ISaveTransactionsCommand
    {
        private readonly IInsertTransactionCommand _insertTransactionCommand;

        public SaveTransactionsOneToOneCommand(IInsertTransactionCommand insertTransactionCommand) 
        {
            _insertTransactionCommand = insertTransactionCommand;
        }

        protected override VoidOutput ImplementExecute(IEnumerable<Transaction> input)
        {
            foreach (var transaction in input)
            {
                _insertTransactionCommand.Execute(transaction);
            }

            return VoidOutput.Empty;
        }
    }
}
