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
    
}