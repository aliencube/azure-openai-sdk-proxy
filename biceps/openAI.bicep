@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention, the name of the resource group for your application will use this name, prefixed with rg-')
param environmentName string

@description('List of tags for Azure resources')
param tags object = {
  'azd-env-name': environmentName
}

@description('The location where the resources will be deployed')
param location string = resourceGroup().location

@description('The OpenAI model to deploy')
param model object = {
  name: 'model-name'
  deploymentName: 'deployment-name'
  version: 'version'
  skuName: 'SKU'
  skuCapacity: 4
}

var resourceToken = uniqueString(resourceGroup().id)

var openai = {
  name: 'aoai-${resourceToken}-${model.deploymentName}-${location}'
  location: location
  tags: tags
  skuName: 'S0'
  model: model
}

resource aoai 'Microsoft.CognitiveServices/accounts@2023-05-01' = {
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
}

resource deployment 'Microsoft.CognitiveServices/accounts/deployments@2023-05-01' = {
  name: openai.model.deploymentName
  parent: aoai
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
}

output instance object = {
  id: aoai.id
  name: aoai.name
  endpoint: aoai.properties.endpoint
  apiKey: listKeys(aoai.id, '2023-05-01').key1
  deploymentName: deployment.name
}
