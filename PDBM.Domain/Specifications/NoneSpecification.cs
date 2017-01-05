﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Specifications
{
    /// <summary>
    /// None规约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class NoneSpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> GetExpression()
        {
            return o => false;
        }
    }
}
