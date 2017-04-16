#!/bin/bash
git submodule update --init --recursive
cd "IronPython/"
git pull origin ipy-2.7-maint
xbuild "Solutions/IronPython.sln" /p:Configuration=v45Debug

cd ../

git pull origin Mono
cp -ufR IronPython/bin/v45Debug/* Domo/Domo.ConsoleApp/bin/Debug/

xbuild Domo/Domo.sln /p:Configuration=DebugNoTesting
