using System;

namespace CountdownEngine.Solvers
{
    public class RpnOpNode : RpnNode
    {
        public override char Op { get; }
        public override int Value => throw new NotImplementedException();

        public RpnOpNode(char op)
        {
            Op = op;
        }

        public override bool IsOp() { return true; }

        public override RpnNode Copy()
        {
            return new RpnOpNode(this.Op);
        }
    }
}