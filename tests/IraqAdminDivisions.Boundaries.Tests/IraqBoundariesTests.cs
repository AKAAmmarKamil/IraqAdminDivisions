using IraqAdminDivisions.Boundaries;

namespace IraqAdminDivisions.Boundaries.Tests;

public class IraqBoundariesTests
{
    public static IEnumerable<object[]> AllCodes() => IraqDivisions.All.Select(division => new object[] { division.Code });

    public static IEnumerable<object[]> GovernorateSamples() => new[]
    {
        new object[] { "Baghdad Tahrir Square", 33.3340, 44.4170, "IQG08" },
        new object[] { "Erbil citadel", 36.1912, 44.0094, "IQG11" },
        new object[] { "Basra", 30.5085, 47.7804, "IQG02" },
        new object[] { "Najaf old city", 31.9960, 44.3148, "IQG04" },
        new object[] { "Mosul", 36.3400, 43.1300, "IQG15" },
        new object[] { "Sulaymaniyah", 35.5613, 45.4370, "IQG06" },
        new object[] { "Halabja town", 35.1778, 45.9861, "IQG19" }
    };

    public static IEnumerable<object[]> OutsidePoints() => new[]
    {
        new object[] { "Riyadh", 24.71, 46.67 },
        new object[] { "Tehran", 35.69, 51.39 },
        new object[] { "Damascus", 33.51, 36.29 },
        new object[] { "Gulf sea", 29.3, 49.5 },
        new object[] { "Null Island", 0.0, 0.0 },
        new object[] { "South Pole", -90.0, 180.0 }
    };

    [Theory]
    [MemberData(nameof(AllCodes))]
    public void Locate_AtDivisionCentre_ReturnsThatDivision(string code)
    {
        var division = IraqDivisions.Find(code)!;

        var located = IraqBoundaries.Locate(division.Latitude, division.Longitude, division.Level);

        Assert.Same(division, located);
    }

    [Fact]
    public void Locate_AtEverySubDistrictCentre_ReturnsTheSubDistrict()
    {
        foreach (var subDistrict in IraqDivisions.All.Where(division => division.Level == AdminLevel.SubDistrict))
        {
            Assert.Same(subDistrict, IraqBoundaries.Locate(subDistrict.Latitude, subDistrict.Longitude));
        }
    }

    [Fact]
    public void Locate_AtEveryDivisionCentre_NeedsNoSnapFallback()
    {
        var index = BoundaryIndex.Instance;
        var snapped = IraqDivisions.All.Count(division =>
        {
            index.Locate(division.Latitude, division.Longitude, out var isSnapped);
            return isSnapped;
        });

        Assert.Equal(0, snapped);
    }

    [Theory]
    [MemberData(nameof(GovernorateSamples))]
    public void Locate_KnownPoint_ReturnsExpectedGovernorate(string name, double latitude, double longitude, string governorateCode)
    {
        var governorate = IraqBoundaries.Locate(latitude, longitude, AdminLevel.Governorate);

        Assert.True(governorate is not null, name);
        Assert.Equal(governorateCode, governorate!.Code);
        Assert.Equal(governorateCode, IraqBoundaries.Locate(latitude, longitude)!.Governorate.Code);
    }

    [Fact]
    public void Locate_KnownPoint_ReturnsDistrictChain()
    {
        var subDistrict = IraqBoundaries.Locate(33.3340, 44.4170)!;
        var district = IraqBoundaries.Locate(33.3340, 44.4170, AdminLevel.District)!;

        Assert.Equal(AdminLevel.SubDistrict, subDistrict.Level);
        Assert.Same(subDistrict.District, district);
        Assert.Equal(AdminLevel.District, district.Level);
    }

    [Theory]
    [MemberData(nameof(OutsidePoints))]
    public void Locate_OutsideIraq_ReturnsNull(string name, double latitude, double longitude)
    {
        Assert.True(IraqBoundaries.Locate(latitude, longitude) is null, name);
        Assert.Null(IraqBoundaries.Locate(latitude, longitude, AdminLevel.Governorate));
        Assert.False(IraqBoundaries.Contains("IQG08", latitude, longitude));
    }

    [Fact]
    public void Locate_NearSharedBorder_NeverThrowsAndStaysInside()
    {
        var data = BoundaryLoader.Load();
        var offsets = new[] { -0.0003, -0.00005, 0.00005, 0.0003 };
        var vertexCount = data.RingLengths[0];
        var resolved = 0;
        for (var i = 0; i < vertexCount; i += 7)
        {
            var longitude = data.Coordinates[data.RingStarts[0] + 2 * i];
            var latitude = data.Coordinates[data.RingStarts[0] + 2 * i + 1];
            foreach (var offset in offsets)
            {
                var found = IraqBoundaries.Locate(latitude + offset, longitude + offset);
                resolved += found is null ? 0 : 1;
            }
        }

        Assert.True(resolved > 0);
    }

    [Fact]
    public void Locate_JustOutsideAPolygon_SnapsToItWithinSnapDistance()
    {
        var data = BoundaryLoader.Load();
        var index = BoundaryIndex.Instance;
        var snappedCount = 0;
        var snapOffset = BoundaryIndex.SnapDistanceDegrees / 2;
        for (var polygon = 0; polygon < 30 && polygon < index.PolygonCount; polygon++)
        {
            var ringStart = data.RingStarts[data.PolygonFirstRings[polygon]];
            var ringLength = data.RingLengths[data.PolygonFirstRings[polygon]];
            for (var i = 0; i < ringLength; i += 5)
            {
                var longitude = data.Coordinates[ringStart + 2 * i];
                var latitude = data.Coordinates[ringStart + 2 * i + 1];
                foreach (var (latitudeShift, longitudeShift) in new[] { (snapOffset, 0.0), (-snapOffset, 0.0), (0.0, snapOffset), (0.0, -snapOffset) })
                {
                    var found = index.Locate(latitude + latitudeShift, longitude + longitudeShift, out var isSnapped);
                    if (isSnapped)
                    {
                        Assert.NotNull(found);
                        snappedCount++;
                    }
                }
            }
        }

        Assert.True(snappedCount > 0);
    }

    [Fact]
    public void Locate_BeyondSnapDistanceFromTheCoast_ReturnsNull()
    {
        Assert.Null(IraqBoundaries.Locate(29.3, 48.9));
    }

    [Theory]
    [InlineData(double.NaN, 44.0)]
    [InlineData(33.0, double.NaN)]
    public void Locate_NaN_ThrowsArgumentException(double latitude, double longitude)
    {
        Assert.Throws<ArgumentException>(() => IraqBoundaries.Locate(latitude, longitude));
        Assert.Throws<ArgumentException>(() => IraqBoundaries.Locate(latitude, longitude, AdminLevel.District));
        Assert.Throws<ArgumentException>(() => IraqBoundaries.Contains("IQG08", latitude, longitude));
    }

    [Theory]
    [InlineData(90.0001, 44.0)]
    [InlineData(-90.0001, 44.0)]
    [InlineData(33.0, 180.0001)]
    [InlineData(33.0, -180.0001)]
    [InlineData(double.PositiveInfinity, 44.0)]
    public void Locate_OutOfRange_ThrowsArgumentOutOfRange(double latitude, double longitude)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => IraqBoundaries.Locate(latitude, longitude));
        Assert.Throws<ArgumentOutOfRangeException>(() => IraqBoundaries.Locate(latitude, longitude, AdminLevel.Governorate));
        Assert.Throws<ArgumentOutOfRangeException>(() => IraqBoundaries.Contains("IQG08", latitude, longitude));
    }

    [Fact]
    public void Locate_UnknownLevel_ThrowsArgumentOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => IraqBoundaries.Locate(33.3, 44.4, (AdminLevel)42));
    }

    [Fact]
    public void Locate_AtExactRangeLimits_DoesNotThrow()
    {
        Assert.Null(IraqBoundaries.Locate(90, 180));
        Assert.Null(IraqBoundaries.Locate(-90, -180));
    }

    [Fact]
    public void Contains_AtAnyLevel_MatchesTheLocatedChain()
    {
        var subDistrict = IraqBoundaries.Locate(33.3340, 44.4170)!;

        Assert.True(IraqBoundaries.Contains(subDistrict.Code, 33.3340, 44.4170));
        Assert.True(IraqBoundaries.Contains(subDistrict.District!.Code, 33.3340, 44.4170));
        Assert.True(IraqBoundaries.Contains(subDistrict.Governorate.Code, 33.3340, 44.4170));
    }

    [Fact]
    public void Contains_OtherDivisions_ReturnsFalse()
    {
        Assert.False(IraqBoundaries.Contains("IQG11", 33.3340, 44.4170));
        var subDistrict = IraqBoundaries.Locate(33.3340, 44.4170)!;
        var sibling = IraqDivisions.All.First(division => division.Level == AdminLevel.SubDistrict && !ReferenceEquals(division, subDistrict));
        Assert.False(IraqBoundaries.Contains(sibling.Code, 33.3340, 44.4170));
        var otherDistrict = IraqDivisions.All.First(division => division.Level == AdminLevel.District && !ReferenceEquals(division, subDistrict.District));
        Assert.False(IraqBoundaries.Contains(otherDistrict.Code, 33.3340, 44.4170));
    }

    [Theory]
    [InlineData("NOPE")]
    [InlineData("")]
    [InlineData(null)]
    public void Contains_UnknownCode_ThrowsKeyNotFound(string? code)
    {
        Assert.Throws<KeyNotFoundException>(() => IraqBoundaries.Contains(code!, 33.3340, 44.4170));
    }

    [Fact]
    public async Task Locate_FromSixteenThreads_GivesIdenticalResults()
    {
        var points = Enumerable.Range(0, 3000)
            .Select(i => (Latitude: 29.0 + (i * 7919 % 3000) / 3000.0 * 8.5, Longitude: 38.8 + (i * 104729 % 3000) / 3000.0 * 10.0))
            .ToArray();
        var expected = points.Select(point => IraqBoundaries.Locate(point.Latitude, point.Longitude)?.Code).ToArray();

        var results = Enumerable.Range(0, 16)
            .Select(_ => Task.Run(() => points.Select(point => IraqBoundaries.Locate(point.Latitude, point.Longitude)?.Code).ToArray()))
            .ToArray();
        var all = await Task.WhenAll(results);

        Assert.Contains(expected, code => code is not null);
        foreach (var result in all)
        {
            Assert.Equal(expected, result);
        }
    }

    [Fact]
    public void Index_EverySubDistrictHasPolygons()
    {
        var index = BoundaryIndex.Instance;
        var subDistricts = IraqDivisions.All.Where(division => division.Level == AdminLevel.SubDistrict).ToArray();

        Assert.Equal(294, subDistricts.Length);
        Assert.Equal(294, index.LoadedDivisions.Count());
        Assert.All(subDistricts, subDistrict => Assert.True(index.PolygonCountOf(subDistrict.Code) > 0, subDistrict.Code));
        Assert.True(index.PolygonCount >= 294);
        Assert.True(index.GridCellCount > 1000);
    }

    [Fact]
    public void Index_WithMalformedData_Throws()
    {
        Assert.Throws<InvalidDataException>(() => BoundaryLoader.Parse(System.Text.Encoding.UTF8.GetBytes("[]")));
        Assert.Throws<InvalidDataException>(() => BoundaryLoader.Parse(System.Text.Encoding.UTF8.GetBytes("{\"divisions\":{}}")));
    }

    [Fact]
    public void Index_WithUnknownCode_Throws()
    {
        var json = "{\"schemaVersion\":1,\"divisions\":[{\"code\":\"ZZ\",\"bbox\":[0,0,1,1],\"polygons\":[[[[0,0],[1,0],[1,1],[0,0]]]]}]}";

        Assert.Throws<InvalidDataException>(() => new BoundaryIndex(BoundaryLoader.Parse(System.Text.Encoding.UTF8.GetBytes(json))));
    }

    [Fact]
    public void Locate_WithHole_ExcludesThePointInsideTheHole()
    {
        var code = IraqDivisions.All.First(division => division.Level == AdminLevel.SubDistrict).Code;
        var json = "{\"divisions\":[{\"code\":\"" + code + "\",\"polygons\":[[" +
            "[[44,33],[45,33],[45,34],[44,34]],[[44.4,33.4],[44.6,33.4],[44.6,33.6],[44.4,33.6]]]]}]}";
        var index = new BoundaryIndex(BoundaryLoader.Parse(System.Text.Encoding.UTF8.GetBytes(json)));

        Assert.NotNull(index.Locate(33.1, 44.1, out _));
        Assert.Null(index.Locate(33.5, 44.5, out _));
    }

    [Fact]
    public void Locate_WithOverlap_PrefersSmallestPolygonThenCode()
    {
        var codes = IraqDivisions.All.Where(division => division.Level == AdminLevel.SubDistrict).Select(division => division.Code).OrderBy(c => c, StringComparer.Ordinal).Take(3).ToArray();
        var json = "{\"divisions\":[" +
            "{\"code\":\"" + codes[0] + "\",\"polygons\":[[[[44,33],[46,33],[46,35],[44,35]]]]}," +
            "{\"code\":\"" + codes[1] + "\",\"polygons\":[[[[44.5,33.5],[45.5,33.5],[45.5,34.5],[44.5,34.5]]]]}," +
            "{\"code\":\"" + codes[2] + "\",\"polygons\":[[[[44.5,33.5],[45.5,33.5],[45.5,34.5],[44.5,34.5]]]]}]}";
        var index = new BoundaryIndex(BoundaryLoader.Parse(System.Text.Encoding.UTF8.GetBytes(json)));

        Assert.Equal(codes[1], index.Locate(34.0, 45.0, out _)!.Code);
        Assert.Equal(codes[0], index.Locate(33.2, 44.2, out _)!.Code);
    }

    [Fact]
    public void Locate_WithGap_SnapsToTheNearestEdgeWithinDistance()
    {
        var codes = IraqDivisions.All.Where(division => division.Level == AdminLevel.SubDistrict).Select(division => division.Code).Take(2).ToArray();
        var json = "{\"divisions\":[" +
            "{\"code\":\"" + codes[0] + "\",\"polygons\":[[[[44,33],[45,33],[45,34],[44,34]]]]}," +
            "{\"code\":\"" + codes[1] + "\",\"polygons\":[[[[45.0006,33],[46,33],[46,34],[45.0006,34]]]]}]}";
        var index = new BoundaryIndex(BoundaryLoader.Parse(System.Text.Encoding.UTF8.GetBytes(json)));

        var nearFirst = index.Locate(33.5, 45.0002, out var isFirstSnapped);
        var nearSecond = index.Locate(33.5, 45.0004, out var isSecondSnapped);
        var farFromBoth = index.Locate(33.5, 45.0030, out var isFarSnapped);

        Assert.True(isFirstSnapped);
        Assert.Equal(codes[0], nearFirst!.Code);
        Assert.True(isSecondSnapped);
        Assert.Equal(codes[1], nearSecond!.Code);
        Assert.Equal(codes[1], farFromBoth!.Code);
        Assert.False(isFarSnapped);
        Assert.Null(index.Locate(33.5, 43.9990, out _));
    }
}
