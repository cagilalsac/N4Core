#nullable disable

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using N4Core.Enums;
using N4Core.Utilities;
using System.Linq.Expressions;

namespace N4Core.Extensions
{
    public static class HtmlHelperExtension
    {
        public static IHtmlContent DisplayNameFor<TModel, TResult>(this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TResult>> expression, Languages language = Languages.English)
        {
            ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider)helper.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ModelExpressionProvider));
            ModelExpression modelExpression = modelExpressionProvider.CreateModelExpression(helper.ViewData, expression);
            string displayName;
            if (!string.IsNullOrWhiteSpace(modelExpression.Metadata.DisplayName))
            {
                displayName = modelExpression.Metadata.DisplayName;
            }
            else if (!string.IsNullOrWhiteSpace(modelExpression.Metadata.PropertyName))
            {
                displayName = modelExpression.Metadata.PropertyName;
            }
            else
            {
                displayName = modelExpression.Metadata.Name;
            }
            displayName = HelperUtil.GetDisplayName(displayName, '{', '}', ';', language);
            TagBuilder labelTag = new TagBuilder("label");
            labelTag.Attributes.Add("for", helper.IdFor(expression).ToString());
            labelTag.InnerHtml.AppendHtml(displayName);
            return labelTag;
        }
    }
}
