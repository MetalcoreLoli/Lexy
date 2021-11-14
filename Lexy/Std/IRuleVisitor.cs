namespace Lexy.Std
{
    public interface IRuleVisitor<out TReturn>
    {
        TReturn VisitNumber(Number number);
        TReturn VisitWhitespace(Whitespace number);
        TReturn VisitWord(Word value, IRuleContext<string> ruleContext);
        TReturn VisitCharacter(Character value, IRuleContext<char> ruleContext);
    }

    public interface IRuleVisitable
    {
        T Visit<T>(IRuleVisitor<T> visitor);
    }

}