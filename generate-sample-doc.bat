@echo off
dotnet build
.\src\XMLDoc2Markdown\bin\Debug\net7.0\XMLDoc2Markdown.exe .\sample\MyClassLib\bin\Debug\netstandard2.0\MyClassLib.dll .\docs\sample --examples-path .\sample\docs\examples --github-pages --back-button
