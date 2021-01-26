using BookLibrary.ViewModels.Sorting.States;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace SortApp.TagHelpers
{
    public class SortHeaderTagHelper : TagHelper
    {
        public SortEnum Property { get; set; }
        public SortEnum Current { get; set; }
        public string Action { get; set; }
        public bool Up { get; set; }

        private IUrlHelperFactory urlHelperFactory;
        public SortHeaderTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";
            PageUrlValues["sortOrder"] = Property;
            string url = urlHelper.Action(Action, PageUrlValues);
            output.Attributes.SetAttribute("href", url);

            switch (Current)
            {
                case SortEnum.NAME_DESC:
                    Current = SortEnum.NAME_ASC;
                    break;
                case SortEnum.SURNAME_ASC:
                    Current = SortEnum.SURNAME_DESC;
                    break;
                case SortEnum.SURNAME_DESC:
                    Current = SortEnum.SURNAME_ASC;
                    break;
                case SortEnum.NAME_ASC:
                    Current = SortEnum.NAME_DESC;
                    break;
                case SortEnum.TITLE_DESC:
                    Current = SortEnum.TITLE_ASC;
                    break;
                case SortEnum.YEAR_ASC:
                    Current = SortEnum.YEAR_DESC;
                    break;
                case SortEnum.YEAR_DESC:
                    Current = SortEnum.YEAR_ASC;
                    break;
                case SortEnum.AUTHOR_NAME_ASC:
                    Current = SortEnum.AUTHOR_NAME_DESC;
                    break;
                case SortEnum.AUTHOR_NAME_DESC:
                    Current = SortEnum.AUTHOR_NAME_ASC;
                    break;
                case SortEnum.AUTHOR_SURNAME_ASC:
                    Current = SortEnum.AUTHOR_SURNAME_DESC;
                    break;
                case SortEnum.AUTHOR_SURNAME_DESC:
                    Current = SortEnum.AUTHOR_SURNAME_ASC;
                    break;
                case SortEnum.RATE_ASC:
                    Current = SortEnum.RATE_DESC;
                    break;
                case SortEnum.RATE_DESC:
                    Current = SortEnum.RATE_ASC;
                    break;
                case SortEnum.EMAIL_ASC:
                    Current = SortEnum.EMAIL_DESC;
                    break;
                case SortEnum.EMAIL_DESC:
                    Current = SortEnum.EMAIL_ASC;
                    break;
                case SortEnum.LOGINNAME_ASC:
                    Current = SortEnum.LOGINNAME_DESC;
                    break;
                case SortEnum.LOGINNAME_DESC:
                    Current = SortEnum.LOGINNAME_ASC;
                    break;
                default:
                    Current = SortEnum.TITLE_DESC;
                    break;
            }

            if (Current == Property)
            {
                TagBuilder tag = new TagBuilder("i");

                if (Up == true)
                    tag.AddCssClass("fas fa-chevron-up");
                else
                    tag.AddCssClass("fas fa-chevron-down");
                output.PreContent.AppendHtml(tag);
            }
        }
    }
}