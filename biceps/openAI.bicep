@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention, the name of the resource group for your application will use this name, prefixed with rg-')
param environmentName string

var tags = {
  'azd-env-name': environmentName
}

// GPT3.5 Turbo 16K available regions
var locations = [
  'australiaeast'
  'canadaeast'
  'eastus'
  'eastus2'
  'francecentral'
  'japaneast'
  'northcentralus'
//   'norwayeast'
//   'southindia'
  'swedencentral'
  'switzerlandnorth'
  'uksouth'
//   'westeurope'
//   'westus'
]

var openais = [for location in locations: {
  name: 'aoai-${environmentName}-${location}'
  location: location
  tags: tags
  skuName: 'S0'
  model: {
    name: 'gpt-35-turbo-16k'
    deploymentName: 'model-gpt35turbo16k'
    version: '0613'
    skuName: 'Standard'
    skuCapacity: 4
  }
}]

resource aoais 'Microsoft.CognitiveServices/accounts@2023-05-01' = [for openai in openais: {
  name: openai.name
  location: openai.location
  kind: 'OpenAI'
  tags: openai.tags
  sku: {
    name: openai.skuName
  }
  properties: {
    customSubDomainName: openai.name
    publicNetworkAccess: 'Enabled'
  }
}]

resource aoaiDeployments 'Microsoft.CognitiveServices/accounts/deployments@2023-05-01' = [for (openai, i) in openais : {
  name: openai.model.deploymentName
  parent: aoais[i]
  sku: {
    name: openai.model.skuName
    capacity: openai.model.skuCapacity
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: openai.model.name
      version: openai.model.version
    }
  }
}]

output aoaiInstances array = [for i in range(0, length(openais)): {
  id: aoais[i].id
  name: aoais[i].name
  endpoint: aoais[i].properties.endpoint
  apiKey: listKeys(aoais[i].id, '2023-05-01').key1
  deploymentName: aoaiDeployments[i].name
}]
