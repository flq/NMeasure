mkdir out
%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe NMeasure.sln /p:Configuration=Release
copy NMeasure\bin\Release\Nmeasure.dll out\Nmeasure.dll
pause