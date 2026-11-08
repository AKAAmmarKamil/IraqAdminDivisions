using System.ComponentModel.DataAnnotations;

namespace IraqAdminDivisions;

/// <summary>Validates that a string is a known Iraqi division code, optionally of a specific level. Null and empty values pass; combine with <c>[Required]</c> to reject them.</summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class IraqDivisionCodeAttribute : ValidationAttribute
{
    private const string AnyLevelMessage = "The {0} field must be a valid Iraqi administrative division code.";

    private readonly AdminLevel? _level;

    /// <summary>Accepts a code of any level.</summary>
    public IraqDivisionCodeAttribute()
        : base(AnyLevelMessage)
    {
    }

    /// <summary>Accepts only a code of the given level.</summary>
    /// <param name="level">The required level.</param>
    public IraqDivisionCodeAttribute(AdminLevel level)
        : base("The {0} field must be a valid Iraqi " + level + " code.")
    {
        _level = level;
    }

    /// <summary>The required level, or <c>null</c> when any level is accepted.</summary>
    public AdminLevel? Level => _level;

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (!(value is string code))
        {
            return false;
        }

        return string.IsNullOrWhiteSpace(code) || IraqDivisions.IsValid(code, _level);
    }
}
