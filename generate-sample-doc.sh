#!/bin/bash

rm -rf ./out
find ./docs/sample -name "*.md" -type f -delete

dotnet publish src/XMLDoc2Markdown/XMLDoc2Markdown.csproj -o ./out/XMLDoc2Markdown --verbosity quiet
dotnet publish sample/MyClassLib/MyClassLib.csproj -o ./out/sample --verbosity quiet

./out/XMLDoc2Markdown/xmldoc2md ./out/sample/MyClassLib.dll --output ./docs/sample --examples-path ./sample/docs/examples --github-pages --back-button
