using Oyster.Examples.IntXTest.Math.Calculators;

namespace Oyster.Examples.IntXTest.Math
{
    public class Matrix22<T>
    {
        private static readonly ICalculator<T> Calc = Calculator<T>.Default;

        public readonly T A;
        public readonly T B;
        public readonly T C;
        public readonly T D;

        public Matrix22(T a, T b, T c, T d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public Matrix22<T> Mul(Matrix22<T> other)
        {
            var a2 = other.A;
            var b2 = other.B;
            var c2 = other.C;
            var d2 = other.D;

            var x1 = Calc.Mul(Calc.Add(A, D), Calc.Add(a2, d2));
            var x2 = Calc.Mul(Calc.Add(C, D), a2);
            var x3 = Calc.Mul(A, Calc.Sub(b2, d2));
            var x4 = Calc.Mul(D, Calc.Sub(c2, a2));
            var x5 = Calc.Mul(Calc.Add(A, B), d2);
            var x6 = Calc.Mul(Calc.Sub(C, A), Calc.Add(a2, b2));
            var x7 = Calc.Mul(Calc.Sub(B, D), Calc.Add(c2, d2));

            return new Matrix22<T>(
                Calc.Add(Calc.Sub(Calc.Add(x1, x4), x5), x7),
                Calc.Add(x3, x5),
                Calc.Add(x2, x4),
                Calc.Add(Calc.Add(Calc.Sub(x1, x2), x3), x6));
        }
    }
}
