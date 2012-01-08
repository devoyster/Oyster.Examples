using Oyster.Examples.IntXTest.Math.Calculators;

namespace Oyster.Examples.IntXTest.Math
{
    public class Matrix22<T>
    {
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
            var a = A.Wrap();
            var b = B.Wrap();
            var c = C.Wrap();
            var d = D.Wrap();

            var a2 = other.A.Wrap();
            var b2 = other.B.Wrap();
            var c2 = other.C.Wrap();
            var d2 = other.D.Wrap();

            var x1 = (a + d) * (a2 + d2);
            var x2 = (c + d) * a2;
            var x3 = a * (b2 - d2);
            var x4 = d * (c2 - a2);
            var x5 = (a + b) * d2;
            var x6 = (c - a) * (a2 + b2);
            var x7 = (b - d) * (c2 + d2);

            return new Matrix22<T>(
                x1 + x4 - x5 + x7,
                x3 + x5,
                x2 + x4,
                x1 - x2 + x3 + x6);
        }
    }
}
