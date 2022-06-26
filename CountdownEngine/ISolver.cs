using System.Collections.Generic;

namespace CountdownEngine.Solvers
{
    public interface ISolver
    {
        IEnumerable<Solution> Solve(List<int> numbers, int target);
        int NumCalls();
        int NumSkipped();
    }
}