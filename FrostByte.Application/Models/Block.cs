using System.Text.Json.Serialization;

namespace FrostByte.Application.Models;

[JsonDerivedType(typeof(ParagraphBlock), "Text")]
[JsonDerivedType(typeof(CodeBlock), "Code")]
[JsonDerivedType(typeof(ListBlock), "List")]
public abstract record Block;

public sealed record ParagraphBlock(IList<Text> Text) : Block;

public sealed record CodeBlock(string Code) : Block;

public sealed record ListBlock(IList<IList<Text>> Items) : Block;
