using System;

namespace CleanArchitectureFinanceImportAutomator._04_CrossCutting
{
    public abstract class ExecuteAspectBase
    {
        public abstract void Start(string typeName, object input);

        public abstract void End(string typeName, object input, object output, TimeSpan executionTime);

        public abstract void Error(string typeName, object input, object output, Exception exception);
    }
}
