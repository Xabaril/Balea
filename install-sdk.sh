#!/bin/bash

SdkVersion="3.1.404"
./dotnet-install.sh -Version $SdkVersion
SdkVersion="5.0.101"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
