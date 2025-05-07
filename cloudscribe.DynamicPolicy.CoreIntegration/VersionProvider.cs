using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.DynamicPolicy.CoreIntegration
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(SiteCloneHandler).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("8t3t3daa-7f4f-4939-831c-401fcec37335"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}