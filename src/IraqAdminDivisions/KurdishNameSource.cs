namespace IraqAdminDivisions;

/// <summary>Where a Kurdish name came from, so apps can show or hide unverified names.</summary>
public enum KurdishNameSource
{
    /// <summary>A Wikidata Central Kurdish label.</summary>
    Wikidata,

    /// <summary>An open dataset whose Kurdish names agree with Wikidata above a calibrated threshold (see <see cref="Division.KurdishNameDataset"/>); not proven to be native-reviewed.</summary>
    Dataset,

    /// <summary>An AI-assisted Sorani transliteration of the Arabic or English name; not verified by a native reviewer.</summary>
    Transliterated,

    /// <summary>A name set by hand as a correction.</summary>
    Manual
}
