using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Crawler.Logic
{
    public class CustomUriComparer : IEqualityComparer<Uri>
    {
        public bool Equals(Uri x, Uri y)
        {
            var result = Uri.Compare(x, y, UriComponents.Host | UriComponents.PathAndQuery, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);

            return result == 0;
        }

        public int GetHashCode([DisallowNull] Uri uri)
        {
            return HashCode.Combine(uri.Host, uri.AbsolutePath);
        }
    }
}
