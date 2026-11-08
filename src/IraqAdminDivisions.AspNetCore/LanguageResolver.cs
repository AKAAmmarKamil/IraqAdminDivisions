using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace IraqAdminDivisions.AspNetCore;

internal static class LanguageResolver
{
    private const string LanguageQueryKey = "lang";

    public static Language Resolve(HttpRequest request)
    {
        if (TryParseTag(request.Query[LanguageQueryKey].ToString(), out var queried))
        {
            return queried;
        }

        var accepted = request.GetTypedHeaders().AcceptLanguage
            .Where(value => value.Quality is null or > 0)
            .OrderByDescending(value => value.Quality ?? 1);

        foreach (var value in accepted)
        {
            if (TryParseTag(value.Value.ToString(), out var language))
            {
                return language;
            }
        }

        return Language.Arabic;
    }

    private static bool TryParseTag(string tag, out Language language)
    {
        language = Language.Arabic;
        var primary = tag.Trim().ToLowerInvariant();
        if (primary.StartsWith("ar", StringComparison.Ordinal))
        {
            return true;
        }

        if (primary.StartsWith("en", StringComparison.Ordinal))
        {
            language = Language.English;
            return true;
        }

        if (primary.StartsWith("ku", StringComparison.Ordinal) || primary.StartsWith("ckb", StringComparison.Ordinal))
        {
            language = Language.Kurdish;
            return true;
        }

        return false;
    }
}
