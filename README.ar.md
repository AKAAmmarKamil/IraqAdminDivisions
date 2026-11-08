<div dir="rtl">

# IraqAdminDivisions

[English](README.md)

كل محافظات العراق وأقضيته ونواحيه بالعربية والإنجليزية والكردية، مع رموز ثابتة، وبحث، وعنوان كامل بكل اللغات، وتحديد العنوان من موقع GPS دون إنترنت. حزمة NuGet واحدة، دون جداول مرجعية ودون أي اتصال بالشبكة.

## لماذا؟

نماذج العناوين في المشاريع العراقية تنتهي عادةً بجداول للمحافظات والأقضية والنواحي، ومفاتيح أجنبية، وسكربتات تعبئة، وعمود لكل لغة. هذه الحزمة تستبدل كل ذلك **بعمود نصي واحد**: خزّن رمز المنطقة، والمكتبة تعطيك أسماءها وما فوقها والعنوان الكامل بالعربية والإنجليزية والكردية.

- الرموز هي [رموز OCHA (P-codes)](https://data.humdata.org/dataset/cod-ab-irq) المستخدمة في بيانات الأمم المتحدة والمنظمات.
- الرموز هرمية، فـ"كل زبائن بغداد" مجرد `LIKE 'IQG08%'`.
- كل البيانات مدمجة داخل المكتبة: دون إنترنت، دون قاعدة بيانات، دون API.

## الحزم

| الحزمة | ماذا تضيف |
|---|---|
| `IraqAdminDivisions` | الأسماء، الرموز، الهرمية، البحث، العناوين، التحقق |
| `IraqAdminDivisions.Boundaries` | `Locate(lat, lng)`: نقطة GPS ← ناحية ← قضاء ← محافظة |
| `IraqAdminDivisions.AspNetCore` | `app.MapIraqDivisions()`: endpoints جاهزة للقوائم المنسدلة |

</div>

```bash
dotnet add package IraqAdminDivisions
dotnet add package IraqAdminDivisions.Boundaries   # اختياري: GPS ← عنوان
dotnet add package IraqAdminDivisions.AspNetCore   # اختياري: HTTP endpoints
```

<div dir="rtl">

تدعم `netstandard2.0` و`net8.0` (من .NET Framework 4.6.1 إلى أحدث .NET)، ومتوافقة مع Trimming وNative AOT على .NET 8 فما فوق.

## البداية السريعة

### ١. خزّن رمزاً واحداً

</div>

```csharp
public class Customer
{
    public int Id { get; set; }

    [IraqDivisionCode(AdminLevel.SubDistrict)]
    public string AreaCode { get; set; } = "";      // "IQG07Q02N02"

    public string Landmark { get; set; } = "";      // نص حر: أقرب نقطة دالة
}
```

<div dir="rtl">

### ٢. اعرض العنوان بلغة واحدة أو بالثلاث

</div>

```csharp
IraqDivisions.FormatAddress("IQG07Q02N02", Language.Arabic);   // ابي غرق، الحلة، بابل
IraqDivisions.FormatAddress("IQG07Q02N02", Language.English);  // Abi Gharaq, Al-Hilla, Babil

var address = IraqDivisions.GetAddress("IQG07Q02N02");
address.Ar;   // ابي غرق، الحلة، بابل
address.En;   // Abi Gharaq, Al-Hilla, Babil
address.Ku;   // ئەبی غەرەق، حیللە، بابل

var area = IraqDivisions.Find("IQG07Q02N02")!;
area.Name.Ar;               // ابي غرق
area.District!.Name.Ar;     // الحلة
area.Governorate.Name.Ar;   // بابل
```

<div dir="rtl">

### ٣. الفلترة حسب المحافظة أو القضاء في قاعدة بياناتك

رمز أي منطقة يبدأ دائماً برمز المنطقة الأعلى منها:

</div>

```
IQG08Q03N03   ناحية المنصور
IQG08Q03      قضاء الكرخ
IQG08         محافظة بغداد
```

```csharp
db.Customers.Where(c => c.AreaCode.StartsWith("IQG08"));      // كل بغداد
db.Customers.Where(c => c.AreaCode.StartsWith("IQG08Q03"));   // الكرخ فقط
// SQL: WHERE "AreaCode" LIKE 'IQG08%'   (يستفيد من الـ index)

var place = IraqDivisions.Search("بغداد")[0].Division;          // IQG08 (وكذلك "baghdad" و"بغدات")
db.Customers.Where(c => c.AreaCode.StartsWith(place.Code));
```

<div dir="rtl">

في PostgreSQL أعطِ العمود index من نوع `text_pattern_ops` (أو collation `C`) حتى يستخدمه `LIKE 'prefix%'`.

### ٤. محافظة ← منطقة فقط (قائمتان)

كثير من التطبيقات لا تريد قضاء وناحية. `Localities` قائمة واحدة مسطّحة بمناطق المحافظة (الأقضية والنواحي، دون تكرار "مركز القضاء"):

</div>

```csharp
IraqDivisions.Governorates;                       // القائمة الأولى
IraqDivisions.Find("IQG07")!.Localities;          // الثانية: الهاشمية، الشوملي، المدحتية، الحلة، ...
```

<div dir="rtl">

خزّن رمز المنطقة المختارة، و`FormatAddress` و`GetAddress` والفلترة بـ `StartsWith` تعمل كما هي.

### ٥. من GPS إلى عنوان (حزمة Boundaries)

</div>

```csharp
using IraqAdminDivisions.Boundaries;

var area = IraqBoundaries.Locate(33.3340, 44.4170);                       // الناحية، أو null خارج العراق
var governorate = IraqBoundaries.Locate(lat, lng, AdminLevel.Governorate);
IraqBoundaries.Contains("IQG08", lat, lng);                               // هل النقطة داخل بغداد؟
```

<div dir="rtl">

يفحص الحدود الحقيقية لكل منطقة (point-in-polygon)، وليس أقرب مركز. الوحدات التي أضافها تعداد 2024 ليس لها حدود مفتوحة بعد: إذا اقتُطع بعضها من الناحية التي يرجعها `Locate` فهي مذكورة في `CarvedOut` الخاصة بها، وقد تكون النقطة في إحداها.

### ٦. ASP.NET Core

</div>

```csharp
using IraqAdminDivisions.AspNetCore;

app.MapIraqDivisions();                      // البادئة الافتراضية /api/iraq-divisions
app.MapIraqDivisions("/api/divisions")       // أو بادئتك الخاصة
   .RequireAuthorization();
```

<div dir="rtl">

| Endpoint | يرجع |
|---|---|
| `GET /governorates` | كل المحافظات |
| `GET /{code}` | منطقة واحدة |
| `GET /{code}/children` | ما تحتها (للقوائم المتتالية) |
| `GET /{code}/localities` | قائمة مناطق المحافظة المسطّحة |
| `GET /search?q=&level=&limit=` | البحث مع الدرجة |
| `GET /nearest?lat=&lng=&level=` | أقرب مركز منطقة |

اللغة عبر `?lang=ar|en|ku` أو ترويسة `Accept-Language`. كل عنصر يحمل `name` و`names {ar,en,ku}` و`address` و`addresses {ar,en,ku}` و`kurdishNameSource`.

## الأداء

مقاس بـ BenchmarkDotNet على .NET 10. البيانات تُحمَّل مرة واحدة عند أول استخدام، وبأمان مع تعدد الخيوط.

| العملية | الوقت | الذاكرة المحجوزة |
|---|---|---|
| `Find` | 17 نانوثانية | 0 |
| `FormatAddress` / `GetAddress` | 17 نانوثانية | 0 |
| `Localities` | 6 نانوثانية | 0 |
| `Search` (عربي، إنجليزي، خطأ إملائي) | 14 ميكروثانية | ~250 بايت (قائمة النتائج) |
| أول تحميل للمناطق | 1.5 ملي ثانية | |
| `IraqBoundaries.Locate` (نقطة في بغداد / عشوائية) | 56 / 35 نانوثانية | 0 |
| `IraqBoundaries.Locate` (خارج العراق) | أقل من 1 نانوثانية | 0 |
| أول تحميل للحدود (قراءة + فهرس مكاني) | ~25 ملي ثانية | ~3 MB |

`Locate` يعطي نفس نتائج الفحص الكامل لكل الحدود بالضبط (تم التحقق على أكثر من 400,000 نقطة).

## البيانات والتغطية

| المستوى | العدد | الكردي من Wikidata | من بيانات مفتوحة | مكتوب آلياً | يدوي |
|---|---|---|---|---|---|
| محافظة | 19 | 18 | 0 | 0 | 1 |
| قضاء | 175 | 61 | 83 | 30 | 1 |
| ناحية | 442 | 60 | 64 | 317 | 1 |
| المجموع | 636 | 139 | 147 | 347 | 3 |

لكل منطقة اسم عربي وإنجليزي وكردي. الأقضية الـ 101 والنواحي الـ 294 من OCHA لها حدود (polygon)، أما الأقضية الـ 74 والنواحي الـ 148 المضافة من تعداد 2024 فليس لها حدود (انظر أدناه). التقارير الكاملة: [reports/build-report.md](reports/build-report.md) و[reports/kurdish-crosscheck.md](reports/kurdish-crosscheck.md) و[reports/census-reconciliation.md](reports/census-reconciliation.md).

### عن الأسماء الكردية

1. **Wikidata** أولاً، عندما يكون التطابق مؤكداً (نفس الاسم ونفس المنطقة الأعلى ومسافة قريبة).
2. **بيانات مفتوحة** بعدها. قارنّا ثلاث مجموعات بيانات عراقية مفتوحة بأسماء Wikidata الـ 96؛ واحدة فقط، [aryzonedev/iraq-cities](https://github.com/aryzonedev/iraq-cities) (CC BY 4.0)، تطابقت مع Wikidata بما يكفي (81% تحت مستوى المحافظة) لاستخدامها، وأسماؤها تملأ 102 صفاً من OCHA (`KurdishNameDataset` يذكر المصدر)، و45 قضاءً من التعداد رُقّي من إحدى تلك النواحي يحمل الاسم نفسه. هذا مصدر معايَر وليس دليلاً على مراجعة متحدث كردي. أما [karam-fikre/iraqi-data](https://github.com/karam-fikre/iraqi-data) فاستُخدم للمقارنة فقط لأن توثيقه نفسه يصف أسماءه الكردية بأنها مسودات غير مراجعة.
3. **مكتوبة آلياً**: **الـ 347 الباقية** مكتوبة وفق الإملاء السوراني (مثلاً ة ← ە، و"مركز X" ← "ناوەندی X"). صفوف OCHA الـ 214 كُتبت بمساعدة الذكاء الاصطناعي من الاسم العربي والإنجليزي، مع استخدام الاسم الكردي المحلي المعروف حيث يوجد (مثل التون كوبري ← پردێ). وصفوف التعداد الـ 133 كُتبت بقواعد ثابتة، من التهجئة الكردية اللاتينية التي تذكرها قائمة التعداد في إقليم كردستان، ومن العربي في غيره. **لم يراجع أياً منها متحدث كردي بعد**.

كل اسم يحمل مصدره، فتقرر أنت ماذا تعرض:

</div>

```csharp
var kurdish = area.KurdishNameSource == KurdishNameSource.Transliterated
    ? area.Name.Ar          // أو اعرضه مع تنبيه "مكتوب آلياً"
    : area.Name.Ku;
```

<div dir="rtl">

تصحيحات المتحدثين بالكردية هي أثمن مساهمة في هذا المشروع.

## كيف تُبنى البيانات

أداة `tools/IraqAdminDivisions.Builder` تنتج `data/divisions.json` و`data/boundaries.json`، وتُرفع إلى المستودع وتُدمج في الحزم.

1. **الأساس**: OCHA COD-AB Iraq v01: الرموز والأسماء العربية والإنجليزية والمراكز والمساحات والحدود.
2. **الكردي**: تسميات `ckb` من Wikidata بتطابق الاسم + المنطقة الأعلى + المسافة؛ يُرفض التطابق بالاسم فقط، وتُحذف بادئات النوع ("پارێزگای"، "قەزای")، ويُرفض أي نص ليس سورانياً خالصاً.
3. **التصحيحات**: [data/overrides/corrections.tsv](data/overrides/corrections.tsv) يصحح أخطاء المصدر مع سبب لكل تصحيح (مثلاً OCHA تسمي قضاء بلدروز باسم ناحية مندلي)، و[data/overrides/kurdish-transliterated.tsv](data/overrides/kurdish-transliterated.tsv) يكمل الأسماء الكردية، و[data/overrides/restructures.json](data/overrides/restructures.json) يطبق ما لم تلحقه OCHA بعد (حلبجة، وتعداد 2024). القيم المستبدلة تبقى قابلة للبحث كأسماء بديلة.
4. **تعداد 2024**: الوحدات التي يذكرها التعداد وليست في OCHA تُضاف برمز من المشروع (انظر أدناه). كل وحدة تحتاج مصدراً، وإحداثية Wikidata إن وُجدت يجب أن تقع داخل ناحية OCHA التي اقتُطعت منها.
5. **الحدود**: حدود OCHA مبسطة بخوارزمية Douglas-Peucker (دقة ~11 متر)، ومركز كل منطقة من OCHA يقع داخل حدودها.
6. **الفحص**: البناء يفشل عند خطأ في العدد، أو رمز مكرر أو غير هرمي، أو رمز مشروع أقل من 51، أو اسم ناقص بأي لغة، أو كردي غير سوراني، أو ناحية من OCHA دون حدود، أو وحدة من التعداد دون مصدر أو بإحداثية خارج ناحيتها الأصلية، أو قضاء دون نواحٍ.
7. **غير مستخدم**: OpenStreetMap (رخصة ODbL) وGADM (يمنع الاستخدام التجاري).

workflow شهري يعيد البناء ويفتح Pull Request إذا تغيرت المصادر.

## محافظة حلبجة

أصبحت حلبجة المحافظة التاسعة عشرة بقانون (صوّت عليه مجلس النواب في 2025-04-14 ونُشر في الوقائع العراقية في 2025-05-05). OCHA COD-AB v01 ما زالت تعدّها قضاءً تابعاً للسليمانية، لذلك تطبّق الحزمة التغيير فوق OCHA عبر [data/overrides/restructures.json](data/overrides/restructures.json):

| الرمز | ماذا |
|---|---|
| `IQG19` | محافظة حلبجة (رمز من هذا المشروع؛ لا يوجد رمز ISO 3166-2 مؤكد بعد) |
| `IQG19Q01` | قضاء حلبجة (OCHA `IQG06Q05`) وفيه مركز حلبجة وشهرزور |
| `IQG19Q52` و`IQG19Q53` و`IQG19Q54` | أقضية بيارة وخورمال وسيروان من تعداد 2024؛ ناحيتا بيارة وخورمال من OCHA صارتا مركزيهما (`IQG19Q52N51` و`IQG19Q53N51`) |

`OchaCode` يحتفظ برموز OCHA (`IQG06Q05…`) فتبقى الصفوف قابلة للربط مع بيانات الأمم المتحدة. قضاء بامو، الخامس في التعداد، لم يُضف بعد لأن أي مصدر مفتوح لا يحدد موقعه. رموز 1.0.1 `IQG06Q05…` حُذفت في 1.1.0 (انظر "ثبات الرموز").

## تعداد 2024

ينظّم تعداد 2024 (الجهاز المركزي للإحصاء، 2024-11-25) العراق في 19 محافظة ونحو 185 قضاءً و433 ناحية، مقابل 101 و294 في OCHA. النسخة 1.1.0 تضيف ما لا تذكره OCHA:

- **74 قضاءً و148 ناحية** برموز تبدأ من `51` داخل المنطقة الأعلى (`IQG11Q52` و`IQG11Q52N51`)، و`OchaCode = null` و`HasBoundary = false`. كل واحدة تذكر مصادرها (`sources`) في `divisions.json` والملفات المصدّرة: عنصر Wikidata عند التطابق (111 صفاً)، والعلامة `census-2024` لقائمة التعداد (111 صفاً ليس لها مصدر مفتوح آخر بعد).
- **71 ناحية من OCHA انتقلت إلى القضاء الذي يضعها فيه التعداد**، برمز جديد تحت ذلك القضاء، و`OchaCode` يحتفظ برمز OCHA. الناحية التي رُقّيت إلى قضاء تصبح مركزه (`مركز X` / `Markaz X` و`IsDistrictCentre`)، واسمها القديم يبقى قابلاً للبحث.
- **لا حدود للوحدات الجديدة.** لا يوجد مصدر مفتوح لحدودها، ولا نخترعها. `CarvedFrom` يذكر ناحية OCHA التي اقتُطعت منها كل وحدة: التي تقع فيها إحداثيتها من Wikidata، أو إحدى نواحي قضائها في التعداد عند غياب الإحداثية. `Locate` يرجع ناحية OCHA ويذكر الوحدات الجديدة في `CarvedOut`.
- **لم يُضف:** وحدات لا يحدد موقعها أي مصدر مفتوح (مثل الصدر الأولى والثانية والزهور والزوراء في بغداد، وخبات في أربيل، والحر في كربلاء، وبامو في حلبجة)، وقائمة إقليم كردستان لأقضية تضعها OCHA والقوائم الاتحادية في محافظة أخرى (عقرة والشيخان وبردرش ضمن دهوك؛ ومخمور وخانقين وكفري). هذه تبقى في محافظتها حسب OCHA. كل وحدة وسببها في [reports/census-reconciliation.md](reports/census-reconciliation.md).

من قائمة التعداد أُخذت الأسماء والتسلسل فقط؛ لا تُنشر أي أرقام سكانية ولا محتوى قاعدة بيانات لطرف ثالث.

## القيود

- **347 اسماً كردياً مكتوبة آلياً** و147 من مصدر مفتوح معايَر؛ لم يراجع أياً منها متحدث كردي (انظر أعلاه).
- **الوحدات المضافة من تعداد 2024 ليس لها حدود** (انظر أعلاه)، وحدود الأقضية والمحافظات في ملف GeoJSON هي حدود OCHA: حدود القضاء ما زالت تشمل نواحي نقلها التعداد إلى قضاء جديد. `Locate` لا يتأثر على مستوى القضاء والمحافظة لأنه يصعد من القضاء الحالي للناحية.
- **لا توجد أحياء (محلات)** بعد؛ احتفظ بنص حر لأقرب نقطة دالة بجانب الرمز.
- **التركيبة الداخلية لحلبجة** تتبع OCHA وليس القائمة الاتحادية (انظر أعلاه). حدود محافظة السليمانية في ملف GeoJSON ما زالت تشمل حلبجة؛ `Locate` لا يتأثر لأنه يعمل على مستوى النواحي.
- الإملاء العربي يتبع OCHA (مثل "ابي غرق" دون همزة)؛ البحث يتجاهل هذا الفرق.

## ثبات الرموز

من النسخة 1.1.0 فصاعداً، أي رمز يُنشر لا يُحذف ولا يُعاد استخدامه أبداً. إذا تغيرت منطقة يبقى رمزها القديم يعمل، ويُعلَّم `IsDeprecated` ويشير إلى الرمز الجديد عبر `ReplacedByCode`، فتحديث الحزمة لا يكسر البيانات المخزنة. اختبار snapshot يتحقق من كل رمز منشور، وفحص الحزمة (package validation) يمنع أي تغيير يكسر الـ API.

النسخة 1.1.0 هي الأساس: لم تُحمَّل 1.0.1 أي مرة، لذلك أُعيد ترقيم 76 من رموزها بدل إبقائها كصفوف قديمة (رموز حلبجة `IQG06Q05…` والنواحي الـ 71 التي نقلها التعداد). القائمة الكاملة في [reports/code-changes-1.1.0.md](reports/code-changes-1.1.0.md)، و`OchaCode` يحتفظ برمز OCHA لكل صف منقول.

- **رمز أي منطقة يبدأ دائماً برمز المنطقة الأعلى منها**، فـ `StartsWith` تعمل على كل المستويات.
- **الترقيم:** الأرقام `01`–`50` داخل أي منطقة محجوزة لترقيم OCHA. الأقضية والنواحي التي يضيفها هذا المشروع أو ينقلها تبدأ من `51` (`IQGxxQ51` و`…N51`) حتى يتميز رمز المشروع عن رمز OCHA. المحافظة `IQG19` (حلبجة) هي الرمز الوحيد الذي خصصه المشروع لمحافظة.

## الجودة

- الاختبارات تعمل على .NET 8 و.NET 10 و.NET Framework 4.8 (نسخة `netstandard2.0`).
- تغطية 100% من أسطر الحزمة الأساسية وحزمة ASP.NET Core؛ وحزمة الحدود تفحص مركز كل منطقة ونقاط مدن معروفة (منها حلبجة) ونقاطاً خارج العراق، وتثبت أن طريقها السريع يعطي نفس نتيجة الفحص الكامل.

## ملفات للغات الأخرى

| الملف | الصيغة |
|---|---|
| [data/divisions.json](data/divisions.json) | المصدر الأساسي، كل المستويات |
| [data/exports/divisions.csv](data/exports/divisions.csv) | CSV (UTF-8 مع BOM، يفتح في Excel) |
| [data/exports/divisions.sql](data/exports/divisions.sql) | SQL (PostgreSQL / عام) |
| [data/exports/divisions.sqlserver.sql](data/exports/divisions.sqlserver.sql) | SQL Server (`NVARCHAR` و`N'...'`) |
| [data/exports/divisions.geojson](data/exports/divisions.geojson) | GeoJSON مع الحدود، كل المستويات (وحدات OCHA فقط؛ الوحدات دون حدود غير موجودة فيه) |

## المساهمة

وجدت اسماً خاطئاً أو ناقصاً، خصوصاً كردياً؟ افتح [طلب تصحيح بيانات](https://github.com/AKAAmmarKamil/IraqAdminDivisions/issues/new?template=data-correction.yml) أو PR على ملفات `data/overrides/` مع المصدر. انظر [CONTRIBUTING.md](CONTRIBUTING.md).

## الإسناد

التقسيمات الإدارية والحدود: مكتب الأمم المتحدة لتنسيق الشؤون الإنسانية (OCHA)، Common Operational Dataset - Administrative Boundaries (COD-AB) Iraq، عبر Humanitarian Data Exchange، برخصة [CC BY-IGO 3.0](https://creativecommons.org/licenses/by/3.0/igo/). البيانات معدّلة (مصححة، ومعاد هيكلتها لحلبجة ولتعداد 2024، ومدمجة مع تسميات Wikidata وأسماء البيانات المفتوحة والأسماء المكتوبة آلياً، ومبسطة، ومعاد تصديرها). هذا ليس منتجاً رسمياً للأمم المتحدة أو OCHA. الوحدات المضافة من تعداد 2024 (الجهاز المركزي للإحصاء) تستخدم أسماء التعداد وتسلسله فقط. الأسماء الكردية: Wikidata (CC0)، و[aryzonedev/iraq-cities](https://github.com/aryzonedev/iraq-cities) (CC BY 4.0)، وهذا المشروع. انظر [DATA_LICENSE.md](DATA_LICENSE.md).

## الرخصة

الكود: [MIT](LICENSE). البيانات: CC BY-IGO 3.0 (OCHA) وCC0 (Wikidata) وCC BY 4.0 (aryzonedev/iraq-cities)، انظر [DATA_LICENSE.md](DATA_LICENSE.md).

</div>
