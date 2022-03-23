using CleanArchitectureFinanceImportAutomator._01_Application;
using System;
using System.Data.Common;

namespace CleanArchitectureFinanceImportAutomator._03_Infra
{
    public class GetCategoryByDescriptionQuery : DatabaseInteractor<string, string>, IGetCategoryByDescriptionQuery
    {
        public string Description => Input;

        public GetCategoryByDescriptionQuery(DbConnection connection) : base(connection) { }

        protected override string ImplementExecute(string input)
        {
            Command.CommandText = $@"
                            SELECT Category FROM Categorize WHERE @Description LIKE '%' + Description + '%'";

            object value = Command.ExecuteScalar();

            if (value != null)
                return value.ToString();

            return null;
        }

        protected override void AddParameters(DbParameterCollection parameters)
        {
            Command.Parameters.Add(Command.CreateParameter("@Description", Description));
        }
    }
}
