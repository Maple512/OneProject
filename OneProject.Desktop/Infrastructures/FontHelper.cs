namespace OneProject.Desktop.Infrastructures;

using System;
using System.Collections.Generic;
using System.Linq;

public static class FontHelper
{
    private static readonly Lazy<IEnumerable<FontFamily>> _fonts = new(
        static () => Fonts.SystemFontFamilies.OrderBy(x => x.Source));

    public static IEnumerable<FontFamily> SystemFonts => _fonts.Value;
}
