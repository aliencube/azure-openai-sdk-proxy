@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention, the name of the resource group for your application will use this name, prefixed with rg-')
param environmentName string

var tags = {
  'azd-env-name': environmentName
}

// GPT3.5 Turbo 16K available regions
var locationsGpt35Turbo16k = [
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

// GPT4 32K available regions
var locationsGpt432k = [
  'australiaeast'
  'canadaeast'
  'eastus'
//   'eastus2'
  'francecentral'
//   'japaneast'
//   'northcentralus'
//   'norwayeast'
//   'southindia'
  'swedencentral'
  'switzerlandnorth'
//   'uksouth'
//   'westeurope'
//   'westus'
]

module gpt35Turbo16ks 'openAI.bicep' = [for location in locationsGpt35Turbo16k: {
  name: 'AOAI_GPT35TURBO16K'
  params: {
    environmentName: environmentName
    location: location
    tags: tags
    model: {
      name: 'gpt-35-turbo-16k'
      deploymentName: 'model-gpt35turbo16k'
      version: '0613'
      skuName: 'Standard'
      skuCapacity: 4
    }
  }
}]

module gpt432ks 'openAI.bicep' = [for location in locationsGpt432k: {
  name: 'AOAI_GPT432K'
  params: {
    environmentName: environmentName
    location: location
    tags: tags
    model: {
      name: 'gpt-4-32k'
      deploymentName: 'model-gpt432k'
      version: '0613'
      skuName: 'Standard'
      skuCapacity: 4
    }
  }
}]

output instancesGpt35Turbo16k array = [for i in range(0, length(locationsGpt35Turbo16k)): {
  id: gpt35Turbo16ks[i].outputs.instance.id
  name: gpt35Turbo16ks[i].outputs.instance.name
  endpoint: gpt35Turbo16ks[i].outputs.instance.endpoint
  apiKey: gpt35Turbo16ks[i].outputs.instance.apiKey
  deploymentName: 'model-gpt35turbo16k'
}]

output instancesGpt432k array = [for i in range(0, length(locationsGpt432k)): {
  id: gpt432ks[i].outputs.instance.id
  name: gpt432ks[i].outputs.instance.name
  endpoint: gpt432ks[i].outputs.instance.endpoint
  apiKey: gpt432ks[i].outputs.instance.apiKey
  deploymentName: 'model-gpt432k'
}]
