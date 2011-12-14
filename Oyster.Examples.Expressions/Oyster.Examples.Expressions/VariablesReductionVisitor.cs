using System.Collections.Generic;
using System.Linq.Expressions;

namespace Oyster.Examples.Expressions
{
    /// <summary>
    /// Converts sub-expressions to constants (if possible).
    /// </summary>
    public class VariablesReductionVisitor : ExpressionVisitor
    {
        private List<Expression> _children;

        /// <summary>
        /// Recursively reduces variables in given expression.
        /// </summary>
        /// <param name="node">Expression to visit.</param>
        /// <returns>Expression with reduced variables.</returns>
        public override Expression Visit(Expression node)
        {
            // Preserve parent's child expressions collection
            var siblings = _children;
            _children = new List<Expression>();

            node = base.Visit(node);

            // If all the child expressions are constants then current one should be constant as well
            if (_children.Count > 0 && _children.TrueForAll(e => e == null || e.NodeType == ExpressionType.Constant))
            {
                // Evaluate expression locally
                node = Expression.Constant(Expression.Lambda(node).Compile().DynamicInvoke(), node.Type);
            }

            // Add to parent's child expressions collection
            if (siblings != null)
            {
                siblings.Add(node);
            }
            _children = siblings;

            return node;
        }
    }
}
