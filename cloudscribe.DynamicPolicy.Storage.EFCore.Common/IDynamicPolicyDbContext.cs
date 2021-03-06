﻿using cloudscribe.DynamicPolicy.Storage.EFCore.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.Common
{
    public interface IDynamicPolicyDbContext : IDisposable
    {
        DbSet<AuthorizationPolicyEntity> Policies { get; set; }

        DbSet<AllowedRoleEntity> AllowedRoles { get; set; }

        DbSet<AuthenticationSchemeEntity> AuthenticationSchemes { get; set; }

        DbSet<ClaimRequirementEntity> ClaimRequirements { get; set; }

        DbSet<AllowedClaimValueEntity> AllowedClaimValues { get; set; }

        DatabaseFacade Database { get; }

        ChangeTracker ChangeTracker { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
