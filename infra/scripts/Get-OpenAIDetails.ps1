# Gets list of the Azure OpenAI instances with specific deployment name
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $AzureEnvironmentName = $null,

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This gets list of the Azure OpenAI instances with specific deployment name

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-AzureEnvironmentName  <Azure environment name>] ``

            [-Help]

    Options:
        -AzureEnvironmentName   Azure environment name.

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

if ($AzureEnvironmentName -eq $null) {
    Show-Usage
    Exit 0
}

$rg = "rg-$AzureEnvironmentName"
$repositoryRoot = git rev-parse --show-toplevel

$openAIs = az resource list -g $rg --query "[?type=='Microsoft.CognitiveServices/accounts'].name" | ConvertFrom-Json | Sort-Object

$instances = @()
$openAIs | ForEach-Object {
    $name = $_
    $endpoint = az cognitiveservices account show -g $rg -n $name --query "properties.endpoint" -o tsv
    $apiKey = az cognitiveservices account keys list -g $rg -n $name --query "key1" -o tsv
    $deploymentNames = az cognitiveservices account deployment list -g $rg -n $name --query "[].name" | ConvertFrom-Json
    if ($deploymentNames.Count -eq 1) {
        $deploymentNames = @( $deploymentNames )
    }

    $instance = @{ Endpoint = $endpoint; ApiKey = $apiKey; DeploymentNames = $deploymentNames }
    $instances += $instance
}

$instances | ConvertTo-Json -Depth 100 | Out-File -FilePath "$repositoryRoot/infra/instances.json" -Encoding utf8 -Force
