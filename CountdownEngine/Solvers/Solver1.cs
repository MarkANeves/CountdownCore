using System;
using System.Collections.Generic;
using System.Linq;

namespace CountdownEngine.Solvers
{
    public class Solver1 : ISolver
    {
        private int _numCalls = 0;
        private int _numSkipped = 0;

        public SolutionResults Solve(List<int> numbers, int target)
        {
            _numCalls = 0;
            _numSkipped = 0;

            var comparer = new SolutionEqualityComparer();
            var solutions = new HashSet<Solution>(comparer);
            var permutations = Permutater.Permutate(numbers);

            foreach (var numList in permutations)
            {
                var rpnNodes = new List<RpnNode>();
                var results = Solve(rpnNodes, numList, numList.Count-1, target).ToArray();
                var c = results.Count();
                foreach (var r in results)
                {
                    var rpnInts =  ConvertToListOfRpnInts(r);
                    var solution = new Solution(rpnInts, target, _numCalls);
                    solutions.Add(solution);
                }
            }

            var solutionResults = new SolutionResults(numbers, target, _numCalls, solutions.ToList());

            return solutionResults;
        }

        private int[] ConvertToListOfRpnInts(List<RpnNode> r)
        {
            int[] result = new int[12];
            int i = 0;
            foreach (var rpnNode in r)
            {
                if (rpnNode.IsOp())
                {
                    switch (rpnNode.Op)
                    {
                        case '+': result[i++] = Rpn.Plus; break;
                        case '-': result[i++] = Rpn.Minus; break;
                        case '*': result[i++] = Rpn.Mul; break;
                        case '/': result[i++] = Rpn.Div; break;
                    }
                }
                else
                { result[i++] = rpnNode.Value; }
            }

            result[i] = Rpn.End;
            return result;
        }

        List<RpnNode> Copy(List<RpnNode> irpnNodes)
        {
            var rpnNodes = new List<RpnNode>();
            foreach (var n in irpnNodes)
                rpnNodes.Add(n.Copy());

            return rpnNodes;
        }


        private IEnumerable<List<RpnNode>> Solve(List<RpnNode> rpnNodes, List<int> numbers, int opsLeft, int target)
        {
            _numCalls++;

            if (rpnNodes.Count == 0)
            {
                rpnNodes.Add(new RpnValueNode(numbers[0]));
                rpnNodes.Add(new RpnValueNode(numbers[1]));
                numbers.RemoveAt(0);
                numbers.RemoveAt(0);
            }

            var numbersLeft = numbers.Count;
            //var s = ConvertRpnNodesToString(rpnNodes);

            if (numbersLeft < opsLeft)
            {
                foreach (var c in "+-*/")
                {
                    var newRpnNodes = Copy(rpnNodes);
                    newRpnNodes.Add(new RpnOpNode(c));
                    var results = Solve(newRpnNodes, numbers, opsLeft - 1, target);
                    foreach (var r in results)
                        yield return r;
                }

                if (numbers.Count > 0)
                {
                    var newRpnNodes = Copy(rpnNodes);
                    newRpnNodes.Add(new RpnValueNode(numbers[0]));
                    var newNumbers = new List<int>(numbers);
                    newNumbers.RemoveAt(0);
                    var results = Solve(newRpnNodes, newNumbers, opsLeft, target);
                    foreach (var r in results)
                        yield return r;
                }
            }
            else
            {
                var result = CalcRpn(rpnNodes);
                if (result >= 0)
                {
                    if (result == target)
                    {
                        yield return rpnNodes;
                    }
                    else
                    {
                        if (numbers.Count > 0)
                        {
                            var newRpnNodes = Copy(rpnNodes);
                            newRpnNodes.Add(new RpnValueNode(numbers[0]));
                            var newNumbers = new List<int>(numbers);
                            newNumbers.RemoveAt(0);
                            var results = Solve(newRpnNodes, newNumbers, opsLeft, target);
                            foreach (var r in results)
                                yield return r;
                        }
                    }
                }

            }
        }

        private int CalcRpn(List<RpnNode> rpnNodes)
        {
            try
            {
                var stack = new Stack<int>();
                foreach (var n in rpnNodes)
                {
                    if (n.IsOp())
                    {
                        int n1 = stack.Pop(); int n2 = stack.Pop();
                        switch (n.Op)
                        {
                            case '+':
                                {
                                    stack.Push(n2 + n1);
                                    break;
                                }
                            case '-':
                                {
                                    if (n2 <= n1) { return -1; }
                                    stack.Push(n2 - n1);
                                    break;
                                }
                            case '*':
                                {
                                    stack.Push(n2 * n1);
                                    break;
                                }
                            case '/':
                                {
                                    if (n2 % n1 != 0) { return -1; }
                                    stack.Push(n2 / n1);
                                    break;
                                }
                            default:
                                { throw new Exception($"Unknown op '{n.Op}'"); }
                        }
                    }
                    else
                    {
                        stack.Push(n.Value);
                    }
                }

                if (stack.Count != 1)
                {
                    throw new Exception($"Stack left with {stack.Count} items");
                }

                return stack.Peek();
            }
            catch (Exception e)
            {
                throw new Exception($"Illegal RPN expression {ConvertRpnNodesToString(rpnNodes)} ({e.Message})"); 
            }
        }

        public string ConvertRpnNodesToString(List<RpnNode> rpnNodes)
        {
            string result="";
            foreach (var rpnNode in rpnNodes)
            {
                if (rpnNode.IsOp())
                { result += rpnNode.Op + " "; }
                else
                { result += rpnNode.Value + " "; }
            }

            return result;
        }

        public int NumCalls() => _numCalls;

        public int NumSkipped() => _numSkipped;
    }
}
