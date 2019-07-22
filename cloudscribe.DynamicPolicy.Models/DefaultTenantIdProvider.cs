namespace cloudscribe.DynamicPolicy.Models
{
    public class DefaultTenantIdProvider : ITenantIdProvider
    {
        public string GetTenantId()
        {
            return "default";
        }
    }
}
