# Census 2024 reconciliation

Generated 2026-11-08 by the Builder (`CensusReconciler`, PLAN-1.1 Phase 1). No data was changed.

Target list: the 2024 census hierarchy (COSIT) as tallied from a local, gitignored checklist (`data/local/`). Only names and hierarchy are compared; no population figure or third-party ID is used or shipped. Matching uses the normalised Arabic name first, then English, and prefers the same parent, then the same governorate, then anywhere (anywhere needs an Arabic match, exact or one letter apart).

## Buckets

| Bucket | Districts | Sub-districts |
|---|---:|---:|
| Missing | 13 | 26 |
| Re-parented | 0 | 1 |
| Cross-governorate | 2 | 16 |
| Renamed | 3 | 18 |
| Extra | 3 | 35 |
| Matched | 167 | 372 |

## Per governorate

| Governorate | Code | Districts, census | Districts, dataset | Sub-districts, census | Sub-districts, dataset |
|---|---|---:|---:|---:|---:|
| Al-Anbār | IQG01 | 12 | 12 | 23 | 25 |
| Al-Baṣrah [Basra] | IQG02 | 11 | 10 | 17 | 20 |
| Al-Muthannā | IQG03 | 9 | 9 | 12 | 12 |
| An-Najaf | IQG04 | 4 | 4 | 12 | 12 |
| Al-Qādisiyah | IQG05 | 13 | 13 | 15 | 16 |
| As-Sulaymāniyah | IQG06 | 17 | 15 | 56 | 55 |
| Bābil [Babylon] | IQG07 | 7 | 7 | 19 | 20 |
| Baghdād | IQG08 | 15 | 12 | 38 | 31 |
| Dahūk [Dohuk] | IQG09 | 8 | 5 | 29 | 23 |
| Diyālā | IQG10 | 8 | 8 | 24 | 29 |
| Arbīl [Erbil] | IQG11 | 15 | 16 | 46 | 51 |
| Karbalā' [Kerbala] | IQG12 | 6 | 5 | 7 | 6 |
| Kirkūk | IQG13 | 4 | 4 | 16 | 16 |
| Maysān | IQG14 | 7 | 7 | 15 | 16 |
| Nīnawā [Nineveh] | IQG15 | 9 | 9 | 31 | 34 |
| Ṣalāḥ ad-Dīn [Saladin] | IQG16 | 11 | 11 | 25 | 28 |
| Dhī Qār [Theqar] | IQG17 | 15 | 15 | 23 | 23 |
| Wasiṭ | IQG18 | 9 | 9 | 19 | 20 |
| Ḥalabjah [Halabja] | IQG19 | 5 | 4 | 5 | 5 |
| **Total** | | 185 | 175 | 432 | 442 |

Census sub-district counts leave out units the list marks as dissolved.

## Missing (39)

needs a new row (Phase 2: a source other than the checklist, then a 51+ code).

| Governorate | Level | Census name | Census Arabic | Census district | Dataset code | Dataset name | Note |
|---|---|---|---|---|---|---|---|
| Al-Baṣrah [Basra] | District | Aṣ-Ṣādiq | الصادق |  |  |  | needs a new row |
| Al-Baṣrah [Basra] | SubDistrict | Aṣ-Ṣādiq | الصادق | Aṣ-Ṣādiq |  |  | needs a new row; its census parent is new too |
| Arbīl [Erbil] | District | Khabāt [Xebat] | خبات |  |  |  | needs a new row |
| Arbīl [Erbil] | SubDistrict | Khabāt [Xebat] | خبات | Khabāt [Xebat] |  |  | needs a new row; its census parent is new too |
| Arbīl [Erbil] | SubDistrict | Kūrkūsak [Kewergosk] | كوركوسك | Khabāt [Xebat] |  |  | needs a new row; its census parent is new too |
| Arbīl [Erbil] | SubDistrict | Rizkāry [Rizgarî] | رِزكارى | Khabāt [Xebat] |  |  | needs a new row; its census parent is new too |
| Arbīl [Erbil] | SubDistrict | Shūrash [Şoriş] | ناحية | Kuysanjaq [Koye] |  |  | needs a new row |
| As-Sulaymāniyah | District | Khānaqīn [Xaneqîn] | خانقین |  |  |  | needs a new row |
| As-Sulaymāniyah | District | Kifrī [Kifrî] | كفري |  |  |  | needs a new row |
| As-Sulaymāniyah | SubDistrict | Āwah Siby [Awespî] | آوة سبي | Kifrī [Kifrî] |  |  | needs a new row; its census parent is new too |
| As-Sulaymāniyah | SubDistrict | Kūkas [Kokis] | كوكس | Kifrī [Kifrî] |  |  | needs a new row; its census parent is new too |
| As-Sulaymāniyah | SubDistrict | Nawjūl [Newcol] | نوجول | Kifrī [Kifrî] |  |  | needs a new row; its census parent is new too |
| As-Sulaymāniyah | SubDistrict | Sir Qal'at [Serqela] | سر قلعة | Kifrī [Kifrî] |  |  | needs a new row; its census parent is new too |
| Baghdād | District | Az-Zahūr | الزھور |  |  |  | needs a new row |
| Baghdād | District | Az-Zawrā' [← Ar-Ruṣāfah, Aṣ-Ṣadr al-Āwlā, Aṣ-Ṣadr ath-Thāniyah] | الزوراء |  |  |  | needs a new row |
| Baghdād | District | Aṣ-Ṣadr al-Āwlā [Sadr City 1] | الصدر الاولى |  |  |  | needs a new row |
| Baghdād | District | Aṣ-Ṣadr ath-Thāniyah [Sadr City 2] | الصدر الثانیة |  |  |  | needs a new row |
| Baghdād | SubDistrict | Ar-Rāshidiyah | الجدیدة | Ar-Rāshidiyah |  |  | needs a new row |
| Baghdād | SubDistrict | Az-Zahūr | الزھور | Az-Zahūr |  |  | needs a new row; its census parent is new too |
| Baghdād | SubDistrict | Az-Zawrā' | الزوراء | Az-Zawrā' [← Ar-Ruṣāfah, Aṣ-Ṣadr al-Āwlā, Aṣ-Ṣadr ath-Thāniyah] |  |  | needs a new row; its census parent is new too |
| Baghdād | SubDistrict | Al-Furāt | الفرات | Aṣ-Ṣadr al-Āwlā [Sadr City 1] |  |  | needs a new row; its census parent is new too |
| Baghdād | SubDistrict | Aṣ-Ṣadīq al-Akbar | الصدیق الاكبر | Aṣ-Ṣadr al-Āwlā [Sadr City 1] |  |  | needs a new row; its census parent is new too |
| Baghdād | SubDistrict | Aṣ-Ṣadr al-Āwlā | الصدر الاولى | Aṣ-Ṣadr al-Āwlā [Sadr City 1] |  |  | needs a new row; its census parent is new too |
| Baghdād | SubDistrict | Abnā'a ar-Rāfadaīn | ابناء الرافدین | Aṣ-Ṣadr ath-Thāniyah [Sadr City 2] |  |  | needs a new row; its census parent is new too |
| Baghdād | SubDistrict | Al-Munawarah | المنورة | Aṣ-Ṣadr ath-Thāniyah [Sadr City 2] |  |  | needs a new row; its census parent is new too |
| Baghdād | SubDistrict | Aṣ-Ṣadr ath-Thāniyah | الصدر الثانیة | Aṣ-Ṣadr ath-Thāniyah [Sadr City 2] |  |  | needs a new row; its census parent is new too |
| Dahūk [Dohuk] | District | Ash-Shaīkhān [Shekhan] | الشیخان |  |  |  | needs a new row |
| Dahūk [Dohuk] | District | Bardarash [Berdereş] | بردرش |  |  |  | needs a new row |
| Dahūk [Dohuk] | SubDistrict | Bā'adrī [Baedrê] | باعدري | Ash-Shaīkhān [Shekhan] |  |  | needs a new row; its census parent is new too |
| Dahūk [Dohuk] | SubDistrict | Kalkajī | كلكجی | Ash-Shaīkhān [Shekhan] |  |  | needs a new row; its census parent is new too |
| Dahūk [Dohuk] | SubDistrict | Dāratū [Daretû] | دارتو | Bardarash [Berdereş] |  |  | needs a new row; its census parent is new too |
| Dahūk [Dohuk] | SubDistrict | Rūfīā [Rufiya] | روفیا | Bardarash [Berdereş] |  |  | needs a new row; its census parent is new too |
| Karbalā' [Kerbala] | District | Al-Ḥur | الحر |  |  |  | needs a new row |
| Karbalā' [Kerbala] | District | At-Tamur | التمر |  |  |  | needs a new row |
| Karbalā' [Kerbala] | SubDistrict | Al-Ḥur | الحر | Al-Ḥur |  |  | needs a new row; its census parent is new too |
| Nīnawā [Nineveh] | SubDistrict | Kalak [Kelek] | كلك | Al-Ḥamdāniyah |  |  | dissolved in the census list; nothing to add |
| Nīnawā [Nineveh] | SubDistrict | Mulāqarah | ملاقرة | Makhmūr |  |  | needs a new row |
| Ḥalabjah [Halabja] | District | Bamū [Bemo] | بمو |  |  |  | needs a new row |
| Ḥalabjah [Halabja] | SubDistrict | Bamū [Bemo] | بمو | Bamū [Bemo] |  |  | needs a new row; its census parent is new too |

## Re-parented (1)

same unit, census lists it under another district; `reparent[]` keeps the code (D3).

| Governorate | Level | Census name | Census Arabic | Census district | Dataset code | Dataset name | Note |
|---|---|---|---|---|---|---|---|
| Karbalā' [Kerbala] | SubDistrict | 'Aīn at-Tamur | عین التمر | At-Tamur | IQG12Q01N01 | Markaz Ain Al-Tamur / مركز عين التمر | census parent 'At-Tamur' is a new district; reparent from IQG12Q01 |

## Cross-governorate (18)

census lists it in another governorate; only a `moves[]` entry (deprecate + replacedBy) can follow it, so this needs an owner decision.

| Governorate | Level | Census name | Census Arabic | Census district | Dataset code | Dataset name | Note |
|---|---|---|---|---|---|---|---|
| As-Sulaymāniyah | SubDistrict | Shūrash [Şoriş] | شورش | Jamjamāl [Çemçemal] | IQG11Q03N02 | Shorsh / سورش | listed in Al-Sulaymaniyah under Chamchamal but the dataset has it in Erbil; a cross-governorate move changes the code (deprecate + replacedBy) |
| As-Sulaymāniyah | SubDistrict | Maydān [Meydan] | میدان | Khānaqīn [Xaneqîn] | IQG10Q05N03 | Midan / ميدان | listed in Al-Sulaymaniyah under 'Khānaqīn [Xaneqîn]' (new there) but the dataset has it in Diyala; a cross-governorate move changes the code (deprecate + replacedBy) |
| As-Sulaymāniyah | SubDistrict | Qūrah Tū [Qoretû] | قورة تو | Khānaqīn [Xaneqîn] | IQG10Q05N05 | Qaratu / قرة تو | listed in Al-Sulaymaniyah under 'Khānaqīn [Xaneqîn]' (new there) but the dataset has it in Diyala; a cross-governorate move changes the code (deprecate + replacedBy) (Arabic spellings differ by one letter) |
| As-Sulaymāniyah | SubDistrict | Kifrī [Kifrî] | كفري | Kifrī [Kifrî] | IQG10Q06N02 | Markaz Kifri / مركز كفري | listed in Al-Sulaymaniyah under 'Kifrī [Kifrî]' (new there) but the dataset has it in Diyala; a cross-governorate move changes the code (deprecate + replacedBy) |
| Dahūk [Dohuk] | District | 'Ākra [Akrê] | ئاكرى |  | IQG15Q01 | Aqra / عقرة | listed in Duhok under Duhok but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) |
| Dahūk [Dohuk] | SubDistrict | 'Ākra [Akrê] | ئاكرى | 'Ākra [Akrê] | IQG15Q01N05 | Markaz Aqra / مركز عقرة | listed in Duhok under Aqra but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) (matched by English only) |
| Dahūk [Dohuk] | SubDistrict | Bjīl [Bicîl] | بجیل | 'Ākra [Akrê] | IQG15Q01N02 | Begeel / بجيل | listed in Duhok under Aqra but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) |
| Dahūk [Dohuk] | SubDistrict | Dīnārtī [Dînarte] | دينارتي | 'Ākra [Akrê] | IQG15Q01N04 | Dinarta / دينارتة | listed in Duhok under Aqra but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) (Arabic spellings differ by one letter) |
| Dahūk [Dohuk] | SubDistrict | Kirdasīn [Girdesîn] | كردسین | 'Ākra [Akrê] | IQG15Q01N03 | Kurdsein / كردسين | listed in Duhok under Aqra but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) |
| Dahūk [Dohuk] | SubDistrict | 'Ātrūsh [Etrîş] | أتروش | Ash-Shaīkhān [Shekhan] | IQG15Q06N03 | Atreesh / أتروش | listed in Duhok under 'Ash-Shaīkhān [Shekhan]' (new there) but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) |
| Dahūk [Dohuk] | SubDistrict | Qasrūk [Qesrok] | قسروك | Ash-Shaīkhān [Shekhan] | IQG15Q06N02 | Qasruk / قسروك | listed in Duhok under 'Ash-Shaīkhān [Shekhan]' (new there) but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) |
| Dahūk [Dohuk] | SubDistrict | Bardarash [Berdereş] | بردرش | Bardarash [Berdereş] | IQG15Q01N01 | Bardarash / بردرش | listed in Duhok under 'Bardarash [Berdereş]' (new there) but the dataset has it in Ninewa; a cross-governorate move changes the code (deprecate + replacedBy) |
| Nīnawā [Nineveh] | District | Makhmūr | مخمور |  | IQG11Q04 | Makhmour / مخمور | listed in Ninewa under Ninewa but the dataset has it in Erbil; a cross-governorate move changes the code (deprecate + replacedBy) |
| Nīnawā [Nineveh] | SubDistrict | Al-Kuwaīr | الكویر | Makhmūr | IQG11Q04N04 | Gwyer / الكوير | listed in Ninewa under Makhmour but the dataset has it in Erbil; a cross-governorate move changes the code (deprecate + replacedBy) |
| Nīnawā [Nineveh] | SubDistrict | Dībkah [Kandīnāwah] | ديبكة | Makhmūr | IQG11Q04N03 | Dibaga / ديبكة | listed in Ninewa under Makhmour but the dataset has it in Erbil; a cross-governorate move changes the code (deprecate + replacedBy) |
| Nīnawā [Nineveh] | SubDistrict | Makhmūr | مخمور | Makhmūr | IQG11Q04N02 | Markaz Makhmour / مركز مخمور | listed in Ninewa under Makhmour but the dataset has it in Erbil; a cross-governorate move changes the code (deprecate + replacedBy) |
| Nīnawā [Nineveh] | SubDistrict | Qarāj | قراج | Makhmūr | IQG11Q04N01 | Qaraj / القرج | listed in Ninewa under Makhmour but the dataset has it in Erbil; a cross-governorate move changes the code (deprecate + replacedBy) |
| Nīnawā [Nineveh] | SubDistrict | Fāīdah | فایدة | Talkayf [Tel Keppe, Tel Kaif] | IQG09Q03N02 | Fayde / فايدة | listed in Ninewa under Tilkaef but the dataset has it in Duhok; a cross-governorate move changes the code (deprecate + replacedBy) |

## Renamed (21)

same unit, matched by English only; the census Arabic spelling becomes an alias.

| Governorate | Level | Census name | Census Arabic | Census district | Dataset code | Dataset name | Note |
|---|---|---|---|---|---|---|---|
| Al-Anbār | SubDistrict | Al-'Āmiriyah aṣ-Ṣumūd | عامرية الصمود | Al-'Āmiriyah | IQG01Q51N51 | Al-Amirya / العامرية | one Arabic name contains the other; check it is the same unit, then the census spelling becomes an alias |
| Al-Baṣrah [Basra] | District | Abū al-Kahṣīb | أبو الخصيب |  | IQG02Q01 | Abu Al-Khaseeb / ابي الخصيب | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Al-Baṣrah [Basra] | SubDistrict | Abū al-Kahṣīb | أبو الخصيب | Abū al-Kahṣīb | IQG02Q01N02 | Markaz Abu Al-Khaseeb / مركز ابي الخصيب | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Al-Muthannā | District | Ar-Rumaītah | الرمیتة |  | IQG03Q02 | Al-Rumaitha / الرميثة | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Al-Muthannā | SubDistrict | Ar-Rumaītah | الرمیتة | Ar-Rumaītah | IQG03Q02N05 | Markaz Al-Rumaitha / مركز الرميثة | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Al-Qādisiyah | SubDistrict | Akad [Ad-Daghārah] | اكد | Akad [Ad-Daghārah] | IQG05Q51N51 | Markaz Al-Daghara / مركز الدغارة | matched by English only; check it is the same unit, then the census spelling becomes an alias |
| Al-Qādisiyah | SubDistrict | Ash-Shināqiyah | الشناقیة | Ash-Shināqiyah | IQG05Q55N51 | Markaz Al-Shinafiya / مركز الشنافية | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Arbīl [Erbil] | District | Rawāndūz [Rewandiz] | رِواندوز |  | IQG11Q05 | Rawanduz / راوندوز | matched by English only; check it is the same unit, then the census spelling becomes an alias |
| Arbīl [Erbil] | SubDistrict | Harīr [Harîr] | ھریر | Harīr [Harîr] | IQG11Q53N51 | Markaz Harir / مركز ديرة حرير | matched by English only; check it is the same unit, then the census spelling becomes an alias |
| Arbīl [Erbil] | SubDistrict | Ḥāji 'Umrān [Hacî Omeran] | حاج عمران | Jūmān [Çoman] | IQG11Q54N51 | Haji Omaran / الحج عمران | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Arbīl [Erbil] | SubDistrict | Pīrmām [Ṣalāḥ ad-Dīn] | بيرمام | Pīrmām [Pîrmam] | IQG11Q58N51 | Markaz Salah Al-Din / مركز صلاح الدين | matched by English only; check it is the same unit, then the census spelling becomes an alias |
| Arbīl [Erbil] | SubDistrict | Rawāndūz [Rewandiz] | رِواندوز | Rawāndūz [Rewandiz] | IQG11Q05N01 | Markaz Rawanduz / مركز راوندوز | matched by English only; check it is the same unit, then the census spelling becomes an alias |
| As-Sulaymāniyah | SubDistrict | Sīwayl [Siweyl] | سيويل | Shārbāzīr [Şarbajêr] | IQG06Q10N03 | Siwail / سويل | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Baghdād | SubDistrict | Abū Ghraīb | أبو غريب | Abū Ghraīb | IQG08Q51N51 | Markaz Abu Ghraib / مركز ابي غريب | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Baghdād | SubDistrict | Ar-Rāshidiyah al-Jidīdah | الراشدیة الجدیدة | Ar-Rāshidiyah | IQG08Q52N51 | Al-Rashdia / الراشدية | one Arabic name contains the other; check it is the same unit, then the census spelling becomes an alias |
| Baghdād | SubDistrict | Faḍā' al-Kāẓimiyyah [At-Tājī] | فضاء الكاظمية | Faḍā' al-Kāẓimiyyah [At-Tājī] | IQG08Q58N51 | Markaz Al-Taji / مركز التاجي | matched by English only; check it is the same unit, then the census spelling becomes an alias |
| Dhī Qār [Theqar] | SubDistrict | Al-Gharrāf | الغراف | Al-Gharrāf | IQG17Q55N51 | Markaz Al-Gharraf / مركز القراف | matched by English only; check it is the same unit, then the census spelling becomes an alias |
| Dhī Qār [Theqar] | SubDistrict | Karmat Banī Sa'īd | كرمة بني سعید | Karmat Banī Sa'īd | IQG17Q58N52 | Markaz Garmat Beni Said / مركز كرمة بني سعد | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Kirkūk | SubDistrict | Al-'Abāsī | العباسي | Al-Ḥawījah [Hawija] | IQG13Q01N04 | Al-Abbasi / العباسية | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Ḥalabjah [Halabja] | SubDistrict | Bīārā [Biyare] | بیارا | Bīārā [Biyare] | IQG19Q52N51 | Markaz Beyara / مركز بيارة | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Sulaīmān Bik | سلیمان بك | Ṭūz Khūrmātū | IQG16Q07N03 | Sulaiman Baig / سليمان بيك | Arabic spellings differ by one letter; check it is the same unit, then the census spelling becomes an alias |

## Extra (38)

in the dataset, not in the census list; never removed.

| Governorate | Level | Census name | Census Arabic | Census district | Dataset code | Dataset name | Note |
|---|---|---|---|---|---|---|---|
| Al-Anbār | SubDistrict |  |  |  | IQG01Q02N01 | Al-Karabla / الكرابلة | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Al-Anbār | SubDistrict |  |  |  | IQG01Q03N02 | Al-Rahhaliya / الرحالية | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Al-Baṣrah [Basra] | SubDistrict |  |  |  | IQG02Q03N01 | Al-Bahhar / البحار | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Al-Baṣrah [Basra] | SubDistrict |  |  |  | IQG02Q04N01 | Al-Howweir / الهوير | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Al-Baṣrah [Basra] | SubDistrict |  |  |  | IQG02Q04N03 | Talha / طلحة | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Al-Baṣrah [Basra] | SubDistrict |  |  |  | IQG02Q07N03 | Outba / عتبة | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Al-Qādisiyah | SubDistrict |  |  |  | IQG05Q01N02 | Al-Fawwar / الفوار | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Arbīl [Erbil] | District |  |  |  | IQG11Q01 | Al-Zibar / الزيبار | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Arbīl [Erbil] | SubDistrict |  |  |  | IQG11Q01N02 | Mazouri Bala / مزوري بالا | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Arbīl [Erbil] | SubDistrict |  |  |  | IQG11Q05N03 | Balak / بالك | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Arbīl [Erbil] | SubDistrict |  |  |  | IQG11Q05N06 | Bradost / برادوست | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Arbīl [Erbil] | SubDistrict |  |  |  | IQG11Q06N01 | Khoshnaw / خوشناو | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| As-Sulaymāniyah | SubDistrict |  |  |  | IQG06Q01N03 | Sarchnar / سرجنار | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| As-Sulaymāniyah | SubDistrict |  |  |  | IQG06Q04N01 | Gnareen / جنارين | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| As-Sulaymāniyah | SubDistrict |  |  |  | IQG06Q06N03 | Tilako / تيلاكو | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| As-Sulaymāniyah | SubDistrict |  |  |  | IQG06Q08N03 | Markaz Pshdar / مركز بشدر | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| As-Sulaymāniyah | SubDistrict |  |  |  | IQG06Q09N01 | Mirka / ميركه | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| As-Sulaymāniyah | SubDistrict |  |  |  | IQG06Q09N03 | Bitwana / بتوانه | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| As-Sulaymāniyah | SubDistrict |  |  |  | IQG06Q10N02 | Markaz Sharbazher / مركز شهربازار | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Baghdād | District |  |  |  | IQG08Q07 | Al-Thawra / الثورة | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Baghdād | SubDistrict |  |  |  | IQG08Q02N03 | That al Salasil / ذات السلاسل | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Baghdād | SubDistrict |  |  |  | IQG08Q07N01 | Markaz Al-Thawra / مركز الثورة | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Bābil [Babylon] | SubDistrict |  |  |  | IQG07Q03N03 | Al-Mashroo / المشروع | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Dahūk [Dohuk] | SubDistrict |  |  |  | IQG09Q01N01 | Barwari Bala / برواري بالا | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Dahūk [Dohuk] | SubDistrict |  |  |  | IQG09Q01N03 | Nerwa Rekan / نروة ريكان | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Dahūk [Dohuk] | SubDistrict |  |  |  | IQG09Q02N01 | Al-Duski / الدوسكي | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Dahūk [Dohuk] | SubDistrict |  |  |  | IQG09Q03N04 | Sindi / سندي | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Diyālā | SubDistrict |  |  |  | IQG10Q01N01 | Al-Atheem / العظيم | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Diyālā | SubDistrict |  |  |  | IQG10Q01N05 | Qalaa't Ansab / قلعة انصاب | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Karbalā' [Kerbala] | District |  |  |  | IQG12Q01 | Ain Al-Tamur / عين التمر | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Maysān | SubDistrict |  |  |  | IQG14Q01N02 | Al-Teeb / الطيب | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Nīnawā [Nineveh] | SubDistrict |  |  |  | IQG15Q03N03 | Markaz Al-Hamdaniya / مركز الحمدانية | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Nīnawā [Nineveh] | SubDistrict |  |  |  | IQG15Q05N04 | Al-Muhamadath / المحمداث | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Wasiṭ | SubDistrict |  |  |  | IQG18Q04N04 | Al-Hafriya / الحفرية | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Ḥalabjah [Halabja] | SubDistrict |  |  |  | IQG19Q01N04 | Shahrazur / شهرزور | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict |  |  |  | IQG16Q01N01 | Hamreen / حمرين | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict |  |  |  | IQG16Q01N02 | Abu Dalf / ابو دلف | in the dataset but not in the census list; never removed, at most deprecated with a reason |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict |  |  |  | IQG16Q06N01 | Um Al-Qura / ام القرى | in the dataset but not in the census list; never removed, at most deprecated with a reason |

## Matched (539)

same unit, same parent.

| Governorate | Level | Census name | Census Arabic | Census district | Dataset code | Dataset name | Note |
|---|---|---|---|---|---|---|---|
| Al-Anbār | District | 'Anah | عنھ |  | IQG01Q05 | Ana / عنه |  |
| Al-Anbār | District | Al-'Ubūr | العبور |  | IQG01Q54 | Al-Ubur / العبور |  |
| Al-Anbār | District | Al-'Āmiriyah | العامریة |  | IQG01Q51 | Al-Amiriyah / العامرية |  |
| Al-Anbār | District | Al-Fallūjah [Fallujah] | الفلوجة |  | IQG01Q01 | Al-Falluja / الفلوجة |  |
| Al-Anbār | District | Al-Karmah | الكرمة |  | IQG01Q53 | Al-Karmah / الكرمة |  |
| Al-Anbār | District | Al-Qā'im | القائم |  | IQG01Q02 | Al-Kaim / القائم |  |
| Al-Anbār | District | Al-Ḥabbāniyah | الحبانیة |  | IQG01Q52 | Al-Habbaniyah / الحبانية |  |
| Al-Anbār | District | Ar-Ramādī [Ramadi] | الرمادي |  | IQG01Q03 | Al-Ramadi / الرمادي |  |
| Al-Anbār | District | Ar-Ruṭbah | الرطبة |  | IQG01Q04 | Al-Rutba / الرطبة |  |
| Al-Anbār | District | Hīt | ھیت |  | IQG01Q07 | Heet / هيت |  |
| Al-Anbār | District | Rāwah | راوة |  | IQG01Q55 | Rawah / راوة |  |
| Al-Anbār | District | Ḥadīṭha [Haditha] | حدیثة |  | IQG01Q06 | Haditha / حديثة |  |
| Al-Anbār | SubDistrict | 'Anah | عنھ | 'Anah | IQG01Q05N01 | Markaz Ana / مركز عنه |  |
| Al-Anbār | SubDistrict | Al-'Ubūr | الرمانة | Al-'Ubūr | IQG01Q54N51 | Markaz Al-Rummaneh / مركز الرمانة |  |
| Al-Anbār | SubDistrict | Al-Fallūjah [Fallujah] | الفلوجة | Al-Fallūjah [Fallujah] | IQG01Q01N03 | Markaz Al-Falluja / مركز الفلوجة |  |
| Al-Anbār | SubDistrict | Aṣ-Ṣaqlāwiyah | الصقلاویة | Al-Fallūjah [Fallujah] | IQG01Q01N51 | As-Saqlawiyah / الصقلاوية |  |
| Al-Anbār | SubDistrict | Al-Karmah | الكرمة | Al-Karmah | IQG01Q53N51 | Markaz Al-Garma / مركز الكرمة |  |
| Al-Anbār | SubDistrict | Al-Qā'im | القائم | Al-Qā'im | IQG01Q02N51 | Markaz Al-Qaim / مركز القائم |  |
| Al-Anbār | SubDistrict | Al-'Ubaydī | العبیدي | Al-Qā'im | IQG01Q02N02 | Al-Obiadi / العبيدي |  |
| Al-Anbār | SubDistrict | Al-Ḥabbāniyah | الحبانیة | Al-Ḥabbāniyah | IQG01Q52N51 | Markaz Al-Habbaniya / مركز الحبانية |  |
| Al-Anbār | SubDistrict | Al-Wafā' | الوفاء | Ar-Ramādī [Ramadi] | IQG01Q03N51 | Al-Wafa / الوفاء |  |
| Al-Anbār | SubDistrict | Ar-Ramādī [Ramadi] | الرمادي | Ar-Ramādī [Ramadi] | IQG01Q03N03 | Markaz Al-Ramadi / مركز الرمادي |  |
| Al-Anbār | SubDistrict | Al-Walīd | الولید | Ar-Ruṭbah | IQG01Q04N02 | Al-Walid / الوليد |  |
| Al-Anbār | SubDistrict | An-Nukhayb | النخیب | Ar-Ruṭbah | IQG01Q04N01 | Al-Nekhaeeb / النخيب |  |
| Al-Anbār | SubDistrict | Ar-Ruṭbah | الرطبة | Ar-Ruṭbah | IQG01Q04N03 | Markaz Al-Rutba / مركز الرطبة |  |
| Al-Anbār | SubDistrict | Al-Baghdādī | البغدادي | Hīt | IQG01Q07N01 | Al-Baghdady / البغدادي |  |
| Al-Anbār | SubDistrict | Al-Furāt | الفرات | Hīt | IQG01Q07N02 | Al-Forat / الفرات |  |
| Al-Anbār | SubDistrict | Al-Muḥammadī | المحمدي | Hīt | IQG01Q07N51 | Al-Muhammadi / المحمدي |  |
| Al-Anbār | SubDistrict | Hīt | ھیت | Hīt | IQG01Q07N04 | Markaz Heet / مركز هيت |  |
| Al-Anbār | SubDistrict | Kubaīsah | كبیسة | Hīt | IQG01Q07N03 | Kubaisa / كبيسة |  |
| Al-Anbār | SubDistrict | Rāwah | راوة | Rāwah | IQG01Q55N51 | Markaz Rawa / مركز راوه |  |
| Al-Anbār | SubDistrict | Al-Ḥaqlāniyah | الحقلانیة | Ḥadīṭha [Haditha] | IQG01Q06N01 | Al-Haqlaniya / الحقلانية |  |
| Al-Anbār | SubDistrict | Barwānah | بروانة | Ḥadīṭha [Haditha] | IQG01Q06N02 | Barwana / بروانه |  |
| Al-Anbār | SubDistrict | Ḥadīṭha | حدیثة | Ḥadīṭha [Haditha] | IQG01Q06N51 | Markaz Haditha / مركز حديثة |  |
| Al-Baṣrah [Basra] | District | Ad-Daīr | الدیر |  | IQG02Q51 | Ad-Dair / الدير |  |
| Al-Baṣrah [Basra] | District | Al-Baṣrah [Basra] | البصرة |  | IQG02Q02 | Al-Basrah / البصرة |  |
| Al-Baṣrah [Basra] | District | Al-Fāw | الفاو |  | IQG02Q03 | Al-Faw / الفاو |  |
| Al-Baṣrah [Basra] | District | Al-Hārithah | الھارثة |  | IQG02Q52 | Al-Harithah / الهارثة |  |
| Al-Baṣrah [Basra] | District | Al-Midaīnah | المدینة |  | IQG02Q04 | Al-Midaina / المدينة |  |
| Al-Baṣrah [Basra] | District | Al-Qurnah | القرنة |  | IQG02Q05 | Al-Qurna / القرنة |  |
| Al-Baṣrah [Basra] | District | Az-Zubayr | الزبیر |  | IQG02Q06 | Al-Zubair / الزبير |  |
| Al-Baṣrah [Basra] | District | Safwān | سفوان |  | IQG02Q54 | Safwan / سفوان |  |
| Al-Baṣrah [Basra] | District | Shaṭṭ al-'Arab | شط العرب |  | IQG02Q07 | Shat Al-Arab / شط العرب |  |
| Al-Baṣrah [Basra] | SubDistrict | As-Sībah | السيبة | Abū al-Kahṣīb | IQG02Q01N01 | Al-Siba / السيبة |  |
| Al-Baṣrah [Basra] | SubDistrict | Ad-Daīr | الدیر | Ad-Daīr | IQG02Q51N51 | Markaz Al-Dair / مركز الدير |  |
| Al-Baṣrah [Basra] | SubDistrict | Al-Baṣrah [Basra] | البصرة | Al-Baṣrah [Basra] | IQG02Q02N02 | Markaz Al-Basrah / مركز البصرة |  |
| Al-Baṣrah [Basra] | SubDistrict | Al-Fāw | الفاو | Al-Fāw | IQG02Q03N02 | Markaz Al-Faw / مركز الفاو |  |
| Al-Baṣrah [Basra] | SubDistrict | Al-Hārithah | الھارثة | Al-Hārithah | IQG02Q52N51 | Markaz Al-Hartha / مركز الهارثة |  |
| Al-Baṣrah [Basra] | SubDistrict | Al-Midaīnah | المدینة | Al-Midaīnah | IQG02Q04N02 | Markaz Al-Midaina / مركز المدينة |  |
| Al-Baṣrah [Basra] | SubDistrict | 'Az Aldīn Salīm / Al-Huwaīr | عز الدین سلیم | Al-Midaīnah | IQG02Q04N51 | Az Aldin Salim / عز الدين سليم |  |
| Al-Baṣrah [Basra] | SubDistrict | Al-Qurnah | القرنة | Al-Qurnah | IQG02Q05N03 | Markaz Al-Qurna / مركز القرنة |  |
| Al-Baṣrah [Basra] | SubDistrict | Ath-Thughir | الثغر | Al-Qurnah | IQG02Q05N02 | Al-Thagar / الثغر |  |
| Al-Baṣrah [Basra] | SubDistrict | Az-Zubayr [Zubayr] | الزبیر | Az-Zubayr | IQG02Q06N01 | Markaz Al-Zubair / مركز الزبير |  |
| Al-Baṣrah [Basra] | SubDistrict | Khūr az-Zubayr | خور الزبير | Az-Zubayr | IQG02Q06N51 | Khur az-Zubayr / خور الزبير |  |
| Al-Baṣrah [Basra] | SubDistrict | Umm Qaṣr | أم قصر | Az-Zubayr | IQG02Q06N03 | Um Qasr / ام قصر |  |
| Al-Baṣrah [Basra] | SubDistrict | Safwān | سفوان | Safwān | IQG02Q54N51 | Markaz Safwan / مركز سفوان |  |
| Al-Baṣrah [Basra] | SubDistrict | An-Nashwah | النشوة | Shaṭṭ al-'Arab | IQG02Q07N01 | Al-Nashwa / النشوة |  |
| Al-Baṣrah [Basra] | SubDistrict | Shaṭṭ al-'Arab | شط العرب | Shaṭṭ al-'Arab | IQG02Q07N02 | Markaz Shat Al-Arab / مركز شط العرب |  |
| Al-Muthannā | District | Al-Hilāl | الھلال |  | IQG03Q51 | Al-Hilal / الهلال |  |
| Al-Muthannā | District | Al-Khiḍir | الخضر |  | IQG03Q01 | Al-Khidhir / الخضر |  |
| Al-Muthannā | District | Al-Majd | المجد |  | IQG03Q52 | Al-Majd / المجد |  |
| Al-Muthannā | District | Al-Warkā' | الوركاء |  | IQG03Q53 | Al-Warka / الوركاء |  |
| Al-Muthannā | District | An-Najmī | النجمي |  | IQG03Q54 | An-Najmi / النجمي |  |
| Al-Muthannā | District | As-Salmān | السلمان |  | IQG03Q03 | Al-Salman / السلمان |  |
| Al-Muthannā | District | As-Samāwah | السماوة |  | IQG03Q04 | Al-Samawa / السماوة |  |
| Al-Muthannā | District | As-Suwaīr | السویر |  | IQG03Q55 | As-Suwair / السوير |  |
| Al-Muthannā | SubDistrict | Al-Hilāl | الھلال | Al-Hilāl | IQG03Q51N51 | Markaz Al-Hilal / مركز الهلال |  |
| Al-Muthannā | SubDistrict | Ad-Darājī | الدراجي | Al-Khiḍir | IQG03Q01N51 | Ad-Daraji / الدراجي |  |
| Al-Muthannā | SubDistrict | Al-Khiḍir | الخضر | Al-Khiḍir | IQG03Q01N01 | Markaz Al-Khidhir / مركز الخضر |  |
| Al-Muthannā | SubDistrict | Al-Majd | المجد | Al-Majd | IQG03Q52N51 | Markaz Al-Majd / مركز المجد |  |
| Al-Muthannā | SubDistrict | Al-Karāmah | الکرامة | Al-Warkā' | IQG03Q53N52 | Al-Karamah / الكرامة |  |
| Al-Muthannā | SubDistrict | Al-Warkā' | الوركاء | Al-Warkā' | IQG03Q53N51 | Markaz Al-Warka / مركز الوركاء |  |
| Al-Muthannā | SubDistrict | An-Najmī | النجمي | An-Najmī | IQG03Q54N51 | Markaz Al-Najmi / مركز النجمي |  |
| Al-Muthannā | SubDistrict | Al-Buṣaiyah | البصیة | As-Salmān | IQG03Q03N51 | Busayyah / البصية |  |
| Al-Muthannā | SubDistrict | As-Salmān | السلمان | As-Salmān | IQG03Q03N01 | Markaz Al-Salman / مركز السلمان |  |
| Al-Muthannā | SubDistrict | As-Samāwah [Samawah] | السماوة | As-Samāwah | IQG03Q04N51 | Markaz As-Samawah / مركز السماوة |  |
| Al-Muthannā | SubDistrict | As-Suwaīr | السویر | As-Suwaīr | IQG03Q55N51 | Markaz Al-Sowair / مركز السوير |  |
| Al-Qādisiyah | District | 'Afak | عفك |  | IQG05Q01 | Afaq / عفك |  |
| Al-Qādisiyah | District | Ad-Dīwāniyah [Diwaniya] | الديوانية |  | IQG05Q02 | Al-Diwaniya / الديوانية |  |
| Al-Qādisiyah | District | Akad [Ad-Daghārah] | اكد |  | IQG05Q51 | Akad / اكد |  |
| Al-Qādisiyah | District | Al-Budaīr | البدیر |  | IQG05Q52 | Al-Budair / البدير |  |
| Al-Qādisiyah | District | Al-Mahnāwiyah | المھناویة |  | IQG05Q53 | Al-Mahnawiyah / المهناوية |  |
| Al-Qādisiyah | District | Al-Ḥamzah | الحمزة |  | IQG05Q03 | Al-Hamza / الحمزة |  |
| Al-Qādisiyah | District | As-Saniyah | السنیة |  | IQG05Q56 | As-Saniyah / السنية |  |
| Al-Qādisiyah | District | As-Sudayr | السدیر |  | IQG05Q57 | As-Sudayr / السدير |  |
| Al-Qādisiyah | District | Ash-Shināqiyah | الشناقیة |  | IQG05Q55 | Ash-Shinaqiyah / الشناقية |  |
| Al-Qādisiyah | District | Ash-Shāfi'iyah | الشافعیة |  | IQG05Q54 | Ash-Shafiiyah / الشافعية |  |
| Al-Qādisiyah | District | Ash-Shāmiyah | الشامیة |  | IQG05Q04 | Al-Shamiya / الشامية |  |
| Al-Qādisiyah | District | Ghammās | غماس |  | IQG05Q58 | Ghammas / غماس |  |
| Al-Qādisiyah | District | Sūmir | سومر |  | IQG05Q59 | Sumir / سومر |  |
| Al-Qādisiyah | SubDistrict | 'Afak | عفك | 'Afak | IQG05Q01N03 | Markaz Afaq / مركز عفك |  |
| Al-Qādisiyah | SubDistrict | Nafar | نفر | 'Afak | IQG05Q01N51 | Nafar / نفر |  |
| Al-Qādisiyah | SubDistrict | Ad-Dīwāniyah [Diwaniyah] | الديوانية | Ad-Dīwāniyah [Diwaniya] | IQG05Q02N04 | Markaz Al-Diwaniya / مركز الديوانية |  |
| Al-Qādisiyah | SubDistrict | Al-Budaīr | البدیر | Al-Budaīr | IQG05Q52N51 | Markaz Al-Bdair / مركز البدير |  |
| Al-Qādisiyah | SubDistrict | Al-Mahnāwiyah | المھناویة | Al-Mahnāwiyah | IQG05Q53N51 | Markaz Al-Mihanawiya / مركز المهناوية |  |
| Al-Qādisiyah | SubDistrict | Al-Ḥamzah | الحمزة | Al-Ḥamzah | IQG05Q03N03 | Markaz Al-Hamza / مركز الحمزة |  |
| Al-Qādisiyah | SubDistrict | As-Saniyah | السنیة | As-Saniyah | IQG05Q56N51 | Markaz Al-Saniya / مركز السنية |  |
| Al-Qādisiyah | SubDistrict | As-Sudayr | السدیر | As-Sudayr | IQG05Q57N51 | Markaz Al-Sadeer / مركز السدير |  |
| Al-Qādisiyah | SubDistrict | Ash-Shāfi'iyah | الشافعیة | Ash-Shāfi'iyah | IQG05Q54N51 | Markaz Al-Shafeia / مركز الشافعية |  |
| Al-Qādisiyah | SubDistrict | Ash-Shāmiyah | الشامیة | Ash-Shāmiyah | IQG05Q04N51 | Markaz Ash-Shamiyah / مركز الشامية |  |
| Al-Qādisiyah | SubDistrict | Aṣ-Ṣalāḥiyah | الصلاحیة | Ash-Shāmiyah | IQG05Q04N52 | As-Salahiyah / الصلاحية |  |
| Al-Qādisiyah | SubDistrict | Ghammās | غماس | Ghammās | IQG05Q58N51 | Markaz Ghammas / مركز غماس |  |
| Al-Qādisiyah | SubDistrict | Sūmir | سومر | Sūmir | IQG05Q59N51 | Markaz Sumer / مركز سومر |  |
| An-Najaf | District | Al-Kūfah | الكوفة |  | IQG04Q01 | Al-Kufa / الكوفة |  |
| An-Najaf | District | Al-Manādhirah [Al-Manathera] | المناذرة |  | IQG04Q02 | Al-Manathera / المناذرة |  |
| An-Najaf | District | Al-Mishkhāb [Al-Meshkhab] | المشخاب |  | IQG04Q51 | Al-Mishkhab / المشخاب |  |
| An-Najaf | District | An-Najaf [Najaf] | النجف |  | IQG04Q03 | Al-Najaf / النجف |  |
| An-Najaf | SubDistrict | Al-'Abāsiyah | العباسیة | Al-Kūfah | IQG04Q01N51 | Al-Abasiyah / العباسية |  |
| An-Najaf | SubDistrict | Al-Ḥuriyah | الحریة | Al-Kūfah | IQG04Q01N01 | Al-Huriya / الحرية |  |
| An-Najaf | SubDistrict | Al-Kūfah | الكوفة | Al-Kūfah | IQG04Q01N02 | Markaz Al-Kufa / مركز الكوفة |  |
| An-Najaf | SubDistrict | Al-Ḥīrah | الحیرة | Al-Manādhirah [Al-Manathera] | IQG04Q02N01 | Al-Heera / الحيرة |  |
| An-Najaf | SubDistrict | Al-Manādhirah | المناذرة | Al-Manādhirah [Al-Manathera] | IQG04Q02N51 | Markaz Al-Manadhirah / مركز المناذرة |  |
| An-Najaf | SubDistrict | Al-Mishkhāb | المشخاب | Al-Mishkhāb [Al-Meshkhab] | IQG04Q51N51 | Markaz Al-Mishkhab / مركز المشخاب |  |
| An-Najaf | SubDistrict | Al-Qādisiyah | القادسیة | Al-Mishkhāb [Al-Meshkhab] | IQG04Q51N52 | Al-Qadisiyah / القادسية |  |
| An-Najaf | SubDistrict | Al-Ḥaydariyah | الحیدریة | An-Najaf [Najaf] | IQG04Q03N01 | Al-Haydariya / الحيدرية |  |
| An-Najaf | SubDistrict | An-Najaf [Najaf] | النجف | An-Najaf [Najaf] | IQG04Q03N03 | Markaz Al-Najaf / مركز النجف |  |
| An-Najaf | SubDistrict | An-Nūr / Bānīqiyā | النور(بانيقيا) | An-Najaf [Najaf] | IQG04Q03N51 | An-Nur / النور(بانيقيا) |  |
| An-Najaf | SubDistrict | Ar-Raḍawiyah | الرضوية | An-Najaf [Najaf] | IQG04Q03N52 | Ar-Radawiyah / الرضوية |  |
| An-Najaf | SubDistrict | Ash-Shabakah | الشبكة | An-Najaf [Najaf] | IQG04Q03N02 | Al-Shabaka / الشبكة |  |
| Arbīl [Erbil] | District | Ainkāwah [Enkawa] | عنكاوة |  | IQG11Q51 | Enkawa / عنكاوة |  |
| Arbīl [Erbil] | District | Arbīl [Hewlêr, Erbil] | أربيل |  | IQG11Q02 | Erbil / اربيل |  |
| Arbīl [Erbil] | District | Dhashtā Hawlī'r [Deşta Hewlêrê] | دةشتى ھةولیَر |  | IQG11Q52 | Deshta Hewlere / سهل أربيل |  |
| Arbīl [Erbil] | District | Harīr [Harîr] | ھریر |  | IQG11Q53 | Harir / هرير |  |
| Arbīl [Erbil] | District | Jūmān [Çoman] | جومان |  | IQG11Q54 | Choman / جومان |  |
| Arbīl [Erbil] | District | Khalīfān [Xelîfan] | خلیفان |  | IQG11Q56 | Khelifan / خليفان |  |
| Arbīl [Erbil] | District | Kuysanjaq [Koye] | كؤیسنجق |  | IQG11Q03 | Koysinjaq / كويسنجق |  |
| Arbīl [Erbil] | District | Maīrkahsūr [Mêrgesor‎] | مَیركةسور |  | IQG11Q57 | Mergasor / ميركسور |  |
| Arbīl [Erbil] | District | Pīrmām [Pîrmam] | بيرمام |  | IQG11Q58 | Pirmam / بيرمام |  |
| Arbīl [Erbil] | District | Qūshtabah [Quştepe] | قوشتبة |  | IQG11Q59 | Qushtepe / قوشتبة |  |
| Arbīl [Erbil] | District | Shaqlāwah [Şeqlawe] | شقلاوة |  | IQG11Q06 | Shaqlawa / شقلاوة |  |
| Arbīl [Erbil] | District | Sīdakān [Sîdekan] | سيدكان |  | IQG11Q60 | Sidekan / سيدكان |  |
| Arbīl [Erbil] | District | Sūrān [Soran‎] | سؤران |  | IQG11Q61 | Soran / سوران |  |
| Arbīl [Erbil] | SubDistrict | Ainkāwah [Enkawa] | عنكاوة | Ainkāwah [Enkawa] | IQG11Q51N51 | Markaz Enkawa / مركز عنكاوة |  |
| Arbīl [Erbil] | SubDistrict | Arbīl [Erbil, Hewlêr] | أربيل | Arbīl [Hewlêr, Erbil] | IQG11Q02N02 | Markaz Erbil / مركز اربيل |  |
| Arbīl [Erbil] | SubDistrict | Baḥarkah [Behirke] | بحركة | Arbīl [Hewlêr, Erbil] | IQG11Q02N51 | Behirke / بحركة |  |
| Arbīl [Erbil] | SubDistrict | Shamāmak [Şemamik] | شمامك | Arbīl [Hewlêr, Erbil] | IQG11Q02N52 | Shemamik / شمامك |  |
| Arbīl [Erbil] | SubDistrict | Bīstānah | بيستانة | Dhashtā Hawlī'r [Deşta Hewlêrê] | IQG11Q52N51 | Bistanah / بيستانة |  |
| Arbīl [Erbil] | SubDistrict | Dārhtūū [Daretû] | دارةتوو | Dhashtā Hawlī'r [Deşta Hewlêrê] | IQG11Q52N52 | Daretu / دارتوو |  |
| Arbīl [Erbil] | SubDistrict | Dhashtā Hawlī'r [Benṣalāwah, Deşta Hewlêrê] | دةشتى ھةولیَر | Dhashtā Hawlī'r [Deşta Hewlêrê] | IQG11Q52N53 | Markaz Deshta Hewlere / مركز دشتى هولير |  |
| Arbīl [Erbil] | SubDistrict | Khasnahzān [Kesnezan] | كةسنةزان | Dhashtā Hawlī'r [Deşta Hewlêrê] | IQG11Q52N54 | Kesnezan / كسنزان |  |
| Arbīl [Erbil] | SubDistrict | Bāsarmah [Basirme] | باسرمة | Harīr [Harîr] | IQG11Q53N52 | Basirme / باسرمة |  |
| Arbīl [Erbil] | SubDistrict | Jūmān [Çoman] | جومان | Jūmān [Çoman] | IQG11Q54N52 | Markaz Choman / مركز جومان |  |
| Arbīl [Erbil] | SubDistrict | Kalālah [Gelale] | كلالة | Jūmān [Çoman] | IQG11Q54N53 | Galala / كلالة |  |
| Arbīl [Erbil] | SubDistrict | Qasry [Qesrê] | قسري | Jūmān [Çoman] | IQG11Q54N54 | Qesre / قسري |  |
| Arbīl [Erbil] | SubDistrict | Samīlān [Simîlan] | سميلان | Jūmān [Çoman] | IQG11Q54N55 | Similan / سميلان |  |
| Arbīl [Erbil] | SubDistrict | Khalīfān [Xelîfan] | خلیفان | Khalīfān [Xelîfan] | IQG11Q56N51 | Markaz Khailfan / مركز خليفان |  |
| Arbīl [Erbil] | SubDistrict | Sarayishmah [Sarayishme] | سرَيشمة | Khalīfān [Xelîfan] | IQG11Q56N52 | Sarayishme / سريشمة |  |
| Arbīl [Erbil] | SubDistrict | Āshtī [Aştî] | آشتي | Kuysanjaq [Koye] | IQG11Q03N51 | Ashti / آشتي |  |
| Arbīl [Erbil] | SubDistrict | Kuysanjaq [Koye] | كؤیسنجق | Kuysanjaq [Koye] | IQG11Q03N03 | Markaz Koysinjaq / مركز كويسنجق |  |
| Arbīl [Erbil] | SubDistrict | Sīkirdkān [Sêgirdikan] | سيكردكان | Kuysanjaq [Koye] | IQG11Q03N52 | Segrdkan / سيكردكان |  |
| Arbīl [Erbil] | SubDistrict | Siktān [Sikitan] | سكتان | Kuysanjaq [Koye] | IQG11Q03N53 | Siktan / سكتان |  |
| Arbīl [Erbil] | SubDistrict | Ṭaq Ṭaq [Teqteq] | تةق تةق | Kuysanjaq [Koye] | IQG11Q03N01 | Taq Taq / طق طق |  |
| Arbīl [Erbil] | SubDistrict | Bārzān [Barzan] | بارزان | Maīrkahsūr [Mêrgesor‎] | IQG11Q57N51 | Barzan / بارزان |  |
| Arbīl [Erbil] | SubDistrict | Bīrān [Pîran] | بیران | Maīrkahsūr [Mêrgesor‎] | IQG11Q57N53 | Piran / بيران |  |
| Arbīl [Erbil] | SubDistrict | Kawratū [Goretû] | كورتو | Maīrkahsūr [Mêrgesor‎] | IQG11Q57N54 | Goretu / كورتو |  |
| Arbīl [Erbil] | SubDistrict | Maīrkahsūr [Mêrgesor‎] | مَیركةسور | Maīrkahsūr [Mêrgesor‎] | IQG11Q57N52 | Markaz Mergasur / مركز ميركة سور |  |
| Arbīl [Erbil] | SubDistrict | Maznī [Meznê] | مزني | Maīrkahsūr [Mêrgesor‎] | IQG11Q57N55 | Mezne / مزني |  |
| Arbīl [Erbil] | SubDistrict | Shīrwān Mazin [Şêrwan Mezin] | شيروان مزن | Maīrkahsūr [Mêrgesor‎] | IQG11Q57N56 | Sherwan Mezin / شيروان مزن |  |
| Arbīl [Erbil] | SubDistrict | Batst'ūrah [Bastora] | بةستؤِرة | Pīrmām [Pîrmam] | IQG11Q58N52 | Bastora / بستورة |  |
| Arbīl [Erbil] | SubDistrict | Dārshakrān [Dareşekiran] | دارشكران | Pīrmām [Pîrmam] | IQG11Q58N53 | Dareshekiran / دارشكران |  |
| Arbīl [Erbil] | SubDistrict | Kūrā [Kore] | كورى | Pīrmām [Pîrmam] | IQG11Q58N54 | Kore / كورى |  |
| Arbīl [Erbil] | SubDistrict | Qūshtabah [Quştepe] | قوشتبة | Qūshtabah [Quştepe] | IQG11Q59N51 | Markaz Qushtappa / مركز قوش تبه |  |
| Arbīl [Erbil] | SubDistrict | Warty [Wertê] | ورتي | Rawāndūz [Rewandiz] | IQG11Q05N51 | Warte / ورتي |  |
| Arbīl [Erbil] | SubDistrict | Bālīsān [Balîsan] | بالیسان | Shaqlāwah [Şeqlawe] | IQG11Q06N51 | Balisan / باليسان |  |
| Arbīl [Erbil] | SubDistrict | Hīrān [Hîran] | ھیران | Shaqlāwah [Şeqlawe] | IQG11Q06N52 | Hiran / هيران |  |
| Arbīl [Erbil] | SubDistrict | Shaqlāwah [Şeqlawe] | شقلاوة | Shaqlāwah [Şeqlawe] | IQG11Q06N53 | Markaz Sheqlawe / مركز شقلاوة |  |
| Arbīl [Erbil] | SubDistrict | Kāniyahrahsh [Kaniyêreş] | كانيةِرةش | Sīdakān [Sîdekan] | IQG11Q60N51 | Kaniyeresh / كانيرش |  |
| Arbīl [Erbil] | SubDistrict | Sīdakān [Sîdekan] | سيدكان | Sīdakān [Sîdekan] | IQG11Q60N52 | Markaz Sidekan / مركز سيدكان |  |
| Arbīl [Erbil] | SubDistrict | Diyānā [Diyana] | دیانا | Sūrān [Soran‎] | IQG11Q61N51 | Diana / ديانا |  |
| Arbīl [Erbil] | SubDistrict | Sūrān [Soran‎] | سؤران | Sūrān [Soran‎] | IQG11Q61N52 | Markaz Soran‎ / مركز سوران |  |
| As-Sulaymāniyah | District | As-Sulaymāniyah [Silêmanî, Sulaymaniyah] | السليمانية |  | IQG06Q01 | Al-Sulaymaniyah / السليمانية |  |
| As-Sulaymāniyah | District | Banjwīn [Pêncwîn] | بنجوین |  | IQG06Q07 | Panjwin / بنجوين |  |
| As-Sulaymāniyah | District | Bashdar [Pişder] | بشدر |  | IQG06Q08 | Pshdar / بشدر |  |
| As-Sulaymāniyah | District | Bāzyān [Bazyan] | بازیان |  | IQG06Q51 | Bazyan / بازيان |  |
| As-Sulaymāniyah | District | Darbandikhān [Derbendîxan] | دربندخان |  | IQG06Q03 | Derbendikhan / دربندخان |  |
| As-Sulaymāniyah | District | Dūkān [Dûkan] | دوكان |  | IQG06Q04 | Dokan / دوكان |  |
| As-Sulaymāniyah | District | Jamjamāl [Çemçemal] | جمجمال |  | IQG06Q02 | Chamchamal / جمجمال |  |
| As-Sulaymāniyah | District | Kalār [Kelar] | كلار |  | IQG06Q06 | Kalar / كلار |  |
| As-Sulaymāniyah | District | Māwat [Mawet] | ماوت |  | IQG06Q53 | Mawat / ماوت |  |
| As-Sulaymāniyah | District | Qara Dāgh [Qeredax] | قرة داغ |  | IQG06Q54 | Qaradagh / قرة داغ |  |
| As-Sulaymāniyah | District | Rānyah [Raniye] | رانیة |  | IQG06Q09 | Rania / رانية |  |
| As-Sulaymāniyah | District | Saīd Ṣādiq [Seyd Sadiq] | سید صادق |  | IQG06Q55 | Saidsadiq / سيد صادق |  |
| As-Sulaymāniyah | District | Shārah Zūr [Şarezûr] | شارة زوور |  | IQG06Q56 | Sharezur / شارة زوور |  |
| As-Sulaymāniyah | District | Shārbāzīr [Şarbajêr] | شاربازیر |  | IQG06Q10 | Sharbazher / شهربازار |  |
| As-Sulaymāniyah | District | Ḥājīāwah [Hacîawe] | حاجیاوة |  | IQG06Q52 | Haciawe / حاجياوة |  |
| As-Sulaymāniyah | SubDistrict | As-Sulaymāniyah [Sulaymaniyah, Silêmanî] | السليمانية | As-Sulaymāniyah [Silêmanî, Sulaymaniyah] | IQG06Q01N51 | Markaz Silemani / مركز السليمانية |  |
| As-Sulaymāniyah | SubDistrict | Bakrah Jū [Bekreco] | بكرة جو | As-Sulaymāniyah [Silêmanî, Sulaymaniyah] | IQG06Q01N52 | Bekreco / بكرة جو |  |
| As-Sulaymāniyah | SubDistrict | Tānjarū [Tancero] | تانجرو | As-Sulaymāniyah [Silêmanî, Sulaymaniyah] | IQG06Q01N53 | Tancero / تانجرو |  |
| As-Sulaymāniyah | SubDistrict | Banjwīn [Pêncwîn] | بنجوین | Banjwīn [Pêncwîn] | IQG06Q07N02 | Markaz Panjwin / مركز بنجوين |  |
| As-Sulaymāniyah | SubDistrict | Karmak [Germik] | كرمك | Banjwīn [Pêncwîn] | IQG06Q07N01 | Karmak / كرمك |  |
| As-Sulaymāniyah | SubDistrict | Nalbārīz [Nalbarêz] | نلباريز | Banjwīn [Pêncwîn] | IQG06Q07N51 | Nalbarez / نلباريز |  |
| As-Sulaymāniyah | SubDistrict | 'Eīsiway [Îsîwî] | عيسوي | Bashdar [Pişder] | IQG06Q08N51 | Isiwi / عيسوي |  |
| As-Sulaymāniyah | SubDistrict | Halashū [Helşû] | ھلشو | Bashdar [Pişder] | IQG06Q08N52 | Helshu / هلشو |  |
| As-Sulaymāniyah | SubDistrict | Hīrū [Hêro] | ھیرو | Bashdar [Pişder] | IQG06Q08N01 | Hero / هيرو |  |
| As-Sulaymāniyah | SubDistrict | Nāwdasht [Nawdeşt] | ناودشت | Bashdar [Pişder] | IQG06Q08N02 | Nawdasht / ناودشت |  |
| As-Sulaymāniyah | SubDistrict | Qaladiza [Qeladizê] | قلدز | Bashdar [Pişder] | IQG06Q08N53 | Qeladize / قلدز |  |
| As-Sulaymāniyah | SubDistrict | Zārāūh [Jarawe] | زاراوة | Bashdar [Pişder] | IQG06Q08N54 | Zharawa / زاراوة |  |
| As-Sulaymāniyah | SubDistrict | Bāzyān [Bazyan] | بازیان | Bāzyān [Bazyan] | IQG06Q51N51 | Markaz Bazian / مركز بازيان |  |
| As-Sulaymāniyah | SubDistrict | Bāw Khūshīn [Bawexoşîn] | باو خوشین | Darbandikhān [Derbendîxan] | IQG06Q03N51 | Bawekhoshin / باو خوشين |  |
| As-Sulaymāniyah | SubDistrict | Darbandikhān [Derbendîxan] | دربندخان | Darbandikhān [Derbendîxan] | IQG06Q03N01 | Markaz Derbendikhan / مركز دربندخان |  |
| As-Sulaymāniyah | SubDistrict | Binkird [Bingird] | بنكرد | Dūkān [Dûkan] | IQG06Q04N51 | Bingird / بنكرد |  |
| As-Sulaymāniyah | SubDistrict | Bīrah Makrūn [Pîremegirûn] | بیرة مكرون | Dūkān [Dûkan] | IQG06Q04N52 | Piremegirun / بيرة مكرون |  |
| As-Sulaymāniyah | SubDistrict | Dūkān [Dûkan] | دوكان | Dūkān [Dûkan] | IQG06Q04N53 | Markaz Dukan / مركز دوكان |  |
| As-Sulaymāniyah | SubDistrict | Khalakān [Xelekan] | خلکان | Dūkān [Dûkan] | IQG06Q04N54 | Khelekan / خلكان |  |
| As-Sulaymāniyah | SubDistrict | Khidrān [Xidiran] | خدران | Dūkān [Dûkan] | IQG06Q04N55 | Khidiran / خدران |  |
| As-Sulaymāniyah | SubDistrict | Sūrdāsh [Surdaş] | سورداش | Dūkān [Dûkan] | IQG06Q04N02 | Sourdash / سورداش |  |
| As-Sulaymāniyah | SubDistrict | 'Āghjalar [Axceler] | أغجلر | Jamjamāl [Çemçemal] | IQG06Q02N01 | Agjalare / آغجلر |  |
| As-Sulaymāniyah | SubDistrict | Jamjamāl [Çemçemal] | جمجمال | Jamjamāl [Çemçemal] | IQG06Q02N02 | Markaz Chamchamal / مركز جمجمال |  |
| As-Sulaymāniyah | SubDistrict | Qādir Karam [Qadir Kerem] | قادر كرم | Jamjamāl [Çemçemal] | IQG06Q02N03 | Kadr Karam / قادر كرم |  |
| As-Sulaymāniyah | SubDistrict | Sankāw [Sengaw] | سنكاو | Jamjamāl [Çemçemal] | IQG06Q02N04 | Cenkaw / سنكاو |  |
| As-Sulaymāniyah | SubDistrict | Ṭakiyah [Tekiye] | تة كیة | Jamjamāl [Çemçemal] | IQG06Q02N51 | Tekiye / تة كية |  |
| As-Sulaymāniyah | SubDistrict | Takiyah Jabārā [Tekiyey Cebarî] | تكیة جبارى | Jamjamāl [Çemçemal] | IQG06Q02N52 | Tekiyey Cebari / تكية جبارى |  |
| As-Sulaymāniyah | SubDistrict | Bībāz [Pêbaz] | بیباز | Kalār [Kelar] | IQG06Q06N02 | Bebaz / بيباز |  |
| As-Sulaymāniyah | SubDistrict | Kalār [Kelar] | كلار | Kalār [Kelar] | IQG06Q06N01 | Markaz Kalar / مركز كلار |  |
| As-Sulaymāniyah | SubDistrict | Rizkārī [Rizgarî] | رزكاري | Kalār [Kelar] | IQG06Q06N51 | Rizgari / رزكاري |  |
| As-Sulaymāniyah | SubDistrict | Shaīkh Ṭawīl [Şêx Tewîl] | شيخ طويل | Kalār [Kelar] | IQG06Q06N52 | Shekh Tewil / شيخ طويل |  |
| As-Sulaymāniyah | SubDistrict | Māwat [Mawet] | ماوت | Māwat [Mawet] | IQG06Q53N51 | Markaz Mawat / مركز ماوت |  |
| As-Sulaymāniyah | SubDistrict | Qara Dāgh [Qeredax] | قرة داغ | Qara Dāgh [Qeredax] | IQG06Q54N51 | Markaz Qaradagh / مركز قره داغ |  |
| As-Sulaymāniyah | SubDistrict | Sīah Sītān [Sîwsînan] | سيه سيتان | Qara Dāgh [Qeredax] | IQG06Q54N52 | Siwsinan / سيه سيتان |  |
| As-Sulaymāniyah | SubDistrict | Bītwātah [Bêtiwate] | بیتواتة | Rānyah [Raniye] | IQG06Q09N51 | Betiwate / بيتواتة |  |
| As-Sulaymāniyah | SubDistrict | Jwār Qūrnah [Çiwarqurne] | جوار قورنة | Rānyah [Raniye] | IQG06Q09N52 | Chawarqurna / جوار قورنة |  |
| As-Sulaymāniyah | SubDistrict | Rānyah [Raniye] | رانیة | Rānyah [Raniye] | IQG06Q09N02 | Markaz Rania / مركز رانية |  |
| As-Sulaymāniyah | SubDistrict | Sarkabkān [Serkepikan] | سرکبکان | Rānyah [Raniye] | IQG06Q09N53 | Serkepikan / سركبكان |  |
| As-Sulaymāniyah | SubDistrict | Saīd Ṣādiq [Seyd Sadiq] | سید صادق | Saīd Ṣādiq [Seyd Sadiq] | IQG06Q55N52 | Markaz Seyd Sadiq / مركز سيد صادق |  |
| As-Sulaymāniyah | SubDistrict | Sirūjik [Siruçik] | سروجک | Saīd Ṣādiq [Seyd Sadiq] | IQG06Q55N51 | Saruchik / سروجك |  |
| As-Sulaymāniyah | SubDistrict | Ḥalabjah Tāzah [New Halabja, Helebcey Taze] | حلبجة تازة | Shārah Zūr [Şarezûr] | IQG06Q56N51 | New Halabja / حلبجة تازة |  |
| As-Sulaymāniyah | SubDistrict | Wārmāwā [Warmawe] | وارماوا | Shārah Zūr [Şarezûr] | IQG06Q56N52 | Warmawe / وارماوا |  |
| As-Sulaymāniyah | SubDistrict | Juwārtah [Çwarte] | جوارتة | Shārbāzīr [Şarbajêr] | IQG06Q10N51 | Chwarte / جوارتة |  |
| As-Sulaymāniyah | SubDistrict | Kābīlūn [Gayîlon] | كابیلون | Shārbāzīr [Şarbajêr] | IQG06Q10N52 | Gayilon / كابيلون |  |
| As-Sulaymāniyah | SubDistrict | Sītak [Sîtek] | سیتك | Shārbāzīr [Şarbajêr] | IQG06Q10N53 | Sitek / سيتك |  |
| As-Sulaymāniyah | SubDistrict | Zah Lān [Zelan] | زة لان | Shārbāzīr [Şarbajêr] | IQG06Q10N54 | Zelan / زة لان |  |
| As-Sulaymāniyah | SubDistrict | Ḥājīāwah [Hacîawe] | حاجیاوة | Ḥājīāwah [Hacîawe] | IQG06Q52N51 | Markaz Haciawe / مركز حاجياوة |  |
| Baghdād | District | Abū Ghraīb | أبو غريب |  | IQG08Q51 | Abu Ghraib / أبو غريب |  |
| Baghdād | District | Al-Karkh | الكرخ |  | IQG08Q03 | Al-Karkh / الكرخ |  |
| Baghdād | District | Al-Kāẓimiyyah [Kadhimiya] | الكاظمیة |  | IQG08Q02 | Al-Kadhmiyah / الكاظمية |  |
| Baghdād | District | Al-Madā'in | المدائن |  | IQG08Q04 | Al-Mada'in / المدائن |  |
| Baghdād | District | Al-Maḥmūdiyah [Mahmoudiyah] | المحمودیة |  | IQG08Q05 | Al-Mahmoudiya / المحمودية |  |
| Baghdād | District | Al-Ā'ẓamiyah [Adhamiyah] | الاعظمیة |  | IQG08Q01 | Al-Adhamiya / الاعظمية |  |
| Baghdād | District | Ar-Ruṣāfah | الرصافة |  | IQG08Q06 | Al-Risafa / الرصافة |  |
| Baghdād | District | Ar-Rāshidiyah | الراشدیة |  | IQG08Q52 | Ar-Rashidiyah / الراشدية |  |
| Baghdād | District | Aṭ-Ṭārmiyah | الطارمية |  | IQG08Q55 | At-Tarmiyah / الطارمية |  |
| Baghdād | District | Faḍā' al-Kāẓimiyyah [At-Tājī] | فضاء الكاظمية |  | IQG08Q58 | Fada al-Kazimiyyah / فضاء الكاظمية |  |
| Baghdād | District | Samā al-Kāẓimiyyah [Dhāt as-Salāsil] | سما الكاظمية |  | IQG08Q59 | Sama al-Kazimiyyah / سما الكاظمية |  |
| Baghdād | SubDistrict | An-Naṣir & As-Salām | النصر والسلام | Abū Ghraīb | IQG08Q51N52 | An-Nasir & As-Salam / النصر والسلام |  |
| Baghdād | SubDistrict | Bawābat as-Samm | بوابة السم | Abū Ghraīb | IQG08Q51N53 | Bawabat as-Samm / بوابة السم |  |
| Baghdād | SubDistrict | Al-Karkh | الكرخ | Al-Karkh | IQG08Q03N02 | Markaz Al-Karkh / مركز الكرخ |  |
| Baghdād | SubDistrict | Al-M'āmūn | المأمون | Al-Karkh | IQG08Q03N01 | Al-Mamoon / المأمون |  |
| Baghdād | SubDistrict | Al-Manṣūr | المنصور | Al-Karkh | IQG08Q03N03 | Al-Mansour / المنصور |  |
| Baghdād | SubDistrict | Al-Kāẓimiyyah | الكاظمیة | Al-Kāẓimiyyah [Kadhimiya] | IQG08Q02N01 | Markaz Al-Kadhimiya / مركز الكاظمية |  |
| Baghdād | SubDistrict | Al-Jisr | الجسر | Al-Madā'in | IQG08Q04N02 | Al-Jisr / الجسر |  |
| Baghdād | SubDistrict | Al-Madā'in | المدائن | Al-Madā'in | IQG08Q04N01 | Markaz Al-Mada'in / مركز المدائن |  |
| Baghdād | SubDistrict | Al-Waḥdah | الوحدة | Al-Madā'in | IQG08Q04N03 | Al-Wihda / الوحدة |  |
| Baghdād | SubDistrict | Al-Laṭīfiyah | اللطیفیة | Al-Maḥmūdiyah [Mahmoudiyah] | IQG08Q05N03 | Al-Latifya / اللطيفية |  |
| Baghdād | SubDistrict | Al-Maḥmūdiyah | المحمودیة | Al-Maḥmūdiyah [Mahmoudiyah] | IQG08Q05N01 | Markaz Al-Mahmoudiya / مركز المحمودية |  |
| Baghdād | SubDistrict | Al-Yūsufiyah | الیوسفیة | Al-Maḥmūdiyah [Mahmoudiyah] | IQG08Q05N04 | Al-Yousifya / اليوسفية |  |
| Baghdād | SubDistrict | Ar-Rashīd | الرشید | Al-Maḥmūdiyah [Mahmoudiyah] | IQG08Q05N02 | Al-Rasheed / الرشيد |  |
| Baghdād | SubDistrict | Al-Ā'ẓamiyah | الاعظمیة | Al-Ā'ẓamiyah [Adhamiyah] | IQG08Q01N01 | Markaz Al-Adhamiya / مركز الاعظمية |  |
| Baghdād | SubDistrict | Al-Faḥāmah | الفحامة | Al-Ā'ẓamiyah [Adhamiyah] | IQG08Q01N02 | Al-Fahama / الفحامة |  |
| Baghdād | SubDistrict | Ar-Rabī' | الربيع | Al-Ā'ẓamiyah [Adhamiyah] | IQG08Q01N51 | Ar-Rabi / الربيع |  |
| Baghdād | SubDistrict | Al-Karādah ash-Sharqiyah | الكرادة الشرقیة | Ar-Ruṣāfah | IQG08Q06N01 | Al-Karrada Al-Sharqia / الكرادة الشرقية |  |
| Baghdād | SubDistrict | Ar-Ruṣāfah | الرصافة | Ar-Ruṣāfah | IQG08Q06N51 | Markaz Ar-Rusafah / مركز الرصافة |  |
| Baghdād | SubDistrict | Az-Za'frāniyah | الزعفرانية | Ar-Ruṣāfah | IQG08Q06N52 | Az-Zafraniyah / الزعفرانية |  |
| Baghdād | SubDistrict | Baghdād al-Jidīdah | بغداد الجدیدة | Ar-Ruṣāfah | IQG08Q06N02 | Baghdad Al-Jedeeda / بغداد الجديدة |  |
| Baghdād | SubDistrict | Falasṭīn | فلسطین | Ar-Ruṣāfah | IQG08Q06N53 | Felisteen / فلسطين |  |
| Baghdād | SubDistrict | Al-'Abāījī | العبایجي | Aṭ-Ṭārmiyah | IQG08Q55N52 | Al-Abaychi / العبايجي |  |
| Baghdād | SubDistrict | Al-Mushāhidah | المشاھدة | Aṭ-Ṭārmiyah | IQG08Q55N53 | Al-Meshahda / المشاهدة |  |
| Baghdād | SubDistrict | Aṭ-Ṭārmiyah | الطارمية | Aṭ-Ṭārmiyah | IQG08Q55N51 | Markaz Al-Tarmiya / مركز الطارمية |  |
| Baghdād | SubDistrict | Al-Ḥurriyah al-Baghdādiyah | الحرية البغدادية | Samā al-Kāẓimiyyah [Dhāt as-Salāsil] | IQG08Q59N51 | Al-Hurriyah al-Baghdadiyah / الحرية البغدادية |  |
| Baghdād | SubDistrict | Samā al-Kāẓimiyyah [Dhāt as-Salāsil] | سما الكاظمية | Samā al-Kāẓimiyyah [Dhāt as-Salāsil] | IQG08Q59N52 | Markaz Sama al-Kazimiyyah / مركز سما الكاظمية |  |
| Bābil [Babylon] | District | Al-Hāshmiyah | الھاشمیة |  | IQG07Q01 | Al-Hashimiya / الهاشمية |  |
| Bābil [Babylon] | District | Al-Kifl | الكفل |  | IQG07Q51 | Al-Kifl / الكفل |  |
| Bābil [Babylon] | District | Al-Maḥāwīl | المحاویل |  | IQG07Q03 | Al-Mahaweel / المحاويل |  |
| Bābil [Babylon] | District | Al-Musayab | المسیب |  | IQG07Q04 | Al-Mussyab / المسيب |  |
| Bābil [Babylon] | District | Al-Qāsim | القاسم |  | IQG07Q52 | Al-Qasim / القاسم |  |
| Bābil [Babylon] | District | Al-Ḥillah [Hillah] | الحلة |  | IQG07Q02 | Al-Hilla / الحلة |  |
| Bābil [Babylon] | District | Kūthā | كوثى |  | IQG07Q53 | Kutha / كوثى |  |
| Bābil [Babylon] | SubDistrict | Al-Hāshmiyah | الھاشمیة | Al-Hāshmiyah | IQG07Q01N51 | Markaz Al-Hashmiyah / مركز الهاشمية |  |
| Bābil [Babylon] | SubDistrict | Al-Madḥatiyah | المدحتية | Al-Hāshmiyah | IQG07Q01N03 | Al-Madhatiya / المدحتية |  |
| Bābil [Babylon] | SubDistrict | Ash-Shūmaly | الشوملي | Al-Hāshmiyah | IQG07Q01N02 | Al-Shomaly / الشوملي |  |
| Bābil [Babylon] | SubDistrict | Al-Kifl | الكفل | Al-Kifl | IQG07Q51N51 | Markaz Al-Kifil / مركز الكفل |  |
| Bābil [Babylon] | SubDistrict | Al-Imām | الامام | Al-Maḥāwīl | IQG07Q03N02 | Al-Emam / الامام |  |
| Bābil [Babylon] | SubDistrict | Al-Maḥāwīl | المحاویل | Al-Maḥāwīl | IQG07Q03N01 | Markaz Al-Mahawil / مركز المحاويل |  |
| Bābil [Babylon] | SubDistrict | An-Nīl | النیل | Al-Maḥāwīl | IQG07Q03N51 | An-Nil / النيل |  |
| Bābil [Babylon] | SubDistrict | Al-Iskandrīah | الاسكندریة | Al-Musayab | IQG07Q04N02 | Al-Iskandaria / الاسكندرية |  |
| Bābil [Babylon] | SubDistrict | Al-Musayab | المسیب | Al-Musayab | IQG07Q04N51 | Markaz Al-Musayab / مركز المسيب |  |
| Bābil [Babylon] | SubDistrict | Jurf aṣ-Ṣakhar | جرف الصخر | Al-Musayab | IQG07Q04N01 | Jurf Al-Sakhar / جرف الصخر |  |
| Bābil [Babylon] | SubDistrict | Saddat al-Hindiyah | سدة الھندیة | Al-Musayab | IQG07Q04N52 | Saddat al-Hindiyah / سدة الهندية |  |
| Bābil [Babylon] | SubDistrict | Sūrā' | سوراء | Al-Musayab | IQG07Q04N53 | Sura / سوراء |  |
| Bābil [Babylon] | SubDistrict | Al-Qāsim | القاسم | Al-Qāsim | IQG07Q52N51 | Markaz Al-Qasim / مركز القاسم |  |
| Bābil [Babylon] | SubDistrict | Aṭ-Ṭulay'ah | الطلیعة | Al-Qāsim | IQG07Q52N52 | At-Tulayah / الطليعة |  |
| Bābil [Babylon] | SubDistrict | Ibrāhīmīyyah | ابراهيمية | Al-Qāsim | IQG07Q52N53 | Ibrahimiyyah / ابراهيمية |  |
| Bābil [Babylon] | SubDistrict | Abī Gharaq | أبي غرق | Al-Ḥillah [Hillah] | IQG07Q02N02 | Abi Gharaq / ابي غرق |  |
| Bābil [Babylon] | SubDistrict | Al-Ḥillah [Hillah] | الحلة | Al-Ḥillah [Hillah] | IQG07Q02N03 | Markaz Al-Hilla / مركز الحلة |  |
| Bābil [Babylon] | SubDistrict | Al-Bāqr | الباقر | Kūthā | IQG07Q53N51 | Al-Baqr / الباقر |  |
| Bābil [Babylon] | SubDistrict | Kūthā | كوثى | Kūthā | IQG07Q53N52 | Markaz Kutha / مركز كوثى |  |
| Dahūk [Dohuk] | District | Al-'Amādiyah [Amêdî] | العمادية |  | IQG09Q01 | Al-Amadiya / العمادية |  |
| Dahūk [Dohuk] | District | Bātīfā [Batîfa] | باتیفا |  | IQG09Q51 | Batifa / باتيفا |  |
| Dahūk [Dohuk] | District | Dahūk [Dihok, Dohuk] | دهوك |  | IQG09Q02 | Duhok / دهوك |  |
| Dahūk [Dohuk] | District | Sumaīl [Sêmêl, Simele] | سمیل |  | IQG09Q03 | Sumail / سميل |  |
| Dahūk [Dohuk] | District | Zākhū [Zaxo] | زاخو |  | IQG09Q04 | Zakho / زاخو |  |
| Dahūk [Dohuk] | SubDistrict | Al-'Amādiyah [Amêdî] | العمادية | Al-'Amādiyah [Amêdî] | IQG09Q01N02 | Markaz Al-Amadiya / مركز العمادية |  |
| Dahūk [Dohuk] | SubDistrict | Bāmarnī [Bamernê] | بامرني | Al-'Amādiyah [Amêdî] | IQG09Q01N51 | Bamerne / بامرني |  |
| Dahūk [Dohuk] | SubDistrict | Dīrahlūk [Dêreluk] | دیَرةلوك | Al-'Amādiyah [Amêdî] | IQG09Q01N52 | Diraluk / ديرلوك |  |
| Dahūk [Dohuk] | SubDistrict | Jamānkī [Çemankê] | جامانكي | Al-'Amādiyah [Amêdî] | IQG09Q01N53 | Chamanki / جامانكي |  |
| Dahūk [Dohuk] | SubDistrict | Kānī Māsī [Kanî Masî] | كاني ماسي | Al-'Amādiyah [Amêdî] | IQG09Q01N54 | Kani Masi / كاني ماسي |  |
| Dahūk [Dohuk] | SubDistrict | Sarsink [Sersing] | سرسنك | Al-'Amādiyah [Amêdî] | IQG09Q01N04 | Sarsank / سرسنك |  |
| Dahūk [Dohuk] | SubDistrict | Shīladizā [Shêldze] | شێﻼدزێ | Al-'Amādiyah [Amêdî] | IQG09Q01N55 | Sheldze / شێﻼدزێ |  |
| Dahūk [Dohuk] | SubDistrict | Bātīfā [Batîfa] | باتیفا | Bātīfā [Batîfa] | IQG09Q51N51 | Markaz Batifa / مركز باتيفا |  |
| Dahūk [Dohuk] | SubDistrict | Biguwā [Bêgova] | بێگوڤا | Bātīfā [Batîfa] | IQG09Q51N52 | Begova / بێگوڤا |  |
| Dahūk [Dohuk] | SubDistrict | Dahūk [Dohuk, Dihok] | دهوك | Dahūk [Dihok, Dohuk] | IQG09Q02N02 | Markaz Duhok / مركز دهوك |  |
| Dahūk [Dohuk] | SubDistrict | Mānkīshki [Mangêş] | مانكیشك | Dahūk [Dihok, Dohuk] | IQG09Q02N51 | Mangesh / مانكيشك |  |
| Dahūk [Dohuk] | SubDistrict | Zāwītah [Zawîte] | زاویتة | Dahūk [Dihok, Dohuk] | IQG09Q02N03 | Zawita / زاويتة |  |
| Dahūk [Dohuk] | SubDistrict | Khānkā [Xankê] | خانكى | Sumaīl [Sêmêl, Simele] | IQG09Q03N51 | Khanke / خانكى |  |
| Dahūk [Dohuk] | SubDistrict | Sumaīl [Sêmêl] | سمیل | Sumaīl [Sêmêl, Simele] | IQG09Q03N03 | Markaz Sumail / مركز سميل |  |
| Dahūk [Dohuk] | SubDistrict | Bātīl [Batêl] | باتيل | Zākhū [Zaxo] | IQG09Q04N51 | Bateel / باتيل |  |
| Dahūk [Dohuk] | SubDistrict | Darkār [Drkar] | دركار | Zākhū [Zaxo] | IQG09Q04N03 | Dercar / دركار |  |
| Dahūk [Dohuk] | SubDistrict | Rizkāry [Rizgarî] | رزكاري | Zākhū [Zaxo] | IQG09Q04N52 | Rizgari / رزكاري |  |
| Dahūk [Dohuk] | SubDistrict | Zākhū [Zaxo] | زاخو | Zākhū [Zaxo] | IQG09Q04N01 | Markaz Zakho / مركز زاخو |  |
| Dhī Qār [Theqar] | District | Ad-Dawāyah | الدوایة |  | IQG17Q51 | Ad-Dawayah / الدواية |  |
| Dhī Qār [Theqar] | District | Al-Baṭḥā'a | البطحاء |  | IQG17Q52 | Al-Bathaa / البطحاء |  |
| Dhī Qār [Theqar] | District | Al-Fajr | الفجر |  | IQG17Q53 | Alfajr / الفجر |  |
| Dhī Qār [Theqar] | District | Al-Fuhūd | الفھود |  | IQG17Q54 | Al-Fuhud / الفهود |  |
| Dhī Qār [Theqar] | District | Al-Gharrāf | الغراف |  | IQG17Q55 | Al-Gharraf / الغراف |  |
| Dhī Qār [Theqar] | District | Al-Iṣlāḥ | الاصلاح |  | IQG17Q56 | Al-Islah / الاصلاح |  |
| Dhī Qār [Theqar] | District | Al-Jibāyish [Al-Chibayish] | الجبایش |  | IQG17Q01 | Al-Chibayish / الجبايش |  |
| Dhī Qār [Theqar] | District | An-Naṣr | النصر |  | IQG17Q57 | An-Nasr / النصر |  |
| Dhī Qār [Theqar] | District | An-Nāṣiriyah [Nasiriyah] | الناصرية |  | IQG17Q04 | Al-Nasiriya / الناصرية |  |
| Dhī Qār [Theqar] | District | Ar-Rifā'ī | الرفاعي |  | IQG17Q02 | Al-Rifai / الرفاعي |  |
| Dhī Qār [Theqar] | District | Ash-Shaṭrah | الشطرة |  | IQG17Q03 | Al-Shatra / الشطرة |  |
| Dhī Qār [Theqar] | District | Karmat Banī Sa'īd | كرمة بني سعید |  | IQG17Q58 | Karmat Bani Said / كرمة بني سعيد |  |
| Dhī Qār [Theqar] | District | Qal'at Sikar | قلعة سكر |  | IQG17Q59 | Qalat Sikar / قلعة سكر |  |
| Dhī Qār [Theqar] | District | Saīd Dakhīl | سید دخیل |  | IQG17Q60 | Said Dakhil / سيد دخيل |  |
| Dhī Qār [Theqar] | District | Sūq ash-Shuyūkh | سوق الشیوخ |  | IQG17Q05 | Suq Al-Shoyokh / سوق الشيوخ |  |
| Dhī Qār [Theqar] | SubDistrict | Ad-Dawāyah | الدوایة | Ad-Dawāyah | IQG17Q51N51 | Markaz Al-Dawaya / مركز الدواية |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Baṭḥā'a | البطحاء | Al-Baṭḥā'a | IQG17Q52N51 | Markaz Al-Battha'a / مركز البطحاء |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Fajr | الفجر | Al-Fajr | IQG17Q53N51 | Markaz Al-Fajer / مركز الفجر |  |
| Dhī Qār [Theqar] | SubDistrict | Mayslūn | ميسلون | Al-Fajr | IQG17Q53N52 | Mayslun / ميسلون |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Fuhūd | الفھود | Al-Fuhūd | IQG17Q54N51 | Markaz Al-Fuhood / مركز الفهود |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Bū Sa'd | البو سعد | Al-Gharrāf | IQG17Q55N52 | Al-Bu Sad / البو سعد |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Iṣlāḥ | الاصلاح | Al-Iṣlāḥ | IQG17Q56N51 | Markaz Al-Islah / مركز الاصلاح |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Ḥamār | الحمار | Al-Jibāyish [Al-Chibayish] | IQG17Q01N02 | Al-Hammar / الحمار |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Jibāyish | الجبایش | Al-Jibāyish [Al-Chibayish] | IQG17Q01N03 | Markaz Al-Chibayish / مركز الجبايش |  |
| Dhī Qār [Theqar] | SubDistrict | An-Naṣr | النصر | An-Naṣr | IQG17Q57N51 | Markaz Al-Nasr / مركز النصر |  |
| Dhī Qār [Theqar] | SubDistrict | An-Nāṣiriyah [Nasiriyah] | الناصرية | An-Nāṣiriyah [Nasiriyah] | IQG17Q04N02 | Markaz Al-Nasiriya / مركز الناصرية |  |
| Dhī Qār [Theqar] | SubDistrict | 'Aūr | أور | An-Nāṣiriyah [Nasiriyah] | IQG17Q04N51 | Aur / أور |  |
| Dhī Qār [Theqar] | SubDistrict | Ar-Rifā'ī | الرفاعي | Ar-Rifā'ī | IQG17Q02N03 | Markaz Al-Rifai / مركز الرفاعي |  |
| Dhī Qār [Theqar] | SubDistrict | Amārjī / Banī Zaīd | امارجي (بني زيد) | Ash-Shaṭrah | IQG17Q03N51 | Amarji / امارجي (بني زيد) |  |
| Dhī Qār [Theqar] | SubDistrict | Ash-Shaṭrah | الشطرة | Ash-Shaṭrah | IQG17Q03N01 | Markaz Al-Shatra / مركز الشطرة |  |
| Dhī Qār [Theqar] | SubDistrict | Aṭ-Ṭār | الطار | Karmat Banī Sa'īd | IQG17Q58N51 | Al-Tar / الطار |  |
| Dhī Qār [Theqar] | SubDistrict | Qal'at Sikar | قلعة سكر | Qal'at Sikar | IQG17Q59N51 | Markaz Qalat Siker / مركز قلعة السكر |  |
| Dhī Qār [Theqar] | SubDistrict | Saīd Dakhīl | سید دخیل | Saīd Dakhīl | IQG17Q60N51 | Markaz Said Dekhil / مركز سيد دخيل |  |
| Dhī Qār [Theqar] | SubDistrict | Al-Faḍliyah | الفضلیة | Sūq ash-Shuyūkh | IQG17Q05N03 | Al-Fadhliya / الفضلية |  |
| Dhī Qār [Theqar] | SubDistrict | Sūq ash-Shuyūkh | سوق الشیوخ | Sūq ash-Shuyūkh | IQG17Q05N51 | Markaz Suq ash-Shuyukh / مركز سوق الشيوخ |  |
| Dhī Qār [Theqar] | SubDistrict | 'Ukaīkah | عكیكة | Sūq ash-Shuyūkh | IQG17Q05N04 | Akaika / عكيكة |  |
| Diyālā | District | Al-Khāliṣ | الخالص |  | IQG10Q01 | Al-Khalis / الخالص |  |
| Diyālā | District | Al-Manṣūriyah | المنصوریة |  | IQG10Q51 | Al-Mansuriyah / المنصورية |  |
| Diyālā | District | Al-Muqdādiyah | المقدادیة |  | IQG10Q02 | Al-Muqdadiya / المقدادية |  |
| Diyālā | District | Ba'qūbah [Baquba] | بعقوبة |  | IQG10Q04 | Baquba / بعقوبة |  |
| Diyālā | District | Baladrūz | بلدروز |  | IQG10Q03 | Baladruz / بلدروز |  |
| Diyālā | District | Khānaqīn | خانقین |  | IQG10Q05 | Khanaqin / خانقين |  |
| Diyālā | District | Kifrī | كفري |  | IQG10Q06 | Kifri / كفري |  |
| Diyālā | District | Mandalī | مندلي |  | IQG10Q52 | Mandali / مندلي |  |
| Diyālā | SubDistrict | Al-Khāliṣ | الخالص | Al-Khāliṣ | IQG10Q01N04 | Markaz Al-Khalis / مركز الخالص |  |
| Diyālā | SubDistrict | As-Sad al-'Aẓīm | السد العظیم | Al-Khāliṣ | IQG10Q01N51 | As-Sad al-Azim / السد العظيم |  |
| Diyālā | SubDistrict | As-Salām | السلام | Al-Khāliṣ | IQG10Q01N52 | As-Salam / السلام |  |
| Diyālā | SubDistrict | Hibhib | ھبھب | Al-Khāliṣ | IQG10Q01N03 | Hibhib / هبهب |  |
| Diyālā | SubDistrict | Jadīdat ash-Shaṭ | جديدة الشط | Al-Khāliṣ | IQG10Q01N53 | Jadidat ash-Shat / جديدة الشط |  |
| Diyālā | SubDistrict | Jīzānī al-Jūl | جيزاني الجول | Al-Khāliṣ | IQG10Q01N54 | Jizani al-Jul / جيزاني الجول |  |
| Diyālā | SubDistrict | Al-Manṣūriyah | المنصوریة | Al-Manṣūriyah | IQG10Q51N51 | Markaz Al-Mansouriyah / مركز المنصورية |  |
| Diyālā | SubDistrict | 'Ābī Ṣaydā | أبي صیدا | Al-Muqdādiyah | IQG10Q02N51 | Abi Sayda / أبي صيدا |  |
| Diyālā | SubDistrict | Al-Muqdādiyah | المقدادیة | Al-Muqdādiyah | IQG10Q02N02 | Markaz Al-Muqdadiya / مركز المقدادية |  |
| Diyālā | SubDistrict | Al-Wajīhiyah | الوجیھیة | Al-Muqdādiyah | IQG10Q02N01 | Al-Wajihia / الوجيهية |  |
| Diyālā | SubDistrict | Al-'Abārah | العبارة | Ba'qūbah [Baquba] | IQG10Q04N51 | Al-Abarah / العبارة |  |
| Diyālā | SubDistrict | Banī Sa'd | بني سعد | Ba'qūbah [Baquba] | IQG10Q04N02 | Beni Saad / بني سعد |  |
| Diyālā | SubDistrict | Ba'qūbah [Baqubah] | بعقوبة | Ba'qūbah [Baquba] | IQG10Q04N52 | Markaz Baqubah / مركز بعقوبة |  |
| Diyālā | SubDistrict | Buhruz / Ashnūnā | بھرز | Ba'qūbah [Baquba] | IQG10Q04N01 | Buhriz / بهرز |  |
| Diyālā | SubDistrict | Kan'ān | كنعان | Ba'qūbah [Baquba] | IQG10Q04N03 | Kanan / كنعان |  |
| Diyālā | SubDistrict | Muḥammad Sakrān | محمد سكران | Ba'qūbah [Baquba] | IQG10Q04N53 | Muhammad Sakran / محمد سكران |  |
| Diyālā | SubDistrict | Baladrūz | بلدروز | Baladrūz | IQG10Q03N02 | Markaz Baladruz / مركز بلدروز |  |
| Diyālā | SubDistrict | As-S'adiyah | السعدیة | Khānaqīn | IQG10Q05N01 | Al-Saadiya / السعدية |  |
| Diyālā | SubDistrict | Jalawlā' | جلولاء | Khānaqīn | IQG10Q05N04 | Jalawla / جلولاء |  |
| Diyālā | SubDistrict | Khānaqīn | خانقین | Khānaqīn | IQG10Q05N02 | Markaz Khanaqin / مركز خانقين |  |
| Diyālā | SubDistrict | Jabārah | جبارة | Kifrī | IQG10Q06N01 | Jabarra / جبارة |  |
| Diyālā | SubDistrict | Qarah Tabah | قرة تبة | Kifrī | IQG10Q06N03 | Qara Tabe / قرة تبة |  |
| Diyālā | SubDistrict | Mandalī | مندلي | Mandalī | IQG10Q52N51 | Markaz Mandali / مركز مندلي |  |
| Diyālā | SubDistrict | Qazāniyah | قزانیة | Mandalī | IQG10Q52N52 | Qazanya / قزانية |  |
| Karbalā' [Kerbala] | District | Al-Hindiyah | الھندیة |  | IQG12Q02 | Al-Hindiya / الهندية |  |
| Karbalā' [Kerbala] | District | Al-Jadwal al-Gharbī | الجدول الغربي |  | IQG12Q53 | Al-Jadwal al-Gharbi / الجدول الغربي |  |
| Karbalā' [Kerbala] | District | Al-Ḥusayniyah | الحسینیة |  | IQG12Q52 | Al-Husayniyah / الحسينية |  |
| Karbalā' [Kerbala] | District | Karbalā' [Kerbala] | كربلاء |  | IQG12Q03 | Kerbela / كربلاء |  |
| Karbalā' [Kerbala] | SubDistrict | Al-Hindiyah | الھندیة | Al-Hindiyah | IQG12Q02N02 | Markaz Al-Hindiya / مركز الهندية |  |
| Karbalā' [Kerbala] | SubDistrict | Al-Khayrāt | الخیرات | Al-Hindiyah | IQG12Q02N51 | Nāḩiyat al Khayrāt / الخيرات |  |
| Karbalā' [Kerbala] | SubDistrict | Al-Jadwal al-Gharbī | الجدول الغربي | Al-Jadwal al-Gharbī | IQG12Q53N51 | Markaz Al-Jadwal Al-Ghrabi / مركز الجدول الغربي |  |
| Karbalā' [Kerbala] | SubDistrict | Al-Ḥusayniyah | الحسینیة | Al-Ḥusayniyah | IQG12Q52N51 | Markaz Al-Hassainya / مركز الحسينية |  |
| Karbalā' [Kerbala] | SubDistrict | Karbalā' [Kerbala] | كربلاء | Karbalā' [Kerbala] | IQG12Q03N01 | Markaz Kerbela / مركز كربلاء |  |
| Kirkūk | District | Ad-Dibs [Dibis] | الدبس |  | IQG13Q03 | Dibis / دبس |  |
| Kirkūk | District | Al-Ḥawījah [Hawija] | الحویجة |  | IQG13Q01 | Al-Hawiga / الحويجة |  |
| Kirkūk | District | Dāqūq | داقوق |  | IQG13Q02 | Daquq / داقوق |  |
| Kirkūk | District | Kirkūk | كركوك |  | IQG13Q04 | Kirkuk / كركوك |  |
| Kirkūk | SubDistrict | Ad-Dibs | الدبس | Ad-Dibs [Dibis] | IQG13Q03N02 | Markaz Dibis / مركز الدبس |  |
| Kirkūk | SubDistrict | Sarkarān | سركران | Ad-Dibs [Dibis] | IQG13Q03N01 | Sarkran / سركران |  |
| Kirkūk | SubDistrict | Al-Ḥawījah | الحویجة | Al-Ḥawījah [Hawija] | IQG13Q01N01 | Markaz Al-Hawiga / مركز الحويجة |  |
| Kirkūk | SubDistrict | Ar-Riyāḍ | الریاض | Al-Ḥawījah [Hawija] | IQG13Q01N03 | Al-Riyadh / الرياض |  |
| Kirkūk | SubDistrict | Az-Zāb | الزاب | Al-Ḥawījah [Hawija] | IQG13Q01N02 | Al-Zab / الزاب |  |
| Kirkūk | SubDistrict | Ar-Rashād | الرشاد | Dāqūq | IQG13Q02N01 | Al-Rashad / الرشاد |  |
| Kirkūk | SubDistrict | Dāqūq | داقوق | Dāqūq | IQG13Q02N02 | Markaz Daquq / مركز داقوق |  |
| Kirkūk | SubDistrict | Al-Multaqā | الملتقى | Kirkūk | IQG13Q04N03 | Al-Multaka / الملتقى |  |
| Kirkūk | SubDistrict | Altūn Kūbrī | التون كوبري | Kirkūk | IQG13Q04N51 | Altun Qupri / التون كوبري |  |
| Kirkūk | SubDistrict | Kirkūk [Kirkuk] | كركوك | Kirkūk | IQG13Q04N06 | Markaz Kirkuk / مركز كركوك |  |
| Kirkūk | SubDistrict | Laylān | لیلان | Kirkūk | IQG13Q04N02 | Lilan / ليلان |  |
| Kirkūk | SubDistrict | Qarah Hanjīr / Ar-Rabī' | قرة ھنجیر | Kirkūk | IQG13Q04N07 | Qura Hanjeer / قرة هنجير |  |
| Kirkūk | SubDistrict | Shwān | شوان | Kirkūk | IQG13Q04N05 | Shwan / شوان |  |
| Kirkūk | SubDistrict | Tāzah Khūrmātū | تازة خورماتو | Kirkūk | IQG13Q04N01 | Taza Khormato / تازة خورماتو |  |
| Kirkūk | SubDistrict | Yāyjī | یایجي | Kirkūk | IQG13Q04N04 | Yaijy / يايجي |  |
| Maysān | District | 'Alī al-Gharbī | علي الغربي |  | IQG14Q02 | Ali Al-Gharbi / علي الغربي |  |
| Maysān | District | Al-'Amārah [Amara] | العمارة |  | IQG14Q01 | Al-Amara / العمارة |  |
| Maysān | District | Al-Kaḥlā' | الكحلاء |  | IQG14Q03 | Al-Kahla / الكحلاء |  |
| Maysān | District | Al-Majar al-Kabīr | المجر الكبير |  | IQG14Q05 | Al-Mejar Al-Kabir / المجر الكبير |  |
| Maysān | District | Al-Maīmūnah | المیمونة |  | IQG14Q04 | Al-Maimouna / الميمونة |  |
| Maysān | District | Kumayt | كمیت |  | IQG14Q51 | Kumayt / كميت |  |
| Maysān | District | Qal'at Ṣāliḥ | قلعة صالح |  | IQG14Q06 | Qalat Saleh / قلعة صالح |  |
| Maysān | SubDistrict | 'Alī al-Gharbī | علي الغربي | 'Alī al-Gharbī | IQG14Q02N01 | Markaz Ali Al-Gharbi / مركز علي الغربي |  |
| Maysān | SubDistrict | 'Alī ash-Sharqī | علي الشرقي | 'Alī al-Gharbī | IQG14Q02N02 | Ali Al-Sharki / علي الشرقي |  |
| Maysān | SubDistrict | Al-'Amārah [Amara] | العمارة | Al-'Amārah [Amara] | IQG14Q01N51 | Markaz Al-Amarah / مركز العمارة |  |
| Maysān | SubDistrict | Al-Kaḥlā' | الكحلاء | Al-Kaḥlā' | IQG14Q03N03 | Markaz Al-Kahla / مركز الكحلاء |  |
| Maysān | SubDistrict | Al-Musharaḥ | المشرح | Al-Kaḥlā' | IQG14Q03N02 | Al-Msharah / المشرح |  |
| Maysān | SubDistrict | Banī Hāshim | بني ھاشم | Al-Kaḥlā' | IQG14Q03N01 | Beni Hasheem / بني هاشم |  |
| Maysān | SubDistrict | Al-'Adil | العدل | Al-Majar al-Kabīr | IQG14Q05N02 | Al-Adel / العدل |  |
| Maysān | SubDistrict | Al-Khayr | الخیر | Al-Majar al-Kabīr | IQG14Q05N51 | Al-Khayr / الخير |  |
| Maysān | SubDistrict | Al-Majar al-Kabīr | المجر الكبير | Al-Majar al-Kabīr | IQG14Q05N52 | Markaz Al-Majar al-Kabir / مركز المجر الكبير |  |
| Maysān | SubDistrict | Al-Maīmūnah | المیمونة | Al-Maīmūnah | IQG14Q04N02 | Markaz Al-Maimouna / مركز الميمونة |  |
| Maysān | SubDistrict | As-Saīd 'Āḥmad ar-Rifā'aī | السید أحمد الرفاعي | Al-Maīmūnah | IQG14Q04N51 | As-Said Ahmad ar-Rifaai / السيد أحمد الرفاعي |  |
| Maysān | SubDistrict | As-Salām | السلام | Al-Maīmūnah | IQG14Q04N01 | Al-Salam / السلام |  |
| Maysān | SubDistrict | Kumayt | كمیت | Kumayt | IQG14Q51N51 | Markaz Kumait / مركز كميت |  |
| Maysān | SubDistrict | Al-'Uzaīr | العزیر | Qal'at Ṣāliḥ | IQG14Q06N51 | Al-Aziz / العزير |  |
| Maysān | SubDistrict | Qal'at Ṣāliḥ / Madhar | قلعة صالح | Qal'at Ṣāliḥ | IQG14Q06N01 | Markaz Qalat Saleh / مركز قلعة صالح |  |
| Nīnawā [Nineveh] | District | Al-Ba'āj | البعاج |  | IQG15Q02 | Al-Baaj / البعاج |  |
| Nīnawā [Nineveh] | District | Al-Mawṣil [Mosul] | الموصل |  | IQG15Q05 | Al-Mosul / الموصل |  |
| Nīnawā [Nineveh] | District | Al-Ḥamdāniyah | الحمدانية |  | IQG15Q03 | Al-Hamdaniya / الحمدانية |  |
| Nīnawā [Nineveh] | District | Al-Ḥaḍra [Hatra] | الحضر |  | IQG15Q04 | Al-Hatra / الحضر |  |
| Nīnawā [Nineveh] | District | Ash-Shaīkhān [Shekhan] | الشیخان |  | IQG15Q06 | Al-Shikhan / الشيخان |  |
| Nīnawā [Nineveh] | District | Sinjār | سنجار |  | IQG15Q07 | Sinjar / سنجار |  |
| Nīnawā [Nineveh] | District | Tal'afar [Tel Afar] | تلعفر |  | IQG15Q08 | Telafar / تلعفر |  |
| Nīnawā [Nineveh] | District | Talkayf [Tel Keppe, Tel Kaif] | تلكیف |  | IQG15Q09 | Tilkaef / تلكيف |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Ba'āj | البعاج | Al-Ba'āj | IQG15Q02N01 | Markaz Al-Baaj / مركز البعاج |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Qaḥṭānyah | القحطانیة | Al-Ba'āj | IQG15Q02N02 | Al-Qahtanya / القحطانية |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Mawṣil [Mosul] | الموصل | Al-Mawṣil [Mosul] | IQG15Q05N51 | Markaz Al-Mawsil / مركز الموصل |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Muḥalabiyah | المحلبیة | Al-Mawṣil [Mosul] | IQG15Q05N52 | Al-Muhalabiyah / المحلبية |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Qayārah | القیارة | Al-Mawṣil [Mosul] | IQG15Q05N01 | Al-Qayarra / القيارة |  |
| Nīnawā [Nineveh] | SubDistrict | Ash-Shūrah | الشورة | Al-Mawṣil [Mosul] | IQG15Q05N53 | Ash-Shura / الشورة |  |
| Nīnawā [Nineveh] | SubDistrict | Ba'shīqah | بعشیقة | Al-Mawṣil [Mosul] | IQG15Q05N03 | Bashiqa / بعشيقة |  |
| Nīnawā [Nineveh] | SubDistrict | Ḥamām al-'Alīl | حمام العلیل | Al-Mawṣil [Mosul] | IQG15Q05N02 | Hammam Al-Alil / حمام العليل |  |
| Nīnawā [Nineveh] | SubDistrict | Baghdīdā | بغديدا | Al-Ḥamdāniyah | IQG15Q03N51 | Baghdida / بغديدا |  |
| Nīnawā [Nineveh] | SubDistrict | Barṭallah | برطلة | Al-Ḥamdāniyah | IQG15Q03N02 | Bartilla / برطلة |  |
| Nīnawā [Nineveh] | SubDistrict | Nimrūd | نمرود | Al-Ḥamdāniyah | IQG15Q03N01 | Namroud / نمرود |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Ḥaḍra | الحضر | Al-Ḥaḍra [Hatra] | IQG15Q04N01 | Markaz Hatra / مركز الحضر |  |
| Nīnawā [Nineveh] | SubDistrict | At-Tal | التل | Al-Ḥaḍra [Hatra] | IQG15Q04N51 | Al-Tal / التل |  |
| Nīnawā [Nineveh] | SubDistrict | Ash-Shīkhān | الشیخان | Ash-Shaīkhān [Shekhan] | IQG15Q06N01 | Markaz Al-Shikhan / مركز الشيخان |  |
| Nīnawā [Nineveh] | SubDistrict | Zaylakān | زیلكان | Ash-Shaīkhān [Shekhan] | IQG15Q06N51 | Zaylkan / زيلكان |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Qaīrawān | القیروان | Sinjār | IQG15Q07N01 | Al-Qairawan / القيروان |  |
| Nīnawā [Nineveh] | SubDistrict | Ash-Shamāl | الشمال | Sinjār | IQG15Q07N02 | Al-Shamal / الشمال |  |
| Nīnawā [Nineveh] | SubDistrict | Sinjār | سنجار | Sinjār | IQG15Q07N03 | Markaz Sinjar / مركز سنجار |  |
| Nīnawā [Nineveh] | SubDistrict | Al-'Ayāḍīyah | العیاضیة | Tal'afar [Tel Afar] | IQG15Q08N02 | Al-Ayadiya / العياضية |  |
| Nīnawā [Nineveh] | SubDistrict | Rabī'ah | ربیعة | Tal'afar [Tel Afar] | IQG15Q08N04 | Rabia / ربيعة |  |
| Nīnawā [Nineveh] | SubDistrict | Tal'afar | تلعفر | Tal'afar [Tel Afar] | IQG15Q08N01 | Markaz Telafar / مركز تلعفر |  |
| Nīnawā [Nineveh] | SubDistrict | Zumār | زمار | Tal'afar [Tel Afar] | IQG15Q08N03 | Zummar / زمار |  |
| Nīnawā [Nineveh] | SubDistrict | Al-Qūsh | القوش | Talkayf [Tel Keppe, Tel Kaif] | IQG15Q09N02 | Al-Qosh / القوش |  |
| Nīnawā [Nineveh] | SubDistrict | Talkayf [Tel Keppe] | تلكیف | Talkayf [Tel Keppe, Tel Kaif] | IQG15Q09N01 | Markaz Tilkaef / مركز تلكيف |  |
| Nīnawā [Nineveh] | SubDistrict | Wānah | وانة | Talkayf [Tel Keppe, Tel Kaif] | IQG15Q09N03 | Wana / وانة |  |
| Wasiṭ | District | Al-'Azīziyah | العزیزیة |  | IQG18Q52 | Al-Aziziyah / العزيزية |  |
| Wasiṭ | District | Al-Aḥrār | الاحرار |  | IQG18Q51 | Al-Ahrar / الاحرار |  |
| Wasiṭ | District | Al-Kūt [Kut] | الكوت |  | IQG18Q02 | Al-Kut / الكوت |  |
| Wasiṭ | District | Al-Mūwafaqiyah | الموفقیة |  | IQG18Q53 | Al-Muwafaqiyah / الموفقية |  |
| Wasiṭ | District | Al-Ḥay | الحي |  | IQG18Q01 | Al-Hai / الحي |  |
| Wasiṭ | District | An-Nu'māniyah | النعمانیة |  | IQG18Q03 | Al-Namaniya / النعمانية |  |
| Wasiṭ | District | Az-Zubaīdiyah | الزبیدیة |  | IQG18Q54 | Az-Zubaidiyah / الزبيدية |  |
| Wasiṭ | District | Aṣ-Ṣuwaīrah | الصویرة |  | IQG18Q04 | Al-Suwaira / الصويرة |  |
| Wasiṭ | District | Badrah | بدرة |  | IQG18Q05 | Badra / بدرة |  |
| Wasiṭ | SubDistrict | Ad-Dubūnī | الدبوني | Al-'Azīziyah | IQG18Q52N52 | Ad-Dubuni / الدبوني |  |
| Wasiṭ | SubDistrict | Al-'Azīziyah | العزیزیة | Al-'Azīziyah | IQG18Q52N51 | Markaz Al-Aziziya / مركز العزيزية |  |
| Wasiṭ | SubDistrict | Sayyid ash-Shuhadā' | سيد الشهداء | Al-'Azīziyah | IQG18Q52N53 | Sayyid ash-Shuhada / سيد الشهداء |  |
| Wasiṭ | SubDistrict | Tāj Aldīn / Al-Ḥafriyah | تاج الدین | Al-'Azīziyah | IQG18Q52N54 | Taj Aldin / تاج الدين |  |
| Wasiṭ | SubDistrict | Al-Aḥrār | الاحرار | Al-Aḥrār | IQG18Q51N51 | Markaz Al-Ahrar / مركز الاحرار |  |
| Wasiṭ | SubDistrict | Al-Kūt [Kut] | الكوت | Al-Kūt [Kut] | IQG18Q02N03 | Markaz Al-Kut / مركز الكوت |  |
| Wasiṭ | SubDistrict | Shiykh Sa'ad | شیخ سعد | Al-Kūt [Kut] | IQG18Q02N02 | Shaekh Saad / شيخ سعد |  |
| Wasiṭ | SubDistrict | Wāsiṭ | واسط | Al-Kūt [Kut] | IQG18Q02N01 | Wasit / واسط |  |
| Wasiṭ | SubDistrict | Wāsiṭ al-Qadīmah | واسط القديمة | Al-Kūt [Kut] | IQG18Q02N51 | Wasit al-Qadimah / واسط القديمة |  |
| Wasiṭ | SubDistrict | Al-Mūwafaqiyah | الموفقیة | Al-Mūwafaqiyah | IQG18Q53N51 | Markaz Al-Mowafaqiya / مركز الموفقية |  |
| Wasiṭ | SubDistrict | Al-Bashā'īr | البشائر | Al-Ḥay | IQG18Q01N01 | Al-Bashaer / البشائر |  |
| Wasiṭ | SubDistrict | Al-Ḥay | الحي | Al-Ḥay | IQG18Q01N51 | Markaz Al-Hay / مركز الحي |  |
| Wasiṭ | SubDistrict | An-Nu'māniyah | النعمانیة | An-Nu'māniyah | IQG18Q03N02 | Markaz Al-Noamaniya / مركز النعمانية |  |
| Wasiṭ | SubDistrict | Az-Zubaīdiyah | الزبیدیة | Az-Zubaīdiyah | IQG18Q54N51 | Markaz Al-Zubaidiya / مركز الزبيدية |  |
| Wasiṭ | SubDistrict | Ash-Shuḥaymiyah | الشحیمیة | Aṣ-Ṣuwaīrah | IQG18Q04N02 | Al-Shehamiya / الشحيمية |  |
| Wasiṭ | SubDistrict | Aṣ-Ṣuwaīrah | الصویرة | Aṣ-Ṣuwaīrah | IQG18Q04N03 | Markaz Al-Suwaira / مركز الصويرة |  |
| Wasiṭ | SubDistrict | Badrah | بدرة | Badrah | IQG18Q05N01 | Markaz Badra / مركز بدرة |  |
| Wasiṭ | SubDistrict | Jaṣān | جصان | Badrah | IQG18Q05N02 | Jassan / جصان |  |
| Wasiṭ | SubDistrict | Zurbāṭiyah | زرباطیھ | Badrah | IQG18Q05N51 | Zurbatiyah / زرباطيه |  |
| Ḥalabjah [Halabja] | District | Bīārā [Biyare] | بیارا |  | IQG19Q52 | Byara / بيارا |  |
| Ḥalabjah [Halabja] | District | Khūrmāl [Xurmal] | خورمال |  | IQG19Q53 | Khurmal / خورمال |  |
| Ḥalabjah [Halabja] | District | Sīrwān [Sîrwan] | سیروان |  | IQG19Q54 | Sirwan / سيروان |  |
| Ḥalabjah [Halabja] | District | Ḥalabjah [Helebce] | حلبجة |  | IQG19Q01 | Halabcha / حلبجة |  |
| Ḥalabjah [Halabja] | SubDistrict | Khūrmāl [Xurmal] | خورمال | Khūrmāl [Xurmal] | IQG19Q53N51 | Markaz Khourmal / مركز خورمال |  |
| Ḥalabjah [Halabja] | SubDistrict | Sīrwān [Sîrwan] | سیروان | Sīrwān [Sîrwan] | IQG19Q54N51 | Markaz Sirwan / مركز سيروان |  |
| Ḥalabjah [Halabja] | SubDistrict | Ḥalabjah [Helebce] | حلبجة | Ḥalabjah [Helebce] | IQG19Q01N03 | Markaz Halabja / مركز حلبجة |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Ad-Dawr | الدور |  | IQG16Q01 | Al-Daur / الدور |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Ad-Dujaīl | الدجیل |  | IQG16Q51 | Dujail / الدجيل |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Al-'Alam | العلم |  | IQG16Q53 | Al-Alam / العلم |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Ash-Shirqāṭ | الشرقاط |  | IQG16Q02 | Al-Shirqat / الشرقاط |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Aḍ-Ḍulū'īah | الضلوعیة |  | IQG16Q52 | Al-Dhuluiya / الضلوعية |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Balad | بلد |  | IQG16Q04 | Balad / بلد |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Baījī | بیجي |  | IQG16Q03 | Beygee / بيجي |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Sāmarrā' | سامراء |  | IQG16Q05 | Samarra / سامراء |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Tikrīt | تكريت |  | IQG16Q06 | Tikrit / تكريت |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Āmirlī | آمرلي |  | IQG16Q54 | Amirli / آمرلي |  |
| Ṣalāḥ ad-Dīn [Saladin] | District | Ṭūz Khūrmātū | طوز خورماتو |  | IQG16Q07 | Tooz Khurmato / طوز خورماتو |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Ad-Dawr | الدور | Ad-Dawr | IQG16Q01N51 | Markaz Ad-Dawr / مركز الدور |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Ad-Dujaīl | الدجیل | Ad-Dujaīl | IQG16Q51N51 | Markaz Al-Dujeel / مركز الدجيل |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Al-'Alam | العلم | Al-'Alam | IQG16Q53N51 | Markaz Al-Alam / مركز العلم |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Ash-Shirqāṭ | الشرقاط | Ash-Shirqāṭ | IQG16Q02N01 | Markaz Al-Shirqat / مركز الشرقاط |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Ash-Shūr | اشور | Ash-Shirqāṭ | IQG16Q02N51 | Ash-Shur / اشور |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | As-Sahl al-Akhḍar | السهل اخضر | Ash-Shirqāṭ | IQG16Q02N52 | As-Sahl al-Akhdar / السهل اخضر |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Talūl al-Bāj | تلول الباج | Ash-Shirqāṭ | IQG16Q02N53 | Tulool al-Baj / تلول الباج |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Aḍ-Ḍulū'īah | الضلوعیة | Aḍ-Ḍulū'īah | IQG16Q52N51 | Markaz Al-Duloeyah / مركز الضلوعية |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Al-Ḥātamiyah | الحاتمية | Balad | IQG16Q04N51 | Al-Hatamiyah / الحاتمية |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Al-'Isḥāqī | الإسحاقي | Balad | IQG16Q04N02 | Al-Esshaqi / الاسحاقي |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Balad | بلد | Balad | IQG16Q04N52 | Markaz Balad / مركز بلد |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Yathrib | یثرب | Balad | IQG16Q04N01 | Yathreb / يثرب |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Al-Ḥajjāj | الحجاج | Baījī | IQG16Q03N51 | Al-Hajjaj / الحجاج |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Aṣ-Ṣaynīah | الصینیة | Baījī | IQG16Q03N02 | Al-Synia / الصينية |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Az-Zawiyah | الزوية | Baījī | IQG16Q03N52 | Az-Zawiyah / الزوية |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Baījī | بیجي | Baījī | IQG16Q03N01 | Markaz Beygee / مركز بيجي |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Makḥūl | مكحول | Baījī | IQG16Q03N03 | Makkhoul / مكحول |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Al-Mu'taṣam | المعتصم | Sāmarrā' | IQG16Q05N01 | Al-Muotasim / المعتصم |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Ath-Tharthār | الثرثار | Sāmarrā' | IQG16Q05N02 | Al-Tharthar / الثرثار |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Dijlah | دجلة | Sāmarrā' | IQG16Q05N04 | Dijla / دجلة |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Sāmarrā' | سامراء | Sāmarrā' | IQG16Q05N03 | Markaz Samarra / مركز سامراء |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Tikrīt | تكريت | Tikrīt | IQG16Q06N51 | Markaz Tikrit / مركز تكريت |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Āmirlī | آمرلي | Āmirlī | IQG16Q54N51 | Markaz Ameraly / مركز آمرلي |  |
| Ṣalāḥ ad-Dīn [Saladin] | SubDistrict | Ṭūz Khūrmātū | طوز خورماتو | Ṭūz Khūrmātū | IQG16Q07N02 | Markaz Tooz Khurmato / مركز طوز خورماتو |  |

