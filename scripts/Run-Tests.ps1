# Run unit tests and integration tests
Param(
    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("Debug", "Release")]
    $Configuration = "Debug",

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Host "    This runs both unit tests and integration tests.

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-Configuration     <Configuration>] ``

            [-Help]

    Options:
        -Configuration      Configuration. Possible values are 'Debug' or 'Release'. Default is 'Debug'.

        -Help:              Show this message.
"

    Exit 0
}

# Builds apps
Write-Host "Building apps..." -ForegroundColor Cyan

dotnet restore
dotnet build -c $Configuration

# Runs unit tests
Write-Host "Invoking unit tests..." -ForegroundColor Cyan

dotnet test ./test/AzureOpenAIProxy.AppHost.Tests -c $Configuration --no-build --logger "trx" --collect:"XPlat Code Coverage"
dotnet test ./test/AzureOpenAIProxy.ApiApp.Tests -c $Configuration --no-build --logger "trx" --collect:"XPlat Code Coverage"

# Runs integration tests
Write-Host "Invoking integration tests..." -ForegroundColor Cyan

$playwright = Get-ChildItem -File Microsoft.Playwright.dll -Path . -Recurse
$installer = "$($playwright[0].Directory.FullName)/playwright.ps1"
& "$installer" install

Start-Process -NoNewWindow "dotnet" @("run", "--project", "./src/AzureOpenAIProxy.AppHost", "--no-build")
Start-Sleep -s 30

dotnet test ./test/AzureOpenAIProxy.PlaygroundApp.Tests -c $Configuration --logger "trx" --collect:"XPlat Code Coverage"

# Cleans up
$process = Get-Process | Where-Object { $_.Path -like "*AzureOpenAIProxy.AppHost" }
Stop-Process -Id $process.Id
