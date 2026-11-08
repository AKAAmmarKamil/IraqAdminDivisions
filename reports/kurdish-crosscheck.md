# Kurdish name cross-check

Generated: 2026-11-08 11:59:34Z

## Policy

- A dataset is trusted only if its exact agreement with our Wikidata Kurdish names beyond governorate level is at least 80.0% over at least 20 comparable rows, and at least 98.0% of its Kurdish names use only Sorani letters. A dataset whose own documentation calls its Kurdish names unreviewed drafts is never trusted.
- Exact means equal after Sorani normalisation (ي/ى/ی, ك/ک, ه/ھ/ە unified, tashkeel and joiners removed, administrative words such as قەزای removed). Near means the same consonant skeleton or one character apart.
- For a transliterated row a trusted dataset name that is exact keeps our text and the source becomes dataset; any other trusted name replaces ours, and our transliteration stays as an alias.
- Wikidata and manual names are never overridden. Every non-exact pair is listed below, with what happened to it.

## Summary

| Dataset | Licence | Trusted | Exact vs Wikidata | Confirmed | Replaced | Disagreements listed |
|---|---|---|---|---|---|---|
| karam-fikre/iraqi-data | MIT | no | 80 of 95 (84.2%) | 0 | 0 | 63 |
| Iraqi-Open-Source/official-iraqi-governorates | MIT | no | 45 of 64 (70.3%) | 0 | 0 | 41 |
| aryzonedev/iraq-cities | CC-BY-4.0 | yes | 49 of 60 (81.7%) | 79 | 23 | 34 |

Agreement between the datasets themselves (same code, both have a Kurdish name):

| Dataset | Dataset | Shared rows | Exact | Near |
|---|---|---|---|---|
| karam-fikre/iraqi-data | Iraqi-Open-Source/official-iraqi-governorates | 130 | 92 | 28 |
| karam-fikre/iraqi-data | aryzonedev/iraq-cities | 161 | 132 | 27 |
| Iraqi-Open-Source/official-iraqi-governorates | aryzonedev/iraq-cities | 100 | 69 | 22 |

Transliterated rows confirmed by a dataset: 79. Replaced by a dataset name: 23. Still transliterated: 214.

## karam-fikre/iraqi-data

https://github.com/karam-fikre/iraqi-data at commit `4b80dabf979101e95bef8fe88f71b61301fa46c4`, licence MIT. Verdict: not trusted: its own documentation states that the Kurdish names are unreviewed drafts.

Rows read: 635. Matched to our codes: 378. Not matched: 257.

| Match method | Rows |
|---|---|
| name | 18 |
| name+distance | 2 |
| name+governorate | 63 |
| pcode | 295 |

### Disagreements (63)

| Code | Level | English | Ours | Our source | Theirs | Agreement | Match | Outcome |
|---|---|---|---|---|---|---|---|---|
| IQG02Q04N03 | SubDistrict | Talha | تەلحە | transliterated | شەھید عیزەددین سەلیم | different | pcode | not applied (dataset not trusted) |
| IQG02Q05N01 | SubDistrict | Al-Dair | دەیر | transliterated | دێر | near | name+governorate | not applied (dataset not trusted) |
| IQG02Q06 | District | Al-Zubair | زوبەیر | transliterated | زوبێر | near | pcode | not applied (dataset not trusted) |
| IQG02Q06N01 | SubDistrict | Markaz Al-Zubair | ناوەندی زوبەیر | transliterated | ناوەندی زوبێر | near | pcode | not applied (dataset not trusted) |
| IQG03 | Governorate | Al-Muthanna | موسەنا | wikidata | موسەننا | near | name | kept ours (wikidata) |
| IQG04Q01N01 | SubDistrict | Al-Huriya | حوڕڕیە | transliterated | حوڕیە | near | pcode | not applied (dataset not trusted) |
| IQG05Q02N01 | SubDistrict | Al-Daghara | دەغغارە | transliterated | ئەکەد | different | name+governorate | not applied (dataset not trusted) |
| IQG05Q03N02 | SubDistrict | Al-Shinafiya | شینافیە | transliterated | شنافیە | near | name+governorate | not applied (dataset not trusted) |
| IQG05Q04N01 | SubDistrict | Al-Mihanawiya | مەھناویە | transliterated | مھناویە | near | name+governorate | not applied (dataset not trusted) |
| IQG06Q02N03 | SubDistrict | Kadr Karam | قادرکەرەم | wikidata | قادر کەرەم | near | pcode | kept ours (wikidata) |
| IQG06Q04N02 | SubDistrict | Sourdash | سورداش | transliterated | سوورداش | near | pcode | not applied (dataset not trusted) |
| IQG06Q06N02 | SubDistrict | Bebaz | بێباز | transliterated | پێباز | near | pcode | not applied (dataset not trusted) |
| IQG06Q07N01 | SubDistrict | Karmak | کەرمک | transliterated | گەرمک | near | pcode | not applied (dataset not trusted) |
| IQG06Q10N01 | SubDistrict | Saruchik | سرۆچک | wikidata | سەرچاوک | different | pcode | kept ours (wikidata) |
| IQG06Q10N03 | SubDistrict | Siwail | سیوەیل | transliterated | سیوەتیل | near | pcode | not applied (dataset not trusted) |
| IQG07Q01N02 | SubDistrict | Al-Shomaly | شوملی | transliterated | شۆمەلی | near | pcode | not applied (dataset not trusted) |
| IQG07Q04 | District | Al-Mussyab | مووسیب | wikidata | مووسەییب | near | pcode | kept ours (wikidata) |
| IQG07Q04N01 | SubDistrict | Jurf Al-Sakhar | جورفولسەخەر | transliterated | جورف نەسر | different | pcode | not applied (dataset not trusted) |
| IQG07Q04N02 | SubDistrict | Al-Iskandaria | ئیسکەندەریە | transliterated | ئەسکەندەریە | near | pcode | not applied (dataset not trusted) |
| IQG08Q01N02 | SubDistrict | Al-Fahama | فەححامە | transliterated | فەحامە | near | pcode | not applied (dataset not trusted) |
| IQG08Q02N02 | SubDistrict | Abu Ghraib | ئەبو غورەیب | transliterated | ئەبو غرێب | different | name+governorate | not applied (dataset not trusted) |
| IQG09Q01N04 | SubDistrict | Sarsank | سەرسەنگ | wikidata | سەرسنگ | near | pcode | kept ours (wikidata) |
| IQG09Q03N02 | SubDistrict | Fayde | فەیدیێ | wikidata | فایدە | different | pcode | kept ours (wikidata) |
| IQG10Q01 | District | Al-Khalis | خالیس | transliterated | خالس | near | pcode | not applied (dataset not trusted) |
| IQG10Q01N03 | SubDistrict | Hibhib | ھیبھیب | transliterated | ھەبھەب | different | pcode | not applied (dataset not trusted) |
| IQG10Q01N04 | SubDistrict | Markaz Al-Khalis | ناوەندی خالیس | transliterated | ناوەندی خالس | near | pcode | not applied (dataset not trusted) |
| IQG10Q02 | District | Al-Muqdadiya | شارەبان | wikidata | مەقدادیە | different | pcode | kept ours (wikidata) |
| IQG10Q02N02 | SubDistrict | Markaz Al-Muqdadiya | ناوەندی شارەبان | transliterated | ناوەندی مەقدادیە | different | pcode | not applied (dataset not trusted) |
| IQG10Q03 | District | Baladruz | بەلەدڕووز | transliterated | بەلەدروز | near | pcode | not applied (dataset not trusted) |
| IQG10Q03N02 | SubDistrict | Markaz Baladruz | ناوەندی بەلەدڕووز | transliterated | ناوەندی بەلەدروز | near | pcode | not applied (dataset not trusted) |
| IQG10Q04N01 | SubDistrict | Buhriz | بوھریز | transliterated | بوھرز | near | pcode | not applied (dataset not trusted) |
| IQG10Q05N05 | SubDistrict | Qaratu | قەرەتوو | transliterated | قۆرەتوو | near | pcode | not applied (dataset not trusted) |
| IQG10Q06N01 | SubDistrict | Jabarra | جەبارە | wikidata | جەببارە | near | pcode | kept ours (wikidata) |
| IQG11Q01 | District | Al-Zibar | زێبار | transliterated | مێرگەسۆر | different | pcode | not applied (dataset not trusted) |
| IQG11Q03N01 | SubDistrict | Taq Taq | تەقتەق | transliterated | تەق تەق | near | pcode | not applied (dataset not trusted) |
| IQG11Q04N04 | SubDistrict | Gwyer | گوێڕ | transliterated | گوێر | near | pcode | not applied (dataset not trusted) |
| IQG11Q06N03 | SubDistrict | Harir | حەریر | transliterated | ھەریر | near | name+governorate | not applied (dataset not trusted) |
| IQG12Q01 | District | Ain Al-Tamur | عەینولتەمر | transliterated | عەین تەمر | different | pcode | not applied (dataset not trusted) |
| IQG12Q01N01 | SubDistrict | Markaz Ain Al-Tamur | ناوەندی عەینولتەمر | transliterated | ناوەندی عەین تەمر | different | pcode | not applied (dataset not trusted) |
| IQG13Q01N02 | SubDistrict | Al-Zab | زاب | transliterated | زێ | different | pcode | not applied (dataset not trusted) |
| IQG13Q01N04 | SubDistrict | Al-Abbasi | عەبباسیە | transliterated | عەباسی | near | name+governorate | not applied (dataset not trusted) |
| IQG13Q03N01 | SubDistrict | Sarkran | سەرگەڕان | wikidata | سەرکەران | different | pcode | kept ours (wikidata) |
| IQG13Q04N01 | SubDistrict | Taza Khormato | تازەخورماتوو | transliterated | تازە خورماتوو | near | pcode | not applied (dataset not trusted) |
| IQG13Q04N03 | SubDistrict | Al-Multaka | مەلا عەبدوڵڵا | wikidata | مولتەقا | different | pcode | kept ours (wikidata) |
| IQG13Q04N04 | SubDistrict | Yaijy | یایچی | wikidata | یایجی | near | pcode | kept ours (wikidata) |
| IQG14 | Governorate | Maysan | میسان | wikidata | مەیسان | near | name | kept ours (wikidata) |
| IQG14Q03N02 | SubDistrict | Al-Msharah | مەشرەح | transliterated | موشەڕەح | different | pcode | not applied (dataset not trusted) |
| IQG14Q06 | District | Qalat Saleh | قەلعەی سالیح | transliterated | قەڵعەت ساڵح | different | pcode | not applied (dataset not trusted) |
| IQG14Q06N01 | SubDistrict | Markaz Qalat Saleh | ناوەندی قەلعەی سالیح | transliterated | ناوەندی قەڵعەت ساڵح | different | pcode | not applied (dataset not trusted) |
| IQG15Q01N03 | SubDistrict | Kurdsein | کوردەسین | transliterated | گرەسێن | different | pcode | not applied (dataset not trusted) |
| IQG15Q05N02 | SubDistrict | Hammam Al-Alil | حەممام عەلیل | transliterated | حەمام عەلیل | near | pcode | not applied (dataset not trusted) |
| IQG15Q06N03 | SubDistrict | Atreesh | ئەتروش | transliterated | ئەترووش | near | pcode | not applied (dataset not trusted) |
| IQG15Q07N02 | SubDistrict | Al-Shamal | سنونێ | wikidata | شەمال | different | pcode | kept ours (wikidata) |
| IQG16 | Governorate | Salah Al-Din | سەلاحەددین | wikidata | سەلاحەدین | near | name | kept ours (wikidata) |
| IQG16Q04N03 | SubDistrict | Al-Duloeyah | زولووعیە | transliterated | زلوعیە | different | name+governorate | not applied (dataset not trusted) |
| IQG16Q05N01 | SubDistrict | Al-Muotasim | موعتەسیم | transliterated | موعتەسەم | near | pcode | not applied (dataset not trusted) |
| IQG17Q01 | District | Al-Chibayish | ئەلچیبەیش | wikidata | چەبایش | different | pcode | kept ours (wikidata) |
| IQG17Q01N03 | SubDistrict | Markaz Al-Chibayish | ناوەندی ئەلچیبەیش | transliterated | ناوەندی چەبایش | different | pcode | not applied (dataset not trusted) |
| IQG17Q02N04 | SubDistrict | Qalat Siker | قەلعەت سوککەر | transliterated | قەڵعەت سوکەر | near | name+governorate | not applied (dataset not trusted) |
| IQG17Q05 | District | Suq Al-Shoyokh | سووقولشیوخ | transliterated | سووق شیوخ | different | pcode | not applied (dataset not trusted) |
| IQG17Q05N01 | SubDistrict | Garmat Beni Said | کەرمەی بەنی سەعید | transliterated | کەرمەت بەنی سەعید | near | name+governorate | not applied (dataset not trusted) |
| IQG18Q02N01 | SubDistrict | Wasit | واسیت | transliterated | واست | near | pcode | not applied (dataset not trusted) |
| IQG18Q04N04 | SubDistrict | Al-Hafriya | حەفریە | transliterated | تاجەددین | different | pcode | not applied (dataset not trusted) |

## Iraqi-Open-Source/official-iraqi-governorates

https://github.com/Iraqi-Open-Source/official-iraqi-governorates at commit `bd1f03c841fd4cfd35484f51f4765ee525e46a85`, licence MIT. Verdict: not trusted: exact agreement with Wikidata beyond governorates 67.4% is below 80.0%.

Rows read: 179. Matched to our codes: 132. Not matched: 47.

| Match method | Rows |
|---|---|
| name | 18 |
| name+governorate | 114 |

### Disagreements (41)

| Code | Level | English | Ours | Our source | Theirs | Agreement | Match | Outcome |
|---|---|---|---|---|---|---|---|---|
| IQG01Q01 | District | Al-Falluja | فەللووجە | transliterated | فەلووجە | near | name+governorate | not applied (dataset not trusted) |
| IQG01Q02 | District | Al-Kaim | قائیم | transliterated | قائم | near | name+governorate | not applied (dataset not trusted) |
| IQG01Q03 | District | Al-Ramadi | ڕومادی | transliterated | ڕامادی | near | name+governorate | not applied (dataset not trusted) |
| IQG01Q05 | District | Ana | عانە | transliterated | ئانە | near | name+governorate | not applied (dataset not trusted) |
| IQG02Q01 | District | Abu Al-Khaseeb | ئەبولخەسیب | transliterated | ئەبوو ئەلخەسیب | near | name+governorate | not applied (dataset not trusted) |
| IQG02Q04 | District | Al-Midaina | مەدینە | transliterated | مدینە | near | name+governorate | not applied (dataset not trusted) |
| IQG02Q06N03 | SubDistrict | Um Qasr | ئوم قەسر | transliterated | ئومم قەسر | near | name+governorate | not applied (dataset not trusted) |
| IQG02Q07 | District | Shat Al-Arab | شەتولعەرەب | transliterated | شەتەلعەرەب | near | name+governorate | not applied (dataset not trusted) |
| IQG03 | Governorate | Al-Muthanna | موسەنا | wikidata | مسەنا | near | name | kept ours (wikidata) |
| IQG03Q01 | District | Al-Khidhir | خدر | transliterated | خزر | near | name+governorate | not applied (dataset not trusted) |
| IQG04Q02 | District | Al-Manathera | مەنازیرە | transliterated | مەنازرە | near | name+governorate | not applied (dataset not trusted) |
| IQG06Q10 | District | Sharbazher | شارباژێڕ | wikidata | شارباژێر | near | name+governorate | kept ours (wikidata) |
| IQG07Q04 | District | Al-Mussyab | مووسیب | wikidata | موسەییب | near | name+governorate | kept ours (wikidata) |
| IQG08Q02N02 | SubDistrict | Abu Ghraib | ئەبو غورەیب | transliterated | ئەبوو غرێب | different | name+governorate | not applied (dataset not trusted) |
| IQG08Q06 | District | Al-Risafa | ڕەسافە | wikidata | ڕوسافە | near | name+governorate | kept ours (wikidata) |
| IQG10Q01 | District | Al-Khalis | خالیس | transliterated | خالس | near | name+governorate | not applied (dataset not trusted) |
| IQG10Q02 | District | Al-Muqdadiya | شارەبان | wikidata | موقدادیە | different | name+governorate | kept ours (wikidata) |
| IQG10Q03 | District | Baladruz | بەلەدڕووز | transliterated | بەلەدرووز | near | name+governorate | not applied (dataset not trusted) |
| IQG11Q03 | District | Koysinjaq | کۆیە | wikidata | کۆیسنجق | different | name+governorate | kept ours (wikidata) |
| IQG11Q06N03 | SubDistrict | Harir | حەریر | transliterated | ھەریر | near | name+governorate | not applied (dataset not trusted) |
| IQG12Q01 | District | Ain Al-Tamur | عەینولتەمر | transliterated | عەینوتتەمر | near | name+governorate | not applied (dataset not trusted) |
| IQG13Q01 | District | Al-Hawiga | حەویجە | wikidata | ھەویجە | near | name+governorate | kept ours (wikidata) |
| IQG13Q03 | District | Dibis | دووبز | wikidata | دبس | different | name+governorate | kept ours (wikidata) |
| IQG14 | Governorate | Maysan | میسان | wikidata | مەیسان | near | name | kept ours (wikidata) |
| IQG14Q02 | District | Ali Al-Gharbi | عەلی غەربی | transliterated | عەلی ئەلغەربی | different | name+governorate | not applied (dataset not trusted) |
| IQG14Q04 | District | Al-Maimouna | مەیموونە | transliterated | مەیمونە | near | name+governorate | not applied (dataset not trusted) |
| IQG14Q05 | District | Al-Mejar Al-Kabir | مەجەڕی کەبیر | transliterated | مەجەری کەبیر | near | name+governorate | not applied (dataset not trusted) |
| IQG14Q06 | District | Qalat Saleh | قەلعەی سالیح | transliterated | قەڵای ساڵح | different | name+governorate | not applied (dataset not trusted) |
| IQG15Q03N01 | SubDistrict | Namroud | نەمروود | wikidata | نمروود | near | name+governorate | kept ours (wikidata) |
| IQG15Q04 | District | Al-Hatra | حەزەر | wikidata | حەترا | different | name+governorate | kept ours (wikidata) |
| IQG15Q05N02 | SubDistrict | Hammam Al-Alil | حەممام عەلیل | transliterated | حەمامی عەلیل | different | name+governorate | not applied (dataset not trusted) |
| IQG15Q08 | District | Telafar | تەلەعفەر | wikidata | تەلعەفەر | near | name+governorate | kept ours (wikidata) |
| IQG15Q09N02 | SubDistrict | Al-Qosh | ئەلقۆش | transliterated | ئەلقووش | near | name+governorate | not applied (dataset not trusted) |
| IQG16 | Governorate | Salah Al-Din | سەلاحەددین | wikidata | سەلاحەدین | near | name | kept ours (wikidata) |
| IQG16Q02 | District | Al-Shirqat | شەرقات | wikidata | شرقات | near | name+governorate | kept ours (wikidata) |
| IQG16Q03 | District | Beygee | بێجی | wikidata | بەیجی | near | name+governorate | kept ours (wikidata) |
| IQG16Q05 | District | Samarra | سامەڕا | wikidata | سامەرا | near | name+governorate | kept ours (wikidata) |
| IQG16Q07 | District | Tooz Khurmato | دووزخورماتوو | wikidata | تووز خورماتوو | near | name+governorate | kept ours (wikidata) |
| IQG17Q01 | District | Al-Chibayish | ئەلچیبەیش | wikidata | جبایش | different | name+governorate | kept ours (wikidata) |
| IQG17Q05 | District | Suq Al-Shoyokh | سووقولشیوخ | transliterated | سووقی شویووخ | different | name+governorate | not applied (dataset not trusted) |
| IQG18 | Governorate | Wassit | واسیت | wikidata | واسط | different | name | kept ours (wikidata) |

## aryzonedev/iraq-cities

https://github.com/aryzonedev/iraq-cities at commit `ce066afd246be55353f74a7cb70a2132e4e9f252`, licence CC-BY-4.0. Verdict: trusted.

Rows read: 199. Matched to our codes: 163. Not matched: 36.

| Match method | Rows |
|---|---|
| name | 18 |
| name+governorate | 145 |

### Disagreements (34)

| Code | Level | English | Ours | Our source | Theirs | Agreement | Match | Outcome |
|---|---|---|---|---|---|---|---|---|
| IQG01Q03N01 | SubDistrict | Al-Habbaniya | حەببانیە | transliterated | حەبانیە | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG01Q03N03 | SubDistrict | Markaz Al-Ramadi | ناوەندی ڕومادی | transliterated | ناوەندی ڕەمادی | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG02Q02N01 | SubDistrict | Al-Hartha | ھارسە | transliterated | ھاریسە | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG02Q04 | District | Al-Midaina | مەدینە | transliterated | میدەینە | different | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG02Q05N01 | SubDistrict | Al-Dair | دەیر | transliterated | دێر | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG02Q06 | District | Al-Zubair | زوبەیر | transliterated | زوبێر | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG03 | Governorate | Al-Muthanna | موسەنا | wikidata | موسەننا | near | name | kept ours (wikidata) |
| IQG04Q02 | District | Al-Manathera | مەنازیرە | transliterated | مەنازرە | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG05Q01N04 | SubDistrict | Sumer | سۆمەر | transliterated | سومەر | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG05Q02N03 | SubDistrict | Al-Shafeia | شافیعیە | transliterated | شافعیە | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG07Q02N01 | SubDistrict | Al-Kifil | کیفل | transliterated | کفل | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG07Q04 | District | Al-Mussyab | مووسیب | wikidata | موسەیب | near | name+governorate | kept ours (wikidata) |
| IQG08Q01N03 | SubDistrict | Al-Rashdia | ڕاشیدیە | transliterated | ڕاشدیە | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG08Q02N02 | SubDistrict | Abu Ghraib | ئەبو غورەیب | transliterated | ئەبو غرێب | different | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG08Q06 | District | Al-Risafa | ڕەسافە | wikidata | ڕوسافە | near | name+governorate | kept ours (wikidata) |
| IQG10Q01 | District | Al-Khalis | خالیس | transliterated | خالس | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG10Q02 | District | Al-Muqdadiya | شارەبان | wikidata | موقدادیە | different | name+governorate | kept ours (wikidata) |
| IQG11Q06N03 | SubDistrict | Harir | حەریر | transliterated | ھەریر | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG12Q01 | District | Ain Al-Tamur | عەینولتەمر | transliterated | عەین تەمر | different | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG13Q03 | District | Dibis | دووبز | wikidata | دبس | different | name+governorate | kept ours (wikidata) |
| IQG14Q04 | District | Al-Maimouna | مەیموونە | transliterated | مەیمونە | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG14Q05 | District | Al-Mejar Al-Kabir | مەجەڕی کەبیر | transliterated | مەجەڕ کەبیر | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG14Q06 | District | Qalat Saleh | قەلعەی سالیح | transliterated | قەڵعەت ساڵح | different | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG15Q09 | District | Tilkaef | تلکێف | wikidata | تەلکێف | near | name+governorate | kept ours (wikidata) |
| IQG16 | Governorate | Salah Al-Din | سەلاحەددین | wikidata | سەڵاحەدین | near | name | kept ours (wikidata) |
| IQG16Q01 | District | Al-Daur | دۆر | wikidata | دەور | near | name+governorate | kept ours (wikidata) |
| IQG16Q04N03 | SubDistrict | Al-Duloeyah | زولووعیە | transliterated | زولوعیە | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG16Q07 | District | Tooz Khurmato | دووزخورماتوو | wikidata | دووز خورماتوو | near | name+governorate | kept ours (wikidata) |
| IQG17Q01 | District | Al-Chibayish | ئەلچیبەیش | wikidata | چبایش | different | name+governorate | kept ours (wikidata) |
| IQG17Q02N04 | SubDistrict | Qalat Siker | قەلعەت سوککەر | transliterated | قەڵعەت سکەر | different | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG17Q03N02 | SubDistrict | Al-Gharraf | غەڕڕاف | transliterated | غەڕاف | near | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG17Q05 | District | Suq Al-Shoyokh | سووقولشیوخ | transliterated | سوق شیوخ | different | name+governorate | replaced our transliteration, which is kept as an alias |
| IQG18 | Governorate | Wassit | واسیت | wikidata | واست | near | name | kept ours (wikidata) |
| IQG18Q01N02 | SubDistrict | Al-Mowafaqiya | موەففەقیە | transliterated | موەفەقیە | near | name+governorate | replaced our transliteration, which is kept as an alias |

