namespace XMLDoc2Markdown.Signatures;

/// <summary>
/// Token-based signature composer. Modifiers append tokens; ToString joins them
/// with single spaces. Use AppendRaw for tokens that should not be space-separated
/// from their predecessor (e.g. <c>"("</c> in a parameter list).
/// </summary>
internal sealed class SignatureBuilder
{
    private readonly List<string> tokens = [];

    internal SignatureBuilder Append(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            this.tokens.Add(token);
        }
        return this;
    }

    internal SignatureBuilder AppendJoined(string token)
    {
        if (this.tokens.Count == 0)
        {
            this.tokens.Add(token);
        }
        else
        {
            this.tokens[^1] += token;
        }
        return this;
    }

    public override string ToString() => string.Join(' ', this.tokens);
}
