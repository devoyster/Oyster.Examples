using Oyster.Examples.IntXTest.Math.Calculators;

namespace Oyster.Examples.IntXTest.Math
{
    public static class Algorithms<T>
    {
        private static readonly ICalculator<T> Calc = Calculator<T>.Default;
        private static readonly Matrix22<T> FibOneMatrix = new Matrix22<T>(Calc.One(), Calc.One(), Calc.One(), Calc.Zero());

        public static T Pow(T x, int exp)
        {
            var res = Calc.Constant(1);
            while (exp != 0)
            {
                if ((exp & 1) == 1)
                {
                    res = Calc.Mul(res, x);
                }
                x = Calc.Mul(x, x);
                exp >>= 1;
            }
            return res;
        }

        public static T Fib(int n)
        {
            return Algorithms<Matrix22<T>>.Pow(FibOneMatrix, n).B;
        }
    }
}
