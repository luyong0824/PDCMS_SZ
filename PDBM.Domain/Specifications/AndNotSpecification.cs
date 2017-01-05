using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Specifications
{
    /// <summary>
    /// AndNot规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AndNotSpecification<T> : CompositeSpecification<T>
    {
        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            var bodyNot = Expression.Not(Right.GetExpression().Body);
            var bodyExpression = Expression.Lambda<Func<T, bool>>(bodyNot, Right.GetExpression().Parameters);
            return Left.GetExpression().And(bodyExpression);
        }
    }
}
