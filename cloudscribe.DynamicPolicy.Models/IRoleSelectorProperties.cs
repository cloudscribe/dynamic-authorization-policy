using System.Collections.Generic;

namespace cloudscribe.DynamicPolicy.Models
{
    public interface IRoleSelectorProperties
    {
        string Controller { get; }

        string Action { get; }

        Dictionary<string, string> GetAttributes(string csvTargetElementId, string displayTargetId = "");

        Dictionary<string, string> GetRouteParams(string projectId);

        List<string> RequiredScriptPaths { get; }


    }
}
