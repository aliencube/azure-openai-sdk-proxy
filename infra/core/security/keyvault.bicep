metadata description = 'Creates an Azure Key Vault.'
param name string
param location string = resourceGroup().location
param tags object = {}

param principalId string = ''

@description('Allow the key vault to be used during resource creation.')
param enabledForDeployment bool = false
@description('Allow the key vault to be used for template deployment.')
param enabledForTemplateDeployment bool = false
@description('Allow the key vault to be used for disk encryption.')
param enabledForDiskEncryption bool = false
@description('Enable RBAC authorization for the key vault.')
param enableRbacAuthorization bool = true
@description('Enable soft delete for the key vault.')
param enableSoftDelete bool = true
@description('The number of days to retain soft deleted keys.')
param softDeleteRetentionInDays int = 90

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    tenantId: subscription().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
    accessPolicies: !empty(principalId) ? [
      {
        objectId: principalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
        tenantId: subscription().tenantId
      }
    ] : []
    enabledForDeployment: enabledForDeployment
    enabledForTemplateDeployment: enabledForTemplateDeployment
    enabledForDiskEncryption: enabledForDiskEncryption
    enableRbacAuthorization: enableRbacAuthorization
    enableSoftDelete: enableSoftDelete
    softDeleteRetentionInDays: softDeleteRetentionInDays
  }
}

output endpoint string = keyVault.properties.vaultUri
output id string = keyVault.id
output name string = keyVault.name
