using System;

namespace Crawler.Logic.Crawlers.Website
{
    public class Verifier
    {
        public virtual bool VerifyUri(Uri baseUri, string path)
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
            var verifyResult = VerifyUrlPointsToHtml(absoluteUri);

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

        private bool VerifyUrlPointsToHtml(Uri absoluteUri)
        {
            var urlEndContainsDot = absoluteUri.AbsolutePath.Substring(absoluteUri.AbsolutePath.LastIndexOf("/")).Contains(".");

            if (urlEndContainsDot)
            {
                var path = absoluteUri.AbsolutePath;

                var urlEndsWithHtml = String.Equals(path.Substring(path.LastIndexOf(".")), ".html", StringComparison.OrdinalIgnoreCase);
                var urlEndsWithHtm = String.Equals(path.Substring(path.LastIndexOf(".")), ".htm", StringComparison.OrdinalIgnoreCase);

                return urlEndsWithHtml || urlEndsWithHtm;
            }

            return true;
        }
    }
}
