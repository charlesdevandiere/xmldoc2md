# XMLDoc2Markdown

Tool to generate markdown from C# XML documentation, based on [MarkdownGenerator](https://github.com/neuecc/MarkdownGenerator) project.

[![Build Status](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_apis/build/status/charlesdevandiere.xmldoc2md?branchName=master)](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_build/latest?definitionId=2&branchName=master)

[![Nuget](https://img.shields.io/nuget/v/XMLDoc2Markdown.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)

## How to use

### Install tool

```shell
> dotnet tool install -g XMLDoc2Markdown
```

### Generate documentation

```shell
> xmldoc2md <DLL_SOURCE_PATH> <OUTPUT_DIRECTORY>
```

#### Example

```shell
> xmldoc2md Sample.dll docs
```

### Display command line help

```shell
> xmldoc2md -h
```

See complete documentation [here](https://charlesdevandiere.github.io/xmldoc2md/).
