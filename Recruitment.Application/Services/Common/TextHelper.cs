using System.Net;
using System.Text.RegularExpressions;

namespace Recruitment.Application.Services.Common
{
    public static class TextHelper
    {
        public static string CleanText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string text = Regex.Replace(input, "<.*?>", string.Empty);

            text = WebUtility.HtmlDecode(text);

            text = Regex.Replace(text, @"\s+", " ").Trim();

            return text;
        }
        public static string TruncateText(string input, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            if (input.Length <= maxLength)
                return input;

            return input.Substring(0, maxLength) + "...";
        }
    }
}
