using System;

namespace Lexy.Std
{
    public record Word : Rule, IRuleVisitable
    {
        private readonly IRuleContext<string> _ruleContext;

        public  Word (IRuleContext<string> ruleContext)
        {
            _ruleContext = ruleContext;
        }

        public override ExecutionResult ExecuteOn(string context)
        {
            string word = MoveThoughtContextWhile(context, char.IsLetter); 
            return word == _ruleContext.Head 
                ? new ExecutionResult(new Symbol(Symbol.Forma.Word, word, word), Context) 
                : null;
        }
        public override string ToString() => $"{nameof(Word)}(\"{_ruleContext.Head}\")";
        public T Visit<T>(IRuleVisitor<T> visitor) => 
            visitor.VisitWord(this, _ruleContext);
    }
}