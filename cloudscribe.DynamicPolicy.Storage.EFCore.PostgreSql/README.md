﻿### How to generate migrations

open a command/powershell window on the project folder

Since this project is a netstandard20 class library it is not executable, therefore you have to pass in the --startup-project that is executable

dotnet ef --startup-project ../cloudscribeDemo.Web migrations add  --context cloudscribe.DynamicPolicy.Storage.EFCore.PostgreSql.DynamicPolicyDbContext cs-dynamic-policy-yyyymmdd
