using IntXLib;

namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public class IntXCalculator : ICalculator<IntX>
    {
        public virtual IntX Add(IntX x, IntX y)
        {
            return x + y;
        }

        public virtual IntX Sub(IntX x, IntX y)
        {
            return x - y;
        }

        public virtual IntX Mul(IntX x, IntX y)
        {
            return x * y;
        }

        public virtual IntX Constant(int x)
        {
            return x;
        }
    }
}
