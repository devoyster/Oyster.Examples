using System;

namespace Oyster.Examples.IntXTest.Math.Calculators
{
    public class Matrix22Calculator<T> : ICalculator<Matrix22<T>>
    {
        private static readonly Matrix22<T> Identity = new Matrix22<T>(1.Wrap<T>(), 0.Wrap<T>(), 1.Wrap<T>(), 0.Wrap<T>());

        public virtual Matrix22<T> Add(Matrix22<T> x, Matrix22<T> y)
        {
            throw new NotImplementedException();
        }

        public virtual Matrix22<T> Sub(Matrix22<T> x, Matrix22<T> y)
        {
            throw new NotImplementedException();
        }

        public virtual Matrix22<T> Mul(Matrix22<T> x, Matrix22<T> y)
        {
            return x.Mul(y);
        }

        public virtual Matrix22<T> Constant(int x)
        {
            return x == 1 ? Identity : null;
        }
    }
}
