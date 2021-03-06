namespace Lexy.Std
{
    public record Number : Rule, IRuleVisitable
    {
        public override ExecutionResult ExecuteOn(string context)
        {
            string num = MoveThoughtContextWhile(context, char.IsDigit); 
            return int.TryParse(num, out var number) 
                ? new ExecutionResult(new Symbol(Symbol.Forma.Num, num, number), Context) 
                : null;
        }
        
        public override string ToString() => $"{nameof(Number)}";
        public T Visit<T>(IRuleVisitor<T> visitor) => 
            visitor.VisitNumber(this);
    };
}