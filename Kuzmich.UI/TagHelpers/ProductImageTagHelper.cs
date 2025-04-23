using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Kuzmich.UI.TagHelpers
{
    [HtmlTargetElement("product-image")]
    public class ProductImageTagHelper : TagHelper
    {
        [HtmlAttributeName("image")]
        public string ImageName { get; set; } = string.Empty;

        [HtmlAttributeName("alt")]
        public string? Alt { get; set; }

        [HtmlAttributeName("width")]
        public int? Width { get; set; }

        [HtmlAttributeName("css-class")]
        public string? CssClass { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            output.Attributes.SetAttribute("src", $"/images/{ImageName}");

            if (!string.IsNullOrWhiteSpace(Alt))
                output.Attributes.SetAttribute("alt", Alt);

            if (Width.HasValue)
                output.Attributes.SetAttribute("width", Width.ToString());

            if (!string.IsNullOrWhiteSpace(CssClass))
                output.Attributes.SetAttribute("class", CssClass);
        }
    }
}

