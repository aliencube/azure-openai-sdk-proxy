# Run unit tests and integration tests
Param(
    [string]
    [Parameter(Mandatory=$false)]
    [ValidateSet("Debug", "Release")]
    $Configuration = "Debug",

    [switch]
    [Parameter(Mandatory=$false)]
    $KillPorts,

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Host "    This runs both unit tests and integration tests.

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-Configuration     <Configuration>] ``
            [-KillPorts] ``

            [-Help]

    Options:
        -Configuration      Configuration. Possible values are 'Debug' or 'Release'. Default is 'Debug'.
        -KillPorts          Kill the processes that are using the ports 21000 and 22000.

        -Help:              Show this message.
"

    Exit 0
}

if ($KillPorts -eq $true) {
    Write-Host "Killing any running ports..." -ForegroundColor Cyan

    $OTLP_PID = Get-NetTCPConnection -LocalPort 21000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | Get-Unique
    if ($OTLP_PID) {
        $OTLP_PID | ForEach-Object { Stop-Process -Id $_ -Force }
    }
    $SERVICE_PID = Get-NetTCPConnection -LocalPort 22000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | Get-Unique
    if ($SERVICE_PID) {
        $SERVICE_PID | ForEach-Object { Stop-Process -Id $_ -Force }
    }
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

# Generates OpenAPI document
Write-Host "Invoking OpenAPI lint..." -ForegroundColor Cyan

$OPENAPI_DOCVERSION = $(Get-Content ./src/AzureOpenAIProxy.ApiApp/appsettings.json | ConvertFrom-Json).OpenApi.DocVersion
Invoke-WebRequest -Uri "https://localhost:7001/swagger/$OPENAPI_DOCVERSION/swagger.json" -OutFile ./swagger.json

# Lints OpenAPI document
$SPECTRAL_PATH = Get-Command spectral -ErrorAction SilentlyContinue
if ($SPECTRAL_PATH -eq $null) {
    Write-Host "Spectral CLI has not been installed. Installing Spectral CLI first." -ForegroundColor Cyan
    Exit 1
}
spectral lint -F warn -D -q ./swagger.json

Write-Host "... Done" -ForegroundColor Cyan

# Cleans up
$process = if ($IsWindows -eq $true) {
    Get-Process | Where-Object { $_.ProcessName -eq "AzureOpenAIProxy.AppHost" }
} else {
    Get-Process | Where-Object { $_.Path -like "*AzureOpenAIProxy.AppHost" }
}
Stop-Process -Id $process.Id
$OTLP_PID = Get-NetTCPConnection -LocalPort 21000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | Get-Unique
if ($OTLP_PID) {
    $OTLP_PID | ForEach-Object { Stop-Process -Id $_ -Force -ErrorAction SilentlyContinue }
}
$SERVICE_PID = Get-NetTCPConnection -LocalPort 22000 -ErrorAction SilentlyContinue | Select-Object -ExpandProperty OwningProcess | Get-Unique
if ($SERVICE_PID) {
    $SERVICE_PID | ForEach-Object { Stop-Process -Id $_ -Force -ErrorAction SilentlyContinue }
}
