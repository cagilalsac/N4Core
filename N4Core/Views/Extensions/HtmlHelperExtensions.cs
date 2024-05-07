#nullable disable

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using N4Core.Culture;
using N4Core.Views.Utils;
using System.Linq.Expressions;

namespace N4Core.Views.Extensions
{
    public static class HtmlHelperExtensions
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
            labelTag.Attributes.Add("style", "cursor:pointer");
            labelTag.InnerHtml.AppendHtml(displayName);
            return labelTag;
        }
    }
}
