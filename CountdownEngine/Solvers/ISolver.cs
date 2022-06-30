using System.Collections.Generic;

namespace CountdownEngine.Solvers
{
    public interface ISolver
    {
        SolutionResults Solve(List<int> numbers, int target);
        int NumCalls();
        int NumSkipped();
    }
}