#!/bin/bash
# Run unit tests and integration tests

set -e

# Function to show usage
usage() {
    echo "    This runs both unit tests and integration tests.

    Usage: $0 [-c|--config|--configuration <Configuration>] [-h|--help]

    Options:
        -c|--config|--configuration     Configuration. Possible values are 'Debug' or 'Release'. Default is 'Debug'.
        -h|--help                       Show this message.
"
    exit 0
}

# Default configuration
CONFIGURATION="Debug"

if [[ $# -eq 0 ]]; then
    CONFIGURATION="Debug"
fi

while [[ "$1" != "" ]]; do
    case $1 in
        -c | --config | --configuration)
            shift
            CONFIGURATION="$1"
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

# Builds apps
echo -e "\e[36mBuilding apps...\e[0m"

dotnet restore
dotnet build -c $CONFIGURATION

# Runs unit tests
echo -e "\e[36mInvoking unit tests...\e[0m"

dotnet test ./test/AzureOpenAIProxy.AppHost.Tests -c $CONFIGURATION --no-build --logger "trx" --collect:"XPlat Code Coverage"
dotnet test ./test/AzureOpenAIProxy.ApiApp.Tests -c $CONFIGURATION --no-build --logger "trx" --collect:"XPlat Code Coverage"

# Runs integration tests
echo -e "\e[36mInvoking integration tests...\e[0m"

dotnet run --project ./src/AzureOpenAIProxy.AppHost --no-build &
APP_PID=$!
sleep 30

dotnet test ./test/AzureOpenAIProxy.PlaygroundApp.Tests -c $CONFIGURATION --logger "trx" --collect:"XPlat Code Coverage"

# Cleans up
kill $APP_PID
