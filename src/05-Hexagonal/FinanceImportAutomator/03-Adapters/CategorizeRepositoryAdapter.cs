using HexagonalFinanceImportAutomator._02_Ports;
using System;
using System.Data.Common;
using Utilities;

namespace HexagonalFinanceImportAutomator._03_Adapters
{
    /// <summary>
    /// Adapter for categorize repository operations - implements ICategorizeRepositoryPort
    /// This is a driven adapter (secondary adapter)
    /// </summary>
    public class CategorizeRepositoryAdapter : DatabaseAdapterBase, ICategorizeRepositoryPort
    {
        public CategorizeRepositoryAdapter(DbConnection connection) : base(connection)
        {
        }

        public CategorizeRepositoryAdapter(string connectionString) : base(connectionString)
        {
        }

        public string GetCategoryByDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return null;

            LogHelper.Log(LogType.Start, nameof(GetCategoryByDescription));

            string category = null;

            try
            {
                ExecuteWithConnection(connection =>
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            SELECT Category 
                            FROM Categorize 
                            WHERE @Description LIKE '%' + Description + '%'";

                        var parameter = CreateParameter(command, "@Description", description);
                        command.Parameters.Add(parameter);

                        var result = command.ExecuteScalar();
                        category = result?.ToString();
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogType.Error, $"Error getting category: {ex.Message}");
                throw;
            }
            finally
            {
                LogHelper.Log(LogType.End, nameof(GetCategoryByDescription));
            }

            return category;
        }
    }
}
