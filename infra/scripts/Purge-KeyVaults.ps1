# Purges the deleted the Key Vault instances.
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $ApiVersion = "2023-07-01",

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This permanently deletes the Key Vault instances

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-ApiVersion <API version>] ``

            [-Help]

    Options:
        -ApiVersion     REST API version. Default is `2023-07-01`.

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

# List soft-deleted Key Vault instances
function List-DeletedKeyVaults {
    param (
        [string] $ApiVersion
    )

    $account = $(az account show | ConvertFrom-Json)

    $url = "https://management.azure.com/subscriptions/$($account.id)/providers/Microsoft.KeyVault/deletedVaults?api-version=$($ApiVersion)"

    # Uncomment to debug
    # $url

    $options = ""

    $kvs = $(az rest -m get -u $url --query "value" | ConvertFrom-Json)
    if ($kvs -eq $null) {
        $options = "All soft-deleted Key Vault instances purged or no such instance found to purge"
        $returnValue = @{ kvs = $kvs; options = $options }
        return $returnValue
    }

    if ($kvs.Count -eq 1) {
        $name = $kvs.name
        $options += "    1: $name `n"
    } else {
        $kvs | ForEach-Object {
            $i = $kvs.IndexOf($_)
            $name = $_.name
            $options += "    $($i +1): $name `n"
        }
    }
    $options += "    a: Purge all`n"
    $options += "    q: Quit`n"

    $returnValue = @{ kvs = $kvs; options = $options }
    return $returnValue
}

# Purge all soft-deleted Key Vault instances at once.
function Purge-AllDeletedKeyVaults {
    param (
        [string] $ApiVersion,
        [object[]] $Instances
    )

    Process {
        $Instances | ForEach-Object {
            Write-Output "Purging $($_.name) ..."

            $url = "https://management.azure.com$($_.id)/purge?api-version=$($ApiVersion)"
    
            $kv = $(az rest -m get -u $($url -replace "/purge", ""))
            if ($kv -ne $null) {
                $deleted = $(az rest -m post -u $url)
            }

            Write-Output "... $($_.name) purged"
        }

        Write-Output "All soft-deleted Key Vault instances purged"
    }
}


# Purge soft-deleted Key Vault instances
function Purge-DeletedKeyVaults {
    param (
        [string] $ApiVersion
    )

    $continue = $true
    $result = List-DeletedKeyVaults -ApiVersion $ApiVersion
    if ($result.kvs -eq $null) {
        $continue = $false
    }

    while ($continue -eq $true) {
        $options = $result.options

        $input = Read-Host "Select the number to purge the soft-deleted Key Vault instance, 'a' to purge all or 'q' to quit: `n`n$options"
        if ($input -eq "q") {
            $continue = $false
            break
        }

        if ($input -eq "a") {
            Purge-AllDeletedKeyVaults -ApiVersion $ApiVersion -Instances $result.kvs
            break
        }

        $parsed = $input -as [int]
        if ($parsed -eq $null) {
            Write-Output "Invalid input"
            $continue = $false
            break
        }

        $kvs = $result.kvs
        if ($parsed -gt $kvs.Count) {
            Write-Output "Invalid input"
            $continue = $false
            break
        }

        $index = $parsed - 1

        $url = "https://management.azure.com$($kvs[$index].id)/purge?api-version=$($ApiVersion)"

        # Uncomment to debug
        # $url

        $kv = $(az rest -m get -u $($url -replace "/purge", ""))
        if ($kv -ne $null) {
            $deleted = $(az rest -m post -u $url)
        }

        $result = List-DeletedKeyVaults -ApiVersion $ApiVersion
        if ($result.kvs -eq $null) {
            $continue = $false
        }
    }

    if ($continue -eq $false) {
        return $result.options
    }
}

Purge-DeletedKeyVaults -ApiVersion $ApiVersion
