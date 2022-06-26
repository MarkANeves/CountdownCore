namespace CountdownEngine.Solvers
{
    public abstract class RpnNode
    {
        public abstract bool IsOp();
        public abstract int Value { get; }
        public abstract char Op { get; }
        public abstract RpnNode Copy();
    }
}