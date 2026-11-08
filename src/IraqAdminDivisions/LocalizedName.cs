namespace IraqAdminDivisions;

/// <summary>A division name in Arabic, English and Kurdish.</summary>
public sealed class LocalizedName
{
    internal LocalizedName(string ar, string en, string ku)
    {
        Ar = ar;
        En = en;
        Ku = ku;
    }

    /// <summary>The Arabic name.</summary>
    public string Ar { get; }

    /// <summary>The English name.</summary>
    public string En { get; }

    /// <summary>The Kurdish (Sorani) name. Always present; see <see cref="Division.KurdishNameSource"/> for how reliable it is.</summary>
    public string Ku { get; }

    /// <summary>Returns the name in the requested language.</summary>
    /// <param name="language">The language to return.</param>
    public string Get(Language language) => language switch
    {
        Language.Arabic => Ar,
        Language.English => En,
        Language.Kurdish => Ku,
        _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
    };

    /// <inheritdoc />
    public override string ToString() => En;
}
