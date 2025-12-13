using System.Text.RegularExpressions;

namespace Common;

public static class TextUtils
{
    /// <summary>
    /// Safe way to return the substring of a string, or if it is too short, the full string.
    /// </summary>
    /// <param name="text">The text string to get a substring of.</param>
    /// <param name="start">The starting index of the string.</param>
    /// <param name="end">The ending index of the string.</param>
    /// <param name="includeEllipsis">Will include an ellipsis on truncated text returned if true. If false, it does not.</param>
    /// <returns></returns>
    public static string GetSubstring(string text, int start, int end, bool includeEllipsis = false)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }
        else
        {
            var length = text.Length;

            if (length >= end)
            {
                var postFix = string.Empty;
                if (includeEllipsis) postFix = "...";
                
                return text.Substring(start, end - start) + postFix;
            }
            else
            {
                return text;
            }
        }
    }
    
    /// <summary>
    ///  Counts number of occurrences of a string in another string
    /// </summary>
    /// <param name="val">string containing text</param>
    /// <param name="stringToMatch">string or pattern find</param>
    /// <returns>The number of occurrences as an int </returns>
    public static int CountOccurrences(this string val, string stringToMatch)
    {
        return Regex.Matches(val, stringToMatch, RegexOptions.IgnoreCase).Count;
    }
    
    /// <summary>
    /// Creates a "code" value from a passed in string.
    /// Suitable for code values in databases.
    /// </summary>
    /// <param name="val"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Codify(string val, int length)
    {
        if (string.IsNullOrEmpty(val)) return string.Empty;
        
        val = val.Replace(" ", ""); //remove spaces if there are any
        
        var maxLength = length-1;
        
        if (length < val.Length) 
            maxLength = length;
        
        
        return val.ToUpperInvariant().Substring(0,maxLength);
        
    }
}