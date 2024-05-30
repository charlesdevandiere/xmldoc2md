dotnet build
. "$PSScriptRoot/src/XMLDoc2Markdown/bin/Debug/net8.0/XMLDoc2Markdown" "$PSScriptRoot/sample/MyClassLib/bin/Debug/netstandard2.0/MyClassLib.dll" "$PSScriptRoot/docs/sample" --examples-path "$PSScriptRoot/sample/docs/examples" --github-pages --back-button
