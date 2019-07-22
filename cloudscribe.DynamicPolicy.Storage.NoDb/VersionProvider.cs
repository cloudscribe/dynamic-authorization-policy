using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.DynamicPolicy.Storage.NoDb
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(AuthorizationPolicyQueries).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("627b5a2f-d8c0-4afc-a846-4d1128747a56"); } }

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
