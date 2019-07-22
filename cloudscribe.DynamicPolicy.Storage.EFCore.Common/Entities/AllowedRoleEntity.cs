using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities
{
    public class AllowedRoleEntity
    {
        public AllowedRoleEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string AllowedRole { get; set; }

        public AuthorizationPolicyEntity Policy { get; set; }
    }
}
