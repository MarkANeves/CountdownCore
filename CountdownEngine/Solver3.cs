using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CountdownEngine.Solver3
{
    public class Solver
    {
        public static int numCalls = 0;
        public static int numSkipped = 0;

        public IEnumerable<Solution> Solve(List<int> numbers, int target)
        {
            var solutions = new List<Solution>();
            var permutations = Permutater.Permutate(numbers);

            numCalls = 0;
            numSkipped = 0;
            foreach (var numList in permutations)
            {
                var numArry = numList.ToArray();
                var rpnNodes = new int[Rpn.MaxRpnNodes]; for (int i = 0; i < rpnNodes.Length; i++) { rpnNodes[i] = Rpn.End; }

                Solve(rpnNodes, numArry, numArry.Length - 1, 0, 0, target, solutions);
            }

            foreach (var r in solutions)
            {
                r.RpnString = r.RpnNodes.ConvertRpnNodesToString();
                r.InlineString = r.RpnNodes.ConvertRpnNodesToInlineString();
            }

            return solutions;
        }

        private void Solve(int[] rpnNodes, int[] numbers, int opsLeft, int nextNum, int nextRpnNode, int target, List<Solution> solutions)
        {
            numCalls++;

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
                    var newRpnNodes = new int[Rpn.MaxRpnNodes];
                    Array.Copy(rpnNodes, newRpnNodes, newRpnNodes.Length);
                    newRpnNodes[nextRpnNode] = o;

                    var i = nextRpnNode;
                    if ((o == Rpn.Plus || o == Rpn.Mul) && rpnNodes[i - 1] > 0 && rpnNodes[i - 2] > 0 && rpnNodes[i - 1] < rpnNodes[i - 2])
                    {
                        numSkipped++;
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
                solutions.Add(new Solution(rpnNodes, target));
            }
            else if (numbersLeft > 0)
            {
                var newRpnNodes = new int[Rpn.MaxRpnNodes];
                Array.Copy(rpnNodes, newRpnNodes, newRpnNodes.Length);
                newRpnNodes[nextRpnNode] = numbers[nextNum];
                Solve(newRpnNodes, numbers, opsLeft, nextNum + 1, nextRpnNode + 1, target, solutions);
            }
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
                        if (n == Rpn.Plus) { stack[sp++] = n2 + n1; }
                        else if (n == Rpn.Minus) { if (n2 <= n1) return -1; stack[sp++] = n2 - n1; }
                        else if (n == Rpn.Mul) { stack[sp++] = n2 * n1; }
                        else if (n == Rpn.Div) { if (n2 % n1 != 0) return -1; stack[sp++] = n2 / n1; }
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
    }
}
