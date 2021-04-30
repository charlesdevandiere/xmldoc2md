@echo off
dotnet build .\sample\MyClassLib\MyClassLib.csproj -c Release
dotnet build .\src\XMLDoc2Markdown\XMLDoc2Markdown.csproj -c Release
.\src\XMLDoc2Markdown\bin\Release\net5.0\XMLDoc2Markdown.exe .\sample\MyClassLib\bin\Release\netstandard2.0\MyClassLib.dll .\docs\sample --examples-path .\sample\docs\examples --github-pages --back-button
