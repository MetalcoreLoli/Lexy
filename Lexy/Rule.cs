using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lexy.Std;

namespace Lexy
{
    internal static class LinqHelper
    {
        public static string CharsToString(this IEnumerable<char> chars) => chars.Aggregate("", (s, c) => s + c);
    }

    public abstract record Rule
    {
        #region PUBLIC

        public Rule()
        {
        }


        public abstract ExecutionResult ExecuteOn(string context);

        public virtual (ExecutionResult head, string error) SaveExecuteOn(string context)
        {
            try
            {
                return (ExecuteOn(context), "");
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public static TRule Run<TRule>() where TRule : Rule => 
            Activator.CreateInstance<TRule>();

        public static TRule Run<TRule, T>(IRuleContext<T> context) where TRule : Rule => 
            (TRule) Activator.CreateInstance(typeof(TRule), context);

        public static Rule Whitespace => new Whitespace();
        public static Rule Number => new Number();
        public static Rule MaybeSomething => new MaybeSomething();
        public static Rule Character(IRuleContext<char> ruleContext)=> new Character(ruleContext);
        public static Rule Character(char value)=> new Character(NewContextOf(value));
        public static Rule Word(IRuleContext<string> ruleContext) => new Word(ruleContext);
        public static Rule Word(string value) => new Word(NewContextOf(value));
        
        public static IRuleContext<T> NewContextOf<T>(T head) => new RuleContextImpl<T>(head);

        public static (ExecutionResult Result, IEnumerable<string> Errors) TestRulesOn(string context,
            params Rule[] rules)
        {
            string cxt = context;
            ExecutionResult result = new ExecutionResult("", context);
            var errors = new List<string>();
            foreach (var rule in rules)
            {
                var res = rule.SaveExecuteOn(result.Tail);
                if (res.error == String.Empty)
                {
                    result.Append(res.head);
                }
                else
                {
                    errors.Add(res.error);
                }
            }
            return (result, errors);
        }

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
                _results.Add(this);
                Tail = tail;
            }

            internal string Tail { get; private set; }

            public ExecutionResult Append(ExecutionResult result)
            {
                //if (result is MaybeSomethingResult) return this;
                _results.AddRange(result);
                Tail = result.Tail;
                result._results.Clear();
                return this;
            }

            public T As<T>() => (T) _value;

            public IEnumerable<T> AllAsOrDefault<T>() => 
                _results.Count > 0 ? _results.Select(r => r.As<T>()) : default;

            public IEnumerator<ExecutionResult> GetEnumerator() => _results.GetEnumerator();

            public override string ToString()
            {
                var sb = new StringBuilder();
                foreach (var result in _results)
                    sb.Append($"{result._value} ");
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

    public record MaybeSomething : Rule
    {
        public override ExecutionResult ExecuteOn(string context)
        {
            Context = context;
            return new MaybeSomethingResult(context);
        }

    }

    public record MaybeSomethingResult : Rule.ExecutionResult
    {
        public MaybeSomethingResult(string context) : base("", context)
        {
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}