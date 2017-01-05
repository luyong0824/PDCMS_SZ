using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Data.EntityFramework.Repositories
{
    /// <summary>
    /// IQueryable排序扩展，以支持字符串排序
    /// </summary>
    public static class OrderExtension
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending)
            where T : class
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException("无法根据指定的属性名称找到所需的属性");
            }
            ParameterExpression param = Expression.Parameter(type, "p");
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);
            string methodName = ascending ? "OrderBy" : "OrderByDescending";
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName, bool ascending)
            where T : class
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException("无法根据指定的属性名称找到所需的属性");
            }
            ParameterExpression param = Expression.Parameter(type, "p");
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);
            string methodName = ascending ? "ThenBy" : "ThenByDescending";
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortString)
            where T : class
        {
            var sortParameters = sortString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(str => str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)).ToList();
            if (sortParameters == null || sortParameters.Count == 0)
            {
                throw new ArgumentNullException("排序字符串");
            }
            var first = sortParameters.FirstOrDefault();
            sortParameters.Remove(first);
            bool descending = (first.Count() > 1 && first[1].Trim().ToLower().Equals("desc"));
            IOrderedQueryable<T> orderedQuery = source.OrderBy(first[0], !descending);
            foreach (var sortParameter in sortParameters)
            {
                descending = (sortParameter.Count() > 1 && sortParameter[1].Trim().ToLower().Equals("desc"));
                orderedQuery = orderedQuery.ThenBy(sortParameter[0], !descending);
            }
            return orderedQuery;
        }
    }
}
