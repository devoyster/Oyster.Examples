namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public static class CalculatorExtensions
    {
        public static T Zero<T>(this ICalculator<T> calc)
        {
            return calc.Constant(0);
        }

        public static T One<T>(this ICalculator<T> calc)
        {
            return calc.Constant(1);
        }
    }
}
