#!/bin/bash

SdkVersion="3.1.102"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
