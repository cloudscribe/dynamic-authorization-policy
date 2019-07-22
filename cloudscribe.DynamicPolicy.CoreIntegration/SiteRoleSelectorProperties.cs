using cloudscribe.Core.Models;
using cloudscribe.DynamicPolicy.Models;
using System.Collections.Generic;

namespace cloudscribe.DynamicPolicy.CoreIntegration
{
    public class SiteRoleSelectorProperties : IRoleSelectorProperties
    {
        public SiteRoleSelectorProperties(SiteContext currentSite)
        {
            this.currentSite = currentSite;

            RequiredScriptPaths = new List<string>();
            RequiredScriptPaths.Add("~/cr/js/jquery.unobtrusive-ajax.min.js");
            RequiredScriptPaths.Add("~/cr/js/cloudscribe-role-selector.min.js");

        }

        private SiteContext currentSite;

        public string Action
        {
            get { return "Modal"; }
        }

        public string Controller
        {
            get { return "RoleAdmin"; }
        }

        public Dictionary<string, string> GetAttributes(string csvTargetElementId, string displayTargetId = "")
        {
            var result = new Dictionary<string, string>();
            result.Add("data-ajax", "true");
            result.Add("data-ajax-begin", "roleSelector.prepareModal('" + csvTargetElementId + "','" + displayTargetId + "')");
            result.Add("data-ajax-failure", "roleSelector.clearModal()");
            result.Add("data-ajax-method", "GET");
            result.Add("data-ajax-mode", "replace");
            result.Add("data-ajax-success", "roleSelector.openModal()");
            result.Add("data-ajax-update", "#roledialog");

            return result;

        }

        public Dictionary<string, string> GetRouteParams(string projectId)
        {
            var result = new Dictionary<string, string>();
            result.Add("siteId", projectId);
            return result;
        }


        public List<string> RequiredScriptPaths { get; private set; }

    }
}
