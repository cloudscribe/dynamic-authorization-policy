using cloudscribe.DynamicPolicy.Models;
using cloudscribe.Pagination.Models;

namespace cloudscribe.DynamicPolicy.Web.Mvc.ViewModels
{
    public class PolicyListViewModel
    {
        public PolicyListViewModel()
        {
            Policies = new PagedResult<AuthorizationPolicyInfo>();
        }

        //the query
        public string Q { get; set; }

        public int PageSize { get; set; } = 20;

        public PagedResult<AuthorizationPolicyInfo> Policies { get; set; }

    }
}
