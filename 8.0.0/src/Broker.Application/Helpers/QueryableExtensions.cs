using System;
using System.Linq;
using Broker.Datatable.Dtos;
using System.Collections.Generic;
using Broker.Helpers;
using System.Linq.Expressions;
using Abp.Linq.Expressions;

namespace Broker.Linq.Extensions
{
    /// <summary>
    /// Some useful extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="filterInput">DataTable filter input</param>
        /// <returns>Filtered or not filtered query based on <see cref="condition"/></returns>
        public static IQueryable<T> FilterDataTable<T>(this IQueryable<T> query, DataTableInputDto filterInput)
        {
            List<Filter> filter = new List<Filter>();
            //Filter by creation params
            if (!string.IsNullOrEmpty(filterInput.creatorUserName))
                if (filterInput.lang.Equals("ar"))
                    filter.Add(new Filter() { PropertyName = "CreatorUser.Name", Operation = Op.Contains, Value = filterInput.creatorUserName });

            if (filterInput.creationTimeFrom.HasValue)
                filter.Add(new Filter() { PropertyName = "CreationTime", Operation = Op.GreaterThanOrEqual, Value = filterInput.creationTimeFrom });
            if (filterInput.creationTimeTo.HasValue)
            {
                filterInput.creationTimeTo = filterInput.creationTimeTo.Value.Add(TimeSpan.FromSeconds(86399)); // add 1 day minus 1 second
                filter.Add(new Filter() { PropertyName = "CreationTime", Operation = Op.LessThanOrEqual, Value = filterInput.creationTimeTo });
            }
            //Filter by modification params
            if (!string.IsNullOrEmpty(filterInput.modifierUserName))
                filter.Add(new Filter() { PropertyName = "LastModifierUser.Name", Operation = Op.Contains, Value = filterInput.modifierUserName });
            if (filterInput.lastModificationTimeFrom.HasValue)
            {
                query = query.GetExpressionForDate(new Filter() { PropertyName = "LastModificationTime", Operation = Op.GreaterThanOrEqual, Value = filterInput.lastModificationTimeFrom });
            }
            if (filterInput.lastModificationTimeTo.HasValue)
            {
                filterInput.lastModificationTimeTo = filterInput.lastModificationTimeTo.Value.Add(TimeSpan.FromSeconds(86399)); // add 1 day minus 1 second
                query = query.GetExpressionForDate(new Filter() { PropertyName = "LastModificationTime", Operation = Op.LessThanOrEqual, Value = filterInput.lastModificationTimeTo });
            }
            //Filter by deletion params
            // if delete filter applied respect it otherwise get the not deleted
            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                if (filterInput.isDeleted.HasValue)
                    filter.Add(new Filter() { PropertyName = "IsDeleted", Operation = Op.Equals, Value = filterInput.isDeleted });
                else
                    filter.Add(new Filter() { PropertyName = "IsDeleted", Operation = Op.Equals, Value = false });
            }
            if (!string.IsNullOrEmpty(filterInput.deleterUserName))
                if (filterInput.lang.Equals("ar"))
                    filter.Add(new Filter() { PropertyName = "DeleterUser.Name", Operation = Op.Contains, Value = filterInput.deleterUserName });
            if (filterInput.deletionTimeFrom.HasValue)
                query = query.GetExpressionForDate(new Filter() { PropertyName = "DeletionTime", Operation = Op.GreaterThanOrEqual, Value = filterInput.deletionTimeFrom });
            if (filterInput.deletionTimeTo.HasValue)
            {
                filterInput.deletionTimeTo = filterInput.deletionTimeTo.Value.Add(TimeSpan.FromSeconds(86399)); // add 1 day minus 1 second
                query = query.GetExpressionForDate(new Filter() { PropertyName = "DeletionTime", Operation = Op.LessThanOrEqual, Value = filterInput.deletionTimeTo });
            }

            var deleg = ExpressionBuilder.GetExpression<T>(filter);
            if (deleg != null)
                return query.Where(deleg);
            else
                return query;
        }

        public static IQueryable<T> FilterDataTable<T>(this IQueryable<T> query, ExcelBaseInput filterInput)
        {
            List<Filter> filter = new List<Filter>();
            //Filter by creation params
            if (!string.IsNullOrEmpty(filterInput.creatorUserName))
                if (filterInput.lang.Equals("ar"))
                    filter.Add(new Filter() { PropertyName = "CreatorUser.Name", Operation = Op.Contains, Value = filterInput.creatorUserName });

            if (filterInput.creationTimeFrom.HasValue)
                filter.Add(new Filter() { PropertyName = "CreationTime", Operation = Op.GreaterThanOrEqual, Value = filterInput.creationTimeFrom });
            if (filterInput.creationTimeTo.HasValue)
            {
                filterInput.creationTimeTo = filterInput.creationTimeTo.Value.Add(TimeSpan.FromSeconds(86399)); // add 1 day minus 1 second
                filter.Add(new Filter() { PropertyName = "CreationTime", Operation = Op.LessThanOrEqual, Value = filterInput.creationTimeTo });
            }
            //Filter by modification params
            if (!string.IsNullOrEmpty(filterInput.modifierUserName))
                filter.Add(new Filter() { PropertyName = "LastModifierUser.Name", Operation = Op.Contains, Value = filterInput.modifierUserName });
            if (filterInput.lastModificationTimeFrom.HasValue)
                filter.Add(new Filter() { PropertyName = "LastModificationTime", Operation = Op.GreaterThanOrEqual, Value = filterInput.lastModificationTimeFrom });
            if (filterInput.lastModificationTimeTo.HasValue)
            {
                filterInput.lastModificationTimeTo = filterInput.lastModificationTimeTo.Value.Add(TimeSpan.FromSeconds(86399)); // add 1 day minus 1 second
                filter.Add(new Filter() { PropertyName = "LastModificationTime", Operation = Op.LessThanOrEqual, Value = filterInput.lastModificationTimeTo });
            }
            //Filter by deletion params
            // if delete filter applied respect it otherwise get the not deleted
            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                if (filterInput.isDeleted.HasValue)
                    filter.Add(new Filter() { PropertyName = "IsDeleted", Operation = Op.Equals, Value = filterInput.isDeleted });
                else
                    filter.Add(new Filter() { PropertyName = "IsDeleted", Operation = Op.Equals, Value = false });
            }
            if (!string.IsNullOrEmpty(filterInput.deleterUserName))
                if (filterInput.lang.Equals("ar"))
                    filter.Add(new Filter() { PropertyName = "DeleterUser.Name", Operation = Op.Contains, Value = filterInput.deleterUserName });
            if (filterInput.deletionTimeFrom.HasValue)
                filter.Add(new Filter() { PropertyName = "DeletionTime", Operation = Op.GreaterThanOrEqual, Value = filterInput.deletionTimeFrom });
            if (filterInput.deletionTimeTo.HasValue)
            {
                filterInput.deletionTimeTo = filterInput.deletionTimeTo.Value.Add(TimeSpan.FromSeconds(86399)); // add 1 day minus 1 second
                filter.Add(new Filter() { PropertyName = "DeletionTime", Operation = Op.LessThanOrEqual, Value = filterInput.deletionTimeTo });
            }

            var deleg = ExpressionBuilder.GetExpression<T>(filter);
            if (deleg != null)
                return query.Where(deleg);
            else
                return query;
        }

        private static IQueryable<T> GetExpressionForDate<T>(this IQueryable<T> query, Filter filter)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            var member = Expression.Property(param, filter.PropertyName);
            var constant2 = Expression.Constant(filter.Value, member.Type);
            switch (filter.Operation)
            {
                case Op.GreaterThanOrEqual:
                    var exp = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant2), param);
                    return query.Where(exp);
                case Op.LessThanOrEqual:
                    var exp1 = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant2), param);
                    return query.Where(exp1);
            }
            return null;
        }

    }
}