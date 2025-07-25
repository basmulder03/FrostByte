using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using FrostByte.Application.Models;

namespace FrostByte.Application.Parsing;

public class PuzzleTransformer
{
    public static PuzzleDto Transform(string html, int year, int day)
    {
        var doc = new HtmlParser().ParseDocument(html);
        var title = doc.QuerySelector("h2")!.TextContent.Trim('-', ' ');
        var parts = doc.QuerySelectorAll("article.day-desc")
            .Select((el, i) => new PartDto(
                i + 1,
                ParseBlocks(el)))
            .ToArray();
        return new PuzzleDto(year, day, title, parts);
    }

    private static List<Block> ParseBlocks(IElement element)
    {
        var blocks = new List<Block>();
        foreach (var child in element.Children)
        {
            if (child.TagName.Equals("P", StringComparison.OrdinalIgnoreCase))
            {
                blocks.Add(new ParagraphBlock(child.TextContent.Trim()));
            }
            else if (child.TagName.Equals("PRE", StringComparison.OrdinalIgnoreCase))
            {
                var code = child.QuerySelector("code")?.TextContent ?? child.TextContent;
                blocks.Add(new CodeBlock(code.Trim()));
            }
        }
        return blocks;
    }
}
