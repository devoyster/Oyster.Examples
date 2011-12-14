using System;
using System.Linq.Expressions;
using NUnit.Framework;
using SharpTestsEx;

namespace Oyster.Examples.Expressions.Test
{
    [TestFixture]
    public class VariablesReductionExtensionsTest
    {
        private static readonly int StaticValue = 4;

        [Test]
        public void ReduceVariables()
        {
            int value = 4;
            Func<int> getValue = () => value;

            Expression<Func<string, string>> expr1 = s => s.Length > 4 ? s.Substring(0, 4) : s;
            Expression<Func<string, string>> expr2 = s => s.Length > StaticValue ? s.Substring(0, StaticValue) : s;
            Expression<Func<string, string>> expr3 = s => s.Length > value ? s.Substring(0, value) : s;
            Expression<Func<string, string>> expr4 = s => s.Length > getValue() ? s.Substring(0, getValue()) : s;

            var reduced1 = expr1.ReduceVariables();
            var reduced2 = expr2.ReduceVariables();
            var reduced3 = expr3.ReduceVariables();
            var reduced4 = expr4.ReduceVariables();

            Action<Expression<Func<string, string>>, Expression<Func<string, string>>> checkEqual =
                (e1, e2) => e1.ToString().Should().Be(e2.ToString());
            checkEqual(reduced1, reduced2);
            checkEqual(reduced2, reduced3);
            checkEqual(reduced3, reduced4);

            value = 1;
            Action<Expression<Func<string, string>>> checkScope = e => e.Compile()("123456").Length.Should().Be(4);
            checkScope(reduced1);
            checkScope(reduced2);
            checkScope(reduced3);
            checkScope(reduced4);
        }
    }
}
