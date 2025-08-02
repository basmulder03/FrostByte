using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using FrostByte.Application.Models;
using Microsoft.Extensions.Logging;

namespace FrostByte.Application.Parsing;

public class PuzzleTransformer(ILogger<PuzzleTransformer> logger) : IPuzzleTransformer
{
    private readonly ILogger<PuzzleTransformer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public PuzzleDto Transform(string html, int year, int day)
    {
        _logger.LogInformation("Transforming puzzle HTML for year {Year}, day {Day}", year, day);
        var doc = new HtmlParser().ParseDocument(html);
        var title = doc.QuerySelector("h2")!.TextContent.Trim('-', ' ');
        _logger.LogInformation("Parsed title: {Title}", title);
        var parts = doc.QuerySelectorAll("article.day-desc")
            .Select((el, i) => new PartDto(
                i + 1,
                ParseBlocks(el)))
            .ToArray();
        return new PuzzleDto(year, day, title, parts);
    }

    private List<Block> ParseBlocks(IElement element)
    {
        var blocks = new List<Block>();
        foreach (var child in element.Children)
            if (child.TagName.Equals("H2", StringComparison.OrdinalIgnoreCase))
            {
                // H2 elements are already parsed as title blocks
                _logger.LogDebug("Already parsed title block: {Title}, skipping...", child.TextContent.Trim());
            }
            else if (child.TagName.Equals("P", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("Parsing paragraph block");
                blocks.Add(new ParagraphBlock(ParseInlineText(child)));
            }
            else if (child.TagName.Equals("PRE", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("Parsing code block");
                var code = child.QuerySelector("code")?.TextContent ?? child.TextContent;
                blocks.Add(new CodeBlock(code.Trim()));
            }
            else if (child.TagName.Equals("UL", StringComparison.OrdinalIgnoreCase) ||
                     child.TagName.Equals("OL", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("Parsing list block: {TagName}", child.TagName);
                var items = child.QuerySelectorAll("li")
                    .Select(ParseInlineText)
                    .ToList();
                blocks.Add(new ListBlock(items));
            }
            else
            {
                _logger.LogWarning("Ignoring unsupported block type: {TagName}", child.TagName);
            }

        return blocks;
    }

    private List<Text> ParseInlineText(IElement element)
    {
        _logger.LogDebug("Parsing inline text for element: {TagName}", element.TagName);
        var texts = new List<Text>();
        foreach (var child in element.ChildNodes)
            if (child.NodeType == NodeType.Text)
            {
                _logger.LogDebug("Found plain text node: {Text}", child.TextContent.Trim());
                texts.Add(new PlainText(child.TextContent.Trim()));
            }
            else if (child is IElement el)
            {
                _logger.LogDebug("Found element node: {TagName}", el.TagName);
                if (el.TagName.Equals("EM", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogDebug("Parsing emphasized text");
                    if (el.ClassList.Contains("star"))
                    {
                        _logger.LogDebug("Parsing star emphasized text");
                        texts.Add(new StarEmphasizedText(el.TextContent.Trim()));
                    }
                    else
                    {
                        _logger.LogDebug("Parsing regular emphasized text");
                        texts.Add(new EmphasizedText(el.TextContent.Trim()));
                    }
                }
                else if (el.TagName.Equals("CODE", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogDebug("Parsing code text");
                    if (el.Children.Length > 0 &&
                        el.FirstElementChild?.TagName.Equals("EM", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        _logger.LogDebug("Parsing emphasized code text");
                        texts.Add(new EmphasizedCodeText(el.TextContent.Trim()));
                    }
                    else
                    {
                        _logger.LogDebug("Parsing regular code text");
                        texts.Add(new CodeText(el.TextContent.Trim()));
                    }
                }
                else if (el.TagName.Equals("SPAN", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogDebug("Parsing title text with hover: {Text}", el.TextContent.Trim());
                    var title = el.GetAttribute("title") ?? string.Empty;
                    texts.Add(new TitleText(el.TextContent.Trim(), title));
                }
                else if (el.TagName.Equals("A", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogDebug("Parsing link text: {Text}", el.TextContent.Trim());
                    var href = el.GetAttribute("href") ?? string.Empty;
                    texts.Add(new LinkText(el.TextContent.Trim(), href));
                }
                else
                {
                    _logger.LogWarning("Ignoring unsupported inline element: {TagName}", el.TagName);
                }
            }

        return texts;
    }
}
