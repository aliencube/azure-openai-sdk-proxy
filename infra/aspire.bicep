// The main bicep module to provision Azure resources.
// For a more complete walkthrough to understand how this file works with azd,
// see https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/make-azd-compatible?pivots=azd-create

@minLength(1)
@maxLength(64)
@description('Name of the the environment which is used to generate a short unique hash used in all resources.')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

// Parameters for Key Vault
param keyVaultName string = ''
param enabledForDeployment bool = true
param enabledForTemplateDeployment bool = true
param enableRbacAuthorization bool = true

var abbrs = loadJsonContent('./abbreviations.json')

// Tags that should be applied to all resources.
var tags = {
  // Tag all resources with the environment name.
  'azd-env-name': environmentName
}

// Generate a unique token to be used in naming resources.
// Remove linter suppression after using.
#disable-next-line no-unused-vars
// var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var resourceToken = uniqueString(resourceGroup().id)

// Name of the service defined in azure.yaml
// A tag named azd-service-name with this value should be applied to the service host resource, such as:
//   Microsoft.Web/sites for appservice, function
// Example usage:
//   tags: union(tags, { 'azd-service-name': apiServiceName })
#disable-next-line no-unused-vars
// var apiServiceName = 'python-api'


// Define tables for the storage account
param tables array = [
    {
        name: 'events'
    }
]

// Add resources to be provisioned below.

// Provision Key Vault
module keyVault './core/security/keyvault.bicep' = {
  name: 'keyVault'
  params: {
    name: !empty(keyVaultName) ? keyVaultName : '${abbrs.keyVaultVaults}${resourceToken}'
    location: location
    tags: tags
    enabledForDeployment: enabledForDeployment
    enabledForTemplateDeployment: enabledForTemplateDeployment
    enableRbacAuthorization: enableRbacAuthorization
  }
}

// Provision Storage Account
module storageAccount 'core/storage/storage-account.bicep' = {
    name: 'storageAccount'
    params: {
        name: 'st${resourceToken}'
        location: location
        tags: tags
        tables: tables
    }
}

// Add outputs from the deployment here, if needed.
//
// This allows the outputs to be referenced by other bicep deployments in the deployment pipeline,
// or by the local machine as a way to reference created resources in Azure for local development.
// Secrets should not be added here.
//
// Outputs are automatically saved in the local azd environment .env file.
// To see these outputs, run `azd env get-values`,  or `azd env get-values --output json` for json output.
output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId

output AZURE_KEYVAULT_NAME string = keyVault.outputs.name
output AZURE_KEYVAULT_ENDPOINT string = keyVault.outputs.endpoint
