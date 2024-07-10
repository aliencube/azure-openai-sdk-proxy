# Set variables for GitHub Actions
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $GitHubAlias = $null,

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This sets variables for GitHub Actions

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

gh variable set -f ./.azure/$env:AZURE_ENV_NAME/.env
