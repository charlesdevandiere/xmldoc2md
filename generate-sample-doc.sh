#!/bin/bash

rm -rf ./out

dotnet build -o ./out

./out/XMLDoc2Markdown ./out/MyClassLib.dll ./docs/sample --examples-path ./sample/docs/examples --github-pages --back-button
