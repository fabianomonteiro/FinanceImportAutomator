using HexagonalFinanceImportAutomator._01_Core.Services;
using HexagonalFinanceImportAutomator._02_Ports;
using HexagonalFinanceImportAutomator._04_CrossCutting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTest
{
    [TestClass]
    public class HexagonalUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fileReaderPortMock = new Mock<IFileReaderPort>();
            var categorizeRepositoryPortMock = new Mock<ICategorizeRepositoryPort>();
            var transactionRepositoryPortMock = new Mock<ITransactionRepositoryPort>();
            var notificationPort = new NotificationAdapter();

            var transactionService = new TransactionService(
                fileReaderPortMock.Object,
                categorizeRepositoryPortMock.Object,
                transactionRepositoryPortMock.Object,
                notificationPort);

            transactionService.ImportTransactions("FAKE");
        }
    }
}