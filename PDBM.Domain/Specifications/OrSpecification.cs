using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Specifications
{
    /// <summary>
    /// Or规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {
        }

        public override Expression<Func<T, bool>> GetExpression()
        {
            return Left.GetExpression().Or(Right.GetExpression());
        }
    }
}
