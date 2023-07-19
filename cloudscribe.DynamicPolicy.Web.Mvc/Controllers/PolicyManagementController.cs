// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:					Joe Audette
// Created:					2018-01-01
// Last Modified:			2019-07-08
// 

using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using cloudscribe.DynamicPolicy.Models;
using cloudscribe.DynamicPolicy.Services;
using cloudscribe.DynamicPolicy.Web.Mvc.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.DynamicPolicy.Web.Mvc.Controllers
{
    [Authorize("PolicyManagementPolicy")]
    public class PolicyManagementController : Controller
    {
        public PolicyManagementController(
            PolicyManagementService policyManagementService,
            IStringLocalizer<PolicyResources> localizer,
            ILogger<PolicyManagementController> logger
            )
        {
            _service = policyManagementService;
            _sr = localizer;
            _log = logger;
        }

        private PolicyManagementService _service;
        private IStringLocalizer _sr;
        private ILogger _log;

        [HttpGet]
        public async Task<IActionResult> Index(
            CancellationToken cancellationToken, 
            string q = null,
            int pageNumber = 1, 
            int pageSize = 20)
        {
            var model = new PolicyListViewModel()
            {
                Q = q,
                PageSize = pageSize
            };
            model.Policies = await _service.GetPageOfPolicies(
                q,
                pageNumber, 
                pageSize, 
                cancellationToken);

            return View(model);
        }

        [HttpGet]
        public IActionResult NewPolicy()
        {
            var model = new NewPolicyViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewPolicy(NewPolicyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var foundPolicy = await _service.FetchPolicy(model.Name);
            if (foundPolicy != null)
            {
                ModelState.AddModelError("nameinuse", _sr["The policy name is already in use."]);
                return View(model);
            }

            var policy = new AuthorizationPolicyInfo();
            policy.Name = model.Name;
            var result = await _service.CreatePolicy(policy);
            if (result.Succeeded)
            {
                var successFormat = _sr["The policy {0} was successfully created."];
                this.AlertSuccess(string.Format(successFormat, model.Name), true);
            }
            else
            {
                this.AlertDanger(_sr[result.Message], true);
            }

            return RedirectToAction("EditPolicy", new { id = policy.Id });
        }


        [HttpGet]
        public async Task<IActionResult> EditPolicy(CancellationToken cancellationToken, Guid id)
        {
            var policy = await _service.FetchPolicy(id, cancellationToken);
            if (policy == null)
            {
                _log.LogError($"policy not found with id {id}, so redirecting to index");
                return RedirectToAction("Index");
            }

            var model = new PolicyEditViewModel();
            model.Id = policy.Id;
            model.TenantId = policy.TenantId;

            model.AllowedRolesCsv = string.Join(",", policy.AllowedRoles);
            model.AuthenticationSchemesCsv = string.Join(",", policy.AuthenticationSchemes);
            model.Name = policy.Name;
            model.RequireAuthenticatedUser = policy.RequireAuthenticatedUser;
            model.RequiredUserName = policy.RequiredUserName;
            model.Notes = policy.Notes;
            foreach (var claim in policy.RequiredClaims)
            {
                var claimModel = new ClaimRequirementViewModel(policy.Id);
                claimModel.ClaimName = claim.ClaimName;
                claimModel.AllowedValuesCsv = string.Join(",", claim.AllowedValues);
                model.RequiredClaims.Add(claimModel);
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPolicy(PolicyEditViewModel model)
        {
            var result = await _service.UpdatePolicy(
                model.Id,
                model.RequireAuthenticatedUser,
                model.AllowedRolesCsv,
                model.AuthenticationSchemesCsv,
                model.RequiredUserName,
                model.Notes);

            if (result.Succeeded)
            {
                var successFormat = _sr["The policy {0} was successfully updated."];
                this.AlertSuccess(string.Format(successFormat, model.Name), true);
            }
            else
            {
                this.AlertDanger(_sr[result.Message], true);
            }

            return RedirectToAction("EditPolicy", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePolicy(Guid id)
        {
            var result = await _service.DeletePolicy(id);
            if (result.Succeeded)
            {
                var successFormat = _sr["The policy was successfully deleted."];
                this.AlertSuccess(successFormat, true);
            }
            else
            {
                this.AlertDanger(_sr[result.Message], true);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> NewClaimRequirement(ClaimRequirementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AddClaimRequirement(model.PolicyId, model.ClaimName, model.AllowedValuesCsv);
                if (result.Succeeded)
                {
                    var successFormat = _sr["The claim requirement {0} was successfully added."];
                    this.AlertSuccess(string.Format(successFormat, model.ClaimName), true);
                }
                else
                {
                    this.AlertDanger(_sr[result.Message], true);
                }

            }

            return RedirectToAction("EditPolicy", new { id = model.PolicyId });

        }

        [HttpPost]
        public async Task<IActionResult> EditClaimRequirement(ClaimRequirementViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateClaimRequirement(model.PolicyId, model.ClaimName, model.AllowedValuesCsv);
                if (result.Succeeded)
                {
                    var successFormat = _sr["The claim requirement {0} was successfully updated."];
                    this.AlertSuccess(string.Format(successFormat, model.ClaimName), true);
                }
                else
                {
                    this.AlertDanger(_sr[result.Message], true);
                }

            }

            return RedirectToAction("EditPolicy", new { id = model.PolicyId });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteClaimRequirement(Guid policyId, string claimName)
        {
            var result = await _service.DeleteClaimRequirement(policyId, claimName);
            if (result.Succeeded)
            {
                var successFormat = _sr["The claim requirement {0} was successfully removed."];
                this.AlertSuccess(string.Format(successFormat, claimName), true);
            }
            else
            {
                this.AlertDanger(_sr[result.Message], true);
            }

            return RedirectToAction("EditPolicy", new { id = policyId });
        }

    }
}
