using System;

namespace Lexy
{
    internal record OrCombinatorRule : Rule
    {
        private readonly Rule _left;
        private readonly Rule _right;

        public OrCombinatorRule(Rule left, Rule right)
        {
            _left  = left;
            _right = right;
        }

        public override ExecutionResult ExecuteOn(string context)
        {
            var resultOfLeft = _left?.ExecuteOn(context);
            var resultOrRight = _right?.ExecuteOn(context);
            if (resultOfLeft != default && resultOrRight != default)
                return resultOfLeft.Append(resultOrRight);
            return resultOfLeft ?? resultOrRight;
        }
    }
}