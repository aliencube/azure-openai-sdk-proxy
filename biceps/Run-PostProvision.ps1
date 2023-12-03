# Run post-provisioning tasks
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $GitHubAlias = $null,

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This runs the post-provisioning tasks

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-GitHubAlias <GitHub alias>] ``

            [-Help]

    Options:
        -GitHubAlias    GitHub username. eg) 'aliencube' of https://github.com/aliencube. Default is `$null.

        -Help:          Show this message.
"

    Exit 0
}

# Show usage
$needHelp = $Help -eq $true
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

if ($GitHubAlias -ne $null) {
    $vars = gh api repos/$GitHubAlias/azure-openai-sdk-proxy/actions/variables | ConvertFrom-Json
    $env:AZURE_ENV_NAME = $($vars.variables | Where-Object { $_.name -eq "AZURE_ENV_NAME" }).value
}

$rg = "rg-$env:AZURE_ENV_NAME"

# Provision AOAI instances
Write-Output "Provisioning Azure OpenAI instances ..."

$provisioned = az deployment group create -n aoai -g $rg `
    --template-file ./biceps/openAIModels.bicep `
    --parameters environmentName=$env:AZURE_ENV_NAME | ConvertFrom-Json

Write-Output "... Provisioned"

$instances = $provisioned.properties.outputs.instances.value | `
    Select-Object -Property @{ Name = "Endpoint"; Expression = "endpoint" }, @{ Name = "ApiKey"; Expression = "apiKey" }, @{ Name = "DeploymentName"; Expression = "deploymentName" }

# Get resource token
Write-Output "Updating Azure Table Storage ..."

$token = az deployment group create -n resourceToken -g $rg `
    --template-file ./biceps/resourceGroup.bicep | ConvertFrom-Json

$token = $token.properties.outputs.resourceToken.value

# Get connection string
$connectionString = az storage account show-connection-string -g $rg -n storage$token --query "connectionString" -o tsv

# Add tables to table storage
$table = az storage table create -n managements --connection-string $connectionString
$table = az storage table create -n accesscodes --connection-string $connectionString

# Add master entity to table
$rowKey = $(New-Guid).Guid                                                                                                                                       
$apiKey = $(New-Guid).Guid 
$upserted = az storage entity insert -t managements --connection-string $connectionString `
    -e PartitionKey='master' RowKey=$rowKey `
        EventId=$rowKey EventName='master' `
        EventDescription='Master key of Azure OpenAI Proxy Service' `
        EventOrganiser='Azure OpenAI Proxy' `
        EventOrganiserEmail='proxy@azure.openai' `
        ApiKey=$apiKey

Write-Output "... Updated"

# Update appsettings.Development.json
Write-Output "Updating appsettings.Development.json ..."

Copy-Item -Path ./src/AzureOpenAIProxy.AppHost/appsettings.Development.sample.json `
          -Destination ./src/AzureOpenAIProxy.AppHost/appsettings.Development.json -Force

$appsettings = Get-Content -Path ./src/AzureOpenAIProxy.AppHost/appsettings.Development.json | ConvertFrom-Json
$appsettings.ConnectionStrings.table = $connectionString
$appsettings.AOAI.Instances = $instances
$appsettings | ConvertTo-Json -Depth 100 | Out-File -Path ./src/AzureOpenAIProxy.AppHost/appsettings.Development.json -Force

Write-Output "... Updated"
