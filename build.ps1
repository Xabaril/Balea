# Taken from psake https://github.com/psake/psake

<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec {
  [CmdletBinding()]
  param(
    [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
    [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
  )
  & $cmd
  if ($lastexitcode -ne 0) {
    throw ("Exec: " + $errorMessage)
  }
}

if (Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

$suffix = "-ci-local"
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = "$($suffix)-$($commitHash)"

Write-Output "build: Version suffix is $buildSuffix"

exec { & dotnet build Balea.sln -c Release --version-suffix=$buildSuffix -v q /nologo }
	
Write-Output "Starting docker containers"

exec { & docker-compose -f build\docker-compose-infrastructure.yml up -d }

Write-Output "Running functional tests [NETCOREAPP3.1]"

try {

  Push-Location -Path .\test\FunctionalTests
  exec { & dotnet test --framework netcoreapp3.1}
}
finally {
  Pop-Location
}

Write-Output "Running functional tests [NET5.0]"

try {

  Push-Location -Path .\test\FunctionalTests
  exec { & dotnet test --framework net5.0}
}
finally {
  Pop-Location
}


Write-Output "Finalizing docker containers"
exec { & docker-compose -f build\docker-compose-infrastructure.yml down }

exec { & dotnet pack .\src\Balea\Balea.csproj -c Release -o .\artifacts --include-symbols --no-build --version-suffix=$buildSuffix }
