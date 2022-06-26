using System;

namespace CountdownEngine.Solvers
{
    public class RpnValueNode : RpnNode
    {
        public override int Value { get; }

        public override char Op => throw new NotImplementedException();

        public RpnValueNode(int value)
        {
            Value = value;
        }

        public override bool IsOp() { return false; }

        public override RpnNode Copy()
        {
            return new RpnValueNode(this.Value);
        }
    }
}