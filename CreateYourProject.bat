echo "if you install template error,please email to 'feihuazhuyu@126.com'"

dotnet new install Wpf.MvvmLight.SelfHost.Template

set /p OP=Please set your project name(for example:Range.Demo):


md .YourProject

cd .YourProject

dotnet new wmstpl -n %OP%

cd ../


echo "Create project on yourself successfully!!!! ^ please see the folder .YourProject"

dotnet new uninstall Wpf.MvvmLight.SelfHost.Template


echo "Delete Template Successfully"

pause
