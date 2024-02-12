# Azure OpenAI Service Proxy

This provides a proxy server application of Azure OpenAI Service API that round-robins multiple Azure OpenAI Service instances.

## Prerequisites

- .NET SDK 8.0.100 or later + Aspire workload
- Visual Studio 2022 17.9+ or Visual Studio Code + C# DevKit
- Azure Subscription
- Azure OpenAI Subscription

## Getting Started

1. Run PowerShell script to prepare AOAI instance locations and models &ndash; `ADA`, `GPT 3.5 Turbo 16K`, `GPT 4 32K` and `DALL-E 3`.

```powershell
$RESOURCE_GROUP_LOCATION = "{{ RESOURCE_GROUP_LOCATION }}"
$AZURE_ENV_NAME = "{{ AZURE_ENVIRONMENT_NAME }}"

# Text embedding with Ada
./biceps/Get-OpenAILocations.ps1 -ResourceGroupLocation $RESOURCE_GROUP_LOCATION -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "text-embedding-ada-002" -ModelVersion "2"

# GPT 3.5 Turbo 16K
./biceps/Get-OpenAILocations.ps1 -ResourceGroupLocation $RESOURCE_GROUP_LOCATION -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "gpt-35-turbo-16k" -ModelVersion "0613"

# GPT 4 32K
./biceps/Get-OpenAILocations.ps1 -ResourceGroupLocation $RESOURCE_GROUP_LOCATION -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "gpt-4-32k" -ModelVersion "0613"

# DALL-E 3.0
./biceps/Get-OpenAILocations.ps1 -ResourceGroupLocation $RESOURCE_GROUP_LOCATION -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "dall-e-3" -ModelVersion "3.0"
```

1. azd init
1. azd provision
1. azd pipeline config
1. Set-GitHubActionsVariables.ps1
1. Run-PostProvision.ps1
1. azd deploy

1. Create a new event
1. Create a new access code that belongs to the event

1. azd down
1. Purge-CognitiveServices.ps1

