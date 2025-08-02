using FrostByte.Application.Models;

namespace FrostByte.Application.Parsing;

public interface IPuzzleTransformer
{
    public PuzzleDto Transform(string html, int year, int day);
}
