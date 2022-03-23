using System.Data.Common;

namespace DDDFinanceImportAutomator._03_Infra
{
    public static class DbCommandExtensions
    {
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }
    }

    public abstract class RepositoryBase
    {
        protected DbConnection Connection { get; private set; }

        public RepositoryBase(DbConnection connection)
        {
            Connection = connection;
        }
    }
}
