using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.DynamicPolicy.Web.Mvc.ViewModels
{
    public class PolicyEditViewModel
    {
        public PolicyEditViewModel()
        {
            RequiredClaims = new List<ClaimRequirementViewModel>();
        }

        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; set; }

        /// <summary>
        /// This is for display only, we don't allow editing the name after a policy is created.
        /// </summary>
        public string Name { get; set; }

        public string TenantId { get; set; }

        public bool RequireAuthenticatedUser { get; set; }
        public string RequiredUserName { get; set; }
        public string AllowedRolesCsv { get; set; }
        public string AuthenticationSchemesCsv { get; set; }
        public List<ClaimRequirementViewModel> RequiredClaims { get; set; }

        public string Notes { get; set; }

    }
}
