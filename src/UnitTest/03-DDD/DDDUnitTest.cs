﻿using DDDFinanceImportAutomator._01_Application;
using DDDFinanceImportAutomator._02_Domain;
using DDDFinanceImportAutomator._04_CrossCutting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTest
{
    [TestClass]
    public class DDDUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var categorizeRepositoryMock = new Mock<ICategorizeRepository>();
            var transactionRepositoryMock = new Mock<ITransactionRepository>();
            var transactionReaderServiceMock = new Mock<ITransactionReaderService>();
            var notification = new Notification();

            var transactionService = new TransactionService(
                transactionReaderServiceMock.Object
                , categorizeRepositoryMock.Object
                , transactionRepositoryMock.Object
                , notification);

            transactionService.ImportTransactions("FAKE");
        }
    }
}
