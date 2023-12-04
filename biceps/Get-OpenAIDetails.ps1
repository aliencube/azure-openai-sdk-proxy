# Gets list of the Azure OpenAI instances with specific deployment name
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $EnvironmentName = $null,

    [string]
    [Parameter(Mandatory=$false)]
    $DeploymentName = $null,

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This gets list of the Azure OpenAI instances with specific deployment name

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-EnvironmentName <Azure environment name>] ``
            [-DeploymentName <Azure OpenAI deployment name>] ``

            [-Help]

    Options:
        -EnvironmentName    Azure environment name.
        -DeploymentName     Azure OpenAI deployment name.

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

if ($EnvironmentName -eq $null) {
    Show-Usage
    Exit 0
}

$rg = "rg-$EnvironmentName"

$openAIs = az resource list -g $rg --query "[?type=='Microsoft.CognitiveServices/accounts'].name" | ConvertFrom-Json | Sort-Object
if ($DeploymentName -ne $null) {
    $openAIs = $openAIs | Where-Object { $_ -like "*$DeploymentName*" }
}

$instances = @()
$openAIs | ForEach-Object {
    $name = $_
    $endpoint = az cognitiveservices account show -g $rg -n $name --query "properties.endpoint" -o tsv
    $apiKey = az cognitiveservices account keys list -g $rg -n $name --query "key1" -o tsv
    $deploymentName = az cognitiveservices account deployment list -g $rg -n $name --query "[].name" -o tsv

    $instance = @{ Endpoint = $endpoint; ApiKey = $apiKey; DeploymentName = $deploymentName }
    $instances += $instance
}

$instances | ConvertTo-Json -Depth 100 | Out-File -FilePath ./biceps/instances-$DeploymentName.json -Encoding utf8 -Force
