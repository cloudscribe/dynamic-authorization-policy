using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.DynamicPolicy.Web.Mvc.ViewModels
{
    public class ClaimRequirementViewModel
    {
        public ClaimRequirementViewModel()
        {

        }

        public ClaimRequirementViewModel(Guid policyId)
        {
            PolicyId = policyId;
        }

        [Required(ErrorMessage = "PolicyId is required.")]
        public Guid PolicyId { get; set; }

        [Required(ErrorMessage = "Claim name is required.")]
        public string ClaimName { get; set; }

        public string AllowedValuesCsv { get; set; }
    }
}
