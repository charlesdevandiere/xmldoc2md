# XMLDoc2Markdown

Tool to generate markdown from C# XML documentation.

[![Build Status](https://github.com/charlesdevandiere/xmldoc2md/actions/workflows/ci.yml/badge.svg)](https://github.com/charlesdevandiere/xmldoc2md/actions/workflows/ci.yml)
[![Nuget](https://img.shields.io/nuget/v/XMLDoc2Markdown.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)
[![Downloads](https://img.shields.io/nuget/dt/XMLDoc2Markdown.svg?logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)

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

See complete documentation [here](https://charlesdevandiere.github.io/xmldoc2md/).
