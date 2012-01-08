namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public static class OperandWrapperExtensions
    {
        public static OperandWrapper<T> Wrap<T>(this T value)
        {
            return new OperandWrapper<T>(value);
        }

        public static OperandWrapper<T> Wrap<T>(this int constant)
        {
            return new OperandWrapper<T>(constant);
        }
    }
}
