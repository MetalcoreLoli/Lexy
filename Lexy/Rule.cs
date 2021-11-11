using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lexy
{
    internal static class LinqHelper
    {
        public static string CharsToString(this IEnumerable<char> chars) => chars.Aggregate("", (s, c) => s + c);
    }

    public abstract record Rule
    {

        #region PUBLIC
        public abstract ExecutionResult ExecuteOn(string context);

        public static TRule Run<TRule>() where TRule : Rule => 
            Activator.CreateInstance<TRule>();

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

            internal string Tail { get; private set; }

            public ExecutionResult Append(ExecutionResult result)
            {
                _results.Add(result);
                Tail = result.Tail;
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
        #endregion
        #region PROTECTED

        protected string Context = String.Empty;
        private int _current;

        protected char Peek() => !IsEnd? Context[0] : '\0';
        protected char PeekNext() => !IsEnd ? Context[1] : Peek();

        protected Rule Shift(int steps = 1)
        {
            Context = Context.Skip(1).CharsToString();
            return this;
        }


        protected string MoveThoughtContextWhile(string context, Func<char, bool> predicate)
        {
            Context = context;
            if (IsEnd) return String.Empty;
            string tail = String.Empty;
            char current = Peek();
            while (Context != String.Empty && predicate(current))
            {
                tail += current;
                Shift();
                current = Peek();
            }
            return tail;
        }

        protected bool IsEnd => Context == string.Empty;

        #endregion
    }
}