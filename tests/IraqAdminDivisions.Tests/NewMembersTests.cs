namespace IraqAdminDivisions.Tests;

public class NewMembersTests
{
    private const string BabilCode = "IQG07";
    private const string SubDistrictCode = "IQG07Q02N02";
    private const string ArabicSeparator = "، ";
    private const string EnglishSeparator = ", ";

    [Fact]
    public void GetAddress_ReturnsAllThreeLanguagesForARealSubDistrict()
    {
        var subDistrict = IraqDivisions.Find(SubDistrictCode)!;
        var district = subDistrict.District!;
        var governorate = subDistrict.Governorate;

        var address = IraqDivisions.GetAddress(SubDistrictCode);

        Assert.Equal(string.Join(ArabicSeparator, subDistrict.Name.Ar, district.Name.Ar, governorate.Name.Ar), address.Ar);
        Assert.Equal(string.Join(EnglishSeparator, subDistrict.Name.En, district.Name.En, governorate.Name.En), address.En);
        Assert.Equal(string.Join(ArabicSeparator, subDistrict.Name.Ku, district.Name.Ku, governorate.Name.Ku), address.Ku);
    }

    [Fact]
    public void GetAddress_IsCaseInsensitiveAndThrowsForUnknownCode()
    {
        Assert.Same(IraqDivisions.GetAddress(SubDistrictCode), IraqDivisions.GetAddress(" " + SubDistrictCode.ToLowerInvariant() + " "));

        var exception = Assert.Throws<KeyNotFoundException>(() => IraqDivisions.GetAddress("IQG99X"));
        Assert.Contains("IQG99X", exception.Message);
        Assert.Throws<KeyNotFoundException>(() => IraqDivisions.GetAddress(""));
    }

    [Fact]
    public void Address_EqualsFormatAddressForEveryDivisionAndLanguage()
    {
        foreach (var division in IraqDivisions.All)
        {
            Assert.Equal(division.Address.Ar, division.FormatAddress(Language.Arabic));
            Assert.Equal(division.Address.En, division.FormatAddress(Language.English));
            Assert.Equal(division.Address.Ku, division.FormatAddress(Language.Kurdish));
            Assert.Equal(division.Address.En, IraqDivisions.FormatAddress(division.Code, Language.English));
        }
    }

    [Fact]
    public void Address_OfGovernorateIsItsName_AndEndsWithParentAddress()
    {
        var governorate = IraqDivisions.Governorates[0];
        Assert.Same(governorate.Name, governorate.Address);

        foreach (var division in IraqDivisions.All.Where(d => d.Parent != null))
        {
            Assert.EndsWith(EnglishSeparator + division.Parent!.Address.En, division.Address.En, StringComparison.Ordinal);
            Assert.EndsWith(ArabicSeparator + division.Parent.Address.Ku, division.Address.Ku, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void EveryDivision_HasArabicEnglishAndKurdishNames()
    {
        Assert.All(IraqDivisions.All, division =>
        {
            Assert.False(string.IsNullOrWhiteSpace(division.Name.Ar), division.Code);
            Assert.False(string.IsNullOrWhiteSpace(division.Name.En), division.Code);
            Assert.False(string.IsNullOrWhiteSpace(division.Name.Ku), division.Code);
        });
    }

    [Theory]
    [InlineData(Language.Arabic)]
    [InlineData(Language.English)]
    [InlineData(Language.Kurdish)]
    public void LocalizedName_GetReturnsTheMatchingLanguage(Language language)
    {
        var name = IraqDivisions.Find(SubDistrictCode)!.Name;

        var expected = language == Language.Arabic ? name.Ar : language == Language.English ? name.En : name.Ku;
        Assert.Equal(expected, name.Get(language));
        Assert.Equal(name.En, name.ToString());
    }

    [Fact]
    public void LocalizedName_GetRejectsUnknownLanguage()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => IraqDivisions.Find(SubDistrictCode)!.Name.Get((Language)99));
    }

    [Fact]
    public void KurdishNameSource_EachValueIsUsedByAtLeastOneDivision()
    {
        foreach (var source in new[] { KurdishNameSource.Wikidata, KurdishNameSource.Transliterated, KurdishNameSource.Manual })
        {
            Assert.True(IraqDivisions.All.Count(d => d.KurdishNameSource == source) > 0, source.ToString());
        }
    }

    [Fact]
    public void IsDistrictCentre_IsTrueExactlyForMarkazSubDistricts()
    {
        foreach (var division in IraqDivisions.All)
        {
            var isNamedMarkaz = division.Name.En.StartsWith("Markaz", StringComparison.Ordinal)
                || division.Name.Ar.StartsWith("مركز", StringComparison.Ordinal);

            Assert.Equal(isNamedMarkaz, division.IsDistrictCentre);
            if (division.IsDistrictCentre)
            {
                Assert.Equal(AdminLevel.SubDistrict, division.Level);
            }
        }
    }

    [Fact]
    public void Localities_OfAGovernorateAreItsDistrictsPlusNonCentreSubDistrictsOrderedByCode()
    {
        var babil = IraqDivisions.Find(BabilCode)!;
        var codes = babil.Localities.Select(l => l.Code).ToList();

        Assert.All(babil.Children, district => Assert.Contains(district.Code, codes));
        var subDistricts = babil.Children.SelectMany(district => district.Children).ToList();
        Assert.Contains(subDistricts, subDistrict => subDistrict.IsDistrictCentre);
        Assert.All(subDistricts.Where(s => s.IsDistrictCentre), centre => Assert.DoesNotContain(centre.Code, codes));
        Assert.All(subDistricts.Where(s => !s.IsDistrictCentre), area => Assert.Contains(area.Code, codes));
        Assert.All(babil.Localities, locality => Assert.StartsWith(BabilCode, locality.Code, StringComparison.Ordinal));
        Assert.Equal(codes.OrderBy(code => code, StringComparer.Ordinal), codes);
        Assert.Equal(codes.Count, codes.Distinct().Count());
    }

    [Fact]
    public void Localities_NeverContainRowsOfOtherGovernorates()
    {
        foreach (var governorate in IraqDivisions.Governorates)
        {
            Assert.NotEmpty(governorate.Localities);
            Assert.All(governorate.Localities, locality => Assert.Same(governorate, locality.Governorate));
            Assert.All(governorate.Localities, locality => Assert.False(locality.IsDistrictCentre));
        }
    }

    [Fact]
    public void Localities_AreEmptyForDistrictsAndSubDistricts()
    {
        foreach (var division in IraqDivisions.All.Where(d => d.Level != AdminLevel.Governorate))
        {
            Assert.Empty(division.Localities);
        }
    }

    [Fact]
    public void Find_ReturnsTheSameInstanceWithOrWithoutTrimming()
    {
        Assert.Same(IraqDivisions.Find(BabilCode), IraqDivisions.Find(BabilCode.ToLowerInvariant()));
        Assert.Same(IraqDivisions.Find(BabilCode), IraqDivisions.Find(BabilCode + " "));
        Assert.Same(IraqDivisions.Find(BabilCode), IraqDivisions.Find("\n" + BabilCode));
    }

    [Theory]
    [InlineData("مركزالناصرية", "IQG17Q04N02")]
    [InlineData("مركز الناصرية", "IQG17Q04N02")]
    [InlineData("مندلي", "IQG10Q03N01")]
    [InlineData("مندلي", "IQG10Q03")]
    public void Search_ResolvesNamesAndAliasesThatCameFromCorrections(string query, string expectedCode)
    {
        var results = IraqDivisions.Search(query, 5);

        Assert.Contains(results, result => result.Division.Code == expectedCode);
        Assert.Equal(100, results.First(result => result.Division.Code == expectedCode).Score);
    }

    [Fact]
    public void Search_ReplacedOchaArabicNameStaysResolvable()
    {
        var results = IraqDivisions.Search("مركزالناصرية", 1);

        Assert.Equal("IQG17Q04N02", Assert.Single(results).Division.Code);
    }

    [Fact]
    public void Search_MatchesKurdishNames()
    {
        var governorate = IraqDivisions.Governorates[0];

        var results = IraqDivisions.Search(governorate.Name.Ku, 5, AdminLevel.Governorate);

        Assert.Equal(governorate.Code, results[0].Division.Code);
    }

    [Fact]
    public void Search_RanksByScoreThenLevelThenCode_AndReturnsAtMostLimit()
    {
        var all = IraqDivisions.Search("a", int.MaxValue);
        var top = IraqDivisions.Search("a", 7);

        Assert.Equal(7, top.Count);
        Assert.Equal(all.Take(7).Select(r => r.Division.Code), top.Select(r => r.Division.Code));
        for (var i = 1; i < all.Count; i++)
        {
            var previous = all[i - 1];
            var current = all[i];
            var isOrdered = previous.Score > current.Score
                || (previous.Score == current.Score && previous.Division.Level < current.Division.Level)
                || (previous.Score == current.Score && previous.Division.Level == current.Division.Level
                    && string.CompareOrdinal(previous.Division.Code, current.Division.Code) < 0);
            Assert.True(isOrdered, previous.Division.Code + " before " + current.Division.Code);
        }
    }

    [Fact]
    public void Search_ReturnsEmptyForNonPositiveLimit()
    {
        Assert.Empty(IraqDivisions.Search("basra", 0));
        Assert.Empty(IraqDivisions.Search("basra", -3));
    }

    [Fact]
    public void Search_HandlesQueriesLongerThanTheStackBuffer()
    {
        var longQuery = new string('x', SearchNormalizer.StackBufferLength + 50);

        Assert.Empty(IraqDivisions.Search(longQuery));
        Assert.Equal("basrah", SearchNormalizer.Normalize("  EL-Basrah " + new string(' ', 300)));
    }

    [Theory]
    [InlineData("al-", "")]
    [InlineData("Al Basrah", "basrah")]
    [InlineData("EL-Zubair", "zubair")]
    [InlineData("  ", "")]
    [InlineData("ال", "ال")]
    [InlineData("  البصرة   الجديدة ", "بصره الجديده")]
    [InlineData("کتاب", "كتاب")]
    [InlineData("یاسە", "ياسه")]
    [InlineData("مؤتمرئ", "موتمري")]
    public void Normalizer_HandlesEdgeCases(string input, string expected)
    {
        Assert.Equal(expected, SearchNormalizer.Normalize(input));
    }

    [Fact]
    public void Division_ExposesItsDataFields()
    {
        var subDistrict = IraqDivisions.Find(SubDistrictCode)!;

        Assert.True(subDistrict.AreaSqKm > 0);
        Assert.InRange(subDistrict.Latitude, 28, 38);
        Assert.InRange(subDistrict.Longitude, 38, 49);
        Assert.False(subDistrict.IsDeprecated);
        Assert.Null(subDistrict.ReplacedByCode);
        Assert.Contains(IraqDivisions.All, d => d.WikidataId != null);
        Assert.Empty(IraqDivisions.Find("IQG10Q03")!.Children.SelectMany(c => c.Localities));
    }

    [Fact]
    public void Loader_BuildsAFreshStoreEqualToTheSharedOne()
    {
        var store = DivisionStore.Load();

        Assert.Equal(IraqDivisions.All.Count, store.All.Count);
        Assert.Equal(IraqDivisions.All.Select(d => d.Code), store.All.Select(d => d.Code));
        Assert.Equal(IraqDivisions.Governorates.Count, store.Governorates.Count);
        Assert.Equal(store.All.Count, store.SearchEntries.Length);
        Assert.NotNull(store.Find("iqg02"));
        Assert.Null(store.Find("IQG99"));
    }

    [Fact]
    public void Parsers_RejectUnknownValues()
    {
        Assert.Equal(AdminLevel.District, DivisionStore.ParseLevel("District"));
        Assert.Equal(KurdishNameSource.Manual, DivisionStore.ParseKurdishNameSource("manual"));
        Assert.Throws<InvalidOperationException>(() => DivisionStore.ParseLevel("Street"));
        Assert.Throws<InvalidOperationException>(() => DivisionStore.ParseKurdishNameSource("guess"));
    }

    [Fact]
    public void SearchResult_ToStringShowsDivisionAndScore()
    {
        var result = IraqDivisions.Search("basra")[0];

        Assert.Contains(result.Division.Code, result.ToString());
        Assert.Contains(result.Score.ToString(), result.ToString());
    }

    [Fact]
    public void IraqDivisionCodeAttribute_ExposesItsLevel()
    {
        Assert.Equal(AdminLevel.District, new IraqDivisionCodeAttribute(AdminLevel.District).Level);
        Assert.Null(new IraqDivisionCodeAttribute().Level);
    }

#if NET
    [Fact]
    public void FindFormatAddressAndGetAddress_AllocateNothing()
    {
        IraqDivisions.Find(SubDistrictCode);
        IraqDivisions.FormatAddress(SubDistrictCode, Language.Arabic);
        IraqDivisions.GetAddress(SubDistrictCode);
        var before = GC.GetAllocatedBytesForCurrentThread();

        for (var i = 0; i < 1000; i++)
        {
            IraqDivisions.Find(SubDistrictCode);
            IraqDivisions.Find("iqg07q02n02");
            IraqDivisions.FormatAddress(SubDistrictCode, Language.Kurdish);
            IraqDivisions.GetAddress(SubDistrictCode);
        }

        var allocated = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.Equal(0, allocated);
    }
#endif
}
