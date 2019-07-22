using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities
{
    public class AllowedClaimValueEntity
    {
        public AllowedClaimValueEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string AllowedValue { get; set; }
        public ClaimRequirementEntity ClaimRequirement { get; set; }
    }
}
