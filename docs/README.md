# XMLDoc2Markdown

Tool to generate markdown from C# XML documentation, based on [MarkdownGenerator](https://github.com/neuecc/MarkdownGenerator) project.

See generated sample documentation [here](sample).

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
