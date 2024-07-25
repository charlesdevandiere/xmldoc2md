@echo off

dotnet build src/XMLDoc2Markdown/XMLDoc2Markdown.csproj -o ./out/XMLDoc2Markdown
dotnet build sample/MyClassLib/MyClassLib.csproj -o ./out/sample

./out/XMLDoc2Markdown/xmldoc2md.exe ./out/sample/MyClassLib.dll --output ./docs/sample --examples-path ./sample/docs/examples --github-pages --back-button
