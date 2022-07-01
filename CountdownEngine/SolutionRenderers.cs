using System;
using System.Collections.Generic;
using System.Text;

namespace CountdownEngine
{
    public static class SolutionRenderers
    {
        public static string ConvertToInlineString(this Solution solution)
        {
            var rpnNodes = solution.GetRpnNodes();

            try
            {
                var stack = new Stack<string>(10);
                int lastOp = Rpn.End;
                foreach (int n in rpnNodes)
                {
                    if (n <= Rpn.Plus)
                    {
                        if (n == Rpn.End)
                            return stack.Peek();

                        var n1 = stack.Pop(); var n2 = stack.Pop();

                        if (lastOp == Rpn.End) lastOp = n;

                        if (lastOp != n)
                        {
                            int i;
                            if (!int.TryParse(n1, out i)) n1 = $"({n1})";
                            if (!int.TryParse(n2, out i)) n2 = $"({n2})";
                        }

                        string expr = "";
                        if (n == Rpn.Plus) { expr = $"{n2}+{n1}"; }
                        else if (n == Rpn.Minus) { expr = $"{n2}-{n1}"; }
                        else if (n == Rpn.Mul) { expr = $"{n2}*{n1}"; }
                        else if (n == Rpn.Div) { expr = $"{n2}/{n1}"; }
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
                throw new Exception($"Illegal RPN expression {solution.ConvertToString()} ({e.Message})");
            }
        }

        public static string ConvertToString(this Solution solution)
        {
            return solution.GetRpnNodes().ConvertRpnNodesToString();
        }

        public static string ConvertRpnNodesToString(this int[] rpnNodes)
        {
            string result = "";
            string ops = "+-*/?";

            foreach (var rpnNode in rpnNodes)
            {
                if (rpnNode == Rpn.End)
                    break;

                if (rpnNode < 0)
                { result += ops[Math.Abs(rpnNode) + Rpn.Plus] + " "; }
                else
                { result += rpnNode + " "; }
            }

            return result;
        }

        public static List<string> ConvertToSeparateCalculations(this Solution solution)
        {
            var rpnNodes = solution.GetRpnNodes();

            try
            {
                var calculations = new List<string>();
                
                int[] stack = new int[10];
                int sp = 0;

                foreach (int n in rpnNodes)
                {
                    if (n <= Rpn.Plus)
                    {
                        if (n == Rpn.End)
                            return calculations;

                        int n1 = stack[--sp]; int n2 = stack[--sp];
                             if (n == Rpn.Plus)  { stack[sp++] = n2 + n1; calculations.Add($"{n2}+{n1}={n2 + n1}"); }
                        else if (n == Rpn.Minus) { stack[sp++] = n2 - n1; calculations.Add($"{n2}-{n1}={n2 - n1}"); }
                        else if (n == Rpn.Mul)   { stack[sp++] = n2 * n1; calculations.Add($"{n2}*{n1}={n2 * n1}"); }
                        else if (n == Rpn.Div)   { stack[sp++] = n2 / n1; calculations.Add($"{n2}/{n1}={n2 / n1}"); }
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
                throw new Exception($"Illegal RPN expression {solution.ConvertToString()} ({e.Message})");
            }
        }
    }
}
