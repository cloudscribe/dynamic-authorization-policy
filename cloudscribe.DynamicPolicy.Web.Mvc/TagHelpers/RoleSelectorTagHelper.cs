using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using cloudscribe.DynamicPolicy.Models;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Web.Mvc.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "cdp-role-selector,cdp-tenant-id")]
    public class RoleSelectorTagHelper : AnchorTagHelper
    {
        private const string RoleSelectorAttributeName = "cdp-role-selector";
        private const string TenantIdAttributeName = "cdp-tenant-id";
        private const string CsvTargetIdAttributeName = "cdp-target-id";
        private const string DisplayTargetIdAttributeName = "cdp-display-target-id";

        public RoleSelectorTagHelper(
            IHtmlGenerator generator
            ) : base(generator)
        {

        }

        [HtmlAttributeName(RoleSelectorAttributeName)]
        public IRoleSelectorProperties RoleSelectorInfo { get; set; } = null;

        [HtmlAttributeName(TenantIdAttributeName)]
        public string TenantId { get; set; } = string.Empty;

        [HtmlAttributeName(CsvTargetIdAttributeName)]
        public string CsvTargetElementId { get; set; } = string.Empty;

        [HtmlAttributeName(DisplayTargetIdAttributeName)]
        public string DisplayTargetElementId { get; set; } = string.Empty;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (RoleSelectorInfo == null
                || string.IsNullOrEmpty(RoleSelectorInfo.Controller)
                || string.IsNullOrEmpty(RoleSelectorInfo.Action)
                || string.IsNullOrEmpty(TenantId)
                || string.IsNullOrEmpty(CsvTargetElementId)
                )
            {
                output.SuppressOutput();
                return;
            }

            // we don't need to render these
            var markerAttribute = new TagHelperAttribute(RoleSelectorAttributeName);
            output.Attributes.Remove(markerAttribute);
            var projectAttribute = new TagHelperAttribute(TenantIdAttributeName);
            output.Attributes.Remove(projectAttribute);
            var csvAttribute = new TagHelperAttribute(CsvTargetIdAttributeName);
            output.Attributes.Remove(csvAttribute);
            var displayAttribute = new TagHelperAttribute(DisplayTargetIdAttributeName);
            output.Attributes.Remove(displayAttribute);
            //

            this.Action = RoleSelectorInfo.Action;
            this.Controller = RoleSelectorInfo.Controller;
            var routeParams = RoleSelectorInfo.GetRouteParams(TenantId);
            if (routeParams != null)
            {
                foreach (var param in routeParams)
                {
                    this.RouteValues.Add(param.Key, param.Value);
                }
            }
            var atts = RoleSelectorInfo.GetAttributes(CsvTargetElementId, DisplayTargetElementId);
            if (atts != null)
            {
                foreach (var att in atts)
                {
                    output.Attributes.Add(new TagHelperAttribute(att.Key, att.Value));
                }
            }

            await base.ProcessAsync(context, output);
        }

    }
}
