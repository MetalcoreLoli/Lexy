namespace Lexy
{
    public record AndCombinatorRule : Rule
    {
        private readonly Rule _left;
        private readonly Rule _right;

        public AndCombinatorRule(Rule left, Rule right)
        {
            _left = left;
            _right = right;
        }

        public override ExecutionResult ExecuteOn(string context)
        {
            var resultOfLeft = _left.ExecuteOn(context) ?? throw new RuleExecutionException($"{_left.GetType().Name}", context);
            var resultOfRight = _right.ExecuteOn(context) ?? throw new RuleExecutionException($"{_right.GetType().Name}", context);

            return resultOfLeft.Append(resultOfRight);
        }
    }
}