using System.Collections.Generic;

namespace CountdownEngine.Solver
{
    public interface ISolver
    {
        IEnumerable<Solution> Solve(List<int> numbers, int target);
    }
}