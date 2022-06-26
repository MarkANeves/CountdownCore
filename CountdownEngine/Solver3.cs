using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownEngine.Solvers
{
    public class Solver3 : ISolver
    {
        private int _numCalls;
        private int _numSkipped;
        private readonly object _lockObject = new object();

        public IEnumerable<Solution> Solve(List<int> numbers, int target)
        {
            var comparer = new SolutionEqualityComparer();
            var solutions = new HashSet<Solution>(comparer);
            var permutations = Permutater.Permutate(numbers);

            _numCalls = 0;
            _numSkipped = 0;

            Solve(permutations, solutions, target);
            //SolveParallel(permutations, solutions, target);
            
            foreach (var s in solutions)
            {
                s.RpnString = s.RpnNodes.ConvertRpnNodesToString();
                s.InlineString = s.RpnNodes.ConvertRpnNodesToInlineString();
                s.SeparateCalculationsString = s.RpnNodes.ConvertRpnNodesToSeparateCalculations()+s.CallNum+"<br>";
            }

            return solutions;
        }



        void Solve(IEnumerable<List<int>> permutations, ICollection<Solution> solutions, int target)
        {
            foreach (var numList in permutations)
            {
                var numbers = numList.ToArray();
                var rpnNodes = NewRpnANodesArray();
                Solve(rpnNodes, numbers, numbers.Length - 1, 0, 0, target, solutions);
            }
        }

        void SolveParallel(IEnumerable<List<int>> permutations, ICollection<Solution> solutions, int target)
        {
            var tasks = new List<Task>();
            foreach (var numList in permutations)
            {
                var numbers = numList.ToArray();
                var rpnNodes = NewRpnANodesArray();
                tasks.Add(Task.Run(() => Solve(rpnNodes, numbers, numbers.Length - 1, 0, 0, target, solutions)));
            }

            Task.WhenAll(tasks).Wait();
        }

        int[] NewRpnANodesArray()
        {
            var rpnNodes = new int[Rpn.MaxRpnNodes]; for (int i = 0; i < rpnNodes.Length; i++) { rpnNodes[i] = Rpn.End; }
            return rpnNodes;
        }

        private void Solve(int[] rpnNodes, int[] numbers, int opsLeft, int nextNum, int nextRpnNode, int target, ICollection<Solution> solutions)
        {
            int callNum = Interlocked.Increment(ref _numCalls);

            //if (solutions.Count > 0) return;

            if (nextNum == 0)
            {
                rpnNodes[nextRpnNode++] = numbers[nextNum++];
                rpnNodes[nextRpnNode++] = numbers[nextNum++];
            }

            var numbersLeft = numbers.Length - nextNum;
            //var s = ConvertRpnNodesToString(rpnNodes);

            var result = Rpn.End;

            if (numbersLeft < opsLeft)
            {
                foreach (int o in Rpn.OpCodes)
                {
                    var newRpnNodes = Copy(rpnNodes);
                    newRpnNodes[nextRpnNode] = o;

                    var i = nextRpnNode;
                    if ((o == Rpn.Plus || o == Rpn.Mul) && rpnNodes[i - 1] > 0 && rpnNodes[i - 2] > 0 && rpnNodes[i - 1] < rpnNodes[i - 2])
                    {
                        Interlocked.Increment(ref _numSkipped);
                        continue;
                    }

                    Solve(newRpnNodes, numbers, opsLeft - 1, nextNum, nextRpnNode + 1, target, solutions);
                }
            }
            else
            {
                result = CalcRpn(rpnNodes);
                if (result <= 0)
                    return;
            }

            //var t = ConvertRpnNodesToString(rpnNodes);
            if (result == target)
            {
                lock (_lockObject)
                    solutions.Add(new Solution(rpnNodes, target, callNum));
            }
            else if (numbersLeft > 0)
            {
                var newRpnNodes = Copy(rpnNodes);
                newRpnNodes[nextRpnNode] = numbers[nextNum];
                Solve(newRpnNodes, numbers, opsLeft, nextNum + 1, nextRpnNode + 1, target, solutions);
            }
        }

        private int[] Copy(int[] rpnNodes)
        {
            var newRpnNodes = new int[rpnNodes.Length];
            Array.Copy(rpnNodes, newRpnNodes, rpnNodes.Length);
            return newRpnNodes;
        }

        private int CalcRpn(int[] rpnNodes)
        {
            try
            {
                int[] stack = new int[10];
                int sp = 0;

                foreach (int n in rpnNodes)
                {
                    if (n <= Rpn.Plus)
                    {
                        if (n == Rpn.End)
                            return stack[0];

                        int n1 = stack[--sp]; int n2 = stack[--sp];
                        if (n == Rpn.Plus) { if (n2 > n1) return -1; stack[sp++] = n2 + n1; }
                        else if (n == Rpn.Minus) { if (n2 <= n1) return -1; stack[sp++] = n2 - n1; }
                        else if (n == Rpn.Mul) { if (n2 > n1) return -1;  if (n1 == 1 || n2 == 1) return -1; stack[sp++] = n2 * n1; }
                        else if (n == Rpn.Div) { if (n2 % n1 != 0 || n1 == 1) return -1; stack[sp++] = n2 / n1; }
                        else { throw new Exception($"Unknown op '{n}'"); }
                    }
                    else
                        stack[sp++] = n;
                }

                if (sp != 0)
                {
                    throw new Exception($"Stack left with {sp} items");
                }

                throw new Exception("No RPN end value found");
            }
            catch (Exception e)
            {
                throw new Exception($"Illegal RPN expression {rpnNodes.ConvertRpnNodesToString()} ({e.Message})");
            }
        }

        public int NumCalls() => _numCalls;

        public int NumSkipped() => _numSkipped;
    }
}
