namespace Lexy.Std
{
    public record Character: Rule
    {
        private readonly IRuleContext<char> _ruleContext;

        public Character (IRuleContext<char> ruleContext)
        {
            _ruleContext = ruleContext;
        }

        public override ExecutionResult ExecuteOn(string context)
        {
            var @char = MoveThoughtContextWhile(context, c => c == _ruleContext.Head);
            return @char[0] == _ruleContext.Head
                ? new ExecutionResult(new Symbol(Symbol.Forma.Character, @char, _ruleContext.Head), Context)
                : null;
        }
    }
}