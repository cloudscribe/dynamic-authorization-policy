using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace cloudscribe.DynamicPolicy.Storage.EFCore.SQLite.Migrations
{
    public partial class dynamicauth_initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "csp_AuthPolicy",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    RequireAuthenticatedUser = table.Column<bool>(nullable: false),
                    RequiredUserName = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csp_AuthPolicy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "csp_AuthPolicyClaim",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClaimName = table.Column<string>(maxLength: 255, nullable: false),
                    PolicyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csp_AuthPolicyClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csp_AuthPolicyClaim_csp_AuthPolicy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "csp_AuthPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csp_AuthPolicyRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AllowedRole = table.Column<string>(maxLength: 200, nullable: false),
                    PolicyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csp_AuthPolicyRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csp_AuthPolicyRole_csp_AuthPolicy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "csp_AuthPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csp_AuthPolicyScheme",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthenticationScheme = table.Column<string>(maxLength: 255, nullable: false),
                    PolicyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csp_AuthPolicyScheme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csp_AuthPolicyScheme_csp_AuthPolicy_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "csp_AuthPolicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "csp_AuthPolicyClaimValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AllowedValue = table.Column<string>(maxLength: 255, nullable: false),
                    ClaimRequirementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_csp_AuthPolicyClaimValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_csp_AuthPolicyClaimValue_csp_AuthPolicyClaim_ClaimRequirementId",
                        column: x => x.ClaimRequirementId,
                        principalTable: "csp_AuthPolicyClaim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicy_Name",
                table: "csp_AuthPolicy",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicy_TenantId",
                table: "csp_AuthPolicy",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicyClaim_PolicyId",
                table: "csp_AuthPolicyClaim",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicyClaimValue_ClaimRequirementId",
                table: "csp_AuthPolicyClaimValue",
                column: "ClaimRequirementId");

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicyRole_PolicyId",
                table: "csp_AuthPolicyRole",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_csp_AuthPolicyScheme_PolicyId",
                table: "csp_AuthPolicyScheme",
                column: "PolicyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "csp_AuthPolicyClaimValue");

            migrationBuilder.DropTable(
                name: "csp_AuthPolicyRole");

            migrationBuilder.DropTable(
                name: "csp_AuthPolicyScheme");

            migrationBuilder.DropTable(
                name: "csp_AuthPolicyClaim");

            migrationBuilder.DropTable(
                name: "csp_AuthPolicy");
        }
    }
}
