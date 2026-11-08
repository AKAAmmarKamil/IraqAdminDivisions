namespace IraqAdminDivisions.Builder.Overrides;

public sealed record Correction(string Code, string Field, string Value, string Reason, int LineNumber);

public sealed record Transliteration(string Code, string Ku, int LineNumber);

public sealed record AppliedCorrection(string Code, string Field, string? OldValue, string NewValue, string Reason);
