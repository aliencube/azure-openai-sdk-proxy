metadata description = 'Creates an Azure storage account.'
param name string
param location string = resourceGroup().location
param tags object = {}

@allowed([
  'Cool'
  'Hot'
  'Premium' ])
param accessTier string = 'Hot'
param allowBlobPublicAccess bool = true
param allowCrossTenantReplication bool = true
param allowSharedKeyAccess bool = true
param containers array = []
param corsRules array = []
param defaultToOAuthAuthentication bool = false
param deleteRetentionPolicy object = {}
@allowed([ 'AzureDnsZone', 'Standard' ])
param dnsEndpointType string = 'Standard'
param files array = []
param kind string = 'StorageV2'
param minimumTlsVersion string = 'TLS1_2'
param queues array = []
param shareDeleteRetentionPolicy object = {}
param supportsHttpsTrafficOnly bool = true
param tables array = []
param networkAcls object = {
  bypass: 'AzureServices'
  defaultAction: 'Allow'
}
@allowed([ 'Enabled', 'Disabled' ])
param publicNetworkAccess string = 'Enabled'
param sku object = { name: 'Standard_LRS' }
param keyVaultName string = ''

resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: name
  location: location
  tags: tags
  kind: kind
  sku: sku
  properties: {
    accessTier: accessTier
    allowBlobPublicAccess: allowBlobPublicAccess
    allowCrossTenantReplication: allowCrossTenantReplication
    allowSharedKeyAccess: allowSharedKeyAccess
    defaultToOAuthAuthentication: defaultToOAuthAuthentication
    dnsEndpointType: dnsEndpointType
    minimumTlsVersion: minimumTlsVersion
    networkAcls: networkAcls
    publicNetworkAccess: publicNetworkAccess
    supportsHttpsTrafficOnly: supportsHttpsTrafficOnly
  }

  resource blobServices 'blobServices' = if (!empty(containers)) {
    name: 'default'
    properties: {
      cors: {
        corsRules: corsRules
      }
      deleteRetentionPolicy: deleteRetentionPolicy
    }
    resource container 'containers' = [for container in containers: {
      name: container.name
      properties: {
        // todo: Warning use-safe-access: Use the safe access (.?) operator instead of checking object contents with the 'contains' function. [https://aka.ms/bicep/linter/use-safe-access]
        publicAccess: contains(container, 'publicAccess') ? container.publicAccess : 'None'
      }
    }]
  }

  resource fileServices 'fileServices' = if (!empty(files)) {
    name: 'default'
    properties: {
      cors: {
        corsRules: corsRules
      }
      shareDeleteRetentionPolicy: shareDeleteRetentionPolicy
    }
  }

  resource queueServices 'queueServices' = if (!empty(queues)) {
    name: 'default'
    properties: {

    }
    resource queue 'queues' = [for queue in queues: {
      name: queue.name
      properties: {
        metadata: {}
      }
    }]
  }

  resource tableServices 'tableServices' = if (!empty(tables)) {
    name: 'default'
    properties: {}
    // create tables pre-defined in aspire.bicep
    resource table 'tables' = [for table in tables: {
      name: table
      properties: {}
    }]
  }
}

// Save Storage Account Connection String in Key Vault Secret
module keyVaultSecrets '../../core/security/keyvault-secret.bicep' = {
    name: 'keyVaultSecrets'
    params: {
        name: 'storage-connection-string'
        secretValue:'DefaultEndpointsProtocol=https;EndpointSuffix=${environment().suffixes.storage};AccountName=${storage.name};AccountKey=${storage.listKeys().keys[0].value};BlobEndpoint=${storage.properties.primaryEndpoints.blob};FileEndpoint=${storage.properties.primaryEndpoints.file};QueueEndpoint=${storage.properties.primaryEndpoints.queue};TableEndpoint=${storage.properties.primaryEndpoints.table}'
        keyVaultName:keyVaultName
    }
}

output id string = storage.id
output name string = storage.name
output primaryEndpoints object = storage.properties.primaryEndpoints
