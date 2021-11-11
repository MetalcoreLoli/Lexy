using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexy
{
    public abstract record Rule
    {
        #region PUBLIC
        public abstract ExecutionResult ExecuteOn(string context);
        #endregion

        #region OPERATORS

        public static Rule operator |(Rule left, Rule right) => new OrCombinatorRule(left, right);
        public static Rule operator &(Rule left, Rule right) => new AndCombinatorRule(left, right);

        #endregion

        public record ExecutionResult : IEnumerable<ExecutionResult>
        {
            protected readonly List<ExecutionResult> _results;
            protected readonly object _value;

            public ExecutionResult(object context, string  tail = "")
            {
                _value = context ?? throw new NullReferenceException();
                _results = new List<ExecutionResult>();
                Tail = tail;
            }

            internal string Tail { get; }

            public ExecutionResult Append(ExecutionResult result)
            {
                _results.Add(result);
                return this;
            }

            public T As<T>() => (T) _value;

            public IEnumerable<T> AllAsOrDefault<T>() => 
                _results.Count > 0 ? _results.Select(r => r.As<T>()).Append(As<T>()) : default;

            public IEnumerator<ExecutionResult> GetEnumerator() => _results.GetEnumerator();

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append($"`{_value}` ");
                foreach (var result in _results)
                    sb.Append($"`{result._value}` ");
               
                return sb.ToString()[..^1];
            }
        

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}