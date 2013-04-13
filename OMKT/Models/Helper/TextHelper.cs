using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OMKT.Models.Helper
{
    public static class TextHelper
    {
        public static string Truncate(this HtmlHelper helper, string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(0, length) + "...";
            }
        }
    }
}