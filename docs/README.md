# XMLDoc2Markdown

Tool to generate markdown from C# XML documentation, based on [MarkdownGenerator](https://github.com/neuecc/MarkdownGenerator) project.

See sample generated documentation [here](sample).

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

Usage: xmldoc2md [arguments] [options]

Arguments:
  src  DLL source path
  out  Output directory

Options:
  -v|--version               Show version information
  -?|-h|--help               Show help information
  --namespace-match <regex>  Regex pattern to select namespaces
  --index-page-name <regex>  Name of the index page (default: "index")

```
