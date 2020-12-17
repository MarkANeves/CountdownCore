using System;
using System.Collections.Generic;
using System.Text;

namespace CountdownEngine
{
    public static class SolutionRenderers
    {
        public static string ConvertRpnNodesToInlineString(this int[] rpnNodes)
        {
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
                throw new Exception($"Illegal RPN expression {rpnNodes.ConvertRpnNodesToString()} ({e.Message})");
            }
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
    }
}
