using System;
using System.Collections.Generic;
using System.Text;

namespace CountdownEngine
{
    public static class Rpn
    {
        public const int Plus = -1;
        public const int Minus = -2;
        public const int Mul = -3;
        public const int Div = -4;
        public const int End = -5;

        public const int MaxRpnNodes = 12;
        public static readonly int[] OpCodes = { Plus, Minus, Mul, Div };
    }
}
