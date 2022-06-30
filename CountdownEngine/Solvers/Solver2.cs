using System;
using System.Collections.Generic;

namespace CountdownEngine.Solvers
{
    public class Solver2 : ISolver
    {
        public static int _numCalls = 0;
        public static int _numSkipped = 0;

        public const int Plus = -1;
        public const int Minus = -2;
        public const int Mul = -3;
        public const int Div = -4;
        public const int End = -5;

        public const int MaxRpnNodes = 12;

        public static readonly int[] OpCodes = { Plus, Minus, Mul, Div };

        public SolutionResults Solve(List<int> numbers, int target)
        {
            _numCalls = 0;
            _numSkipped = 0;

            var solutions = new List<Solution>();
            var permutations = Permutater.Permutate(numbers);

            foreach (var numList in permutations)
            {
                var numArry = numList.ToArray();

                var rpnNodes = new int[MaxRpnNodes]; for (int i = 0; i < rpnNodes.Length; i++) { rpnNodes[i] = End; }
                var results = Solve(rpnNodes, numArry, numArry.Length - 1, 0, 0, target);
                foreach (var r in results)
                {
                    solutions.Add(r);
                }
            }

            var solutionResults = new SolutionResults(numbers, target, solutions);

            return solutionResults;
        }

        private IEnumerable<Solution> Solve(int[] rpnNodes, int[] numbers, int opsLeft, int nextNum, int nextRpnNode, int target)
        {
            _numCalls++;

            if (nextNum == 0)
            {
                rpnNodes[nextRpnNode++] = numbers[nextNum++];
                rpnNodes[nextRpnNode++] = numbers[nextNum++];
            }

            var numbersLeft = numbers.Length - nextNum;
            //var s = ConvertRpnNodesToString(rpnNodes);

            if (numbersLeft < opsLeft)
            {
                foreach (int o in OpCodes)
                {
                    var newRpnNodes = new int[MaxRpnNodes];
                    Array.Copy(rpnNodes, newRpnNodes, newRpnNodes.Length);
                    newRpnNodes[nextRpnNode] = o;

                    var i = nextRpnNode;
                    if ((o == Plus || o == Mul) && rpnNodes[i - 1] > 0 && rpnNodes[i - 2] > 0 && rpnNodes[i - 1] < rpnNodes[i - 2])
                    {
                        _numSkipped++;
                        continue;
                    }

                    var results = Solve(newRpnNodes, numbers, opsLeft - 1, nextNum, nextRpnNode+1, target);
                    foreach (var r in results)
                        yield return r;
                }

                if (numbersLeft > 0)
                {
                    var newRpnNodes = new int[MaxRpnNodes];
                    Array.Copy(rpnNodes, newRpnNodes, newRpnNodes.Length);
                    newRpnNodes[nextRpnNode] = numbers[nextNum];
                    var results = Solve(newRpnNodes, numbers, opsLeft, nextNum+1, nextRpnNode+1, target);
                    foreach (var r in results)
                        yield return r;
                }
            }
            else
            {
                var result = CalcRpn(rpnNodes);
                //var t = ConvertRpnNodesToString(rpnNodes);
                if (result >= 0)
                {
                    if (result == target)
                    {
                        yield return new Solution(rpnNodes,target, _numCalls);
                    }
                    else
                    {
                        if (numbersLeft > 0)
                        {
                            var newRpnNodes = new int[MaxRpnNodes];
                            Array.Copy(rpnNodes, newRpnNodes, newRpnNodes.Length);
                            newRpnNodes[nextRpnNode] = numbers[nextNum];
                            var results = Solve(newRpnNodes, numbers, opsLeft, nextNum+1, nextRpnNode+1, target);
                            foreach (var r in results)
                                yield return r;
                        }
                    }
                }
            }
        }
        /*




        */

        private int CalcRpn(int[] rpnNodes)
        {
            try
            {
                int[] stack = new int[10];
                int sp = 0;

                for (int i=2;i<rpnNodes.Length;i++)
                {
                    if ((rpnNodes[i] == Plus || rpnNodes[i] == Mul) && rpnNodes[i - 1] > 0 && rpnNodes[i - 2] > 0 && rpnNodes[i - 1] < rpnNodes[i - 2])
                        throw new Exception("Erm");
                }

                foreach (int n in rpnNodes)
                {
                    if (n <= Plus)
                    {
                        if (n == End)
                            return stack[0];

                        int n1 = stack[--sp]; int n2 = stack[--sp];
                        if (n == Plus) { stack[sp++] = n2 + n1; }
                        else if (n == Minus) { if (n2 <= n1) return -1; stack[sp++] = n2 - n1; }
                        else if (n == Mul) { stack[sp++] = n2 * n1; }
                        else if (n == Div) { if (n2 % n1 != 0) return -1; stack[sp++] = n2 / n1; }
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
                throw new Exception($"Illegal RPN expression {ConvertRpnNodesToString(rpnNodes)} ({e.Message})");
            }
        }

        public string ConvertRpnNodesToString(int[] rpnNodes)
        {
            string result = "";
            string ops = "+-*/?";

            foreach (var rpnNode in rpnNodes)
            {
                if (rpnNode == End)
                    break;

                if (rpnNode < 0)
                { result += ops[Math.Abs(rpnNode) + Plus] + " "; }
                else
                { result += rpnNode + " "; }
            }

            return result;
        }

        public string ConvertRpnNodesToInlineString(int[] rpnNodes)
        {
            try
            {
                var stack = new Stack<string>(10);
                int lastOp = End;
                foreach (int n in rpnNodes)
                {
                    if (n <= Plus)
                    {
                        if (n == End)
                            return stack.Peek();

                        var n1 = stack.Pop(); var n2 = stack.Pop();

                        if (lastOp == End) lastOp = n;

                        if (lastOp != n) 
                        {
                            int i;
                            if (!int.TryParse(n1, out i)) n1 = $"({n1})";
                            if (!int.TryParse(n2, out i)) n2 = $"({n2})";
                        }

                        string expr = "";
                        if (n == Plus)       { expr = $"{n2}+{n1}"; }
                        else if (n == Minus) { expr = $"{n2}-{n1}"; }
                        else if (n == Mul)   { expr = $"{n2}*{n1}"; }
                        else if (n == Div)   { expr = $"{n2}/{n1}"; }
                        else { throw new Exception($"Unknown op '{n}'"); }

                        stack.Push(expr);
                        lastOp = n;
                    }
                    else
                        stack.Push(n.ToString());
                }

                if (stack.Count != 1)
                {
                    throw new Exception($"Stack left with {stack.Count} items");
                }

                throw new Exception("No RPN end value found");
            }
            catch (Exception e)
            {
                throw new Exception($"Illegal RPN expression {ConvertRpnNodesToString(rpnNodes)} ({e.Message})");
            }
        }

        public int NumCalls() => _numCalls;

        public int NumSkipped() => _numSkipped;
    }
}
