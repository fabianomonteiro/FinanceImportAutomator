using DDDFinanceImportAutomator._02_Domain;
using System;
using System.Data.Common;
using Utilities;

namespace DDDFinanceImportAutomator._03_Infra
{
    public class CategorizeRepository : RepositoryBase, ICategorizeRepository
    {
        public CategorizeRepository(DbConnection connection) : base(connection) { }

        public string GetCategoryByDescription(string description)
        {
            LogHelper.LogStart(nameof(GetCategoryByDescription));

            Connection.Open();

            try
            {
                var command = Connection.CreateCommand();

                command.CommandText = $@"
                            SELECT Category FROM Categorize WHERE @Description LIKE '%' + Description + '%'";

                command.Parameters.Add(command.CreateParameter("@Description", description));

                object value = command.ExecuteScalar();

                if (value != null)
                    return value.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(nameof(GetCategoryByDescription), ex);
            }
            finally
            {
                Connection.Close();

                LogHelper.LogEnd(nameof(GetCategoryByDescription));
            }

            return null;
        }
    }
}
