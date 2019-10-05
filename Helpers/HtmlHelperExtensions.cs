using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JobPortal.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent HelloWorldHTMLString(this IHtmlHelper htmlHelper)
            => new HtmlString("<strong>Hello World</strong>");

        public static IHtmlContent AddClassIfPropertyInError<TModel, TProperty>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression,
        string errorClassName)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData
                .TemplateInfo.GetFullHtmlFieldName(expressionText);
            var state = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            if (state == null)
            {
                return HtmlString.Empty;
            }

            if (state.Errors.Count == 0)
            {
                return HtmlString.Empty;
            }

            return new HtmlString(errorClassName);
        }
    }
}
