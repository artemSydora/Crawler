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

            if (pathUri.IsAbsoluteUri)
            {
                verifyResult = VerifyAbsoluteUri(baseUri, pathUri);
            }

            return verifyResult;
        }

        private bool VerifyAbsoluteUri(Uri baseUri, Uri absoluteUri)
        {
            if(!absoluteUri.Host.Equals(baseUri.Host) || (absoluteUri.Scheme != Uri.UriSchemeHttps && absoluteUri.Scheme != Uri.UriSchemeHttp))
            {
                return false;
            }
             
            if (!String.IsNullOrEmpty(absoluteUri.Fragment))
            {
                return false;
            }

            if (!String.IsNullOrEmpty(absoluteUri.Query))
            {
                return false;
            }

            return VerifyUrlPointsToHtml(absoluteUri);
        }

        private bool VerifyRelativeUri(Uri baseUri, Uri relativeUri)
        {
            var IsWellFormedAbsoluteUri = Uri.TryCreate(baseUri, relativeUri, out Uri absoluteUri);

            if (!IsWellFormedAbsoluteUri)
            {
                return false;
            }

            var verifyResult = VerifyAbsoluteUri(baseUri, absoluteUri);

            return verifyResult;
        }

        private bool VerifyUrlPointsToHtml(Uri absoluteUri)
        {
            var path = absoluteUri.AbsolutePath;

            var urlEndContainsDot = path.Substring(path.LastIndexOf("/")).Contains(".");

            if (urlEndContainsDot)
            {
                var urlEndsWithHtml = String.Equals(path.Substring(path.LastIndexOf(".")), ".html", StringComparison.OrdinalIgnoreCase);
                var urlEndsWithHtm = String.Equals(path.Substring(path.LastIndexOf(".")), ".htm", StringComparison.OrdinalIgnoreCase);

                return urlEndsWithHtml || urlEndsWithHtm;
            }

            return true;
        }
    }
}
