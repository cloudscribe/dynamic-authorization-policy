using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.MySql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(DynamicPolicyDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("5ba774ae-b5b1-4680-ad9c-ad5354b67e55"); } }

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
