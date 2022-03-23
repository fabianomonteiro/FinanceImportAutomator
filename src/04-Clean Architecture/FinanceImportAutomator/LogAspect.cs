﻿using FinanceImportAutomator._04_CrossCutting;
using System;
using Utilities;

namespace FinanceImportAutomator
{
    public class LogAspect : ExecuteAspectBase
    {
        public override void Start(string typeName, object input)
        {
            LogHelper.LogStart(typeName);
        }

        public override void End(string typeName, object input, object output, TimeSpan executionTime)
        {
            LogHelper.LogEnd(typeName);
        }

        public override void Error(string typeName, object input, object output, Exception exception)
        {
            LogHelper.LogError(typeName, exception);
        }        
    }
}