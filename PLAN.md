# IraqAdminDivisions — Plan

A free, open-source C# NuGet package with every Iraqi administrative division
(governorate → district → sub-district) in Arabic, English and Kurdish, with
stable codes, coordinates, search and hierarchy navigation.

Goal: a portfolio / CV / LinkedIn project that developers actually install.

## Decisions

| Topic | Decision | Why |
|---|---|---|
| Base data | OCHA COD-AB Iraq (HDX `cod-ab-irq`, v01) | Official humanitarian reference: 18 / 101 / 294 rows, AR + EN names, centroids, area |
| Codes | OCHA P-codes (`IQG02`, `IQG02Q01`, `IQG07Q02N02`) as the public `Code` | Stable, already used by UN/NGO datasets, so the package joins with them |
| Kurdish names | Wikidata (`ckb` labels), matched by name + parent + distance | No reviewer available; Wikidata is CC0. Every Kurdish name records its source; unmatched = `null`, never guessed |
| OpenStreetMap | Not used | ODbL share-alike would force the whole dataset under ODbL |
| GADM | Not used | Forbids commercial use |
| Halabja | Follows OCHA v01 (inside Sulaymaniyah); documented as a known limitation | The source does not reflect it yet; no invented restructuring |
| Build pipeline | C# console tool (`tools/`) | Whole repo is one language; reruns when OCHA publishes a new version |
| Targets | `netstandard2.0` + `net8.0` | Works from .NET Framework 4.6.1 to .NET 11 |
| Licenses | Code MIT; data CC BY-IGO (OCHA attribution) + CC0 (Wikidata) | Both allow free and commercial use with attribution |

## v1 additions (2026-11-08, second round)

| Topic | Decision |
|---|---|
| Kurdish for every row | Wikidata first; otherwise an AI-assisted Sorani transliteration in `data/overrides/kurdish-transliterated.tsv` (316 rows). Every name carries `KurdishNameSource` = Wikidata / Transliterated / Manual, so apps can show or hide unverified names. Stated plainly in the README |
| Source corrections | `data/overrides/corrections.tsv` (code, field, value, reason) applied last; replaced source values stay as aliases. Fixes 5 OCHA Arabic errors + 1 Wikidata Kurdish label |
| All-language address | `Division.Address` / `IraqDivisions.GetAddress(code)` → Ar + En + Ku in one call; AspNetCore DTO returns `addresses {ar,en,ku}` |
| Apps that want governorate → area only | `governorate.Localities` = one flat area list per governorate (districts + non-centre sub-districts, no "Markaz X" duplicates), for two-level dropdowns. The app stores the area's code; `FormatAddress` still works. AspNetCore: `GET /{governorateCode}/localities` |
| Polygons | OCHA admin3 polygons, Douglas-Peucker simplified, in a separate package `IraqAdminDivisions.Boundaries`: `IraqBoundaries.Locate(lat, lng)` → sub-district (then `.District` / `.Governorate`). Core stays small |

## Quality bar (non-negotiable for every package)

Tests
- Coverage measured with coverlet; every public member covered; any uncovered line needs a written reason
- Data tests, not only code tests: 3 names per row, hierarchical codes, every sub-district has a polygon, no released code ever disappears (snapshot)
- Every target framework exercised: tests run on net8.0 + net10.0; the netstandard2.0 build is exercised from net48 on a Windows CI job
- `EnablePackageValidation` (+ baseline once 1.0.0 ships) so a breaking public-API change fails the build

Speed
- `Find`: O(1) dictionary; parent/children/address pre-linked at load
- Search: normalised index built once at load, never per query; no LINQ allocations in the hot path
- Locate: bbox prefilter + uniform grid index, then point-in-polygon on the few candidates
- Loading: System.Text.Json source generator (no reflection) → fast startup, trimming + Native AOT compatible (`IsAotCompatible` on net8.0)
- BenchmarkDotNet project (`benchmarks/`) for load, Find, Search, FormatAddress, GetAddress, Localities, Locate; real numbers go in the README

## Layout

```
IraqAdminDivisions/
├── src/IraqAdminDivisions/            NuGet library, data embedded as a resource
├── tools/IraqAdminDivisions.Builder/  downloads OCHA + Wikidata → data/divisions.json + report
├── data/divisions.json                generated, reviewed, committed (source of truth)
├── data/exports/                      divisions.csv, divisions.sql (for non-.NET users)
├── reports/build-report.md            counts, Kurdish coverage, unmatched rows
├── tests/IraqAdminDivisions.Tests/    xUnit: data integrity, hierarchy, search, nearest
├── samples/                           console + ASP.NET minimal API (dropdown endpoints)
└── .github/workflows/                 CI (build, test, pack) + publish on tag
```

## Library API (target)

```csharp
IraqDivisions.Governorates;                       // 18
IraqDivisions.Find("IQG02");                      // Al-Basrah
basrah.Districts;                                 // navigation down
zubair.Governorate;                               // navigation up
IraqDivisions.Search("البصره");                   // tolerant: ه/ة, ي/ى, أ/إ/ا, tashkeel, "ال"
IraqDivisions.Nearest(30.5, 47.8, AdminLevel.District);
division.Name.Ar / .En / .Ku / .Get(language)

IraqDivisions.FormatAddress("IQG07Q02N02", Language.Arabic);   // "أبي غرق، الحلة، بابل"
IraqDivisions.IsValid("IQG07Q02N02", AdminLevel.SubDistrict);  // true
[IraqDivisionCode(AdminLevel.SubDistrict)]                     // DataAnnotations validation
```

### How a consumer uses it

- Their table stores ONE string column (any level's code, usually the sub-district). No lookup tables, FKs or seeding.
- `FormatAddress` / `Find(...).District.Governorate` renders the full address in AR / EN / KU.
- Codes are hierarchical, so `Code.StartsWith("IQG07")` = everything in Babil, translated to SQL `LIKE 'IQG07%'`.
- `IraqAdminDivisions.AspNetCore` (second package): `app.MapIraqDivisions("/api/divisions")` → cascading-dropdown endpoints (`/governorates`, `/{code}/children`, `/{code}`).

### Code stability promise

A published code is NEVER removed or reused. A changed division stays resolvable, flagged `IsDeprecated`
with `ReplacedBy`, so updating the package never breaks stored data. Covered by a test that compares
against the codes of the previous release.

## Phases

1. Builder + data — fetch, merge, validate, write `divisions.json`, exports, report
2. Library + tests — models, embedded data, search, nearest, FormatAddress, IsValid, validation attribute, xUnit
3. AspNetCore package + docs + CI + samples — `MapIraqDivisions`, README (EN) + README.ar.md, workflows, sample apps, package metadata
4. Verify — Release build, all tests, `dotnet pack`, consume the .nupkg from a scratch project
5. Owner steps (Ammar) — create GitHub repo, add `NUGET_API_KEY` secret, push a `v1.0.0` tag, LinkedIn post

## Known limitations (stated in the README)

- Kurdish coverage is whatever Wikidata has; missing names are `null`
- No neighbourhoods (mahallas) in v1
- Halabja is still a district of Sulaymaniyah in OCHA v01

## Status

- [x] Phase 1  - [x] Phase 2  - [x] Phase 3  - [x] Phase 4  - [ ] Phase 5 (owner)

2026-11-08 (round 2): Kurdish for all 413 (96 Wikidata · 316 transliterated · 1 manual); 6 source corrections;
polygons for all 294 sub-districts; three packages (core, Boundaries, AspNetCore).
Tests: core 75 (net8.0) · 75 (net10.0) · 74 (net48); AspNetCore 41; Boundaries 464 × 2 TFMs — 0 failures.
Coverage: core and AspNetCore 100% lines; Boundaries classes 100% lines.
Benchmarks (.NET 10): Find 17 ns · GetAddress 16 ns · Search ~8 µs · Locate 35–57 ns, all 0 B except Search.
All README examples re-run from the packed .nupkg files on net8.0 and match. Nothing committed yet.

Halabja decision (2026-11-08, from sourced research):
- Law "قانون استحداث محافظة حلبجة" passed 2025-04-14, decree 2025-04-29, gazette 2025-05-05, court challenges rejected → in force
- Federal composition: 1 district (Halabja centre) + 4 sub-districts Sirwan, Khurmal, Biyara, Bamo; OCHA v01 has Biyara, Khurmal, Markaz Halabja, Shahrazur. Sirwan/Bamo have no OCHA geometry; Shahrazur's placement unconfirmed
- No ISO 3166-2 code; no newer boundary dataset (OCHA still v01, geoBoundaries 18)
- Model: governorate IQG19 (project-assigned code, not OCHA); OCHA district IQG06Q05 + its 4 sub-districts move under it with new hierarchical codes IQG19Q01, IQG19Q01N01..N04; old IQG06Q05* rows stay resolvable as deprecated with ReplacedBy; every row carries OchaCode; Shahrazur flagged composition-unconfirmed; ISO null. Declarative file data/overrides/restructures.json so future changes reuse the mechanism. Revisit when OCHA v02 or the gazette text lists the sub-districts

Open after round 2 (from the competitor research):
- Halabja as governorate 19 — needs an authoritative source for which districts/sub-districts moved; then new codes + deprecate the old ones via ReplacedBy
- Cross-check the 316 transliterated Kurdish names against MIT/CC-BY datasets with human-written Kurdish (karam-fikre/iraqi-data, Iraqi-Open-Source/official-iraqi-governorates, aryzonedev/iraq-cities)
- ISO 3166-2 codes (IQ-BG, ...) on governorates

Phase 5 checklist (owner):
1. First commit, then create the GitHub repo AKAAmmarKamil/IraqAdminDivisions and push
2. nuget.org: create an API key scoped to `IraqAdminDivisions*`; add it as the repo secret `NUGET_API_KEY`
3. Settings → Actions → Workflow permissions: read/write + allow Actions to create PRs (for refresh-data)
4. Push tag `v1.0.0` → publish.yml pushes both packages
5. Pin the repo on GitHub, add it to the CV, LinkedIn post
