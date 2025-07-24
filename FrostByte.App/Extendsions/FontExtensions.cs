namespace FrostByte.App.Extendsions;

public static class FontExtensions
{
    public static IFontCollection AddSourceCodeProFont(this IFontCollection fonts)
    {
        return fonts
            .AddFont("SourceCodePro-Black.ttf", "SourceCodeProBlack")
            .AddFont("SourceCodePro-Bold.ttf", "SourceCodeProBold")
            .AddFont("SourceCodePro-ExtraLight.ttf", "SourceCodeProExtraLight")
            .AddFont("SourceCodePro-Light.ttf", "SourceCodeProLight")
            .AddFont("SourceCodePro-Regular.ttf", "SourceCodeProRegular")
            .AddFont("SourceCodePro-Semibold.ttf", "SourceCodeProSemibold");
    }
}