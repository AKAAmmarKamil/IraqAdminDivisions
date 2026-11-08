# Changelog

Format follows [Keep a Changelog](https://keepachangelog.com/); versions follow [SemVer](https://semver.org/).
From 1.1.0 on, published codes are never removed (see README, "Code stability").

## [1.1.0] - 2026-11-08

The 2024 census. 1.0.1 had no downloads, so this release is the baseline: from 1.1.0 on, a released code is never removed.

### Added
- 74 districts and 148 sub-districts from the 2024 census (COSIT) that OCHA COD-AB v01 does not list: 19 governorates, 175 districts and 442 sub-districts in all. They have project codes from `51`, `OchaCode = null`, no boundary polygon, and per-row `sources` (a Wikidata item when one matches, `census-2024` for the census listing). See the README, "2024 census", and `reports/census-reconciliation.md`.
- `Division.HasBoundary`, `CarvedFromCode`, `CarvedFrom` and `CarvedOut`: a unit without a polygon names the OCHA sub-district it was taken from, and `IraqBoundaries.Locate(...).CarvedOut` lists the units a point may belong to instead.
- `IraqDivisions.IsWithin(code, ancestorCode)` and `IraqDivisions.Descendants(code)`.
- `divisions.json`: `hasBoundary`, `carvedFrom`, and `sources` on census rows. CSV/SQL exports: `has_boundary`, `carved_from`, `source_urls` columns, appended at the end.
- Builder: census reconciliation report, `moves[]` with `keepOldCode`, new units without `geometryFrom`, the `51+` code rule, and checks for sources, coordinates, Kurdish names and districts without sub-districts.

### Changed
- 76 codes from 1.0.1 were renumbered instead of kept as deprecated rows: the Halabja codes `IQG06Q05…` (now `IQG19Q01…`) and 71 sub-districts the census moved to another district of the same governorate. `OchaCode` keeps the OCHA P-code. Full list: `reports/code-changes-1.1.0.md`.
- A sub-district promoted to a district is that district's centre: it is named `مركز X` / `Markaz X`, `IsDistrictCentre` is true, and its old name is an alias.
- Halabja has four districts: Halabja, plus Byara, Khurmal and Sirwan from the census.
- `IraqBoundaries.Locate(lat, lng, AdminLevel.District)` returns the census district when the point's sub-district moved to one.
- `Search` takes about 14 µs (was 8 µs) with 53% more rows; first load about 1.5 ms.

### Fixed
- Addresses are built level by level, so they no longer depend on code order.

## [1.0.1] - 2026-11-08

First release on nuget.org. The `v1.0.0` tag was never published: nuget.org rejected its license expression, so the packages now ship `PACKAGE_LICENSE.txt` (MIT for the code, CC BY-IGO 3.0 / CC0 / CC BY 4.0 for the data). No code or data changes from 1.0.0.

## [1.0.0] - 2026-11-08 (tagged, not published)

### Added
- 19 governorates, 101 districts and 294 sub-districts: OCHA COD-AB Iraq (v01) with hierarchical OCHA P-codes, plus Halabja Governorate (`IQG19`, law in force since 2025-05-05) applied through `data/overrides/restructures.json`; the old `IQG06Q05…` codes stay resolvable as deprecated.
- Arabic, English and Kurdish (Sorani) names for every division. Kurdish comes from Wikidata (96), a calibrated open dataset (102), AI-assisted transliteration (214) or a manual entry (2), exposed as `Division.KurdishNameSource` and `KurdishNameDataset`.
- `Division.IsoCode` (ISO 3166-2 from Wikidata) and `Division.OchaCode` (original P-code).
- Source corrections with reasons in `data/overrides/corrections.tsv`; replaced values remain searchable aliases.
- `IraqDivisions`: `Find`, `IsValid`, `FormatAddress`, `GetAddress` (all three languages), `Search` (tolerant of Arabic spelling variants, "Al-" and one typo), `Nearest`, hierarchy navigation, `Localities` (flat governorate → area list), `IsDistrictCentre`.
- `IraqDivisionCodeAttribute` for DataAnnotations validation.
- `IraqAdminDivisions.Boundaries`: `IraqBoundaries.Locate` and `Contains`, offline point-in-polygon over simplified OCHA boundaries with a pre-classified spatial grid.
- `IraqAdminDivisions.AspNetCore`: `MapIraqDivisions` endpoints (governorates, division, children, localities, search, nearest) with `ar`/`en`/`ku` responses.
- Data exports: JSON, CSV, SQL, SQL Server and GeoJSON with boundaries.
- Targets `netstandard2.0` and `net8.0`; trimming and Native AOT compatible on .NET 8+.
