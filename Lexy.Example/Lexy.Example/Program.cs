using System;

namespace Lexy.Example
{
    internal record BinaryRule(char Sym) : Rule
    {
        public override ExecutionResult ExecuteOn(string context) => 
            ((Number | Whitespace) & Character(Sym) & (Whitespace | Number)).ExecuteOn(context);
    }

    internal record Add : BinaryRule
    {
        public Add() : base('+')
        {
        }
    }
    internal record Sub : BinaryRule
    {
        public Sub() : base('-')
        {
        }
    }
    internal record Mul : BinaryRule
    {
        public Mul() : base('*')
        {
        }
    }
    internal record Div : BinaryRule
    {
        public Div() : base('/')
        {
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var context = "123+ 45-5*8/2";
            var rule = Rule.Run<Add>() | Rule.Run<Sub>() | Rule.Run<Div>() | Rule.Run<Mul>();
            Console.WriteLine(rule.ExecuteOn(context));
        }
    }
}