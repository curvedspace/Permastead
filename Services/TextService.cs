namespace Services;

public static class TextService
{
    //Returns the substring of a string, or if it is too short, the full string.
    public static string GetSubstring(string text, int start, int end)
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
                return text.Substring(start, end - start);
            }
            else
            {
                return text;
            }
        }
    }
}