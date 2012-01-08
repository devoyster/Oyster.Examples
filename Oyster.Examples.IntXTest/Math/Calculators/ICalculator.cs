namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public interface ICalculator<T>
    {
        T Add(T x, T y);
        T Sub(T x, T y);
        T Mul(T x, T y);
        T Constant(int x);
    }
}
