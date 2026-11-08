# Changelog

Format follows [Keep a Changelog](https://keepachangelog.com/); versions follow [SemVer](https://semver.org/).
Published codes are never removed (see README, "Code stability").

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
