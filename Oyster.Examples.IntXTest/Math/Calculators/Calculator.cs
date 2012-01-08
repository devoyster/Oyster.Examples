using System;
using System.Numerics;
using IntXLib;

namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public static class Calculator<T>
    {
        public static readonly ICalculator<T> Default = CreateDefault();

        private static ICalculator<T>  CreateDefault()
        {
            Type type = typeof(T);

            if (type == typeof(BigInteger))
            {
                return (ICalculator<T>)new BigIntegerCalculator();
            }
            if (type == typeof(IntX))
            {
                return (ICalculator<T>)new IntXCalculator();
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Matrix22<>))
            {
                return (ICalculator<T>)Activator.CreateInstance(typeof(Matrix22Calculator<>).MakeGenericType(type.GetGenericArguments()[0]));
            }
            return null;
        }
    }
}
