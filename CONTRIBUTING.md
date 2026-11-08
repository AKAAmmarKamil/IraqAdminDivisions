# Contributing

## Build and test

```bash
dotnet build IraqAdminDivisions.slnx -c Release
dotnet test IraqAdminDivisions.slnx -c Release
```

## Regenerate the data

```bash
dotnet run --project tools/IraqAdminDivisions.Builder -- --refresh
```

This downloads OCHA and Wikidata, rewrites `data/divisions.json`, `data/exports/` and `reports/build-report.md`.
Run the tests afterwards; the code-stability snapshot test fails if a published code disappears.

## Data pull requests

- Never edit `data/` by hand for a bulk change; change the Builder and rerun it.
- A single-name fix (for example a Kurdish name) is welcome as an issue (use the "Data correction" template) or a PR.
  It must cite a source: an official publication, a government page, or a Wikidata item.
- Names from OpenStreetMap or GADM are not accepted because of their licenses.
- Reviewers check: the source, that the code is unchanged, and that tests pass. Codes are never removed or reused;
  use `IsDeprecated` and `ReplacedBy` instead.
