namespace BasicSettings.Essentials.PredicateBuilder
{
    public static class PredicateBuilder
    {
        public static Expression<Func<X, Y>> Compose<X, Y, Z>(this Expression<Func<Z, Y>> outer, Expression<Func<X, Z>> inner)
        {
            return Expression.Lambda<Func<X, Y>>(
                SubstExpressionVisitor.Replace(outer.Body, outer.Parameters[0], inner.Body),
                inner.Parameters[0]);
        }

        public static Expression<Func<T, bool>> Begin<T>(bool value = false)
        {
            if (value)
            {
                return parameter => true; //value cannot be used in place of true/false
            }

            return parameter => false;
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            // return CombineLambdas(left, right, ExpressionType.AndAlso);
            if (b == null)
            {
                return a;
            }

            ParameterExpression p = a.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = p;

            Expression body = Expression.AndAlso(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            //   return CombineLambdas(left, right, ExpressionType.OrElse);

            if (b == null)
            {
                return a;
            }

            ParameterExpression p = a.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = p;

            Expression body = Expression.OrElse(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        private static bool IsExpressionBodyConstant<T>(Expression<Func<T, bool>> left)
        {
            return left.Body.NodeType == ExpressionType.Constant;
        }

        private static Expression<Func<T, bool>> CombineLambdas<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right, ExpressionType expressionType)
        {
            //Remove expressions created with Begin<T>()
            if (IsExpressionBodyConstant(left))
            {
                return (right);
            }

            ParameterExpression p = left.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[right.Parameters[0]] = p;

            Expression body = Expression.MakeBinary(expressionType, left.Body, visitor.Visit(right.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        internal class SubstExpressionVisitor : ExpressionVisitor
        {
            public Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();
            private ParameterExpression _parameter;
            private Expression _replacement;

            public SubstExpressionVisitor()
            {
            }

            public SubstExpressionVisitor(ParameterExpression parameter, Expression replacement)
            {
                _parameter = parameter;
                _replacement = replacement;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return subst.TryGetValue(node, out var newValue) ? newValue : node;
            }

            public static Expression Replace(Expression expression, ParameterExpression parameter, Expression replacement)
            {
                return new SubstExpressionVisitor(parameter, replacement).Visit(expression);
            }
        }
    }
}
