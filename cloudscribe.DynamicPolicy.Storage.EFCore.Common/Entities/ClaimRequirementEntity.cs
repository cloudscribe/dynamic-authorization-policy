using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities
{
    public class ClaimRequirementEntity
    {
        public ClaimRequirementEntity()
        {
            Id = Guid.NewGuid();
            AllowedValues = new List<AllowedClaimValueEntity>();
        }

        public Guid Id { get; set; }
        public string ClaimName { get; set; }

        public List<AllowedClaimValueEntity> AllowedValues { get; set; }

        public AuthorizationPolicyEntity Policy { get; set; }
    }
}
