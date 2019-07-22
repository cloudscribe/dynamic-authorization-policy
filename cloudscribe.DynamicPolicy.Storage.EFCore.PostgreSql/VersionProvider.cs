using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(DynamicPolicyDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("6e80468f-4115-450f-8a38-8cc884412835"); } }

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
