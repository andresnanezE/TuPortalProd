using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Presentacion.Mvc.App.Helpers
{
    public static class MvcHtmlHelpers
    {
        /// <summary>
        /// Returns hiddens for every IEnumerable item, with it's selected properties, if any memberPropsExpression provided.
        /// </summary>
        public static MvcHtmlString HiddenForEnumerable<TModel, TModelProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, IEnumerable<TModelProperty>>> expression, params Expression<Func<TModelProperty, object>>[] memberPropsExpressions)
        {
            var sb = new StringBuilder();

            var membername = expression.GetMemberName();
            var model = helper.ViewData.Model;
            var list = expression.Compile()(model);

            var memPropsInfo = memberPropsExpressions.Select(x => new
            {
                MemberPropName = x.GetMemberName(),
                ListItemPropGetter = x.Compile()
            }).ToList();

            for (var i = 0; i < list.Count(); i++)
            {
                var listItem = list.ElementAt(i);
                if (memPropsInfo.Any())
                {
                    foreach (var q in memPropsInfo)
                    {
                        sb.Append(helper.Hidden(string.Format("{0}[{1}].{2}", membername, i, q.MemberPropName), q.ListItemPropGetter(listItem)));
                    }
                }
                else
                {
                    sb.Append(helper.Hidden(string.Format("{0}[{1}]", membername, i), listItem));
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Returns hiddens for every IEnumerable item, with it's all public writable properties, if allPublicProps set to true.
        /// </summary>
        public static MvcHtmlString HiddenForEnumerable<TModel, TModelProperty>(this HtmlHelper<TModel> helper,
           Expression<Func<TModel, IEnumerable<TModelProperty>>> expression, bool allPublicProps)
        {
            if (!allPublicProps)
                return HiddenForEnumerable(helper, expression);

            var sb = new StringBuilder();

            var membername = expression.GetMemberName();
            var model = helper.ViewData.Model;
            var list = expression.Compile()(model);

            var type = typeof(TModelProperty);
            var memPropsInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetSetMethod(false) != null && x.GetGetMethod(false) != null)
                .Select(x => new
                {
                    MemberPropName = x.Name,
                    ListItemPropGetter = (Func<TModelProperty, object>)(p => x.GetValue(p, null))
                }).ToList();

            if (memPropsInfo.Count == 0)
                return HiddenForEnumerable(helper, expression);

            for (var i = 0; i < list.Count(); i++)
            {
                var listItem = list.ElementAt(i);
                foreach (var q in memPropsInfo)
                {
                    sb.Append(helper.Hidden(string.Format("{0}[{1}].{2}", membername, i, q.MemberPropName), q.ListItemPropGetter(listItem)));
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static string GetMemberName<TModel, T>(this Expression<Func<TModel, T>> input)
        {
            if (input == null)
                return null;

            MemberExpression memberExp = null;

            if (input.Body.NodeType == ExpressionType.MemberAccess)
                memberExp = input.Body as MemberExpression;
            else if (input.Body.NodeType == ExpressionType.Convert)
                memberExp = ((UnaryExpression)input.Body).Operand as MemberExpression;

            return memberExp != null ? memberExp.Member.Name : null;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}