@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention, the name of the resource group for your application will use this name, prefixed with rg-')
param environmentName string

param tags object = {
  'azd-env-name': environmentName
}

param locations array = []

param model object = {
  name: 'model-name'
  deploymentName: 'deployment-name'
  version: 'version'
  skuName: 'SKU'
  skuCapacity: 4
}

module openAIs 'openAI.bicep' = [for location in locations: {
  name: 'AOAI_${model.deploymentName}_${location}'
  params: {
    environmentName: environmentName
    location: location
    tags: tags
    model: model
  }
}]

output instances array = [for i in range(0, length(locations)): {
  id: openAIs[i].outputs.instance.id
  name: openAIs[i].outputs.instance.name
  endpoint: openAIs[i].outputs.instance.endpoint
  apiKey: openAIs[i].outputs.instance.apiKey
  deploymentName: model.deploymentName
}]
