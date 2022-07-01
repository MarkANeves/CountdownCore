using System;
using System.Collections.Generic;
using System.Text;

namespace CountdownEngine
{
    public class Solution
    {
        public readonly int Target;
        public readonly List<string> SeparateCalculations;
        public readonly string InlineString;
        public readonly string RpnString;
        public readonly int RpnNodesLength;
        public readonly int Iteration;

        private readonly int[] _rpnNodes;

        public Solution(int[] rpnNodes, int target, int iteration)
        {
            _rpnNodes = rpnNodes;
            Target = target;
            Iteration = iteration;

            for (int l = 0; l < _rpnNodes.Length && _rpnNodes[l] != Rpn.End; l++)
                RpnNodesLength = l+1;
            
            RpnString = this.ConvertToString();
            InlineString = this.ConvertToInlineString();
            SeparateCalculations = this.ConvertToSeparateCalculations();
        }

        public int[] GetRpnNodes() => _rpnNodes;
    }
}
