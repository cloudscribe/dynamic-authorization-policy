namespace cloudscribe.DynamicPolicy.Models
{
    public class PolicyCacheOptions
    {
        public int CacheDurationInSeconds { get; set; } = 3600; //default 1 hour
        public long CacheItemSize { get; set; } = 1;
    }
}
