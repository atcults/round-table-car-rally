using PdfSharp.Fonts;

namespace ResultCalculator.Writers
{
    internal class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            faceName = faceName.ToLower();
            var fontPath = Path.Combine(ConfigProvider.GetFontSourcePath(), faceName + ".ttf");
            return File.ReadAllBytes(fontPath);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            return new FontResolverInfo(familyName);
        }
    }
}
