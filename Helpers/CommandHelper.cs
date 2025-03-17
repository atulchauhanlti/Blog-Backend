using System;

namespace backend.Helpers;

public static class CommandHelper
{
    public static string GenerateSlug(string title)
    {
        return title.ToLower()
                    .Replace(" ", "-")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Trim();
    }
}
