# دليل مشروع MediCare

الدليل ده مكتوب للفريق اللي عارف Flutter و SQL ولسه بيبدأ .NET. اقرأه من الأول للآخر قبل ما تمسك تذكرة على Jira. مثال التخصص `Specialization` موجود في الكود عشان تنسخه، مش عشان المشروع يخلص بيه.

## المشروع بيعمل إيه؟

MediCare نظام لإدارة عيادة وحجز المواعيد. فيه ثلاث أدوار.

### المريض (Patient)

- يعمل حساب ويدخل عليه.
- يدور على دكتور حسب التخصص.
- يحجز معاد، يعدله، أو يلغيه.
- يشوف مواعيده القديمة والجاية.
- يستقبل إيميل أو رسالة لما الحجز يتأكد.

### الدكتور (Doctor)

- يشوف جدول مواعيده.
- يأكد الميعاد أو يعلّمه إنه خلص.
- يشوف المرضى المرتبطين بمواعيده.

### الأدمن (Admin)

- يضيف الدكاترة والتخصصات ويدير حسابات المرضى.
- يدير الأدوار: مين مريض، مين دكتور، ومين أدمن.
- يشوف تقارير بسيطة عن المواعيد.

## إيه هو ASP.NET Core MVC؟

التطبيق متقسم لثلاث أجزاء، زي ما Flutter متقسم لـ model وواجهة، بس التنفيذ مختلف.

- **Model**: البيانات وقواعدها. في Flutter ده قريب من الـ class بتاعة البيانات، زي `Appointment` أو `Doctor`.
- **View**: صفحة Razor بترجع HTML. في Flutter أقرب حاجة للـ Widget، بس هنا الصفحة بتترسم على السيرفر والمتصفح بيعرضها. مش شجرة Widgets على الموبايل.
- **Controller**: يستقبل الطلب من المتصفح (فتح صفحة أو إرسال فورم)، ينادي الـ service، ويرجع الـ View أو يحوّل المستخدم لصفحة تانية. زي ما الـ Widget بينادي Provider أو Cubit وما بيحطش منطق الحجز جواه، الـ Controller ما بيحطش SQL جواه.

مثال سريع: المريض ضغط "احجز". المتصفح بعت الطلب لـ `AppointmentsController`. الـ controller استدعى `IAppointmentService`. النتيجة رجعت لصفحة بتأكد الحجز.

القالب الحالي لسه صفحة ترحيب. الحجز نفسه هيتبنى بنفس الطريق، والمثال الجاهز دلوقتي هو التخصص.

## تقسيم الطبقات (N Tier)

المشروع N Tier. كل طبقة ليها مشروع:

- `MediCare.Web`: الصفحات، الـ controllers، وملفات `wwwroot`. الطبقة دي بتنادي Services بس.
- `MediCare.Services`: قواعد الشغل. بتنادي Data بس. هنا بنقرر "الميعاد ده ينفع يتحجز ولا لأ".
- `MediCare.Data`: قاعدة البيانات، الـ repositories، والـ Unit of Work. هنا بس نكتب استعلامات EF Core.

القاعدة اللي ما نكسرهاش:

- الـ Web ما يتكلمش مع Data مباشرة. لو محتاج بيانات، نادّي service.
- الـ service ما يستخدمش `DbContext` بنفسه. يستخدم `IUnitOfWork`.
- الـ repository ما يعملش `SaveChanges`. الحفظ من الـ Unit of Work، عشان أكتر من تعديل يتحفظوا مرة واحدة.

`Program.cs` في الـ Web بيسجل الخدمات بالسطر ده:

```csharp
builder.Services.AddMediCareServices(builder.Configuration);
```

`AddMediCareServices` موجودة في `MediCare.Services`. هي اللي بتنادي `AddMediCareData` وتسجل الـ context. كده مشروع الـ Web مش محتاج مرجع على `MediCare.Data`، والقاعدة تفضل واضحة.

## EF Core والـ migrations

لو كتبت SQL قبل كده، EF Core هو طبقة فوق SQL Server. إنت بتكتب class، وهو يحوّلها لجدول واستعلامات.

- `ApplicationDbContext` هو الباب. فيه `DbSet<Specialization>`، وبعدين هنزود `DbSet` لكل كيان جديد.
- `SpecializationConfiguration` بتحدد اسم الجدول، طول الاسم، وإن الاسم ما يتكررش.
- الـ migration ملف C# بيوصف فرق الجداول. متعدلش في قاعدة البيانات بإيدك وتسيب الكود قديم. الفرق بيتسجل في migration عشان كل الفريق يطبق نفس الشكل.

أول migration اسمها `InitialCreate` وبتعمل جدول `Specializations`.

لما تضيف كيان أو تغيّر عمود، من جذر المستودع:

```bash
dotnet ef migrations add AddAppointment --project src/MediCare.Data --startup-project src/MediCare.Data
dotnet ef database update --project src/MediCare.Data --startup-project src/MediCare.Data
```

الأمر التاني بينشئ قاعدة `MediCare` على SQL Server المحلي ويطبق الـ migrations. محتاج تكون مثبت `dotnet-ef` إصدار 8.

سلسلة الاتصال المحلية (من غير كلمة سر) موجودة في `src/MediCare.Web/appsettings.json` تحت `ConnectionStrings:DefaultConnection`. لو هتستخدم كلمة سر، حطها في user secrets ومترفعهاش على Git.

صفحة البداية لسه مش بتسأل قاعدة البيانات، عشان المشروع يفتح حتى لو SQL Server مش شغال. أول ما نربط صفحة بالتخصصات أو المواعيد، لازم قاعدة البيانات تكون متحدثة.

## ASP.NET Identity والأدوار

ASP.NET Identity هتدير الحسابات وكلمات السر والدخول. الأدوار المطلوبة من الأول:

- `Patient`
- `Doctor`
- `Admin`

لسه مش متوصلة في الكود. حزمة Identity موجودة في `MediCare.Data` عشان المهمة الجاية. لما نبدأها، `ApplicationDbContext` هيورث من `IdentityDbContext`، والـ migration الجاية هتنشئ جداول المستخدمين. بعد كده كل controller هيسأل: المستخدم داخل، ودوره يسمح بالعملية دي؟

متخزنش كلمة السر في جدول تعمله بإيدك. Identity بتعمل الهاش والتحقق.

في Flutter كنت ممكن تعمل login وترجع token. هنا الـ cookie بتاعة Identity هي اللي بتقول مين المستخدم بعد تسجيل الدخول، والـ role بتحدد الصفحات المسموحة.

## الأنماط المستخدمة

### Repository

في Flutter مش بنحط SQL جوه الـ Widget. هنا مش بنحط استعلام EF جوه الـ Controller ولا جوه الـ service.

- `IRepository<T>` و `Repository<T>`: عمليات عامة. هات بالرقم، هات القائمة، أضف، عدل، امسح.
- `ISpecializationRepository` و `SpecializationRepository`: استعلام خاص بالجدول، زي `GetByNameAsync`.

لو الاستعلام ينفع لأي جدول، حطه في الـ generic repository. لو خاص بجدول واحد، حطه في الـ repository بتاع الجدول.

### Unit of Work

`IUnitOfWork` و `UnitOfWork` بيجمعوا الـ repositories. `SaveChangesAsync` بتبعت كل التعديلات لـ SQL Server في مرة واحدة.

لو حجزت معاد وعدلت خانة في جدول الدكتور، الاتنين يا يتحفظوا يا ولا واحد يتحفظ. متستدعيش `SaveChanges` من جوه الـ repository.

### Factory

`SpecializationFactory` بتبني الكائن بعد التحقق: الاسم مش فاضي، والمسافات الزيادة بتتشال. الـ service ما بتعملش `new Specialization` بنفسها.

في Flutter أحيانًا بتعمل function ترجع model جاهز قبل ما تبعته للـ API. نفس الفكرة: مكان واحد لقواعد إنشاء الكائن.

### Dependency Injection ومقارنتها بـ get_it و Provider

في Flutter، `get_it` بتسجل الأنواع مرة واحدة، و `Provider` بيوصل الكائن للـ Widget من غير ما الصفحة تعمل `new`.

هنا نفس الفكرة باسم Dependency Injection:

- التسجيل في `AddMediCareServices` و `AddMediCareData`.
- `IUnitOfWork` متسجل `Scoped`: نسخة واحدة لكل طلب HTTP.
- `SpecializationFactory` متسجلة `Singleton`: مفيش حالة جواها، فنسخة واحدة تكفي.
- `ISpecializationService` متسجل `Scoped` عشان يستخدم نفس الـ Unit of Work بتاع الطلب.

الـ class بتطلب الاعتمادات في الـ constructor. الإطار هو اللي يبني السلسلة. متكتبش `new UnitOfWork` جوه الـ service. الاختبارات بس هي اللي تبني السلسلة بإيدها عشان تستخدم قاعدة InMemory.

## التكاملات

الحاجات دي جزء من الخطة، ولسه مش متشغلة في الكود غير Bootstrap.

- **SignalR**: إشعار فوري للدكتور لما مريض يحجز، من غير تحديث للصفحة. مكان الـ hubs الفاضي: `src/MediCare.Web/Hubs`.
- **Twilio**: رسائل SMS لتأكيد الميعاد أو إلغائه.
- **SendGrid**: إيميل التأكيد. مفتاح الإرسال سر، وميرفعش على Git.
- **FullCalendar.js**: تقويم المواعيد في صفحة المريض والدكتور.
- **Chart.js**: رسوم للأدمن، زي عدد المواعيد في الأسبوع.
- **Bootstrap 5**: شكل الصفحات. موجود مع قالب MVC في `wwwroot/lib/bootstrap`.
- **Azure**: النشر في الآخر. الإعدادات السرية من إعدادات الخدمة هناك، مش من ملف متدفع على GitHub.

## المريض لما يحجز معاد، الطلب يعدي فين؟

المسار ده هو الحجز الكامل. الكود الحالي فيه مثال التخصص بنفس الطبقات، والحجز هيتنسخ منه. اقرأ المسار كأنه خريطة، وبعدين بص على ملفات `Specialization`.

1. المريض يفتح الموقع ويدخل بحساب دوره `Patient`.
2. يختار تخصص، وبعدين دكتور، وبعدين ميعاد. التقويم هيبقى FullCalendar.js. دلوقتي الصفحة الأساسية ترحيب بس.
3. المتصفح يبعت النموذج بطلب POST لـ `AppointmentsController` في `MediCare.Web`.
4. الـ controller يتأكد من Identity إن المستخدم داخل ودوره Patient. لو لأ، يرجعه لتسجيل الدخول أو صفحة منع.
5. الـ controller ما يفتحش قاعدة البيانات. ينادي `IAppointmentService.BookAsync` في `MediCare.Services` ويبعت رقم الدكتور والميعاد.
6. الـ service تستخدم factory عشان تبني كائن الميعاد، وتسأل `IUnitOfWork`: الدكتور موجود؟ الميعاد فاضي لنفس الدكتور؟ المريض معندوش حجز تاني في نفس الوقت؟
7. الـ Unit of Work يوصل للـ repositories في `MediCare.Data`. الاستعلامات تتنفذ بـ EF Core على `ApplicationDbContext`.
8. `ApplicationDbContext` يحوّل الاستعلام لـ SQL على SQL Server.
9. لو القواعد تمام، `SaveChangesAsync` تكتب صف الميعاد. لو قاعدة فشلت، مفيش حفظ جزئي.
10. الـ service ترجع نتيجة النجاح أو سبب الرفض. الـ controller يعرض View فيها التأكيد، أو يرجع الفورم برسالة الخطأ.
11. لما تذاكر التكامل تخلص: SignalR يبلغ الدكتور، SendGrid يبعت إيميل، و Twilio يبعت SMS. التلاتة دول بعد الحفظ، مش بداله.

مثال التخصص ماشي على نفس السلسلة باختصار: التسجيل في `Program.cs`، الخدمة `SpecializationService`، المصنع `SpecializationFactory`، الـ Unit of Work، الـ repository، وبعدين `ApplicationDbContext` وجدول `Specializations`.

## مين بيعمل إيه؟

- **محمود، أحمد، إيهاب**: الـ backend. الكيانات، الـ repositories، الـ services، Identity، وقواعد الحجز.
- **بيتر**: الـ views والواجهة. صفحات Razor، Bootstrap، FullCalendar.js، و Chart.js.
- **سالم**: الاختبارات في `tests/MediCare.Tests`. كل service جديدة ليها اختبار زي اختبارات التخصص.
- **هادي**: المستندات و Jira. الدليل ده، الـ ERD، دليل المستخدم، ومتابعة التذاكر.

لو تذكرتك بتعدي أكتر من جزء، اتفقوا مين هياخد الطبقة إيه قبل ما تشتغلوا على نفس الملفات.

## خطة الأربع أسابيع

### الأسبوع 1

نفهم الحل ونقرأ الدليل ده. نثبت Identity والأدوار الثلاثة. نرسم الـ ERD. نضيف الكيانات الأساسية (تخصص، دكتور، مريض، معاد) بنفس أسلوب `Specialization`، ونراجع الـ migration الأولى.

### الأسبوع 2

نكمل الـ repositories والـ services. نكتب قواعد الحجز: الميعاد ما يتكررش لنفس الدكتور، الدكتور متاح في الوقت ده، والمريض يقدر يلغي. سالم يبدأ اختبارات الخدمات مع الفريق.

### الأسبوع 3

بيتر يبني الصفحات: التقويم، نموذج الحجز، وجدول الدكتور والأدمن. نربط الـ controllers بالـ services من غير ما الصفحة توصل لـ Data. لو الحجز الأساسي شغال، نبدأ SignalR.

### الأسبوع 4

نكمل الاختبارات. الأدمن يشوف أرقام بـ Chart.js. نربط SendGrid و Twilio لو الحسابات جاهزة. هادي يراجع الدليل ودليل المستخدم. نكتب ملاحظات النشر على Azure. في آخر الأسبوع نفتح Pull Request من `develop` إلى `main`.

## إزاي تضيف ميزة جديدة؟

انسخ مثال التخصص. الترتيب ده يكفي لتذكرة backend، وبيتر يزود الصفحة في الآخر.

1. كيان يرث `BaseEntity` في `src/MediCare.Data/Entities`.
2. كلاس إعداد في `src/MediCare.Data/Configurations` يحدد الجدول والأعمدة.
3. `DbSet` جديد جوه `ApplicationDbContext`.
4. واجهة repository في `src/MediCare.Data/Interfaces` ترث `IRepository<T>` لو فيه استعلام خاص.
5. تنفيذها في `src/MediCare.Data/Repositories` ويرث `Repository<T>`.
6. خاصية جديدة في `IUnitOfWork` و `UnitOfWork`.
7. لو إنشاء الكائن له قواعد، حط factory في `src/MediCare.Services/Factories`.
8. واجهة في `src/MediCare.Services/Interfaces` وتنفيذ في `Implementations`. التنفيذ يستخدم `IUnitOfWork` فقط.
9. سجّل الخدمة في `AddMediCareServices`. `Program.cs` مش محتاج سطر جديد لو التسجيل جوه الدالة دي.
10. اعمل migration بالأمر اللي في قسم EF Core، وطبّقها على SQL Server المحلي.
11. أضف اختبار في `MediCare.Tests` على قاعدة InMemory، زي `SpecializationServiceTests`.
12. بيتر يضيف Controller و View. الـ controller يعتمد على واجهة الـ service، مش على `DbContext`.

الملفات بالترتيب اللي تنسخه:

- `src/MediCare.Data/Entities/BaseEntity.cs`
- `src/MediCare.Data/Entities/Specialization.cs`
- `src/MediCare.Data/Configurations/SpecializationConfiguration.cs`
- `src/MediCare.Data/Interfaces/IRepository.cs`
- `src/MediCare.Data/Repositories/Repository.cs`
- `src/MediCare.Data/Interfaces/ISpecializationRepository.cs`
- `src/MediCare.Data/Repositories/SpecializationRepository.cs`
- `src/MediCare.Data/Interfaces/IUnitOfWork.cs`
- `src/MediCare.Data/UnitOfWork/UnitOfWork.cs`
- `src/MediCare.Data/ApplicationDbContext.cs`
- `src/MediCare.Data/DependencyInjection.cs`
- `src/MediCare.Services/Factories/SpecializationFactory.cs`
- `src/MediCare.Services/Interfaces/ISpecializationService.cs`
- `src/MediCare.Services/Implementations/SpecializationService.cs`
- `src/MediCare.Services/DependencyInjection.cs`
- `src/MediCare.Web/Program.cs`
- `tests/MediCare.Tests/SpecializationServiceTests.cs`
