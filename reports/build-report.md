# Build report

Generated: 2026-11-08 11:59:34Z  
Status: OK

## Counts per level

| Level | Rows | Expected |
|---|---|---|
| Governorate | 18 | 18 |
| District | 101 | 101 |
| SubDistrict | 294 | 294 |
| Total | 413 | 413 |

## Wikidata match rate

| Level | Matched | Ambiguous | Unmatched | Rate |
|---|---|---|---|---|
| Governorate | 18 | 0 | 0 | 100.0% |
| District | 92 | 1 | 8 | 91.1% |
| SubDistrict | 61 | 0 | 233 | 20.7% |
| Total | 171 | 1 | 241 | 41.4% |

## Kurdish name coverage

Kurdish names with an administrative-type prefix stripped: 93

| Level | Wikidata | Dataset | Transliterated | Manual | With Kurdish name | Total | Coverage |
|---|---|---|---|---|---|---|---|
| Governorate | 18 | 0 | 0 | 1 | 19 | 19 | 100.0% |
| District | 61 | 83 | 30 | 1 | 175 | 175 | 100.0% |
| SubDistrict | 60 | 64 | 317 | 1 | 442 | 442 | 100.0% |
| Total | 139 | 147 | 347 | 3 | 636 | 636 | 100.0% |

Transliteration entries ignored because Wikidata or a correction already supplies the name: 0

## Kurdish dataset calibration

A dataset is trusted only with at least 80.0% exact agreement with our Wikidata names beyond governorate level (over at least 20 rows) and at least 98.0% pure Sorani names. Details: reports/kurdish-crosscheck.md

| Dataset | Licence | Commit | Rows | Matched | Compared with Wikidata | Exact | Near | Exact beyond governorates | Pure Sorani | Trusted |
|---|---|---|---|---|---|---|---|---|---|---|
| karam-fikre/iraqi-data | MIT | 4b80dab | 635 | 378 | 95 | 80 | 8 | 65 of 77 (84.4%) | 99.4% | no |
| Iraqi-Open-Source/official-iraqi-governorates | MIT | bd1f03c | 179 | 132 | 64 | 45 | 13 | 31 of 46 (67.4%) | 98.9% | no |
| aryzonedev/iraq-cities | CC-BY-4.0 | ce066af | 199 | 163 | 60 | 49 | 8 | 34 of 42 (81.0%) | 99.5% | yes |

Agreement with Wikidata per level (exact / near / compared):

| Dataset | Governorate | District | SubDistrict |
|---|---|---|---|
| karam-fikre/iraqi-data | 15 / 3 / 18 | 46 / 1 / 49 | 19 / 4 / 28 |
| Iraqi-Open-Source/official-iraqi-governorates | 14 / 3 / 18 | 30 / 9 / 44 | 1 / 1 / 2 |
| aryzonedev/iraq-cities | 15 / 3 / 18 | 32 / 5 / 40 | 2 / 0 / 2 |

Transliterated rows resolved by datasets:

| Dataset | Confirmed (kept our text) | Replaced (ours kept as alias) |
|---|---|---|
| karam-fikre/iraqi-data | 0 | 0 |
| Iraqi-Open-Source/official-iraqi-governorates | 0 | 0 |
| aryzonedev/iraq-cities | 79 | 23 |
| Total | 79 | 23 |

Transliterated rows with no trusted dataset name: 214

## ISO 3166-2 codes

| Code | English | ISO 3166-2 | Wikidata |
|---|---|---|---|
| IQG01 | Al-Anbar | IQ-AN | Q187334 |
| IQG02 | Al-Basrah | IQ-BA | Q193551 |
| IQG03 | Al-Muthanna | IQ-MU | Q212761 |
| IQG04 | Al-Najaf | IQ-NA | Q192882 |
| IQG05 | Al-Qadissiya | IQ-QA | Q62987 |
| IQG06 | Al-Sulaymaniyah | IQ-SU | Q213182 |
| IQG07 | Babil | IQ-BB | Q59202 |
| IQG08 | Baghdad | IQ-BG | Q191075 |
| IQG09 | Duhok | IQ-DA | Q189541 |
| IQG10 | Diyala | IQ-DI | Q217075 |
| IQG11 | Erbil | IQ-AR | Q213189 |
| IQG12 | Kerbala | IQ-KA | Q214104 |
| IQG13 | Kirkuk | IQ-KI | Q193268 |
| IQG14 | Maysan | IQ-MA | Q213170 |
| IQG15 | Ninewa | IQ-NI | Q189352 |
| IQG16 | Salah Al-Din | IQ-SD | Q190131 |
| IQG17 | Thi Qar | IQ-DQ | Q215649 |
| IQG18 | Wassit | IQ-WA | Q189747 |
| IQG19 | Halabja |  | Q15631321 |


## Rejected name-only candidates (25)

| OCHA code | English | Candidate |
|---|---|---|
| IQG01Q04N01 | Al-Nekhaeeb | Q12247114 |
| IQG01Q06N01 | Al-Haqlaniya | Q12247029 |
| IQG01Q07N02 | Al-Forat | Q12190882 |
| IQG01Q07N03 | Kubaisa | Q12247267 |
| IQG03Q02N02 | Al-Majd | Q22947392 |
| IQG03Q02N03 | Al-Najmi | Q25449929 |
| IQG03Q04N01 | Al-Sowair | Q31076246 |
| IQG04Q01N01 | Al-Huriya | Q20423530 |
| IQG04Q03N01 | Al-Haydariya | Q22690316 |
| IQG05Q01N04 | Sumer | Q12247215 |
| IQG05Q03N01 | Al-Sadeer | Q12247055 |
| IQG07Q01N02 | Al-Shomaly | Q12247117 |
| IQG07Q02N01 | Al-Kifil | Q12247117 |
| IQG07Q02N02 | Abi Gharaq | Q12246993 |
| IQG12Q02N01 | Al-Jadwal Al-Ghrabi | Q12247016 |
| IQG14Q01N01 | Kumait | Q12236253 |
| IQG14Q02N02 | Ali Al-Sharki | Q31077542 |
| IQG14Q03N01 | Beni Hasheem | Q12247135 |
| IQG14Q04N02 | Markaz Al-Maimouna | Q7069262 |
| IQG14Q05N02 | Al-Adel | Q12190055 |
| IQG14Q05N02 | Al-Adel | Q12247074 |
| IQG17Q02N01 | Al-Fajer | Q12247084 |
| IQG17Q05N02 | Al-Tar | Q25452312 |
| IQG18Q01N01 | Al-Bashaer | Q22684855 |
| IQG18Q01N02 | Al-Mowafaqiya | Q12247110 |

## Unmatched rows (241)

| Code | Level | Arabic | English | Detail |
|---|---|---|---|---|
| IQG01Q01N03 | SubDistrict | مركز الفلوجة | Markaz Al-Falluja | no Wikidata item with a matching name at this level |
| IQG01Q02N01 | SubDistrict | الكرابلة | Al-Karabla | no Wikidata item with a matching name at this level |
| IQG01Q03N02 | SubDistrict | الرحالية | Al-Rahhaliya | no Wikidata item with a matching name at this level |
| IQG01Q03N03 | SubDistrict | مركز الرمادي | Markaz Al-Ramadi | no Wikidata item with a matching name at this level |
| IQG01Q04 | District | الرطبة | Al-Rutba | name matched but rejected: Q4119693 (Ar Rutba District: 97.5 km, parent consistent) |
| IQG01Q04N01 | SubDistrict | النخيب | Al-Nekhaeeb | name matched but rejected: Q12247114 (Al-Nukhib: no coordinates, parent unknown) |
| IQG01Q04N02 | SubDistrict | الوليد | Al-Walid | name matched but rejected: Q4704943 (Al Waleed: 38.6 km, parent consistent) |
| IQG01Q04N03 | SubDistrict | مركز الرطبة | Markaz Al-Rutba | no Wikidata item with a matching name at this level |
| IQG01Q05N01 | SubDistrict | مركز عنه | Markaz Ana | no Wikidata item with a matching name at this level |
| IQG01Q05N02 | SubDistrict | راوه | Rawa | no Wikidata item with a matching name at this level |
| IQG01Q06N01 | SubDistrict | الحقلانية | Al-Haqlaniya | name matched but rejected: Q12247029 (ناحية الحقلانية: no coordinates, parent unknown) |
| IQG01Q07N02 | SubDistrict | الفرات | Al-Forat | name matched but rejected: Q12190882 (الفرات: no coordinates, parent unknown) |
| IQG01Q07N03 | SubDistrict | كبيسة | Kubaisa | name matched but rejected: Q12247267 (ناحية كبيسة: no coordinates, parent unknown) |
| IQG01Q07N04 | SubDistrict | مركز هيت | Markaz Heet | no Wikidata item with a matching name at this level |
| IQG02Q01N01 | SubDistrict | السيبة | Al-Siba | name matched but rejected: Q12188676 (Siba subdistrict: 20.7 km, parent inconsistent) |
| IQG02Q01N02 | SubDistrict | مركز ابي الخصيب | Markaz Abu Al-Khaseeb | no Wikidata item with a matching name at this level |
| IQG02Q02N01 | SubDistrict | الهارثة | Al-Hartha | no Wikidata item with a matching name at this level |
| IQG02Q02N02 | SubDistrict | مركز البصرة | Markaz Al-Basrah | no Wikidata item with a matching name at this level |
| IQG02Q03N01 | SubDistrict | البحار | Al-Bahhar | name matched but rejected: Q22690314 (ناحية البحار: 24.3 km, parent inconsistent) |
| IQG02Q03N02 | SubDistrict | مركز الفاو | Markaz Al-Faw | no Wikidata item with a matching name at this level |
| IQG02Q04N01 | SubDistrict | الهوير | Al-Howweir | no Wikidata item with a matching name at this level |
| IQG02Q04N02 | SubDistrict | مركز المدينة | Markaz Al-Midaina | no Wikidata item with a matching name at this level |
| IQG02Q04N03 | SubDistrict | طلحة | Talha | no Wikidata item with a matching name at this level |
| IQG02Q05N01 | SubDistrict | الدير | Al-Dair | name matched but rejected: Q42530423 (Ad Dayr: 12.4 km, parent inconsistent) |
| IQG02Q05N03 | SubDistrict | مركز القرنة | Markaz Al-Qurna | no Wikidata item with a matching name at this level |
| IQG02Q06 | District | الزبير | Al-Zubair | name matched but rejected: Q4703378 (Al-Zubair District: 98.0 km, parent consistent) |
| IQG02Q06N01 | SubDistrict | مركز الزبير | Markaz Al-Zubair | no Wikidata item with a matching name at this level |
| IQG02Q06N02 | SubDistrict | سفوان | Safwan | no Wikidata item with a matching name at this level |
| IQG02Q07N02 | SubDistrict | مركز شط العرب | Markaz Shat Al-Arab | no Wikidata item with a matching name at this level |
| IQG03Q01N01 | SubDistrict | مركز الخضر | Markaz Al-Khidhir | no Wikidata item with a matching name at this level |
| IQG03Q02N02 | SubDistrict | المجد | Al-Majd | name matched but rejected: Q22947392 (ناحية المجد: no coordinates, parent unknown) |
| IQG03Q02N03 | SubDistrict | النجمي | Al-Najmi | name matched but rejected: Q25449929 (قضاء النجمي: no coordinates, parent unknown) |
| IQG03Q02N04 | SubDistrict | الوركاء | Al-Warka | no Wikidata item with a matching name at this level |
| IQG03Q02N05 | SubDistrict | مركز الرميثة | Markaz Al-Rumaitha | no Wikidata item with a matching name at this level |
| IQG03Q03 | District | السلمان | Al-Salman | name matched but rejected: Q4703177 (Al-Salman District: 82.9 km, parent consistent) |
| IQG03Q03N01 | SubDistrict | مركز السلمان | Markaz Al-Salman | no Wikidata item with a matching name at this level |
| IQG03Q04N01 | SubDistrict | السوير | Al-Sowair | name matched but rejected: Q31076246 (ناحية السوير: no coordinates, parent unknown) |
| IQG04Q01N01 | SubDistrict | الحرية | Al-Huriya | name matched but rejected: Q20423530 (ناحية الحرية: no coordinates, parent unknown) |
| IQG04Q01N02 | SubDistrict | مركز الكوفة | Markaz Al-Kufa | no Wikidata item with a matching name at this level |
| IQG04Q03 | District | النجف | Al-Najaf | name matched but rejected: Q6959877 (Najaf District: 34.9 km, parent inconsistent) |
| IQG04Q03N01 | SubDistrict | الحيدرية | Al-Haydariya | name matched but rejected: Q22690316 (ناحية الحيدرية: no coordinates, parent unknown) |
| IQG04Q03N02 | SubDistrict | الشبكة | Al-Shabaka | name matched but rejected: Q12247059 (al-Shabakah: 33.1 km, parent inconsistent) |
| IQG04Q03N03 | SubDistrict | مركز النجف | Markaz Al-Najaf | no Wikidata item with a matching name at this level |
| IQG05Q01N01 | SubDistrict | البدير | Al-Bdair | no Wikidata item with a matching name at this level |
| IQG05Q01N02 | SubDistrict | الفوار | Al-Fawwar | no Wikidata item with a matching name at this level |
| IQG05Q01N03 | SubDistrict | مركز عفك | Markaz Afaq | no Wikidata item with a matching name at this level |
| IQG05Q01N04 | SubDistrict | سومر | Sumer | name matched but rejected: Q12247215 (ناحية سومر: no coordinates, parent unknown) |
| IQG05Q02N01 | SubDistrict | الدغارة | Al-Daghara | no Wikidata item with a matching name at this level |
| IQG05Q02N02 | SubDistrict | السنية | Al-Saniya | no Wikidata item with a matching name at this level |
| IQG05Q02N03 | SubDistrict | الشافعية | Al-Shafeia | no Wikidata item with a matching name at this level |
| IQG05Q02N04 | SubDistrict | مركز الديوانية | Markaz Al-Diwaniya | no Wikidata item with a matching name at this level |
| IQG05Q03N01 | SubDistrict | السدير | Al-Sadeer | name matched but rejected: Q12247055 (ناحية السدير: no coordinates, parent unknown) |
| IQG05Q03N02 | SubDistrict | الشنافية | Al-Shinafiya | no Wikidata item with a matching name at this level |
| IQG05Q03N03 | SubDistrict | مركز الحمزة | Markaz Al-Hamza | no Wikidata item with a matching name at this level |
| IQG05Q04N01 | SubDistrict | المهناوية | Al-Mihanawiya | no Wikidata item with a matching name at this level |
| IQG05Q04N02 | SubDistrict | غماس | Ghammas | no Wikidata item with a matching name at this level |
| IQG06Q01N02 | SubDistrict | قره داغ | Qaradagh | no Wikidata item with a matching name at this level |
| IQG06Q01N03 | SubDistrict | سرجنار | Sarchnar | no Wikidata item with a matching name at this level |
| IQG06Q02N01 | SubDistrict | ئاغجة لةر | Agjalare | no Wikidata item with a matching name at this level |
| IQG06Q02N02 | SubDistrict | مركز جمجمال | Markaz Chamchamal | no Wikidata item with a matching name at this level |
| IQG06Q03N01 | SubDistrict | مركز دربندخان | Markaz Derbendikhan | no Wikidata item with a matching name at this level |
| IQG06Q04N01 | SubDistrict | جنارين | Gnareen | no Wikidata item with a matching name at this level |
| IQG06Q04N02 | SubDistrict | سورداش | Sourdash | no Wikidata item with a matching name at this level |
| IQG06Q05N02 | SubDistrict | خورمال | Khourmal | no Wikidata item with a matching name at this level |
| IQG06Q05N03 | SubDistrict | مركز حلبجة | Markaz Halabja | no Wikidata item with a matching name at this level |
| IQG06Q05N04 | SubDistrict | شهرزور | Shahrazur | no Wikidata item with a matching name at this level |
| IQG06Q06N01 | SubDistrict | مركز كلار | Markaz Kalar | no Wikidata item with a matching name at this level |
| IQG06Q06N03 | SubDistrict | تيلاكو | Tilako | no Wikidata item with a matching name at this level |
| IQG06Q07N02 | SubDistrict | مركز بنجوين | Markaz Panjwin | no Wikidata item with a matching name at this level |
| IQG06Q08N02 | SubDistrict | ناودشت | Nawdasht | no Wikidata item with a matching name at this level |
| IQG06Q08N03 | SubDistrict | مركز بشدر | Markaz Pshdar | no Wikidata item with a matching name at this level |
| IQG06Q09 | District | رانية | Rania | name matched but rejected: Q7293005 (Ranya District: 82.9 km, parent consistent) |
| IQG06Q09N01 | SubDistrict | ميركه | Mirka | no Wikidata item with a matching name at this level |
| IQG06Q09N02 | SubDistrict | مركز رانية | Markaz Rania | no Wikidata item with a matching name at this level |
| IQG06Q09N03 | SubDistrict | بتوانه | Bitwana | no Wikidata item with a matching name at this level |
| IQG06Q10N02 | SubDistrict | مركز شهربازار | Markaz Sharbazher | no Wikidata item with a matching name at this level |
| IQG06Q10N03 | SubDistrict | سويل | Siwail | no Wikidata item with a matching name at this level |
| IQG06Q10N04 | SubDistrict | ماوت | Mawat | no Wikidata item with a matching name at this level |
| IQG07Q01N01 | SubDistrict | القاسم | Al-Qasim | no Wikidata item with a matching name at this level |
| IQG07Q01N02 | SubDistrict | الشوملي | Al-Shomaly | name matched but rejected: Q12247117 (ناحية النيل: no coordinates, parent unknown) |
| IQG07Q01N03 | SubDistrict | المدحتية | Al-Madhatiya | name matched but rejected: Q4531385 (Al Madhatiyah: 17.8 km, parent inconsistent) |
| IQG07Q02N01 | SubDistrict | الكفل | Al-Kifil | name matched but rejected: Q12247117 (ناحية النيل: no coordinates, parent unknown) |
| IQG07Q02N02 | SubDistrict | ابي غرق | Abi Gharaq | name matched but rejected: Q12246993 (ناحية أبي غرق: no coordinates, parent unknown) |
| IQG07Q02N03 | SubDistrict | مركز الحلة | Markaz Al-Hilla | no Wikidata item with a matching name at this level |
| IQG07Q03N01 | SubDistrict | مركز المحاويل | Markaz Al-Mahawil | no Wikidata item with a matching name at this level |
| IQG07Q03N03 | SubDistrict | المشروع | Al-Mashroo | no Wikidata item with a matching name at this level |
| IQG07Q04N01 | SubDistrict | جرف الصخر | Jurf Al-Sakhar | no Wikidata item with a matching name at this level |
| IQG07Q04N02 | SubDistrict | الاسكندرية | Al-Iskandaria | no Wikidata item with a matching name at this level |
| IQG08Q01N01 | SubDistrict | مركز الاعظمية | Markaz Al-Adhamiya | no Wikidata item with a matching name at this level |
| IQG08Q01N02 | SubDistrict | الفحامة | Al-Fahama | no Wikidata item with a matching name at this level |
| IQG08Q01N03 | SubDistrict | الراشدية | Al-Rashdia | no Wikidata item with a matching name at this level |
| IQG08Q02 | District | الكاظمية | Al-Kadhmiyah | no Wikidata item with a matching name at this level |
| IQG08Q02N01 | SubDistrict | مركز الكاظمية | Markaz Al-Kadhimiya | no Wikidata item with a matching name at this level |
| IQG08Q02N02 | SubDistrict | ابي غريب | Abu Ghraib | no Wikidata item with a matching name at this level |
| IQG08Q02N04 | SubDistrict | الطارمية | Al-Tarmiya | no Wikidata item with a matching name at this level |
| IQG08Q02N05 | SubDistrict | التاجي | Al-Taji | no Wikidata item with a matching name at this level |
| IQG08Q03N02 | SubDistrict | مركز الكرخ | Markaz Al-Karkh | no Wikidata item with a matching name at this level |
| IQG08Q03N03 | SubDistrict | المنصور | Al-Mansour | no Wikidata item with a matching name at this level |
| IQG08Q04N01 | SubDistrict | مركز المدائن | Markaz Al-Mada'in | no Wikidata item with a matching name at this level |
| IQG08Q04N02 | SubDistrict | الجسر | Al-Jisr | no Wikidata item with a matching name at this level |
| IQG08Q05N01 | SubDistrict | مركز المحمودية | Markaz Al-Mahmoudiya | name matched but rejected: Q30314640 (Mahmoudiyah subdistrict: no coordinates, parent inconsistent) |
| IQG08Q05N02 | SubDistrict | الرشيد | Al-Rasheed | no Wikidata item with a matching name at this level |
| IQG08Q05N03 | SubDistrict | اللطيفية | Al-Latifya | name matched but rejected: Q141476764 (al Latifiyah Subdistrict: 9.6 km, parent inconsistent) |
| IQG08Q05N04 | SubDistrict | اليوسفية | Al-Yousifya | name matched but rejected: Q141118899 (Yusufiyah Subdistrict: 9.4 km, parent inconsistent) |
| IQG08Q06N01 | SubDistrict | الكرادة الشرقية | Al-Karrada Al-Sharqia | no Wikidata item with a matching name at this level |
| IQG08Q06N02 | SubDistrict | بغداد الجديدة | Baghdad Al-Jedeeda | no Wikidata item with a matching name at this level |
| IQG08Q07 | District | الثورة | Al-Thawra | no Wikidata item with a matching name at this level |
| IQG08Q07N01 | SubDistrict | مركز الثورة | Markaz Al-Thawra | no Wikidata item with a matching name at this level |
| IQG09Q01N01 | SubDistrict | برواري بالا | Barwari Bala | no Wikidata item with a matching name at this level |
| IQG09Q01N02 | SubDistrict | مركز العمادية | Markaz Al-Amadiya | no Wikidata item with a matching name at this level |
| IQG09Q01N03 | SubDistrict | نروة ريكان | Nerwa Rekan | no Wikidata item with a matching name at this level |
| IQG09Q02N01 | SubDistrict | الدوسكي | Al-Duski | no Wikidata item with a matching name at this level |
| IQG09Q02N02 | SubDistrict | مركز دهوك | Markaz Duhok | no Wikidata item with a matching name at this level |
| IQG09Q03N03 | SubDistrict | مركز سميل | Markaz Sumail | no Wikidata item with a matching name at this level |
| IQG09Q03N04 | SubDistrict | سندي | Sindi | no Wikidata item with a matching name at this level |
| IQG09Q04N01 | SubDistrict | مركز زاخو | Markaz Zakho | no Wikidata item with a matching name at this level |
| IQG09Q04N02 | SubDistrict | باتيفا | Batifa | no Wikidata item with a matching name at this level |
| IQG09Q04N03 | SubDistrict | دركار | Dercar | no Wikidata item with a matching name at this level |
| IQG10Q01N01 | SubDistrict | العظيم | Al-Atheem | no Wikidata item with a matching name at this level |
| IQG10Q01N03 | SubDistrict | هبهب | Hibhib | no Wikidata item with a matching name at this level |
| IQG10Q01N04 | SubDistrict | مركز الخالص | Markaz Al-Khalis | no Wikidata item with a matching name at this level |
| IQG10Q01N05 | SubDistrict | قلعة انصاب | Qalaa't Ansab | no Wikidata item with a matching name at this level |
| IQG10Q02N01 | SubDistrict | الوجيهية | Al-Wajihia | no Wikidata item with a matching name at this level |
| IQG10Q02N02 | SubDistrict | مركز المقدادية | Markaz Al-Muqdadiya | no Wikidata item with a matching name at this level |
| IQG10Q03N01 | SubDistrict | مندلي | Mandali | no Wikidata item with a matching name at this level |
| IQG10Q03N02 | SubDistrict | مركز بلدروز | Markaz Baladruz | no Wikidata item with a matching name at this level |
| IQG10Q03N03 | SubDistrict | قزانية | Qazanya | no Wikidata item with a matching name at this level |
| IQG10Q04N01 | SubDistrict | بهرز | Buhriz | no Wikidata item with a matching name at this level |
| IQG10Q04N02 | SubDistrict | بني سعد | Beni Saad | no Wikidata item with a matching name at this level |
| IQG10Q05N01 | SubDistrict | السعدية | Al-Saadiya | no Wikidata item with a matching name at this level |
| IQG10Q05N02 | SubDistrict | مركز خانقين | Markaz Khanaqin | no Wikidata item with a matching name at this level |
| IQG10Q05N03 | SubDistrict | ميدان | Midan | no Wikidata item with a matching name at this level |
| IQG10Q05N04 | SubDistrict | جلولاء | Jalawla | no Wikidata item with a matching name at this level |
| IQG10Q05N05 | SubDistrict | قرة تو | Qaratu | no Wikidata item with a matching name at this level |
| IQG10Q06N02 | SubDistrict | مركز كفري | Markaz Kifri | no Wikidata item with a matching name at this level |
| IQG10Q06N03 | SubDistrict | قرة تبة | Qara Tabe | no Wikidata item with a matching name at this level |
| IQG11Q01 | District | الزيبار | Al-Zibar | no Wikidata item with a matching name at this level |
| IQG11Q01N02 | SubDistrict | مزوري بالا | Mazouri Bala | no Wikidata item with a matching name at this level |
| IQG11Q02N01 | SubDistrict | قوش تبه | Qushtappa | no Wikidata item with a matching name at this level |
| IQG11Q02N02 | SubDistrict | مركز اربيل | Markaz Erbil | no Wikidata item with a matching name at this level |
| IQG11Q03N01 | SubDistrict | طق طق | Taq Taq | name matched but rejected: Q10690972 (Teq Teq: 12.0 km, parent inconsistent) |
| IQG11Q03N02 | SubDistrict | سورش | Shorsh | no Wikidata item with a matching name at this level |
| IQG11Q03N03 | SubDistrict | مركز كويسنجق | Markaz Koysinjaq | no Wikidata item with a matching name at this level |
| IQG11Q04N02 | SubDistrict | مركز مخمور | Markaz Makhmour | no Wikidata item with a matching name at this level |
| IQG11Q04N03 | SubDistrict | ديبكة | Dibaga | no Wikidata item with a matching name at this level |
| IQG11Q04N04 | SubDistrict | الكوير | Gwyer | name matched but rejected: Q31076039 (ناحية الكوير: 11.4 km, parent inconsistent) |
| IQG11Q05N01 | SubDistrict | مركز راوندوز | Markaz Rawanduz | no Wikidata item with a matching name at this level |
| IQG11Q05N03 | SubDistrict | بالك | Balak | no Wikidata item with a matching name at this level |
| IQG11Q05N05 | SubDistrict | ميركة سور | Mergasur | no Wikidata item with a matching name at this level |
| IQG11Q05N06 | SubDistrict | برادوست | Bradost | no Wikidata item with a matching name at this level |
| IQG11Q05N07 | SubDistrict | الحج عمران | Haji Omaran | no Wikidata item with a matching name at this level |
| IQG11Q06N01 | SubDistrict | خوشناو | Khoshnaw | no Wikidata item with a matching name at this level |
| IQG11Q06N03 | SubDistrict | ديرة حرير | Harir | name matched but rejected: Q4122022 (Harir: 12.4 km, parent inconsistent) |
| IQG12Q01N01 | SubDistrict | مركز عين التمر | Markaz Ain Al-Tamur | no Wikidata item with a matching name at this level |
| IQG12Q02N01 | SubDistrict | الجدول الغربي | Al-Jadwal Al-Ghrabi | name matched but rejected: Q12247016 (ناحية الجدول الغربي: no coordinates, parent unknown) |
| IQG12Q02N02 | SubDistrict | مركز الهندية | Markaz Al-Hindiya | no Wikidata item with a matching name at this level |
| IQG12Q03N01 | SubDistrict | مركز كربلاء | Markaz Kerbela | no Wikidata item with a matching name at this level |
| IQG13Q01N01 | SubDistrict | مركز الحويجة | Markaz Al-Hawiga | no Wikidata item with a matching name at this level |
| IQG13Q01N02 | SubDistrict | الزاب | Al-Zab | no Wikidata item with a matching name at this level |
| IQG13Q01N04 | SubDistrict | العباسية | Al-Abbasi | no Wikidata item with a matching name at this level |
| IQG13Q02N02 | SubDistrict | مركز داقوق | Markaz Daquq | no Wikidata item with a matching name at this level |
| IQG13Q03N02 | SubDistrict | مركز الدبس | Markaz Dibis | no Wikidata item with a matching name at this level |
| IQG13Q03N03 | SubDistrict | التون كوبري | Altun Qupri | no Wikidata item with a matching name at this level |
| IQG13Q04N01 | SubDistrict | تازة خورماتو | Taza Khormato | no Wikidata item with a matching name at this level |
| IQG13Q04N06 | SubDistrict | مركز كركوك | Markaz Kirkuk | no Wikidata item with a matching name at this level |
| IQG14Q01N01 | SubDistrict | كميت | Kumait | name matched but rejected: Q12236253 (كميت: no coordinates, parent unknown) |
| IQG14Q02N01 | SubDistrict | مركز علي الغربي | Markaz Ali Al-Gharbi | no Wikidata item with a matching name at this level |
| IQG14Q02N02 | SubDistrict | علي الشرقي | Ali Al-Sharki | name matched but rejected: Q31077542 (علي الشرقي: no coordinates, parent unknown) |
| IQG14Q03N01 | SubDistrict | بني هاشم | Beni Hasheem | name matched but rejected: Q12247135 (ناحية بني هاشم: no coordinates, parent unknown) |
| IQG14Q03N03 | SubDistrict | مركز الكحلاء | Markaz Al-Kahla | no Wikidata item with a matching name at this level |
| IQG14Q04N01 | SubDistrict | السلام | Al-Salam | name matched but rejected: Q135038341 (ناحية السلام: 406.2 km, parent inconsistent) |
| IQG14Q04N02 | SubDistrict | مركز الميمونة | Markaz Al-Maimouna | name matched but rejected: Q7069262 (ناحية الميمونة: no coordinates, parent unknown) |
| IQG14Q05N01 | SubDistrict | العزير | Al-Aziz | no Wikidata item with a matching name at this level |
| IQG14Q05N02 | SubDistrict | العدل | Al-Adel | name matched but rejected: Q12190055 (العدل: no coordinates, parent unknown); Q12247074 (Al-Adl: no coordinates, parent unknown) |
| IQG14Q06N01 | SubDistrict | مركز قلعة صالح | Markaz Qalat Saleh | no Wikidata item with a matching name at this level |
| IQG15Q01N01 | SubDistrict | بردرش | Bardarash | no Wikidata item with a matching name at this level |
| IQG15Q01N02 | SubDistrict | بجيل | Begeel | no Wikidata item with a matching name at this level |
| IQG15Q01N03 | SubDistrict | كردسين | Kurdsein | no Wikidata item with a matching name at this level |
| IQG15Q01N05 | SubDistrict | مركز عقرة | Markaz Aqra | no Wikidata item with a matching name at this level |
| IQG15Q02N01 | SubDistrict | مركز البعاج | Markaz Al-Baaj | no Wikidata item with a matching name at this level |
| IQG15Q02N02 | SubDistrict | القحطانية | Al-Qahtanya | no Wikidata item with a matching name at this level |
| IQG15Q03N02 | SubDistrict | برطلة | Bartilla | no Wikidata item with a matching name at this level |
| IQG15Q03N03 | SubDistrict | مركز الحمدانية | Markaz Al-Hamdaniya | no Wikidata item with a matching name at this level |
| IQG15Q04N01 | SubDistrict | مركز الحضر | Markaz Hatra | no Wikidata item with a matching name at this level |
| IQG15Q05N02 | SubDistrict | حمام العليل | Hammam Al-Alil | no Wikidata item with a matching name at this level |
| IQG15Q05N04 | SubDistrict | المحمداث | Al-Muhamadath | no Wikidata item with a matching name at this level |
| IQG15Q06N01 | SubDistrict | مركز الشيخان | Markaz Al-Shikhan | no Wikidata item with a matching name at this level |
| IQG15Q06N02 | SubDistrict | قسروك | Qasruk | no Wikidata item with a matching name at this level |
| IQG15Q06N03 | SubDistrict | ئةتريش | Atreesh | no Wikidata item with a matching name at this level |
| IQG15Q07N03 | SubDistrict | مركز سنجار | Markaz Sinjar | no Wikidata item with a matching name at this level |
| IQG15Q08N01 | SubDistrict | مركز تلعفر | Markaz Telafar | no Wikidata item with a matching name at this level |
| IQG15Q08N02 | SubDistrict | العياضية | Al-Ayadiya | no Wikidata item with a matching name at this level |
| IQG15Q08N03 | SubDistrict | زمار | Zummar | no Wikidata item with a matching name at this level |
| IQG15Q08N04 | SubDistrict | ربيعة | Rabia | no Wikidata item with a matching name at this level |
| IQG15Q09N01 | SubDistrict | مركز تلكيف | Markaz Tilkaef | no Wikidata item with a matching name at this level |
| IQG15Q09N02 | SubDistrict | القوش | Al-Qosh | no Wikidata item with a matching name at this level |
| IQG15Q09N03 | SubDistrict | وانة | Wana | no Wikidata item with a matching name at this level |
| IQG16Q01N01 | SubDistrict | حمرين | Hamreen | name matched but rejected: Q1573999 (Hamrin: 128.4 km, parent consistent) |
| IQG16Q01N02 | SubDistrict | ابو دلف | Abu Dalf | no Wikidata item with a matching name at this level |
| IQG16Q02N01 | SubDistrict | مركز الشرقاط | Markaz Al-Shirqat | no Wikidata item with a matching name at this level |
| IQG16Q03N01 | SubDistrict | مركز بيجي | Markaz Beygee | no Wikidata item with a matching name at this level |
| IQG16Q03N02 | SubDistrict | الصينية | Al-Synia | no Wikidata item with a matching name at this level |
| IQG16Q04N02 | SubDistrict | الاسحاقي | Al-Esshaqi | name matched but rejected: Q6080026 (Ishaqi: 37.8 km, parent consistent) |
| IQG16Q04N03 | SubDistrict | الضلوعية | Al-Duloeyah | no Wikidata item with a matching name at this level |
| IQG16Q04N04 | SubDistrict | الدجيل | Al-Dujeel | no Wikidata item with a matching name at this level |
| IQG16Q05N03 | SubDistrict | مركز سامراء | Markaz Samarra | no Wikidata item with a matching name at this level |
| IQG16Q05N04 | SubDistrict | دجلة | Dijla | no Wikidata item with a matching name at this level |
| IQG16Q06N01 | SubDistrict | ام القرى | Um Al-Qura | no Wikidata item with a matching name at this level |
| IQG16Q07N01 | SubDistrict | آمرلي | Ameraly | no Wikidata item with a matching name at this level |
| IQG16Q07N02 | SubDistrict | مركز طوز خورماتو | Markaz Tooz Khurmato | no Wikidata item with a matching name at this level |
| IQG16Q07N03 | SubDistrict | سليمان بيك | Sulaiman Baig | no Wikidata item with a matching name at this level |
| IQG17Q01N01 | SubDistrict | الفهود | Al-Fuhood | name matched but rejected: Q25450206 (Al-Fahd district: 20.5 km, parent unknown) |
| IQG17Q01N02 | SubDistrict | الحمار | Al-Hammar | no Wikidata item with a matching name at this level |
| IQG17Q01N03 | SubDistrict | مركز الجبايش | Markaz Al-Chibayish | no Wikidata item with a matching name at this level |
| IQG17Q02N01 | SubDistrict | الفجر | Al-Fajer | name matched but rejected: Q12247084 (ناحية الفجر: no coordinates, parent unknown) |
| IQG17Q02N02 | SubDistrict | النصر | Al-Nasr | no Wikidata item with a matching name at this level |
| IQG17Q02N03 | SubDistrict | مركز الرفاعي | Markaz Al-Rifai | no Wikidata item with a matching name at this level |
| IQG17Q02N04 | SubDistrict | قلعة السكر | Qalat Siker | no Wikidata item with a matching name at this level |
| IQG17Q03N01 | SubDistrict | مركز الشطرة | Markaz Al-Shatra | no Wikidata item with a matching name at this level |
| IQG17Q03N02 | SubDistrict | القراف | Al-Gharraf | no Wikidata item with a matching name at this level |
| IQG17Q03N03 | SubDistrict | الدواية | Al-Dawaya | no Wikidata item with a matching name at this level |
| IQG17Q04N01 | SubDistrict | البطحاء | Al-Battha'a | no Wikidata item with a matching name at this level |
| IQG17Q04N02 | SubDistrict | مركزالناصرية | Markaz Al-Nasiriya | no Wikidata item with a matching name at this level |
| IQG17Q04N03 | SubDistrict | سيد خليل | Said Dekhil | no Wikidata item with a matching name at this level |
| IQG17Q05N01 | SubDistrict | كرمة بني سعد | Garmat Beni Said | no Wikidata item with a matching name at this level |
| IQG17Q05N02 | SubDistrict | الطار | Al-Tar | name matched but rejected: Q25452312 (الطار: no coordinates, parent unknown) |
| IQG17Q05N04 | SubDistrict | عكيكة | Akaika | no Wikidata item with a matching name at this level |
| IQG18Q01N01 | SubDistrict | البشائر | Al-Bashaer | name matched but rejected: Q22684855 (البشائر: no coordinates, parent unknown) |
| IQG18Q01N02 | SubDistrict | الموفقية | Al-Mowafaqiya | name matched but rejected: Q12247110 (قضاء الموفقية: no coordinates, parent unknown) |
| IQG18Q02N01 | SubDistrict | واسط | Wasit | no Wikidata item with a matching name at this level |
| IQG18Q02N02 | SubDistrict | شيخ سعد | Shaekh Saad | no Wikidata item with a matching name at this level |
| IQG18Q02N03 | SubDistrict | مركز الكوت | Markaz Al-Kut | no Wikidata item with a matching name at this level |
| IQG18Q03N01 | SubDistrict | الاحرار | Al-Ahrar | no Wikidata item with a matching name at this level |
| IQG18Q03N02 | SubDistrict | مركز النعمانية | Markaz Al-Noamaniya | no Wikidata item with a matching name at this level |
| IQG18Q04N01 | SubDistrict | الزبيدية | Al-Zubaidiya | no Wikidata item with a matching name at this level |
| IQG18Q04N02 | SubDistrict | الشحيمية | Al-Shehamiya | no Wikidata item with a matching name at this level |
| IQG18Q04N03 | SubDistrict | مركز الصويرة | Markaz Al-Suwaira | no Wikidata item with a matching name at this level |
| IQG18Q04N04 | SubDistrict | الحفرية | Al-Hafriya | no Wikidata item with a matching name at this level |
| IQG18Q04N05 | SubDistrict | العزيزية | Al-Aziziya | no Wikidata item with a matching name at this level |
| IQG18Q05N01 | SubDistrict | مركز بدرة | Markaz Badra | no Wikidata item with a matching name at this level |
| IQG18Q05N02 | SubDistrict | جصان | Jassan | no Wikidata item with a matching name at this level |

## Ambiguous matches (1)

| Code | Level | Arabic | English | Detail |
|---|---|---|---|---|
| IQG10Q03 | District | مندلي | Baladruz | 2 candidates: Q4849762 (Balad Ruz District: 36.5 km, parent consistent); Q4849785 (Baladrooz District: no coordinates, parent consistent) |

## Rejected Wikidata Kurdish labels (0)

None.

## Applied corrections (6)

| Code | Field | Replaced | New | Reason |
|---|---|---|---|---|
| IQG06Q01N01 | ku | ناوەندی قەزای بازیان | بازیان | Wikidata ckb label is the district-centre item (ناوەندی قەزای بازیان), not the sub-district name |
| IQG06Q02N01 | ar | ئاغجة لةر | آغجلر | OCHA v01 Arabic field holds old Kurdish-in-Arabic typing (ئاغجة لةر) |
| IQG10Q03 | ar | مندلي | بلدروز | OCHA v01 gives the district the Arabic name of its sub-district Mandali; English is Baladruz and its centre is Markaz Baladruz |
| IQG15Q06N03 | ar | ئةتريش | أتروش | OCHA v01 Arabic field holds old Kurdish-in-Arabic typing (ئةتريش) |
| IQG17Q04N02 | ar | مركزالناصرية | مركز الناصرية | OCHA v01 is missing the space between the two words |
| IQG17Q04N03 | ar | سيد خليل | سيد دخيل | OCHA v01 Arabic reads Sayid Khalil while the English and the real sub-district are Sayid Dakhil |

## Polygons

Douglas-Peucker tolerance: 0.0001 degrees, coordinates rounded to 5 decimals

| File | Size |
|---|---|
| data/boundaries.json | 0.67 MB (702,550 bytes) |
| data/exports/divisions.geojson | 1.27 MB (1,336,841 bytes) |

### OCHA centre point inside its own simplified polygon

| Level | Inside | Checked | Rate |
|---|---|---|---|
| Governorate | 19 | 19 | 100.0% |
| District | 101 | 101 | 100.0% |
| SubDistrict | 294 | 294 | 100.0% |
| Total | 414 | 414 | 100.0% |

Misses (0):

None.

### Sub-district polygon box outside its governorate box (0 of 294)

None.

## Validation failures (0)

None.
