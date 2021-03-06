using System;
using Lexy;

namespace Lexy.Example;
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

internal class Program
{
    private static void Main(string[] args)
    {
        ExampleOne();
        ExampleTwo();
    }

    private static void ExampleTwo()
    {
        var context = "hello world";
        var rule = Rule.Word("hello") & Rule.Whitespace & Rule.Word("world");
        Console.WriteLine(rule.ExecuteOn(context));
    }

    private static void ExampleOne()
    {
        var context = "123+ 45-5*8        / 21";
        var (res, errors) =
            Rule.TestRulesOn(context,
                Rule.Run<Add>(), Rule.Run<Sub>(),
                Rule.Run<Mul>(), Rule.Run<Div>(),
                Rule.Word("hello"));
        Console.WriteLine($"Result:\n{res}\nErrors:");
        foreach (var error in errors)
            Console.WriteLine("  " + error);
    }
}