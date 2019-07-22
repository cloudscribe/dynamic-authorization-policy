namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common
{
    public interface IDynamicPolicyDbContextFactory
    {
        IDynamicPolicyDbContext CreateContext();
    }
}
