namespace FrostByte.Application.Models;

public abstract record Block;

public sealed record ParagraphBlock(string Text) : Block;

public sealed record CodeBlock(string Code) : Block;
