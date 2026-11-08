namespace IraqAdminDivisions.Boundaries;

/// <summary>Resolves a GPS point to an Iraqi administrative division, offline.</summary>
public static class IraqBoundaries
{
    private const double MaxLatitude = 90;
    private const double MaxLongitude = 180;

    /// <summary>Finds the sub-district containing a point.</summary>
    /// <param name="latitude">The latitude in degrees, between -90 and 90.</param>
    /// <param name="longitude">The longitude in degrees, between -180 and 180.</param>
    /// <returns>The sub-district, or <c>null</c> when the point is outside Iraq.</returns>
    /// <exception cref="ArgumentException">A coordinate is NaN.</exception>
    /// <exception cref="ArgumentOutOfRangeException">A coordinate is outside its valid range.</exception>
    public static Division? Locate(double latitude, double longitude)
    {
        ValidateCoordinates(latitude, longitude);
        return BoundaryIndex.Instance.Locate(latitude, longitude, out _);
    }

    /// <summary>Finds the division at the requested level that contains a point.</summary>
    /// <param name="latitude">The latitude in degrees, between -90 and 90.</param>
    /// <param name="longitude">The longitude in degrees, between -180 and 180.</param>
    /// <param name="level">The level of the division to return.</param>
    /// <returns>The sub-district, its district or its governorate, or <c>null</c> when the point is outside Iraq.</returns>
    /// <exception cref="ArgumentException">A coordinate is NaN.</exception>
    /// <exception cref="ArgumentOutOfRangeException">A coordinate or the level is outside its valid range.</exception>
    public static Division? Locate(double latitude, double longitude, AdminLevel level)
    {
        ValidateCoordinates(latitude, longitude);
        ValidateLevel(level);
        var subDistrict = BoundaryIndex.Instance.Locate(latitude, longitude, out _);
        return subDistrict is null ? null : AncestorAt(subDistrict, level);
    }

    /// <summary>Tells whether a division contains a point.</summary>
    /// <param name="code">The code of a governorate, district or sub-district.</param>
    /// <param name="latitude">The latitude in degrees, between -90 and 90.</param>
    /// <param name="longitude">The longitude in degrees, between -180 and 180.</param>
    /// <returns><c>true</c> when the point is located in the division or in one of its sub-districts.</returns>
    /// <exception cref="ArgumentException">A coordinate is NaN.</exception>
    /// <exception cref="ArgumentOutOfRangeException">A coordinate is outside its valid range.</exception>
    /// <exception cref="KeyNotFoundException">The code is not a known division.</exception>
    public static bool Contains(string code, double latitude, double longitude)
    {
        ValidateCoordinates(latitude, longitude);
        var division = IraqDivisions.Find(code) ?? throw new KeyNotFoundException($"Unknown division code '{code}'.");
        var subDistrict = BoundaryIndex.Instance.Locate(latitude, longitude, out _);
        return subDistrict is not null && ReferenceEquals(AncestorAt(subDistrict, division.Level), division);
    }

    private static Division AncestorAt(Division subDistrict, AdminLevel level) => level switch
    {
        AdminLevel.Governorate => subDistrict.Governorate,
        AdminLevel.District => subDistrict.District!,
        _ => subDistrict
    };

    private static void ValidateLevel(AdminLevel level)
    {
        if (level < AdminLevel.Governorate || level > AdminLevel.SubDistrict)
        {
            throw new ArgumentOutOfRangeException(nameof(level), level, "Unknown administrative level.");
        }
    }

    private static void ValidateCoordinates(double latitude, double longitude)
    {
        if (double.IsNaN(latitude))
        {
            throw new ArgumentException("Latitude must be a number.", nameof(latitude));
        }

        if (double.IsNaN(longitude))
        {
            throw new ArgumentException("Longitude must be a number.", nameof(longitude));
        }

        if (latitude < -MaxLatitude || latitude > MaxLatitude)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Latitude must be between -90 and 90.");
        }

        if (longitude < -MaxLongitude || longitude > MaxLongitude)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Longitude must be between -180 and 180.");
        }
    }
}
