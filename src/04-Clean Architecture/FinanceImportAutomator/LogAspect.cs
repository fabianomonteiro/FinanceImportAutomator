using CleanArchitectureFinanceImportAutomator._04_CrossCutting;
using System;
using Utilities;

namespace CleanArchitectureFinanceImportAutomator
{
    public class LogAspect : ExecuteAspectBase
    {
        public override void Start(string typeName, object input)
        {
            LogHelper.LogStart(typeName, $"Input={input}");
        }

        public override void End(string typeName, object input, object output, TimeSpan executionTime)
        {
            LogHelper.LogEnd(typeName, $"Input={input}", $"Output={output?.ToString()}", $"ExecutionTime={executionTime}");
        }

        public override void Error(string typeName, object input, object output, Exception exception)
        {
            LogHelper.LogError(typeName, exception);
        }        
    }
}
