#!/bin/bash

export DBUS_SESSION_BUS_ADDRESS=unix:path=/run/user/$(id -u)/bus
export XDG_RUNTIME_DIR=/run/user/$(id -u)

if [ -z "$DBUS_SESSION_BUS_ADDRESS" ]; then
    eval $(dbus-launch --sh-syntax)
    export DBUS_SESSION_BUS_ADDRESS
fi

export LD_PRELOAD=$(pwd)/cef_binary/libHarfBuzzSharp.so:$(pwd)/cef_binary/libcef.so
ulimit -c unlimited
# gdb --args 
./bin/Debug/net8.0/Ailurus
# dotnet run
#
