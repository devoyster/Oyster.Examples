using Oyster.Examples.IntXTest.Math.Calculators;

namespace Oyster.Examples.IntXTest.Math
{
    public static class Algorithms<T>
    {
        private static readonly Matrix22<T> FibOneMatrix = new Matrix22<T>(1.Wrap<T>(), 1.Wrap<T>(), 1.Wrap<T>(), 0.Wrap<T>());

        public static T Pow(T x, int exp)
        {
            var xw = x.Wrap();
            var res = 1.Wrap<T>();
            while (exp != 0)
            {
                if ((exp & 1) == 1)
                {
                    res *= xw;
                }
                xw *= xw;
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
