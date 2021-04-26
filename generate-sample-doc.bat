@echo off
dotnet publish .\sample\MyClassLib\MyClassLib.csproj -c Release -o publish
dotnet build .\src\XMLDoc2Markdown\XMLDoc2Markdown.csproj -c Release
.\src\XMLDoc2Markdown\bin\Release\net5\XMLDoc2Markdown.exe .\publish\MyClassLib.dll .\docs\sample --examples-path .\sample\docs\examples
