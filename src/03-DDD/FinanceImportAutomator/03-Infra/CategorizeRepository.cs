using FinanceImportAutomator._02_Domain;
using System.Data.Common;

namespace FinanceImportAutomator._03_Infra
{
    public class CategorizeRepository : RepositoryBase, ICategorizeRepository
    {
        public CategorizeRepository(DbConnection connection) : base(connection) { }

        public string GetCategoryByDescription(string description)
        {
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

                return null;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}
