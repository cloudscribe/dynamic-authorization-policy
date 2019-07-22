using System.Collections.Generic;

namespace cloudscribe.DynamicPolicy.Models
{
    public class ClaimRequirement
    {
        public ClaimRequirement()
        {
            AllowedValues = new List<string>();
        }

        public string ClaimName { get; set; }
        public List<string> AllowedValues { get; set; }

    }
}
