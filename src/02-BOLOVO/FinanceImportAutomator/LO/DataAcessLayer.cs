using BOLOVOFinanceImportAutomator.VO;
using System;
using System.Data.SqlClient;
using Utilities;

namespace BOLOVOFinanceImportAutomator.LO
{
    public static class DataAcessLayer
    {
        static string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FinanceImportAutomator;Integrated Security=True";
        static SqlConnection _connection;

        static DataAcessLayer()
        {
            _connection = new SqlConnection(_connectionString);
        }

        public static string GetCategory(string description)
        {
            LogHelper.LogStart(nameof(GetCategory));

            _connection.Open();

            try
            {
                var command = _connection.CreateCommand();

                command.CommandText = $@"
                            SELECT Category FROM Categorize WHERE @Description LIKE '%' + Description + '%'";

                command.Parameters.Add(new SqlParameter("@Description", description));

                object value = command.ExecuteScalar();

                if (value != null)
                    return value.ToString();
            }
            catch(Exception ex)
            {
                LogHelper.LogError(nameof(GetCategory), ex);
            }
            finally
            {
                _connection.Close();

                LogHelper.LogEnd(nameof(GetCategory));
            }

            return null;
        }

        public static void InsertTransaction(TransactionVO transaction)
        {
            LogHelper.LogStart(nameof(InsertTransaction));

            _connection.Open();

            try
            {
                var command = _connection.CreateCommand();

                command.CommandText = @"
                            INSERT INTO [dbo].[Transaction]
                                   ([Date]
                                   ,[Description]
                                   ,[Amount]
                                   ,[TransactionType]
                                   ,[Category]
                                   ,[AccountName])
                            VALUES
                                   (@Date
                                   ,@Description
                                   ,@Amount
                                   ,@TransactionType
                                   ,@Category
                                   ,@AccountName)";

                command.Parameters.Add(new SqlParameter("@Date", transaction.Date));
                command.Parameters.Add(new SqlParameter("@Amount", transaction.Amount));
                command.Parameters.Add(new SqlParameter("@Description", transaction.Description));
                command.Parameters.Add(new SqlParameter("@TransactionType", transaction.TransactionType));
                command.Parameters.Add(new SqlParameter("@AccountName", transaction.AccountName));

                object category = (object)transaction.Category ?? DBNull.Value;
                
                command.Parameters.Add(new SqlParameter("@Category", category));

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(nameof(InsertTransaction), ex);
            }
            finally
            {
                _connection.Close();

                LogHelper.LogEnd(nameof(InsertTransaction));
            }
        }
    }
}
