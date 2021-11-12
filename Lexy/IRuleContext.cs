namespace Lexy
{
    public interface IRuleContext<out T>
    {
        T Head { get; }
        string Tail { get; set; }
    }

    public class RuleContextImpl<T> : IRuleContext<T>
    {
        public RuleContextImpl(T head)
        {
            Head = head;
        }

        public T Head { get; }
        public string Tail { get; set; }
    }
}