using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Utilidades.Extensions
{
    public static class StringExtension
    {
        #region Strig normalization

        private static IEnumerable<char> RemoveDiacriticsEnum(string src, bool compatNorm,
                                                              Func<char, char> customFolding)
        {
            foreach (char c in src.Normalize(compatNorm ? NormalizationForm.FormKD : NormalizationForm.FormD))
                switch (CharUnicodeInfo.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.EnclosingMark:
                        //do nothing
                        break;
                    default:
                        yield return customFolding(c);
                        break;
                }
        }

        private static IEnumerable<char> RemoveDiacriticsEnum(string src, bool compatNorm)
        {
            return RemoveDiacritics(src, compatNorm, c => c);
        }

        private static string RemoveDiacritics(string src, bool compatNorm, Func<char, char> customFolding)
        {
            var sb = new StringBuilder();
            foreach (char c in RemoveDiacriticsEnum(src, compatNorm, customFolding))
                sb.Append(c);
            return sb.ToString();
        }

        private static string RemoveDiacritics(string src, bool compatNorm)
        {
            return RemoveDiacritics(src, compatNorm, c => c);
        }

        #endregion

        public static string GenerateQueryString(this string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string newText = text.Trim();
                newText = RemoveDiacritics(newText, true);
                newText = newText.Replace(' ', '-');
                var sb = new StringBuilder();
                foreach (char c in newText)
                {
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' ||
                        c == '-')
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns the given string truncated to the specified length, suffixed with an elipses (...)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">Maximum length of return string</param>
        /// <returns></returns>
        public static string Truncate(this string input, int length)
        {
            return Truncate(input, length, "...");
        }

        /// <summary>
        /// Returns the given string truncated to the specified length, suffixed with the given value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">Maximum length of return string</param>
        /// <param name="suffix">The value to suffix the return value with (if truncation is performed)</param>
        /// <returns></returns>
        public static string Truncate(this string input, int length, string suffix)
        {
            if (input == null) return "";
            if (input.Length <= length) return input;

            if (suffix == null) suffix = "...";

            return input.Substring(0, length - suffix.Length) + suffix;
        }

        /// <summary>
        /// Splits a given string into an array based on character line breaks
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String array, each containing one line</returns>
        public static string[] ToLineArray(this string input)
        {
            return input == null ? new string[] {} : Regex.Split(input, "\r\n");
        }

        /// <summary>
        /// Splits a given string into a strongly-typed list based on character line breaks
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Strongly-typed string list, each containing one line</returns>
        public static List<string> ToLineList(this string input)
        {
            var output = new List<string>();
            output.AddRange(input.ToLineArray());
            return output;
        }

        /// <summary>
        /// Replaces line breaks with self-closing HTML 'br' tags
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceBreaksWithBr(this string input)
        {
            return string.Join("<br/>", input.ToLineArray());
        }

        /// <summary>
        /// Replaces any single apostrophes with two of the same
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string DoubleApostrophes(this string input)
        {
            return Regex.Replace(input, "'", "''");
        }

        /// <summary>
        /// Encodes the input string as HTML (converts special characters to entities)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>HTML-encoded string</returns>
        public static string ToHTMLEncoded(this string input)
        {
            return HttpContext.Current.Server.HtmlEncode(input);
        }

        /// <summary>
        /// Encodes the input string as a URL (converts special characters to % codes)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>URL-encoded string</returns>
        public static string ToUrlEncoded(this string input)
        {
            return HttpContext.Current.Server.UrlEncode(input);
        }

        /// <summary>
        /// Decodes any HTML entities in the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string FromHTMLEncoded(this string input)
        {
            return HttpContext.Current.Server.HtmlDecode(input);
        }

        /// <summary>
        /// Decodes any URL codes (% codes) in the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string FromUrlEncoded(this string input)
        {
            return HttpContext.Current.Server.UrlDecode(input);
        }

        /// <summary>
        /// Removes any HTML tags from the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, @"<(style|script)[^<>]*>.*?</\1>|</?[a-z][a-z0-9]*[^<>]*>|<!--.*?-->", "");
        }

        public static bool IsValidEmailAddress(this string s)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
        }

        public static string GetSHA512(this string s, Encoding enc)
        {
            return BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(enc.GetBytes(s))).Replace("-", String.Empty).ToUpper();
            
        }
    }
}