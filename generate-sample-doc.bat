@echo off

dotnet build -o ./out

./out/XMLDoc2Markdown.exe ./out/MyClassLib.dll ./docs/sample --examples-path ./sample/docs/examples --github-pages --back-button
