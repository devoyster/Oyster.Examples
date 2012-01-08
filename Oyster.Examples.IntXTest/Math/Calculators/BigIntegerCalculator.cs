using System.Numerics;

namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public class BigIntegerCalculator : ICalculator<BigInteger>
    {
        public virtual BigInteger Add(BigInteger x, BigInteger y)
        {
            return x + y;
        }

        public virtual BigInteger Sub(BigInteger x, BigInteger y)
        {
            return x - y;
        }

        public virtual BigInteger Mul(BigInteger x, BigInteger y)
        {
            return x * y;
        }

        public virtual BigInteger Constant(int x)
        {
            return x;
        }
    }
}
