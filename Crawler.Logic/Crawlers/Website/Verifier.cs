using System;

namespace Crawler.Logic
{
    public class Verifier
    {
        internal virtual bool VerifyUrl(Uri baseUri, string path)
        {
            var isWellFormedRelativeUri = Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out Uri pathUri);

            if (!isWellFormedRelativeUri)
            {
                return false;
            }

            var verifyResult = false;

            if (!pathUri.IsAbsoluteUri)
            {
                verifyResult = VerifyRelativeUri(baseUri, pathUri);
            }

            if (pathUri.IsAbsoluteUri && pathUri.Host.Equals(baseUri.Host))
            {
                verifyResult = VerifyAbsoluteUri(pathUri);
            }

            return verifyResult;
        }

        private bool VerifyAbsoluteUri(Uri absoluteUri)
        {
            var verifyResult = true;

            var isPointsToHtml = !absoluteUri.AbsolutePath.Substring(absoluteUri.AbsolutePath.LastIndexOf("/")).Contains(".")
                                 || String.Equals(absoluteUri.AbsolutePath.Substring(absoluteUri.AbsolutePath.LastIndexOf(".")), ".html", StringComparison.OrdinalIgnoreCase)
                                 || String.Equals(absoluteUri.AbsolutePath.Substring(absoluteUri.AbsolutePath.LastIndexOf(".")), ".htm", StringComparison.OrdinalIgnoreCase);

            if (!isPointsToHtml)
            {
                verifyResult = false;
            }

            if (!String.IsNullOrEmpty(absoluteUri.Fragment))
            {
                verifyResult = false;
            }

            if (!String.IsNullOrEmpty(absoluteUri.Query))
            {
                verifyResult = false;
            }

            return verifyResult;
        }

        private bool VerifyRelativeUri(Uri baseUri, Uri relativeUri)
        {
            var IsWellFormedAbsoluteUri = Uri.TryCreate(baseUri, relativeUri, out Uri absoluteUri);

            if (!IsWellFormedAbsoluteUri)
            {
                return false;
            }

            var verifyResult = VerifyAbsoluteUri(absoluteUri);

            return verifyResult;
        }
    }
}
