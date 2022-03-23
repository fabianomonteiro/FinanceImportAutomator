using FinanceImportAutomator._01_Application;
using System;
using System.Data.Common;

namespace FinanceImportAutomator._03_Infra
{
    public class GetCategoryQuery : DatabaseInteractor<string, string>, IGetCategoryQuery
    {
        public GetCategoryQuery(DbConnection connection) : base(connection)
        {
        }

        protected override string ImplementExecute(string input)
        {
            throw new NotImplementedException();
        }
    }
}
