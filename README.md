# IraqAdminDivisions

[العربية](README.ar.md)

Every Iraqi governorate, district and sub-district in Arabic, English and Kurdish, with stable codes, search, full addresses in every language, and offline GPS-to-address lookup. One NuGet package, no lookup tables, no network calls.

[![NuGet](https://img.shields.io/nuget/v/IraqAdminDivisions.svg?label=IraqAdminDivisions)](https://www.nuget.org/packages/IraqAdminDivisions)
[![Downloads](https://img.shields.io/nuget/dt/IraqAdminDivisions.svg)](https://www.nuget.org/packages/IraqAdminDivisions)
[![NuGet Boundaries](https://img.shields.io/nuget/v/IraqAdminDivisions.Boundaries.svg?label=Boundaries)](https://www.nuget.org/packages/IraqAdminDivisions.Boundaries)
[![NuGet AspNetCore](https://img.shields.io/nuget/v/IraqAdminDivisions.AspNetCore.svg?label=AspNetCore)](https://www.nuget.org/packages/IraqAdminDivisions.AspNetCore)
[![CI](https://github.com/AKAAmmarKamil/IraqAdminDivisions/actions/workflows/ci.yml/badge.svg)](https://github.com/AKAAmmarKamil/IraqAdminDivisions/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/code-MIT-blue.svg)](LICENSE)

## Why

Address forms in Iraq usually end up with lookup tables for governorates, districts and sub-districts, foreign keys, seed scripts and a column per language. This package replaces all of that with **one string column**: store the code of the area, and the library gives you its names, its parents and the full address in Arabic, English and Kurdish.

- Codes are [OCHA P-codes](https://data.humdata.org/dataset/cod-ab-irq), already used by UN and NGO datasets.
- Codes are hierarchical, so "all customers in Baghdad" is a single `LIKE 'IQG08%'`.
- Everything is embedded in the assembly: offline, no database, no API.

## Packages

| Package | What it adds | Size |
|---|---|---|
| `IraqAdminDivisions` | Names, codes, hierarchy, search, addresses, validation | small |
| `IraqAdminDivisions.Boundaries` | `Locate(lat, lng)`: GPS point → sub-district → district → governorate | + polygons |
| `IraqAdminDivisions.AspNetCore` | `app.MapIraqDivisions()`: ready endpoints for dropdowns | tiny |

```bash
dotnet add package IraqAdminDivisions
dotnet add package IraqAdminDivisions.Boundaries   # optional: GPS → address
dotnet add package IraqAdminDivisions.AspNetCore   # optional: HTTP endpoints
```

Targets `netstandard2.0` and `net8.0` (.NET Framework 4.6.1 up to current .NET). Trimming and Native AOT compatible on .NET 8+.

## Quick start

### 1. Store one code

```csharp
public class Customer
{
    public int Id { get; set; }

    [IraqDivisionCode(AdminLevel.SubDistrict)]
    public string AreaCode { get; set; } = "";      // "IQG07Q02N02"

    public string Landmark { get; set; } = "";      // free text, e.g. near a mosque
}
```

### 2. Show the address in one language, or all three

```csharp
IraqDivisions.FormatAddress("IQG07Q02N02", Language.Arabic);   // ابي غرق، الحلة، بابل
IraqDivisions.FormatAddress("IQG07Q02N02", Language.English);  // Abi Gharaq, Al-Hilla, Babil

var address = IraqDivisions.GetAddress("IQG07Q02N02");
address.Ar;   // ابي غرق، الحلة، بابل
address.En;   // Abi Gharaq, Al-Hilla, Babil
address.Ku;   // ئەبی غەرەق، حیللە، بابل
```

Or each part on its own:

```csharp
var area = IraqDivisions.Find("IQG07Q02N02")!;
area.Name.Ar;               // ابي غرق
area.District!.Name.Ar;     // الحلة
area.Governorate.Name.Ar;   // بابل
```

### 3. Filter by governorate or district in your database

A child's code always starts with its parent's code:

```
IQG08Q03N03   Al-Mansour sub-district
IQG08Q03      Al-Karkh district
IQG08         Baghdad governorate
```

So EF Core needs no join and no lookup table:

```csharp
db.Customers.Where(c => c.AreaCode.StartsWith("IQG08"));      // all of Baghdad
db.Customers.Where(c => c.AreaCode.StartsWith("IQG08Q03"));   // Al-Karkh only
// SQL: WHERE "AreaCode" LIKE 'IQG08%'   (can use an index)
```

From a search box ("بغداد", "baghdad", even "بغدات"):

```csharp
var place = IraqDivisions.Search("بغداد")[0].Division;          // IQG08
db.Customers.Where(c => c.AreaCode.StartsWith(place.Code));
```

On PostgreSQL, give the column a `text_pattern_ops` index (or `C` collation) so `LIKE 'prefix%'` uses it.

### 4. Only governorate → area? (two dropdowns)

Many apps don't want districts and sub-districts. `Localities` is one flat list of the areas of a governorate (districts plus sub-districts, without the "Markaz X" duplicates):

```csharp
IraqDivisions.Governorates;                       // first dropdown
IraqDivisions.Find("IQG07")!.Localities;          // second dropdown: Al-Hashimiya, Al-Shomaly, Al-Madhatiya, Al-Hilla, ...
```

Store the chosen area's code; `FormatAddress`, `GetAddress` and the `StartsWith` filters work the same.

### 5. GPS → address (Boundaries package)

```csharp
using IraqAdminDivisions.Boundaries;

var area = IraqBoundaries.Locate(33.3340, 44.4170);                       // sub-district, or null outside Iraq
var governorate = IraqBoundaries.Locate(lat, lng, AdminLevel.Governorate);
IraqBoundaries.Contains("IQG08", lat, lng);                               // is this point in Baghdad?
```

Point-in-polygon over the real OCHA boundaries, not the nearest centre. Units the 2024 census added have no open boundary yet: when some were carved out of the sub-district `Locate` returns, they are listed in its `CarvedOut`, and the point may belong to one of them.

### 6. ASP.NET Core endpoints

```csharp
using IraqAdminDivisions.AspNetCore;

app.MapIraqDivisions();                      // default prefix /api/iraq-divisions
app.MapIraqDivisions("/api/divisions")       // or your own; returns a RouteGroupBuilder
   .RequireAuthorization();
```

| Endpoint | Returns |
|---|---|
| `GET /governorates` | All governorates |
| `GET /{code}` | One division |
| `GET /{code}/children` | Its children (cascading dropdowns) |
| `GET /{code}/localities` | Flat area list of a governorate |
| `GET /search?q=&level=&limit=` | Search results with scores |
| `GET /nearest?lat=&lng=&level=` | Nearest division centre |

Language: `?lang=ar|en|ku` or the `Accept-Language` header. Every item carries `name`, `names {ar,en,ku}`, `address`, `addresses {ar,en,ku}` and `kurdishNameSource`.

## API

| Member | Description |
|---|---|
| `IraqDivisions.Governorates` / `All` | The 19 governorates / all 636 current divisions |
| `IraqDivisions.Find(code)` | Division by code (case-insensitive, trimmed), including deprecated codes, or `null` |
| `IraqDivisions.IsValid(code, level?)` | Is the code current (not deprecated) and of that level; use it for new input |
| `IraqDivisions.FormatAddress(code, language)` | Full address in one language |
| `IraqDivisions.GetAddress(code)` | Full address in Arabic, English and Kurdish |
| `IraqDivisions.Search(query, limit, level?)` | Tolerant search: ه/ة, ي/ى, أ/إ/ا, tashkeel, "ال", "Al-", one typo |
| `IraqDivisions.Nearest(lat, lng, level)` | Nearest division centre (approximate; use `Locate` for exact) |
| `IraqDivisions.IsWithin(code, ancestorCode)` | Is the division the ancestor or under it (walks `Parent`, so it also follows a deprecated code's historical parent) |
| `IraqDivisions.Descendants(code)` | Every current division under a governorate or district |
| `Division.Name` / `Address` | `Ar`, `En`, `Ku`, `Get(language)` |
| `Division.Parent / Children / District / Governorate` | Hierarchy, pre-linked |
| `Division.Localities` | Flat area list (governorates only) |
| `Division.KurdishNameSource` / `KurdishNameDataset` | `Wikidata`, `Dataset` (+ which one), `Transliterated` or `Manual` |
| `Division.IsoCode` | ISO 3166-2 code of a governorate (`IQ-BG`, ...) |
| `Division.OchaCode` | The original OCHA P-code, for joining with UN/NGO data |
| `Division.IsDistrictCentre` | The "Markaz X" sub-district of a district |
| `Division.Latitude / Longitude / AreaSqKm / WikidataId / Aliases` | Extra data |
| `Division.IsDeprecated / ReplacedByCode` | Code lifecycle |
| `Division.HasBoundary` | Has its own polygon; `false` for the units the 2024 census added |
| `Division.CarvedFrom` / `CarvedFromCode` / `CarvedOut` | For a unit without a polygon: the OCHA sub-district it was taken from, and the reverse list |
| `[IraqDivisionCode(level?)]` | DataAnnotations validation |
| `IraqBoundaries.Locate(lat, lng[, level])` / `Contains(code, lat, lng)` | GPS lookups (Boundaries package) |

## Performance

Measured with BenchmarkDotNet on .NET 10 (`benchmarks/`). All data is loaded once, lazily and thread-safely.

| Operation | Mean | Allocated |
|---|---|---|
| `Find` | 17 ns | 0 B |
| `FormatAddress` / `GetAddress` | 17 ns | 0 B |
| `Localities` | 6 ns | 0 B |
| `Search` (Arabic, English, one typo) | 14 µs | ~250 B (the result list) |
| First load of all divisions | 1.5 ms | |
| `IraqBoundaries.Locate` (point in Baghdad / random point) | 56 ns / 35 ns | 0 B |
| `IraqBoundaries.Locate` (point outside Iraq) | < 1 ns | 0 B |
| First load of the boundaries (parse + spatial index) | ~25 ms | ~3 MB retained |

`Locate` uses a 0.05° grid where each cell is pre-classified as inside one area, outside, or on a border; border cells only test the polygon edges that cross them. It returns exactly the same results as a brute-force point-in-polygon check (verified on 400,000+ points).

## Data and coverage

| Level | Count | Kurdish from Wikidata | from an open dataset | transliterated | manual |
|---|---|---|---|---|---|
| Governorate | 19 | 18 | 0 | 0 | 1 |
| District | 175 | 61 | 83 | 30 | 1 |
| Sub-district | 442 | 60 | 64 | 317 | 1 |
| Total | 636 | 139 | 147 | 347 | 3 |

Every division has Arabic, English and Kurdish names. The 101 districts and 294 sub-districts from OCHA have boundary polygons; the 74 districts and 148 sub-districts added from the 2024 census do not (see below). Full reports: [reports/build-report.md](reports/build-report.md), [reports/kurdish-crosscheck.md](reports/kurdish-crosscheck.md) and [reports/census-reconciliation.md](reports/census-reconciliation.md).

### About the Kurdish names

1. **Wikidata** first, when a confident match exists (same name, same parent, close enough).
2. **Open datasets** next. Three open Iraqi datasets were compared against our 96 Wikidata names; only one, [aryzonedev/iraq-cities](https://github.com/aryzonedev/iraq-cities) (CC BY 4.0), agreed with Wikidata often enough (81% beyond governorate level) to be used. Its names fill 102 OCHA rows (`KurdishNameDataset` says which); 45 census districts that were promoted from one of those sub-districts carry the same name. This is a calibrated source, not proof of native review. [karam-fikre/iraqi-data](https://github.com/karam-fikre/iraqi-data) was used only for comparison because its own documentation calls its Kurdish names unreviewed drafts.
3. **Transliterated**: the remaining **347 names are transliterations** in Sorani orthography (e.g. ة → ە, "مركز X" → "ناوەندی X"). The 214 OCHA rows were transliterated with AI assistance from the Arabic and English names, using the known local Kurdish name where one is well established (e.g. Altun Kupri → پردێ). The 133 census rows were transliterated by rule, from the Kurdish Latin spelling the census list gives in the Kurdistan Region and from the Arabic elsewhere. None has **been reviewed by a native speaker**.

Each name says where it came from, so you decide what to show:

```csharp
var kurdish = area.KurdishNameSource == KurdishNameSource.Transliterated
    ? area.Name.Ar          // or show it with a "machine transliterated" hint
    : area.Name.Ku;
```

Corrections from Kurdish speakers are the most valuable contribution to this project.

## How the data is built

`tools/IraqAdminDivisions.Builder` produces `data/divisions.json` and `data/boundaries.json`, which are committed and embedded in the packages.

1. **Base**: OCHA COD-AB Iraq v01: codes, Arabic and English names, centres, areas and boundaries.
2. **Kurdish**: Wikidata `ckb` labels matched by name + parent + distance; name-only matches are rejected; type prefixes ("پارێزگای", "قەزای") are stripped; anything that is not pure Sorani is rejected.
3. **Open datasets**: vendored at pinned commits in `data/sources/`, calibrated against Wikidata before use (see [reports/kurdish-crosscheck.md](reports/kurdish-crosscheck.md)).
4. **Overrides**: [data/overrides/corrections.tsv](data/overrides/corrections.tsv) fixes source errors, each with a reason (e.g. OCHA lists Baladruz district with the Arabic name of Mandali); [data/overrides/kurdish-transliterated.tsv](data/overrides/kurdish-transliterated.tsv) fills the remaining Kurdish names; [data/overrides/restructures.json](data/overrides/restructures.json) applies changes OCHA has not caught up with (Halabja, the 2024 census). Replaced values stay searchable as aliases.
5. **ISO 3166-2**: governorate codes from Wikidata (P300), validated as unique `IQ-XX`.
6. **2024 census**: units the census lists and OCHA does not are added with a project code (see below). Each needs a source; a Wikidata coordinate, when there is one, must fall inside the OCHA sub-district it was carved from.
7. **Boundaries**: OCHA polygons simplified with Douglas-Peucker (~11 m tolerance); every OCHA division's centre lies inside its own polygon.
8. **Validation**: the build fails on wrong counts, duplicate or non-hierarchical codes, a project code below 51, a deprecated code without a current replacement, missing names in any language, non-Sorani Kurdish, a bad ISO code, an OCHA sub-district without a polygon, a census unit without a source or with a coordinate outside the sub-district it was carved from, or a district without sub-districts.
9. **Not used**: OpenStreetMap (ODbL share-alike) and GADM (no commercial use).

A scheduled workflow re-runs the builder monthly and opens a pull request when the sources change.

## Halabja Governorate

Halabja became Iraq's 19th governorate by law (passed by the Council of Representatives on 2025-04-14, published in the Official Gazette on 2025-05-05). OCHA COD-AB v01 still lists it as a district of Sulaymaniyah, so the package applies the change on top of OCHA with [data/overrides/restructures.json](data/overrides/restructures.json):

| Code | What |
|---|---|
| `IQG19` | Halabja Governorate (code assigned by this project; no confirmed ISO 3166-2 code yet) |
| `IQG19Q01` | Halabja district (OCHA `IQG06Q05`), with Markaz Halabja and Shahrazur |
| `IQG19Q52`, `IQG19Q53`, `IQG19Q54` | Byara, Khurmal and Sirwan districts from the 2024 census; the OCHA sub-districts Biyara and Khurmal are their centres (`IQG19Q52N51`, `IQG19Q53N51`) |

`OchaCode` keeps the OCHA P-codes (`IQG06Q05…`), so the rows still join with UN data. Bamo, the census's fifth district, is not added yet: no open source places it. The 1.0.1 codes `IQG06Q05…` were removed in 1.1.0 (see "Code stability").

## 2024 census

The 2024 census (COSIT, 2024-11-25) organises Iraq into 19 governorates, about 185 districts and 433 sub-districts, against OCHA's 101 and 294. Version 1.1.0 adds the units OCHA does not list:

- **74 districts and 148 sub-districts**, coded from `51` inside their parent (`IQG11Q52`, `IQG11Q52N51`), with `OchaCode = null` and `HasBoundary = false`. Each one lists its `sources` in `divisions.json` and the exports: a Wikidata item when one matches (111 rows), and the marker `census-2024` for the census listing (111 rows have no other open source yet).
- **71 OCHA sub-districts moved to the district the census places them in**, with a new code under that district. `OchaCode` keeps the OCHA P-code. A sub-district promoted to a district becomes that district's centre (`مركز X` / `Markaz X`, `IsDistrictCentre`); its old name stays searchable as an alias.
- **No polygons for the new units.** No open source has their boundaries, and none are invented. `CarvedFrom` names the OCHA sub-district each unit was taken from: the one its Wikidata coordinate falls in, or one of its census district's sub-districts when there is no coordinate. `Locate` returns that OCHA sub-district and lists the new units in `CarvedOut`.
- **Not added:** units no open source can place (for example Sadr City 1 and 2, Al-Zahur and Al-Zawra in Baghdad, Khabat in Erbil, Al-Hur in Karbala and Bamo in Halabja), and the Kurdistan Region listing of districts that OCHA and the federal lists place in another governorate (Akre, Shekhan and Bardarash under Duhok; Makhmur, Khanaqin, Kifri). These keep their OCHA governorate. Every unit and the reason is in [reports/census-reconciliation.md](reports/census-reconciliation.md).

The census figures come from the census listing by name and hierarchy only; no population figure or third-party database content is shipped.

## Limitations

- **347 Kurdish names are transliterated** and 147 come from a calibrated open dataset; neither is native-reviewed (see above).
- **Units added from the 2024 census have no boundary** (see above), and the district and governorate polygons in the GeoJSON export are OCHA's: a district's polygon still covers sub-districts the census moved to a new district. `Locate` is unaffected at district and governorate level, because it walks up from the sub-district's current parent.
- **No neighbourhoods (mahallas)** yet; keep a free-text landmark next to the code.
- **Halabja's internal composition** follows OCHA, not the federal list (see above). The Sulaymaniyah governorate polygon in the GeoJSON export still includes Halabja; `Locate` is unaffected because it works on sub-districts.
- Arabic spellings follow OCHA (e.g. "ابي غرق" without hamza); search ignores the difference.

## Code stability

From 1.1.0 on, a released code is never removed or reused. If a division changes, its old code still resolves, is flagged `IsDeprecated` and points to its successor through `ReplacedByCode`, so updating the package never breaks stored data. A snapshot test checks every released code, and package validation blocks breaking API changes.

1.1.0 is the baseline: 1.0.1 had no downloads, so 76 of its codes were renumbered instead of being kept as deprecated rows (the Halabja codes `IQG06Q05…` and the 71 sub-districts the census moved). The full list is in [reports/code-changes-1.1.0.md](reports/code-changes-1.1.0.md); `OchaCode` keeps the OCHA P-code of every moved row.

- **A code always starts with its parent's code**, so `StartsWith` works at every level.
- **Numbering:** codes `01`–`50` inside a parent are OCHA's numbering space. Districts and sub-districts this project adds or moves are numbered from `51` (`IQGxxQ51`, `…N51`), so a project code is visibly not an OCHA code. Governorate `IQG19` (Halabja) is the only project-assigned governorate code.

## Quality

- Tests run on .NET 8, .NET 10 and .NET Framework 4.8 (the `netstandard2.0` build).
- 100% line coverage of the core and ASP.NET Core packages; the Boundaries package checks every division centre plus known city points (including Halabja) and points outside Iraq, and proves its fast path returns exactly what a brute-force point-in-polygon check returns.

## Data exports (non-.NET users)

| File | Format |
|---|---|
| [data/divisions.json](data/divisions.json) | Source of truth, all levels |
| [data/exports/divisions.csv](data/exports/divisions.csv) | CSV (UTF-8 with BOM, opens in Excel) |
| [data/exports/divisions.sql](data/exports/divisions.sql) | SQL (PostgreSQL / generic) |
| [data/exports/divisions.sqlserver.sql](data/exports/divisions.sqlserver.sql) | SQL Server (`NVARCHAR`, `N'...'`) |
| [data/exports/divisions.geojson](data/exports/divisions.geojson) | GeoJSON with boundaries, all levels (OCHA units only; units without a boundary are left out) |

## Contributing

Found a wrong or missing name, especially a Kurdish one? Open a [data correction issue](https://github.com/AKAAmmarKamil/IraqAdminDivisions/issues/new?template=data-correction.yml) or a PR to the files in `data/overrides/` with a source. See [CONTRIBUTING.md](CONTRIBUTING.md).

## Attribution

Administrative divisions and boundaries: UN OCHA, Common Operational Dataset - Administrative Boundaries (COD-AB) Iraq, via the Humanitarian Data Exchange, licensed under [CC BY-IGO 3.0](https://creativecommons.org/licenses/by/3.0/igo/). The data was modified (corrected, restructured for Halabja and the 2024 census, merged with Wikidata labels, dataset names and transliterations, simplified and re-exported). This is not an official UN or OCHA product. Units added from the 2024 census (COSIT) use the census names and hierarchy only. Kurdish names: Wikidata (CC0), [aryzonedev/iraq-cities](https://github.com/aryzonedev/iraq-cities) (CC BY 4.0) and this project. See [DATA_LICENSE.md](DATA_LICENSE.md).

## License

Code: [MIT](LICENSE). Data: CC BY-IGO 3.0 (OCHA), CC0 (Wikidata) and CC BY 4.0 (aryzonedev/iraq-cities), see [DATA_LICENSE.md](DATA_LICENSE.md).
