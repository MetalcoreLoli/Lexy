namespace Lexy.Std
{
    public record Symbol(Symbol.Forma Form, string Lexeme, object Value)
    {
        public override string ToString() => $"[{Form};\"{Lexeme}\";{Value}]";
        public enum Forma
        {
            Word,
            Num,
            Character,
            Whitespace,
        }
    }
}