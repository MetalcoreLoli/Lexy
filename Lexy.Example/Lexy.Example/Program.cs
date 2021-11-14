using System;

namespace Lexy.Example
{
    internal record BinaryRule(char Sym) : Rule
    {
        public Rule Binary => Run<Add>() | Run<Sub>() | Run<Mul>() | Run<Div>();
        public override ExecutionResult ExecuteOn(string context) => 
            ((MaybeSomething | Number | Whitespace) & Character(Sym) & (Whitespace | Number)).ExecuteOn(context);
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
            var context = "123+ 45-5*8";
            var (res, errors) = Rule.TestRulesOn(context, Rule.Run<Add>(), Rule.Run<Sub>(), Rule.Run<Mul>(), Rule.Run<Div>());
            Console.WriteLine($"Result:\n{res}\nErrors:");
            foreach (var error in errors)
                Console.WriteLine("  "+error); 
        }
    }
}