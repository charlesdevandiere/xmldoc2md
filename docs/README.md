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

### Insert code example

You can insert custom code example into the documentation.

Create one file for each examples. Give them the full name of corresponding type, property, method,...

Add the CLI option: `--examples-path` with the path to examples files.

#### Examples

##### `MyClassLib.MyClass.md`

~~~markdown
## Example

Lorem ipsum...

```csharp
new MyClass();
```
~~~

##### `MyClassLib.MyClass.MyProperty.md`

~~~markdown
#### Example

Lorem ipsum...

```csharp
foo.MyProperty = "foo";
```
~~~

##### `MyClassLib.MyClass.MyMethod(System.String).md`

~~~markdown
#### Example

Lorem ipsum...

```csharp
foo.MyMethod("foo");
```
~~~

##### `MyClassLib.MyClass.#ctor.md`

~~~markdown
#### Example

Lorem ipsum...

```csharp
new MyClass();
```
~~~

### Display command line help

```shell
> xmldoc2md -h

Usage: xmldoc2md [arguments] [options]

Arguments:
  src  DLL source path
  out  Output directory

Options:
  -v|--version       Show version information
  -?|-h|--help       Show help information
  --index-page-name  Name of the index page (default: "index")
  --examples-path    Path to the code examples to insert in the documentation
```
