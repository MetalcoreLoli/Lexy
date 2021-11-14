using System.Linq;

namespace Lexy.Std
{
    public record Whitespace : Rule, IRuleVisitable
    {
        public override ExecutionResult ExecuteOn(string context)
        {
            string spaces = MoveThoughtContextWhile(context, char.IsWhiteSpace);
            return spaces.Any() 
                ? new ExecutionResult(new Symbol(Symbol.Forma.Whitespace, spaces, ' '), Context)
                : null;
        }

        public override string ToString() => $"{nameof(Whitespace)}";
        public T Visit<T>(IRuleVisitor<T> visitor) => 
            visitor.VisitWhitespace(this);
    }
}