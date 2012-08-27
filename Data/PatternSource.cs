using System;
using System.Collections.Generic;

namespace MobilePatterns.Data
{
    public interface PatternSource
    {
        List<WebMenu> GetMenus();
        List<PatternImages> GetPatterns(Uri uri);
    }
}

