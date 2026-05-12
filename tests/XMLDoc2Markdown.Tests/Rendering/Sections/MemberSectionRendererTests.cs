using System.Reflection;
using System.Xml.Linq;
using Markdown;
using MyClassLib;
using XMLDoc2Markdown.Members;
using XMLDoc2Markdown.Rendering;
using XMLDoc2Markdown.Rendering.Sections;

namespace XMLDoc2Markdown.Tests.Rendering.Sections;

[Collection(nameof(SampleAssemblyCollection))]
public class MemberSectionRendererTests
{
    private readonly SampleAssemblyFixture fixture;

    public MemberSectionRendererTests(SampleAssemblyFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void Render_emits_degraded_heading_when_member_throws_FileNotFoundException()
    {
        MarkdownDocument document = new();
        MethodInfo member = typeof(MyClass).GetMethod(nameof(MyClass.Get))!;
        ThrowingRenderer renderer = this.NewRenderer(new FileNotFoundException("missing"));

        renderer.Render(document, [member]);

        Assert.Contains($"### **{member.Name}**", document.ToString());
    }

    [Theory]
    [InlineData(typeof(FileNotFoundException))]
    [InlineData(typeof(FileLoadException))]
    [InlineData(typeof(TypeLoadException))]
    public void Render_continues_to_next_member_after_target_exception(Type exceptionType)
    {
        MarkdownDocument document = new();
        MethodInfo first = typeof(MyClass).GetMethod(nameof(MyClass.Get))!;
        MethodInfo second = typeof(MyClass).GetMethod(nameof(MyClass.ToString))!;
        Exception toThrow = (Exception)Activator.CreateInstance(exceptionType, "missing")!;
        ThrowingRenderer renderer = this.NewRenderer(toThrow, throwOnlyOnFirst: true);

        renderer.Render(document, [first, second]);

        string output = document.ToString();
        Assert.Contains(first.Name, output);
        Assert.Contains(second.Name, output);
    }

    [Fact]
    public void Render_lets_unrelated_exception_escape()
    {
        MarkdownDocument document = new();
        MethodInfo member = typeof(MyClass).GetMethod(nameof(MyClass.Get))!;
        ThrowingRenderer renderer = this.NewRenderer(new InvalidOperationException("boom"));

        Assert.Throws<InvalidOperationException>(() => renderer.Render(document, [member]));
    }

    private ThrowingRenderer NewRenderer(Exception toThrow, bool throwOnlyOnFirst = false)
    {
        RenderingContext context = new(
            this.fixture.Assembly,
            typeof(MyClass),
            this.fixture.Documentation,
            new TypeDocumentationOptions { MemberAccessibilityLevel = Accessibility.Public });
        XmlDocToMarkdownConverter converter = new(context, context.CrefResolver);
        ExampleInjector examples = new(null);
        return new ThrowingRenderer(context, converter, examples, toThrow, throwOnlyOnFirst);
    }

    private sealed class ThrowingRenderer : MemberSectionRenderer<MethodInfo>
    {
        private readonly Exception toThrow;
        private readonly bool throwOnlyOnFirst;
        private bool alreadyThrew;

        internal ThrowingRenderer(
            RenderingContext context,
            XmlDocToMarkdownConverter converter,
            ExampleInjector examples,
            Exception toThrow,
            bool throwOnlyOnFirst)
            : base(context, converter, examples)
        {
            this.toThrow = toThrow;
            this.throwOnlyOnFirst = throwOnlyOnFirst;
        }

        protected override string SectionHeader => "Methods";
        protected override string ObsoleteDefaultMessage => "obsolete";

        protected override void RenderMember(IMarkdownDocument document, MethodInfo member)
        {
            if (this.throwOnlyOnFirst && this.alreadyThrew)
            {
                document.AppendHeader(new MarkdownStrongEmphasis($"OK-{member.Name}"), 3);
                return;
            }
            this.alreadyThrew = true;
            throw this.toThrow;
        }

        protected override void RenderBody(IMarkdownDocument document, MethodInfo member, XElement? memberDocElement)
        {
        }
    }
}
