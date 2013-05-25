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
            //var currenturl = new urlhelper(html.viewcontext.requestcontext);
            //var imgtagbuilder = new tagbuilder("img"); // build the <img> tag
            //imgtagbuilder.mergeattribute("src", currenturl.content(imagepath));
            //imgtagbuilder.mergeattribute("alt", alt);
            //imgtagbuilder.mergeattribute("class", cssclass);
            //string imghtml = imgtagbuilder.tostring(tagrendermode.selfclosing);
            //var anchortagbuilder = new tagbuilder("a"); // build the <a> tag
            //anchortagbuilder.mergeattribute("href", currenturl.action(action, controllername, routevalues));
            //anchortagbuilder.innerhtml = imghtml; // include the <img> tag inside
            //string anchorhtml = anchortagbuilder.tostring(tagrendermode.normal);
            //return mvchtmlstring.create(anchorhtml);
        }

        public static string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
      
    }
}