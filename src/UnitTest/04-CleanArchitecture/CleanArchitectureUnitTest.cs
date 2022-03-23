using CleanArchitectureFinanceImportAutomator._01_Application;
using CleanArchitectureFinanceImportAutomator._04_CrossCutting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTest
{
    [TestClass]
    public class CleanArchitectureUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var fileReaderMock = new Mock<IFileReader>();
            var getCategoryByDescriptionQueryMock = new Mock<IGetCategoryByDescriptionQuery>();
            var saveTransactionsCommandMock = new Mock<ISaveTransactionsCommand>();
            var notification = new Notification();

            var importUseCase = new ImportUseCase(
                fileReaderMock.Object
                , getCategoryByDescriptionQueryMock.Object
                , saveTransactionsCommandMock.Object
                , notification);

            importUseCase.Execute("INPUT FAKE");
        }
    }
}
