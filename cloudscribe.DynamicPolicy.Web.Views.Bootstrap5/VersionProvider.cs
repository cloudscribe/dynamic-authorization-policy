using cloudscribe.Versioning;
using cloudscribe.Web.Common;
using System;
using System.Reflection;

namespace cloudscribe.DynamicPolicy.Web.Views.Bootstrap5
{
    public class VersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.DynamicPolicy.Web.Views.Bootstrap5"; } }

        public Guid ApplicationId { get { return new Guid("8t3t3daa-7a4a-4939-831c-401fcec37335"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(CloudscribeCommonResources).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}