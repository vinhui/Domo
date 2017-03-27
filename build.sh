#!/bin/bash
git submodule update --init --recursive
cd "IronPython/"
git pull origin ipy-2.7-maint
xbuild "Solutions/IronPython.sln" /p:Configuration=Debug

cd ../

git pull
xbuild Domo/Domo.sln /p:Configuration=DebugNoTesting
