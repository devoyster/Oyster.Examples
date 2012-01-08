using System;

namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public struct OperandWrapper<T>
    {
        private static readonly ICalculator<T> Calc = Calculator<T>.Default;
        private static readonly Func<T, T, T> AddOp = Calc.Add;
        private static readonly Func<T, T, T> SubOp = Calc.Sub;
        private static readonly Func<T, T, T> MulOp = Calc.Mul;

        public readonly T Value;

        public OperandWrapper(T value)
        {
            Value = value;
        }

        public OperandWrapper(int constant)
        {
            Value = Calc.Constant(constant);
        }

        static public implicit operator T(OperandWrapper<T> x)
        {
            return x.Value;
        }

        static public OperandWrapper<T> operator +(OperandWrapper<T> x, OperandWrapper<T> y)
        {
            return BinaryOp(AddOp, x, y);
        }

        static public OperandWrapper<T> operator -(OperandWrapper<T> x, OperandWrapper<T> y)
        {
            return BinaryOp(SubOp, x, y);
        }

        static public OperandWrapper<T> operator *(OperandWrapper<T> x, OperandWrapper<T> y)
        {
            return BinaryOp(MulOp, x, y);
        }

        static private OperandWrapper<T> BinaryOp(Func<T, T, T> op, OperandWrapper<T> x, OperandWrapper<T> y)
        {
            return new OperandWrapper<T>(op(x, y));
        }
    }
}
