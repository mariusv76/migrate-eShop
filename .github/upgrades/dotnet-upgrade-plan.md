# .NET 9.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 9.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9.0 upgrade.
3. Upgrade src\eShop.AppHost\eShop.AppHost.csproj
4. Upgrade src\Basket.API\Basket.API.csproj
5. Upgrade src\Catalog.API\Catalog.API.csproj
6. Upgrade src\eShop.ServiceDefaults\eShop.ServiceDefaults.csproj
7. Upgrade src\EventBus\EventBus.csproj
8. Upgrade src\EventBusRabbitMQ\EventBusRabbitMQ.csproj
9. Upgrade src\Identity.API\Identity.API.csproj
10. Upgrade src\IntegrationEventLogEF\IntegrationEventLogEF.csproj
11. Upgrade src\Mobile.Bff.Shopping\Mobile.Bff.Shopping.csproj
12. Upgrade src\Ordering.API\Ordering.API.csproj
13. Upgrade src\OrderProcessor\OrderProcessor.csproj
14. Upgrade src\Ordering.Domain\Ordering.Domain.csproj
15. Upgrade src\Ordering.Infrastructure\Ordering.Infrastructure.csproj
16. Upgrade src\PaymentProcessor\PaymentProcessor.csproj
17. Upgrade src\WebApp\WebApp.csproj
18. Upgrade src\WebhookClient\WebhookClient.csproj
19. Upgrade src\Webhooks.API\Webhooks.API.csproj
20. Upgrade tests\Basket.UnitTests\Basket.UnitTests.csproj
21. Upgrade tests\Catalog.FunctionalTests\Catalog.FunctionalTests.csproj
22. Upgrade tests\Ordering.FunctionalTests\Ordering.FunctionalTests.csproj
23. Upgrade tests\Ordering.UnitTests\Ordering.UnitTests.csproj
24. Upgrade src\WebAppComponents\WebAppComponents.csproj
25. Run unit tests to validate upgrade in the projects listed below:
  tests\Basket.UnitTests\Basket.UnitTests.csproj; tests\Catalog.FunctionalTests\Catalog.FunctionalTests.csproj; tests\Ordering.FunctionalTests\Ordering.FunctionalTests.csproj; tests\Ordering.UnitTests\Ordering.UnitTests.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

| Project name | Description |
|:-------------|:-----------:|

### Aggregate NuGet packages modifications across all projects

| Package Name | Current Version | New Version | Description |
|:-------------|:---------------:|:-----------:|:------------|
| Aspire.Azure.AI.OpenAI | 8.0.0-preview.8.24258.2 | 9.5.0-preview.1.25474.7 | Recommended for .NET 9.0 |
| Aspire.Hosting.AppHost | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.Hosting.Azure.CognitiveServices | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.Hosting.PostgreSQL | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.Hosting.RabbitMQ | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.Hosting.Redis | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.Npgsql | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.Npgsql.EntityFrameworkCore.PostgreSQL | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.RabbitMQ.Client | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Aspire.StackExchange.Redis | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Duende.IdentityServer | 7.0.7 | 7.3.2 | Security vulnerability |
| FluentValidation.AspNetCore | 11.3.0 | 11.3.1 | Deprecated version |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.Authentication.OpenIdConnect | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.Components.QuickGrid | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.Components.Web | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.Identity.UI | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.Mvc.Testing | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.OpenApi | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.AspNetCore.TestHost | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.EntityFrameworkCore.InMemory | 8.0.8 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.EntityFrameworkCore.Tools | 8.0.8 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 8.0.1 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.Extensions.Http.Resilience | 8.7.0 | 9.9.0 | Recommended for .NET 9.0 |
| Microsoft.Extensions.Identity.Stores | 8.0.7 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.Extensions.Options | 8.0.2 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.Extensions.Options.ConfigurationExtensions | 8.0.0 | 9.0.9 | Recommended for .NET 9.0 |
| Microsoft.Extensions.ServiceDiscovery | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| Microsoft.Extensions.ServiceDiscovery.Yarp | 8.2.0 | 9.5.0 | Deprecated / Recommended for .NET 9.0 |
| OpenTelemetry.Instrumentation.AspNetCore | 1.9.0 | 1.12.0 | Recommended for .NET 9.0 |
| OpenTelemetry.Instrumentation.Http | 1.9.0 | 1.12.0 | Recommended for .NET 9.0 |
| System.Reflection.TypeExtensions | 4.7.0 | (remove) | Included in framework |

### Project upgrade details

#### src\eShop.AppHost\eShop.AppHost.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Hosting.AppHost should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Aspire.Hosting.RabbitMQ should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Aspire.Hosting.Redis should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Aspire.Hosting.PostgreSQL should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Aspire.Hosting.Azure.CognitiveServices should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
Other changes:
  - Remove legacy package usage if transitive duplicates appear after upgrade.

#### src\Basket.API\Basket.API.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.StackExchange.Redis should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
Other changes:
  - Validate Redis connection resilience settings for any new defaults in 9.x.

#### src\Catalog.API\Catalog.API.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Npgsql.EntityFrameworkCore.PostgreSQL should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Microsoft.EntityFrameworkCore.Tools should be updated from `8.0.8` to `9.0.9` (*recommended*)
  - Aspire.Azure.AI.OpenAI should be updated from `8.0.0-preview.8.24258.2` to `9.5.0-preview.1.25474.7` (*recommended*)
Other changes:
  - Review EF Core 9 breaking changes for migrations.

#### src\eShop.ServiceDefaults\eShop.ServiceDefaults.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.AspNetCore.OpenApi should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.AspNetCore.Authentication.JwtBearer should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.Extensions.Http.Resilience should be updated from `8.7.0` to `9.9.0` (*recommended*)
  - Microsoft.Extensions.ServiceDiscovery should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - OpenTelemetry.Instrumentation.AspNetCore should be updated from `1.9.0` to `1.12.0` (*recommended*)
  - OpenTelemetry.Instrumentation.Http should be updated from `1.9.0` to `1.12.0` (*recommended*)
Other changes:
  - Review telemetry configuration for new meters/spans defaults.

#### src\EventBus\EventBus.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.Extensions.DependencyInjection.Abstractions should be updated from `8.0.1` to `9.0.9` (*recommended*)
  - Microsoft.Extensions.Options should be updated from `8.0.2` to `9.0.9` (*recommended*)
Other changes:
  - Re-run DI container validation if using Scrutor or custom scanning.

#### src\EventBusRabbitMQ\EventBusRabbitMQ.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.RabbitMQ.Client should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Microsoft.Extensions.Options.ConfigurationExtensions should be updated from `8.0.0` to `9.0.9` (*recommended*)
Other changes:
  - Validate channel pooling/backpressure settings in new client.

#### src\Identity.API\Identity.API.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.AspNetCore.Identity.UI should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.EntityFrameworkCore.Tools should be updated from `8.0.8` to `9.0.9` (*recommended*)
  - Aspire.Npgsql.EntityFrameworkCore.PostgreSQL should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Duende.IdentityServer should be updated from `7.0.7` to `7.3.2` (*security vulnerability*)
Other changes:
  - Review Duende IdentityServer 7.3.x release notes for protocol tweaks.

#### src\IntegrationEventLogEF\IntegrationEventLogEF.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
Other changes:
  - Ensure logging scopes unaffected by EF Core 9 changes.

#### src\Mobile.Bff.Shopping\Mobile.Bff.Shopping.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.Extensions.ServiceDiscovery.Yarp should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
Other changes:
  - Validate YARP route config if new discovery features enabled.

#### src\Ordering.API\Ordering.API.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Npgsql.EntityFrameworkCore.PostgreSQL should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Microsoft.EntityFrameworkCore.Tools should be updated from `8.0.8` to `9.0.9` (*recommended*)
  - FluentValidation.AspNetCore should be updated from `11.3.0` to `11.3.1` (*deprecated*)
Other changes:
  - Revisit validation pipeline behaviour in ASP.NET Core 9.

#### src\OrderProcessor\OrderProcessor.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Npgsql should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
Other changes:
  - Confirm connection pooling settings for new provider.

#### src\Ordering.Domain\Ordering.Domain.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - System.Reflection.TypeExtensions should be removed (functionality in framework)
Other changes:
  - Run solution-wide find usages for removed API wrappers.

#### src\Ordering.Infrastructure\Ordering.Infrastructure.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
Other changes:
  - Evaluate EF interception changes in 9.x.

#### src\PaymentProcessor\PaymentProcessor.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
Other changes:
  - Review background worker lifetime vs host changes.

#### src\WebApp\WebApp.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Azure.AI.OpenAI should be updated from `8.0.0-preview.8.24258.2` to `9.5.0-preview.1.25474.7` (*recommended*)
  - Microsoft.Extensions.ServiceDiscovery.Yarp should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Microsoft.AspNetCore.Authentication.OpenIdConnect should be updated from `8.0.7` to `9.0.9` (*recommended*)
Other changes:
  - Check Blazor/QuickGrid compatibility if used indirectly.

#### src\WebhookClient\WebhookClient.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.AspNetCore.Authentication.OpenIdConnect should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.AspNetCore.Components.QuickGrid should be updated from `8.0.7` to `9.0.9` (*recommended*)
Other changes:
  - Review OIDC handler config differences in 9.x.

#### src\Webhooks.API\Webhooks.API.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Npgsql.EntityFrameworkCore.PostgreSQL should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Microsoft.EntityFrameworkCore.Tools should be updated from `8.0.8` to `9.0.9` (*recommended*)
Other changes:
  - Verify webhook retry strategies with any new hosting defaults.

#### tests\Basket.UnitTests\Basket.UnitTests.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.AspNetCore.Mvc.Testing should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.Extensions.Identity.Stores should be updated from `8.0.7` to `9.0.9` (*recommended*)
Other changes:
  - Adjust test host builder if any new minimal APIs affect setup.

#### tests\Catalog.FunctionalTests\Catalog.FunctionalTests.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Hosting.AppHost should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Aspire.Hosting.PostgreSQL should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Microsoft.AspNetCore.Mvc.Testing should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.AspNetCore.TestHost should be updated from `8.0.7` to `9.0.9` (*recommended*)
Other changes:
  - Ensure containerized test resources use matching versions.

#### tests\Ordering.FunctionalTests\Ordering.FunctionalTests.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Aspire.Hosting.AppHost should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Aspire.Hosting.PostgreSQL should be updated from `8.2.0` to `9.5.0` (*deprecated / recommended*)
  - Microsoft.AspNetCore.Mvc.Testing should be updated from `8.0.7` to `9.0.9` (*recommended*)
  - Microsoft.AspNetCore.TestHost should be updated from `8.0.7` to `9.0.9` (*recommended*)
Other changes:
  - Ensure containerized test resources use matching versions.

#### tests\Ordering.UnitTests\Ordering.UnitTests.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.EntityFrameworkCore.InMemory should be updated from `8.0.8` to `9.0.9` (*recommended*)
Other changes:
  - Review any provider behavior changes in EF Core InMemory 9.

#### src\WebAppComponents\WebAppComponents.csproj modifications
Project properties changes:
  - Target framework should be changed from `net8.0` to `net9.0`
NuGet packages changes:
  - Microsoft.AspNetCore.Components.Web should be updated from `8.0.7` to `9.0.9` (*recommended*)
Other changes:
  - Validate Blazor component rendering differences in .NET 9.
