using System;

namespace Lexy.Std
{
    public record Word : Rule
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
    }
}