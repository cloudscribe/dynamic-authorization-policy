using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.MSSQL
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(DynamicPolicyDbContext).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("2a733498-dd28-4f0e-b0b2-995166e487ec"); } }

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
