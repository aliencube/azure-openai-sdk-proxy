Param(
    [string]
    [Parameter(Mandatory=$false)]
    $AzureEnvironmentName
)

$models = @(
    @{
        ModelName = "dall-e-3";
        ModelVersion = "3.0";
        Capacity = 1;
    },
    @{
        ModelName = "gpt-35-turbo-16k";
        ModelVersion = "0613";
        Capacity = 12;
    },
    @{
        ModelName = "gpt-4o";
        ModelVersion = "2024-05-13";
        Capacity = 12;
    },
    @{
        ModelName = "text-embedding-ada-002";
        ModelVersion = "2";
        Capacity = 120;
    }
)

$REPOSITORY_ROOT = git rev-parse --show-toplevel

# $models | ForEach-Object {
#     $model = $_
#     & "$REPOSITORY_ROOT/infra/scripts/New-OpenAIs.ps1" -AzureEnvironmentName $AzureEnvironmentName -ModelName "$($model.ModelName)" -ModelVersion "$($model.ModelVersion)" -Capacity $($model.Capacity)
# }

& "$REPOSITORY_ROOT/infra/scripts/New-OpenAIs.ps1" -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "gpt-35-turbo-16k" -ModelVersion "0613" -Capacity 12
& "$REPOSITORY_ROOT/infra/scripts/New-OpenAIs.ps1" -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "gpt-4o" -ModelVersion "2024-05-13" -Capacity 12
& "$REPOSITORY_ROOT/infra/scripts/New-OpenAIs.ps1" -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "text-embedding-ada-002" -ModelVersion "2" -Capacity 120
& "$REPOSITORY_ROOT/infra/scripts/New-OpenAIs.ps1" -AzureEnvironmentName $AZURE_ENV_NAME -ModelName "dall-e-3" -ModelVersion "3.0" -Capacity 1

& "$REPOSITORY_ROOT/infra/scripts/Get-OpenAIDetails.ps1" -AzureEnvironmentName $AZURE_ENV_NAME
