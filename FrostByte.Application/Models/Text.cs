using System.Text.Json.Serialization;

namespace FrostByte.Application.Models;

[JsonDerivedType(typeof(PlainText), "Plain")]
[JsonDerivedType(typeof(EmphasizedText), "Emphasized")]
[JsonDerivedType(typeof(StarEmphasizedText), "StarEmphasized")]
[JsonDerivedType(typeof(CodeText), "Code")]
[JsonDerivedType(typeof(EmphasizedCodeText), "EmphasizedCode")]
[JsonDerivedType(typeof(TitleText), "Title")]
[JsonDerivedType(typeof(LinkText), "Link")]
public abstract record Text;

/// <summary>
///     Represents plain text without any formatting.
/// </summary>
public sealed record PlainText(string Plain) : Text;

/// <summary>
///     Represents emphasized text, typically rendered in italics.
/// </summary>
/// <remarks>
///     HTML &lt;em&gt; element.
/// </remarks>
/// <param name="Emphasized"></param>
public sealed record EmphasizedText(string Emphasized) : Text;

/// <summary>
///     Represents text emphasized for stars.
/// </summary>
/// <remarks>
///     HTML &lt;em class="star"&gt; element.
/// </remarks>
public sealed record StarEmphasizedText(string StarEmphasized) : Text;

/// <summary>
///     Represents an inline block of code text.
/// </summary>
/// <remarks>
///     HTML &lt;code&gt; element.
/// </remarks>
public sealed record CodeText(string Code) : Text;

/// <summary>
///     Represents emphasized code text.
/// </summary>
/// <remarks>
///     HTML &lt;code&gt;&lt;em /&gt;&lt;code&gt; element.
/// </remarks>
public sealed record EmphasizedCodeText(string EmphasizedCode) : Text;

/// <summary>
///     Represents a title with associated text and hover title.
/// </summary>
/// <remarks>
///     HTML &lt;span title="..."&gt;"text"&lt;span /&gt; element.
/// </remarks>
public sealed record TitleText(string Text, string HoverTitle) : Text;

/// <summary>
///     Represents a link with display text and URL.
/// </summary>
/// <remarks>
///     HTML &lt;a href="..."&gt;"text"&lt;/a&gt; element.
/// </remarks>
public sealed record LinkText(string Text, string Url) : Text;
