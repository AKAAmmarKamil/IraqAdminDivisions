# Data licenses

The source code is MIT (see `LICENSE`). The data in `data/` and embedded in the NuGet packages
combines the sources below, each with its own license.

## OCHA COD-AB Iraq: CC BY-IGO 3.0

Administrative boundaries, P-codes, Arabic and English names, centroids and areas come from the
Common Operational Dataset - Administrative Boundaries (COD-AB) for Iraq, published by UN OCHA
through the Humanitarian Data Exchange (https://data.humdata.org/dataset/cod-ab-irq).

License: Creative Commons Attribution 3.0 IGO (https://creativecommons.org/licenses/by/3.0/igo/).
You may copy, redistribute, adapt and use the data commercially, **provided you give attribution**.

Required credit:

> Administrative divisions: UN OCHA, Common Operational Dataset - Administrative Boundaries (COD-AB) Iraq,
> via the Humanitarian Data Exchange, licensed CC BY-IGO 3.0. Compiled in IraqAdminDivisions by Ammar Kamil.

The data was modified: merged with Wikidata labels, normalized and re-exported. This is not an official
UN or OCHA product, and the UN/OCHA does not endorse it. The use of the data does not imply any waiver of
the privileges and immunities of the United Nations.

## Wikidata: CC0 1.0

Kurdish (Sorani, `ckb`) names come from Wikidata (https://www.wikidata.org), released under CC0 1.0
(https://creativecommons.org/publicdomain/zero/1.0/). No attribution is required; crediting Wikidata is appreciated.

## aryzonedev/iraq-cities: CC BY 4.0

Some Kurdish (Sorani) names (`kurdishNameSource` = `dataset`, `kurdishNameDataset` = `aryzonedev/iraq-cities`) come from
the `iraq_cities.json` file of https://github.com/aryzonedev/iraq-cities, copyright (c) 2026 AryZone, at commit
`ce066afd246be55353f74a7cb70a2132e4e9f252`, licensed under Creative Commons Attribution 4.0 International
(https://creativecommons.org/licenses/by/4.0/legalcode). The names were matched to OCHA codes, cleaned of
administrative prefixes and letter-form variants, and in some rows used in place of our own transliteration, so the
data was modified. The original file and licence text are in `data/sources/aryzonedev-iraq-cities/`.

Required credit:

> Some Kurdish names: AryZone, iraq-cities (https://github.com/aryzonedev/iraq-cities), CC BY 4.0, modified.

## Datasets used only for cross-checking

These datasets are compared with our Kurdish names (`reports/kurdish-crosscheck.md`). No name from them is part of
`data/divisions.json`, so no credit is required in apps; they are listed because the build reads them.

| Dataset | Licence | Commit | Local copy |
|---|---|---|---|
| https://github.com/karam-fikre/iraqi-data (c) 2026 Karam Al-Dulaimi | MIT | `4b80dabf979101e95bef8fe88f71b61301fa46c4` | `data/sources/karam-fikre-iraqi-data/` |
| https://github.com/Iraqi-Open-Source/official-iraqi-governorates (c) 2025 Iraqi Open Source | MIT | `bd1f03c841fd4cfd35484f51f4765ee525e46a85` | `data/sources/iraqi-open-source-official-iraqi-governorates/` |

## ISO 3166-2 codes

Governorate ISO 3166-2 codes (`isoCode`, for example `IQ-BG`) are read from Wikidata property P300 (CC0).

## What you must credit

- Apps and services that show or redistribute the division data: credit OCHA as above (README, About page, or docs).
- Apps that only store the codes and call the library: the NuGet package carries the notice; a mention is still recommended.
- AryZone iraq-cities: credit as above wherever the Kurdish names are shown or redistributed.
- Wikidata: nothing required.
