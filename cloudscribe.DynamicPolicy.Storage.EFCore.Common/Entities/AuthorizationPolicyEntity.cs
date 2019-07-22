using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities
{
    public class AuthorizationPolicyEntity
    {
        public AuthorizationPolicyEntity()
        {
            Id = Guid.NewGuid();
            AllowedRoles = new List<AllowedRoleEntity>();
            AuthenticationSchemes = new List<AuthenticationSchemeEntity>();
            RequiredClaims = new List<ClaimRequirementEntity>();
        }

        public Guid Id { get; set; }
        public string TenantId { get; set; } = "default";
        public string Name { get; set; }
        public bool RequireAuthenticatedUser { get; set; }
        public string RequiredUserName { get; set; }

        public List<AllowedRoleEntity> AllowedRoles { get; set; }
        public List<AuthenticationSchemeEntity> AuthenticationSchemes { get; set; }

        public List<ClaimRequirementEntity> RequiredClaims { get; set; }

        public string Notes { get; set; }

    }
}
