#!/bin/bash
#Add the dotnet path to the path
export PATH="$HOME/.dotnet":$PATH

if [ -d "./artifacts" ]
then
    rm -Rf "./artifacts"; 
fi

dotnet restore

commitHash=$(git rev-parse --short HEAD)
suffix="-ci-local"
buildSuffix="$suffix-$commitHash"

echo "build: Version suffix is $buildSuffix"

dotnet build -c Release --version-suffix "$buildSuffix"  -v q /nologo

# echo "Running unit tests"
# dotnet test ./tests/UnitTests/UnitTests.csproj

echo "Starting docker containers"
docker-compose -f build/docker-compose-infrastructure.yml up -d

echo "Runing functional tests"
dotnet test ./test/FunctionalTests/FunctionalTests.csproj

echo "Finalizing docker containers"
docker-compose -f build/docker-compose-infrastructure.yml down
