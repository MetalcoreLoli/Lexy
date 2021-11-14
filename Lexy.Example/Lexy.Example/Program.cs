using System;
using Lexy.Std;

namespace Lexy.Example
{
    internal record BinaryRule(char Sym) : Rule
    {
        public override ExecutionResult ExecuteOn(string context) => 
            ((MaybeSomething | Number | Whitespace) & Character(Sym) & (Whitespace | Number | MaybeSomething)).ExecuteOn(context);
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
            var context = "123+ 45-5*8        / 21";
            var (res, errors) =
                Rule.TestRulesOn(context, 
                     Rule.Run<Add>(), Rule.Run<Sub>(), 
                                Rule.Run<Mul>(), Rule.Run<Div>(),
                                Rule.Word("hello"));
            Console.WriteLine($"Result:\n{res}\nErrors:");
            foreach (var error in errors)
                Console.WriteLine("  "+error); 
        }
    }
}