using System.Linq;

namespace Lexy.Std
{
    public record Whitespace : Rule
    {
        public override ExecutionResult ExecuteOn(string context)
        {
            string spaces = MoveThoughtContextWhile(context, char.IsWhiteSpace);
            return spaces.Any() 
                ? new ExecutionResult(new Symbol(Symbol.Forma.Whitespace, spaces, ' '), Context)
                : null;
        }
    }
}