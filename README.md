# <img align="left" width="100" height="100" src="icon.png">XMLDoc2Markdown 

[![Build Status](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_apis/build/status/charlesdevandiere.xmldoc2md?branchName=master)](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_build/latest?definitionId=2&branchName=master)
[![Nuget](https://img.shields.io/nuget/v/XMLDoc2Markdown.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)

Tool to generate markdown from C# XML documentation.

See sample generated documentation [here](https://charlesdevandiere.github.io/xmldoc2md/).

## How to use

### Install tool

```shell
dotnet tool install -g XMLDoc2Markdown
```

### Generate documentation

```shell
xmldoc2md <DLL_SOURCE_PATH> <OUTPUT_DIRECTORY>
```

#### Example

```shell
xmldoc2md Sample.dll docs
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
xmldoc2md -h
```

```text
Usage: xmldoc2md [options] <src> <out>

Arguments:
  src                      DLL source path
  out                      Output directory

Options:
  -v|--version             Show version information.
  -?|-h|--help             Show help information.
  --index-page-name        Name of the index page, (default: "index").
  --examples-path          Path to the code examples to insert in the documentation.
  --github-pages           Remove '.md' extension from links for GitHub Pages.
  --gitlab-wiki            Remove '.md' extension and './' prefix from links for gitlab wikis.
  --back-button            Add a back button on each page with custom text, (default: "< Back").
  --link-back-button       Set link for back button, (default: "./").
  --private-members        Write documentation for private members.
  --onlyinternal-members   Write documentation for only internal members.
  --excludeinternal        Exclude documentation for internal types.
  --templatefile           Layout template for documentation, (default: "template.md").
  --back-index-button      Add a back button in index page, (default: "< Back").
  --link-backindex-button  Set link for back button in index page, (default: "./").
```

### Template Tokens

```text
{xmldoc2md-Title()}     = Title of Document
{xmldoc2md-Back()}      = back-button for pages 
{xmldoc2md-BackIndex()} = back-button for index page
{xmldoc2md-Body()}      = The Documentation
```

### Example Template 

```text
# <img align="left" width="100" height="100" src="MyImage.png">Custom Title :{xmldoc2md-Title()} 

{xmldoc2md-Back()}{xmldoc2md-BackIndex()}
- - -

{xmldoc2md-Body()}

- - -
{xmldoc2md-Back()}

```