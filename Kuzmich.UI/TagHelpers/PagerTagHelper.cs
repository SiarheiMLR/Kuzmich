using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Kuzmich.UI.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly LinkGenerator _linkGenerator;

        public PagerTagHelper(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        [HtmlAttributeName("current-page")]
        public int CurrentPage { get; set; }

        [HtmlAttributeName("total-pages")]
        public int TotalPages { get; set; }

        [HtmlAttributeName("category")]
        public string? Category { get; set; }

        [HtmlAttributeName("action")]
        public string Action { get; set; } = "Index";

        [HtmlAttributeName("controller")]
        public string Controller { get; set; } = "Laptop";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Гарантируем минимальное количество страниц = 1
            TotalPages = Math.Max(1, TotalPages);

            output.TagName = "nav";
            output.TagMode = TagMode.StartTagAndEndTag; // 👈 ОБЯЗАТЕЛЬНО!

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            int prevPage = Math.Max(1, CurrentPage - 1);
            int nextPage = Math.Min(TotalPages, CurrentPage + 1);

            // Кнопка <<
            ul.InnerHtml.AppendHtml(CreatePageLink("«", prevPage, CurrentPage == 1));

            // Номера страниц ±2 от текущей
            int startPage = Math.Max(1, CurrentPage - 2);
            int endPage = Math.Min(TotalPages, CurrentPage + 2);

            for (int i = startPage; i <= endPage; i++)
            {
                bool isActive = i == CurrentPage;
                ul.InnerHtml.AppendHtml(CreatePageLink(i.ToString(), i, false, isActive));
            }

            // Кнопка >>
            ul.InnerHtml.AppendHtml(CreatePageLink("»", nextPage, CurrentPage == TotalPages));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreatePageLink(string text, int pageNo, bool isDisabled = false, bool active = false)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");

            if (isDisabled) li.AddCssClass("disabled");
            if (active) li.AddCssClass("active");

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");

            // Генерация URL
            var routeValues = new Dictionary<string, object> { { "pageNo", pageNo } };
            if (!string.IsNullOrEmpty(Category))
                routeValues["category"] = Category;

            a.Attributes["href"] = isDisabled
                ? "#"
                : _linkGenerator.GetPathByAction(Action, Controller, routeValues);

            a.InnerHtml.Append(text);
            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}
