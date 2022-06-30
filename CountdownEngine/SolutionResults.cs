using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountdownEngine
{
    public class SolutionResults
    {
        public readonly List<int> Numbers;
        public readonly int Target;
        public readonly int NumSolutions;
        public readonly List<Solution> Solutions;

        public SolutionResults(List<int> numbers, int target, List<Solution> solutions)
        {
            Numbers = numbers;
            Target = target;
            Solutions = solutions.OrderBy(x=>x.RpnNodesLength).ToList();
            NumSolutions = solutions.Count();
        }
    }
}
