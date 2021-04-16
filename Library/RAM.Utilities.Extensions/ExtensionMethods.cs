using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ExtensionMethods {

    /// <summary>
    /// Converts a string that has been encoded into a decoded string.
    /// </summary>
    /// <param name="text">The specified string</param>
    /// <returns>A decoded string</returns>
    public static string Unescape(this string text) {
        return System.Web.HttpUtility.UrlDecode(text);
    }
}