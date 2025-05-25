using System;
using System.Data.Common;
using System.Data.SqlClient;
using Utilities;

namespace HexagonalFinanceImportAutomator._03_Adapters
{
    /// <summary>
    /// Base class for database adapters - provides common database functionality
    /// </summary>
    public abstract class DatabaseAdapterBase : IDisposable
    {
        protected readonly DbConnection Connection;
        private bool _disposed = false;

        protected DatabaseAdapterBase(DbConnection connection)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        protected DatabaseAdapterBase(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            Connection = new SqlConnection(connectionString);
        }

        protected DbParameter CreateParameter(DbCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        protected void ExecuteWithConnection(Action<DbConnection> action)
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Closed)
                    Connection.Open();

                action(Connection);
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogType.Error, $"Database error: {ex.Message}");
                throw;
            }
            finally
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                    Connection.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Connection?.Dispose();
                _disposed = true;
            }
        }
    }
}
