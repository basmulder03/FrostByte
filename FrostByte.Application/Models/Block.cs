using System.Text.Json.Serialization;

namespace FrostByte.Application.Models;

[JsonDerivedType(typeof(ParagraphBlock), "Text")]
[JsonDerivedType(typeof(CodeBlock), "Code")]
[JsonDerivedType(typeof(ListBlock), "List")]
public abstract record Block;

public sealed record ParagraphBlock(IReadOnlyList<Text> Text) : Block;

public sealed record CodeBlock(string Code) : Block;

public sealed record ListBlock(IReadOnlyList<IReadOnlyList<Text>> Items) : Block;
