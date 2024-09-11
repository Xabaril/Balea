#!/bin/bash


SdkVersion="6.0.424"
./dotnet-install.sh -Version $SdkVersion
SdkVersion="8.0.303"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
