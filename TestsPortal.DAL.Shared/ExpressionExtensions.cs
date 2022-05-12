using System.Linq.Expressions;

namespace TestsDelivery.DAL.Shared
{
    // According to answer https://stackoverflow.com/questions/69467586/linq-expressions-can-not-be-translated-to-sql-when-combining-specifications-in-s
    // We need to replace parameter in second expression when doing .AndAlso()
    public static partial class ExpressionExtensions
    {
        public static Expression ReplaceParameter(this Expression target, ParameterExpression parameter, Expression value)
            => new ParameterReplacer { Parameter = parameter, Value = value }.Visit(target);

        class ParameterReplacer : ExpressionVisitor
        {
            public ParameterExpression Parameter;
            public Expression Value;
            protected override Expression VisitParameter(ParameterExpression node)
                => node == Parameter ? Value : node;
        }
    }
}
