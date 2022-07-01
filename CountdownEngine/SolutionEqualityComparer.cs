using System.Collections.Generic;
using System.Linq;

namespace CountdownEngine
{
    public class SolutionEqualityComparer : IEqualityComparer<Solution>
    {
        public bool Equals(Solution s1, Solution s2)
        {
            var r = s1.InlineString == s2.InlineString ||
                    s1.SeparateCalculations.SequenceEqual(s2.SeparateCalculations);
            return r;
        }

        public int GetHashCode(Solution s)
        {
            int h = 1;
            foreach (int i in s.GetRpnNodes())
                h += i * 31;

            return h;
        }
    }
}