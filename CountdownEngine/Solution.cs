using System;
using System.Text;

namespace CountdownEngine
{
    public class Solution
    {
        public readonly int[] RpnNodes;
        public readonly int Target;
        public readonly string RpnString;
        public readonly string InlineString;
        public readonly string SeparateCalculationsString;
        public readonly int NumCalls;

        public Solution(int[] rpnNodes, int target, int numCalls)
        {
            RpnNodes = rpnNodes;
            Target = target;
            NumCalls = numCalls;
            
            RpnString = this.ConvertToString();
            InlineString = this.ConvertToInlineString();
            SeparateCalculationsString = this.ConvertToSeparateCalculations();
        }
    }
}
