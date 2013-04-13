using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace OMKT.Models.Helper
{
    public static class Helpers
    {
        public static string GetSHA1Hash(string value)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();

        }
        public static void ActionImage(this HtmlHelper html, string imagePath, string alt, string cssClass,
            string action, string controllerName, object routeValues)
        {
            //var currentUrl = new UrlHelper(html.ViewContext.RequestContext);
            //var imgTagBuilder = new TagBuilder("img"); // build the <img> tag
            //imgTagBuilder.MergeAttribute("src", currentUrl.Content(imagePath));
            //imgTagBuilder.MergeAttribute("alt", alt);
            //imgTagBuilder.MergeAttribute("class", cssClass);
            //string imgHtml = imgTagBuilder.ToString(TagRenderMode.SelfClosing);
            //var anchorTagBuilder = new TagBuilder("a"); // build the <a> tag
            //anchorTagBuilder.MergeAttribute("href", currentUrl.Action(action, controllerName, routeValues));
            //anchorTagBuilder.InnerHtml = imgHtml; // include the <img> tag inside
            //string anchorHtml = anchorTagBuilder.ToString(TagRenderMode.Normal);
            //return MvcHtmlString.Create(anchorHtml);
        }

        public static string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
    }
}