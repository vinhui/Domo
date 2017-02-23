cd IronPython
git pull
cmd /C "xbuild Solutions/IronPython.sln /p:Configuration=Debug"
cd ../

git pull
cmd /C "xbuild Domo/Domo.sln /p:Configuration=Debug"

xcopy ".\IronPython\bin\Debug\*" ".\Domo\Domo.ConsoleApp\bin\Debug\" /S /Y
