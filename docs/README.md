# XMLDoc2Markdown

Tool to generate markdown from C# XML documentation.

See sample generated documentation: [flat structure](sample) and [tree structure](sample-tree).

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
| `--platform <plain\|github-pages\|jekyll\|gitlab-wiki\|just-the-docs\|docusaurus>` | Target documentation host. Presets link rewriting and front matter (override with the options below). [default: plain] |
| `--link-extension <md\|none>` | Override the link file extension (`md` keeps `.md`, `none` strips it). Defaults to the platform preset. |
| `--link-prefix <relative\|none>` | Override the link prefix (`relative` keeps `./`, `none` strips it). Defaults to the platform preset. |
| `--back-button` | Add a back button on each page |
| `--member-accessibility-level <internal\|private\|protected\|public>` | Minimum accessibility level of members to be documented. [default: protected] |
| `--structure <flat\|tree>` | Documentation structure. [default: flat] |
| `--front-matter <none\|jekyll\|just-the-docs\|docusaurus>` | Override the front matter preset emitted for a documentation system. Defaults to the platform preset. |
| `--front-matter-field <key=value>` | Extra front matter `key=value` pair, merged on top of the preset (repeatable). Value emitted verbatim. |
| `--version` | Show version information |
| `-?, -h, --help` | Show help and usage information |

#### Example

```shell
dotnet xmldoc2md Sample.dll --output docs --platform github-pages --back-button
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

### Target platform

Different documentation hosts resolve inter-page links differently and expect different front matter. Pass `--platform` to pick a preset that sets both at once; each knob can still be overridden individually with `--link-extension`, `--link-prefix`, or `--front-matter`.

| Platform | Link extension | Link prefix | Front matter |
|---|---|---|---|
| `plain` (default) | `.md` | `./` | none |
| `github-pages` | stripped | `./` | `jekyll` |
| `jekyll` | stripped | `./` | `jekyll` |
| `gitlab-wiki` | stripped | stripped | none |
| `just-the-docs` | stripped | `./` | `just-the-docs` |
| `docusaurus` | `.md` | `./` | `docusaurus` |

```shell
# Just the Docs links, but no front matter (the site's _config.yml handles it):
dotnet xmldoc2md Sample.dll --output docs --platform just-the-docs --front-matter none
```

### Front matter

Static-site generators read a block of YAML *front matter* at the top of each Markdown file. Each platform preset selects a default; pass `--front-matter` to override it. The keys are computed from the type tree.

| Preset | Type page | Index page |
|---|---|---|
| `jekyll` | `layout`, `title` | `layout`, `title` |
| `just-the-docs` | `title`, `parent` (the namespace) | `title`, `has_children`, `nav_order` |
| `docusaurus` | `id`, `title`, `sidebar_label` | `id`, `title` |

```shell
dotnet xmldoc2md Sample.dll --output docs --front-matter jekyll
```

```yaml
---
layout: default
title: MyClass
---
```

Add your own keys with `--front-matter-field key=value` (repeatable). They are merged on top of the preset — a custom key overrides a preset key of the same name — so regenerating the docs no longer overwrites your front matter:

```shell
dotnet xmldoc2md Sample.dll --output docs --front-matter just-the-docs \
    --front-matter-field nav_order=3 --front-matter-field has_toc=false
```

> Custom field values are emitted verbatim, so you are responsible for valid YAML (e.g. quote a value that contains a colon).
