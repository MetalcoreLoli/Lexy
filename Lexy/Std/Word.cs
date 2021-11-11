using System;

namespace Lexy.Std
{
    public record Word : Rule
    {
        public override ExecutionResult ExecuteOn(string context)
        {
            string word = MoveThoughtContextWhile(context, char.IsLetter); 
            return new ExecutionResult(new Symbol(Symbol.Forma.Word, word, word), Context);
        }
    }
}