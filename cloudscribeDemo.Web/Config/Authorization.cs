using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Authorization
    {
        public static AuthorizationOptions SetupAuthorizationPolicies(this AuthorizationOptions options)
        {
            //https://docs.asp.net/en/latest/security/authorization/policies.html
            
            // *** comment out default policies which are hard coded in these extension methods, in order to use Dyanamic Authorization Policies managed from the UI
            // *** any policies defined here will not be managed by the UI
            // *** after login see Administrations > Authorization Policies

            //options.AddCloudscribeCoreDefaultPolicies();
            //options.AddCloudscribeLoggingDefaultPolicy();

            //options.AddCloudscribeCoreSimpleContentIntegrationDefaultPolicies();


            //options.AddPolicy(
            //    "FileManagerPolicy",
            //    authBuilder =>
            //    {
            //        authBuilder.RequireRole("Administrators", "Content Administrators");
            //    });

            //options.AddPolicy(
            //    "FileManagerDeletePolicy",
            //    authBuilder =>
            //    {
            //        authBuilder.RequireRole("Administrators", "Content Administrators");
            //    });

            //options.AddPolicy(
            //    "MembershipAdminPolicy",
            //    authBuilder =>
            //    {
            //        authBuilder.RequireRole("Administrators");
            //    });

            //options.AddPolicy(
            //    "MembershipJoinPolicy",
            //    authBuilder =>
            //    {
            //        authBuilder.RequireAuthenticatedUser();
            //    });

            //options.AddPolicy(
            //    "StripeAdminPolicy",
            //    authBuilder =>
            //    {
            //        authBuilder.RequireRole("Administrators");
            //    });


            // policies that are hard coded here will not be managed from the UI and for some policies that is what you want

            //*** best to declare this policy here and not let it be managed from the UI since it
            //    enforces only the root site admins can manage, therefore the policy should never be managed per tenant
            options.AddPolicy(
                "ServerAdminPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins");

                });

            // you could comment this out if you want admins from any site to be able
            // to edit globally shared country state data
            // by commenting this out the policy could be managed per tenant from the UI
            // but it is probably best to only let this be managed from the master tenant
            options.AddPolicy(
                "CoreDataPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins");
                });

            // probably best and recommended to not let this policy be managed from the UI
            // since this policy controls who can manage policies from the UI
            options.AddPolicy(
                "PolicyManagementPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Administrators");
                });


            return options;
        }

    }
}
