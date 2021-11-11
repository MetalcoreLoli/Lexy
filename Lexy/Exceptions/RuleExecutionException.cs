using System;

namespace Lexy.Exceptions
{
    public class RuleExecutionException : Exception
    {
        private readonly string _ruleName;

        public RuleExecutionException(string ruleName, string context)
            : base ($"Rule {ruleName} is failed to execute on {context}")
        {
            _ruleName = ruleName;
        }
    }
}