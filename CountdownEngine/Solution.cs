using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountdownEngine
{
    public class Solution
    {
        public int[] RpnNodes;
        public int Target;
        public string RpnString;
        public string InlineString;
        public string SeparateCalculationsString;
        public int CallNum;

        public Solution(int[] rpnNodes, int target, int callNum)
        {
            RpnNodes = rpnNodes;
            Target = target;
            CallNum = callNum;
        }
     }

    public class SolutionEqualityComparer : IEqualityComparer<Solution>
    {
        public bool Equals(Solution s1, Solution s2)
        {
            var r = s1.RpnNodes.SequenceEqual(s2.RpnNodes);
            return r;
        }

        public int GetHashCode(Solution s)
        {
            int h = 1;
            foreach (int i in s.RpnNodes)
                h += i * 31;

            return h;
        }
    }
}
