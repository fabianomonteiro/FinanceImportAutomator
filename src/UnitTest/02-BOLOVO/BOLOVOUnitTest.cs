using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class BOLOVOUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Não é possível fazer teste unitário,
            //Somente teste integrado pois não é possível fazer mock
            //Projeto não possui injeção de dependência
            //BusinessLogic.ImportFile("caminho do arquivo");
        }
    }
}
