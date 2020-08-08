@echo off
dotnet publish .\sample\MyClassLib\MyClassLib.csproj -c Release -o publish
dotnet run -p .\src\XMLDoc2Markdown\XMLDoc2Markdown.csproj .\publish\MyClassLib.dll .\docs\sample
