using System;
using System.Collections.Generic;
using System.Text;

namespace CountdownEngine
{
    public class Solution
    {
        public int[] RpnNodes;
        public int Target;
        public string RpnString;
        public string InlineString;

        public Solution(int[] rpnNodes, int target)
        {
            RpnNodes = rpnNodes;
            Target = target;
        }
    }
}
