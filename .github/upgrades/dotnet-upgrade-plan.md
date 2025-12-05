# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade `src\NuvTools.Common\NuvTools.Common.csproj`
4. Upgrade `tests\NuvTools.Common.Test\NuvTools.Common.Tests.csproj`
5. Run unit tests to validate upgrade in the projects listed below:
  - `tests\NuvTools.Common.Test\NuvTools.Common.Tests.csproj`

## Settings

### Excluded projects

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|


### Aggregate NuGet packages modifications across all projects

| Package Name                        | Current Version | New Version | Description                                   |
|:------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.Extensions.Logging.Abstractions |   9.0.10        |  10.0.0    | Recommended update for .NET 10.0              |

### Project upgrade details

#### src\NuvTools.Common\NuvTools.Common.csproj modifications

Project properties changes:
  - Target frameworks should be changed from `net8;net9` to `net8;net9;net10.0`

NuGet packages changes:
  - Microsoft.Extensions.Logging.Abstractions should be updated from `9.0.10` to `10.0.0`

Other changes:
  - Ensure code compatibility with .NET 10 if any API changes surface.


#### tests\NuvTools.Common.Test\NuvTools.Common.Tests.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - Update test-related packages if analysis identifies incompatibilities post-upgrade.

Other changes:
  - Run and fix tests as needed.
