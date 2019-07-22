using System;
using System.Collections.Generic;

namespace cloudscribe.DynamicPolicy.Models
{
    public class AuthorizationPolicyInfo
    {
        public AuthorizationPolicyInfo()
        {
            Id = Guid.NewGuid();
            AllowedRoles = new List<string>();
            AuthenticationSchemes = new List<string>();
            RequiredClaims = new List<ClaimRequirement>();
        }
        public Guid Id { get; set; }
        public string TenantId { get; set; } = "default";
        public string Name { get; set; }
        public bool RequireAuthenticatedUser { get; set; }
        public string RequiredUserName { get; set; }
        public List<string> AllowedRoles { get; set; }
        public List<string> AuthenticationSchemes { get; set; }
        public List<ClaimRequirement> RequiredClaims { get; set; }

        //https://github.com/cloudscribe/cloudscribe/issues/643
        public string Notes { get; set; }

    }
}
