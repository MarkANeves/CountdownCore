using System;
using System.Collections.Generic;
using System.Text;

namespace CountdownEngine
{
    public class Solution
    {
        public readonly int[] RpnNodes;
        public readonly int RpnNodesLength;
        public readonly int Target;
        public readonly string RpnString;
        public readonly string InlineString;
        public readonly List<string> SeparateCalculations;
        public readonly int NumCalls;

        public Solution(int[] rpnNodes, int target, int numCalls)
        {
            RpnNodes = rpnNodes;
            Target = target;
            NumCalls = numCalls;

            for (int l = 0; l < RpnNodes.Length && RpnNodes[l] != Rpn.End; l++)
                RpnNodesLength = l+1;
            
            RpnString = this.ConvertToString();
            InlineString = this.ConvertToInlineString();
            SeparateCalculations = this.ConvertToSeparateCalculations();
        }
    }
}
