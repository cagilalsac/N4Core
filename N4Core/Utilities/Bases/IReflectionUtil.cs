using N4Core.Enums;
using N4Core.Models;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace N4Core.Utilities.Bases
{
    public interface IReflectionUtil
    {
        PropertyInfo GetPropertyInfo<T>(T instance, string propertyName) where T : class, new();
        PropertyInfo GetPropertyInfo<T>(string propertyName) where T : class, new();
        List<ReflectionPropertyModel> GetReflectionPropertyModelProperties<T>(TagAttribute tagAttribute = TagAttribute.None) where T : class;
        void TrimStringProperties<T>(T instance) where T : class, new();
        Expression<Func<T, bool>> GetPredicateContainsExpression<T>(string propertyName, string value, bool isCaseInsensitive = true) where T : class, new();
        Expression<Func<T, object>> GetExpression<T>(string propertyName) where T : class, new();
        IQueryable<T> OrderQuery<T>(IQueryable<T> query, bool orderDirectionDescending, string orderExpression, string orderExpressionSuffix) where T : class, new();
        DataTable ConvertToDataTable<T>(List<T> list) where T : class, new();
        ReflectionRecordModel GetReflectionRecordModel<T>() where T : class, new();
    }
}
