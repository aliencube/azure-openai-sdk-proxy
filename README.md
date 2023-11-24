# Azure OpenAI Service Proxy

This provides a proxy server application of Azure OpenAI Service API that round-robins multiple Azure OpenAI Service instances.

## Prerequisites

- .NET SDK 8.0 or later + Aspire workload
- Visual Studio 2022 17.9+ or Visual Studio Code + C# DevKit
- Azure Subscription
- Azure OpenAI Subscription

## Getting Started

1. azd init
2. azd provision
3. azd pipeline config
4. Set-GitHubActionsVariables.ps1
5. Run-PostProvision.ps1
6. azd deploy

7. Create a new event
8. Create a new access code that belongs to the event

9. azd down
10. Purge-CognitiveServices.ps1

