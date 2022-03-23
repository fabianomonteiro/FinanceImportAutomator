using CleanArchitectureFinanceImportAutomator._01_Application;
using CleanArchitectureFinanceImportAutomator._02_Domain;
using CleanArchitectureFinanceImportAutomator._04_CrossCutting;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace CleanArchitectureFinanceImportAutomator._03_Infra
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
