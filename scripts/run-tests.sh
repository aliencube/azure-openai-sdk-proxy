#!/bin/bash
# Run unit tests and integration tests

set -e

# Function to show usage
usage() {
    echo "    This runs both unit tests and integration tests.

    Usage: $0 [-c|--config|--configuration <Configuration>] [-k|--kill-ports] [-h|--help]

    Options:
        -c|--config|--configuration     Configuration. Possible values are 'Debug' or 'Release'. Default is 'Debug'.
        -k|--kill-ports                 Kills any running ports.
        -h|--help                       Show this message.
"
    exit 0
}

# Default configuration
CONFIGURATION="Debug"
KILL_PORTS=

if [[ $# -eq 0 ]]; then
    CONFIGURATION="Debug"
    KILL_PORTS=
fi

while [[ "$1" != "" ]]; do
    case $1 in
        -c | --config | --configuration)
            shift
            CONFIGURATION="$1"
        ;;

        -k | --kill-ports)
            KILL_PORTS="true"
        ;;

        -h | --help)
            usage
            exit 0
        ;;

        *)
            usage
            exit 0
        ;;
    esac

    shift
done

# Kills any running ports
if [ -n "$KILL_PORTS" ]; then
    echo -e "\033[36mKilling any running ports...\033[0m"

    OTLP_PID=$(lsof -t -i:21000)
    if [ "$OTLP_PID" != "" ]; then
        kill -9 $OTLP_PID
    fi
    SERVICE_PID=$(lsof -t -i:22000)
    if [ "$SERVICE_PID" != "" ]; then
        kill -9 $SERVICE_PID
    fi
fi

# Builds apps
echo -e "\033[36mBuilding apps...\033[0m"

dotnet restore
dotnet build -c $CONFIGURATION

# Runs unit tests
echo -e "\033[36mInvoking unit tests...\033[0m"

dotnet test ./test/AzureOpenAIProxy.AppHost.Tests -c $CONFIGURATION --no-build --logger "trx" --collect:"XPlat Code Coverage"
dotnet test ./test/AzureOpenAIProxy.ApiApp.Tests -c $CONFIGURATION --no-build --logger "trx" --collect:"XPlat Code Coverage"

# Runs integration tests
echo -e "\033[36mInvoking integration tests...\033[0m"

pwsh ./test/AzureOpenAIProxy.PlaygroundApp.Tests/bin/Debug/net8.0/playwright.ps1 install

dotnet run --project ./src/AzureOpenAIProxy.AppHost --no-build &
APP_PID=$!
sleep 30

dotnet test ./test/AzureOpenAIProxy.PlaygroundApp.Tests -c $CONFIGURATION --logger "trx" --collect:"XPlat Code Coverage"

# Generates OpenAPI document
echo -e "\033[36mInvoking OpenAPI lint...\033[0m"

OPENAPI_DOCVERSION=$(cat ./src/AzureOpenAIProxy.ApiApp/appsettings.json | jq -r ".OpenApi.DocVersion")
curl https://localhost:7001/swagger/$OPENAPI_DOCVERSION/swagger.json > ./swagger.json

# Lints OpenAPI document
SPECTRAL_PATH=$(which spectral)
if [ -z "$SPECTRAL_PATH" ]; then
    echo -e "\033[36mSpectral CLI has not been installed. Installing Spectral CLI first.\033[0m"
    exit 1
fi
spectral lint -F warn -D -q ./swagger.json

# Cleans up
kill $APP_PID
OTLP_PID=$(lsof -t -i:21000)
if [ "$OTLP_PID" != "" ]; then
    kill -9 $OTLP_PID
fi
SERVICE_PID=$(lsof -t -i:22000)
if [ "$SERVICE_PID" != "" ]; then
    kill -9 $SERVICE_PID
fi
