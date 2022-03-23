using FinanceImportAutomator._04_CrossCutting;
using System.Data.Common;

namespace FinanceImportAutomator._03_Infra
{
    public static class DbParameterExtensions
    {
        public static DbParameter CreateParameter(this DbCommand command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }
    }

    public abstract class DatabaseInteractor<TInput, TOutput> : Interactor<TInput, TOutput>
    {
        protected DbConnection Connection { get; private set; }

        protected DbCommand Command { get; private set; }

        public DatabaseInteractor(DbConnection connection)
        {
            Connection = connection;
            Command = connection.CreateCommand();
        }

        public override TOutput Execute(TInput input)
        {
            AddParameters(Command.Parameters);

            return base.Execute(input);
        }

        protected override void BeforeExecute()
        {
            Connection.Open();
        }

        protected override void FinallyExecute()
        {
            Connection.Close();
        }

        protected virtual void AddParameters(DbParameterCollection parameters) { }
    }
}
