using System.Linq.Expressions;

namespace Oyster.Examples.Expressions
{
    /// <summary>
    /// Converts lambda sub-expressions to constants (if possible).
    /// </summary>
    public static class VariablesReductionExtensions
    {
        /// <summary>
        /// Recursively reduces variables in given lambda expression.
        /// </summary>
        /// <param name="expression">Expression to visit.</param>
        /// <returns>Expression with reduced variables.</returns>
        public static LambdaExpression ReduceVariables(this LambdaExpression expression)
        {
            if (expression == null) return null;
            return (LambdaExpression)new VariablesReductionVisitor().Visit(expression);
        }

        /// <summary>
        /// Recursively reduces variables in given lambda expression.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="expression">Expression to visit.</param>
        /// <returns>Expression with reduced variables.</returns>
        public static Expression<T> ReduceVariables<T>(this Expression<T> expression)
        {
            return (Expression<T>)ReduceVariables((LambdaExpression)expression);
        }
    }
}
