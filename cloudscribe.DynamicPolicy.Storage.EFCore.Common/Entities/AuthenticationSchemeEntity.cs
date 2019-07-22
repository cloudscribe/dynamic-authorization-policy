using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities
{
    public class AuthenticationSchemeEntity
    {
        public AuthenticationSchemeEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string AuthenticationScheme { get; set; }

        public AuthorizationPolicyEntity Policy { get; set; }
    }
}
