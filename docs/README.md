# XMLDoc2Markdown

Tool to generate markdown from C# XML documentation.

See sample generated documentation [here](sample).

## How to use

### Install tool

```shell
dotnet tool install -g XMLDoc2Markdown
```

### Generate documentation

```shell
dotnet xmldoc2md <src> [options]
```

| Argument | Description |
|---|---|
| `<src>` | DLL source path |

| Option | Description |
|---|---|
| `-o, --output <output>` | Output directory |
| `--index-page-name <index-page-name>` | Name of the index page [default: index] |
| `--examples-path <examples-path>` | Path to the code examples to insert in the documentation |
| `--github-pages` | Remove '.md' extension from links for GitHub Pages |
| `--gitlab-wiki` | Remove '.md' extension and './' prefix from links for gitlab wikis |
| `--back-button` | Add a back button on each page |
| `--member-accessibility-level <internal\|private\|protected\|public>` | Minimum accessibility level of members to be documented. [default: protected] |
| `--structure <flat\|tree>` | Documentation structure. [default: flat] |
| `--version` | Show version information |
| `-?, -h, --help` | Show help and usage information |

#### Example

```shell
dotnet xmldoc2md Sample.dll --output docs --github-pages --back-button
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
