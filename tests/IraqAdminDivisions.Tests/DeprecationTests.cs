namespace IraqAdminDivisions.Tests;

/// <summary>The shipped data has no deprecated rows yet (1.1.0 is the baseline), so the deprecation behaviour is checked on a small synthetic file.</summary>
public class DeprecationTests
{
    private static DivisionStore LoadSample() => DivisionStore.Load(new DivisionFile
    {
        Divisions =
        {
            Record("IQG98", "Governorate", null, "محافظة", "Governorate"),
            Record("IQG98Q01", "District", "IQG98", "قضاء قديم", "Old district", isDeprecated: true, replacedBy: "IQG98Q02"),
            Record("IQG98Q01N01", "SubDistrict", "IQG98Q01", "ناحية قديمة", "Old sub-district", isDeprecated: true, replacedBy: "IQG98Q02N01"),
            Record("IQG98Q02", "District", "IQG98", "قضاء جديد", "New district"),
            Record("IQG98Q02N01", "SubDistrict", "IQG98Q02", "ناحية جديدة", "New sub-district")
        }
    });

    private static DivisionRecord Record(string code, string level, string? parent, string ar, string en, bool isDeprecated = false, string? replacedBy = null) => new DivisionRecord
    {
        Code = code,
        Level = level,
        ParentCode = parent,
        Name = new NameRecord { Ar = ar, En = en, Ku = en },
        KurdishNameSource = "manual",
        IsDeprecated = isDeprecated,
        ReplacedBy = replacedBy
    };

    [Fact]
    public void DeprecatedRows_AreFoundButLeftOutOfEveryList()
    {
        var store = LoadSample();
        var governorate = store.Find("IQG98")!;
        var old = store.Find("IQG98Q01N01")!;

        Assert.True(old.IsDeprecated);
        Assert.Equal("IQG98Q02N01", old.ReplacedByCode);
        Assert.DoesNotContain(store.All, division => division.IsDeprecated);
        Assert.DoesNotContain(governorate.Children, division => division.IsDeprecated);
        Assert.DoesNotContain(governorate.Localities, division => division.IsDeprecated);
        Assert.DoesNotContain(store.SearchEntries, entry => entry.Division.IsDeprecated);
        Assert.Equal(3, store.All.Count);
    }

    [Fact]
    public void DeprecatedDistrict_ListsOnlyItsDeprecatedChildren_AndKeepsItsHistoricalAddress()
    {
        var store = LoadSample();
        var oldDistrict = store.Find("IQG98Q01")!;

        Assert.All(oldDistrict.Children, child => Assert.True(child.IsDeprecated));
        Assert.Single(oldDistrict.Children);
        Assert.Equal("Old sub-district, Old district, Governorate", store.Find("IQG98Q01N01")!.Address.En);
    }
}
