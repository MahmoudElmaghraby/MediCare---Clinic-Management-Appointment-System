# إزاي تساهم في MediCare

الملف ده للفريق: محمود، أحمد، إيهاب، بيتر، سالم، وهادي. أغلبنا لسه في أول الطريق مع .NET، فالخطوات مكتوبة بالتفصيل عشان كلنا نشتغل بنفس الطريقة ومحدش يبوّظ فرع التاني.

## الفروع (Branches)

- `main`: النسخة المستقرة بس. ممنوع تعمل push مباشر عليها.
- `develop`: فرع التجميع. كل الشغل بيتدمج هنا الأول. ممنوع تعمل push مباشر عليها كمان.
- `feature/SCRUM-<id>-short-name`: فرع ميزة جديدة. اسم مشروع Jira هو `SCRUM` عشان الكوميت يتربط بالتذكرة.
- `fix/SCRUM-<id>-short-name`: فرع إصلاح مشكلة.

أمثلة:

```text
feature/SCRUM-11-unit-of-work
fix/SCRUM-24-appointment-overlap
```

## رسالة الكوميت

لازم الرسالة تبدأ بمفتاح التذكرة، وبعدين وصف قصير بالإنجليزي:

```text
SCRUM-11: add unit of work
```

## قواعد الدمج

- متعملش push مباشر على `main` أو `develop`.
- افتح Pull Request والـ base بتاعه `develop`.
- لازم زميل واحد على الأقل يراجع الـ PR قبل الدمج.
- قبل ما تطلب المراجعة، خلّي فرعك محدّث من `develop`.
- في آخر كل sprint أسبوعي، افتحوا Pull Request من `develop` إلى `main` وادمجوهم. كده `main` يفضل النسخة المستقرة بتاعة الأسبوع.

## خطوات Git من الأول للآخر

غيّر `SCRUM-11` و `unit-of-work` برقم التذكرة والاسم القصير بتوعك.

### 1) انسخ المستودع على جهازك

```bash
git clone https://github.com/MahmoudElmaghraby/MediCare---Clinic-Management-Appointment-System.git
cd MediCare---Clinic-Management-Appointment-System
```

### 2) هات آخر نسخة وانتقل لـ develop

فرع `develop` بيتعمل من `main` بعد دمج الـ skeleton. لو الأمر اللي تحت فشل لأن الفرع مش موجود، استنى لحد ما صاحب المستودع يعمله.

```bash
git fetch origin
git checkout develop
git pull origin develop
```

### 3) اعمل فرع الشغل

```bash
git checkout -b feature/SCRUM-11-unit-of-work
```

لو بتصلح باج:

```bash
git checkout -b fix/SCRUM-24-appointment-overlap
```

### 4) شوف التغييرات واعمل commit

```bash
git status
git add .
git commit -m "SCRUM-11: add unit of work"
```

لو عايز تضيف مجلد معين بس:

```bash
git add src/MediCare.Data/UnitOfWork
git commit -m "SCRUM-11: add unit of work"
```

### 5) ارفع الفرع على GitHub

أول مرة للفرع ده:

```bash
git push -u origin feature/SCRUM-11-unit-of-work
```

بعد كده:

```bash
git push
```

### 6) افتح Pull Request

1. افتح المستودع على GitHub.
2. اضغط **Compare & pull request**.
3. خلي **base** = `develop` و **compare** = فرعك.
4. املأ القالب: مفتاح Jira، إيه اللي اتغير، وإزاي اتجرب.
5. اطلب Review من زميل واحد على الأقل.
6. استنى الموافقة، وبعدين اعمل Merge.

### 7) حدّث فرعك من develop

اعمل الخطوة دي قبل ما تفتح الـ PR، وكمان لو حد دمج شغل جديد وانت لسه شغال:

```bash
git checkout feature/SCRUM-11-unit-of-work
git fetch origin
git merge origin/develop
git push
```

### 8) حل التعارض (conflicts)

لو Git كتب إن فيه conflict:

1. افتح الملفات اللي فيها العلامات `<<<<<<<` و `=======` و `>>>>>>>`.
2. سيّب الكود الصح وامسح العلامات دي.
3. كمّل بالأوامر دي:

```bash
git add .
git commit -m "SCRUM-11: merge develop into feature branch"
git push
```

لو التعارض كبير ومش متأكد من الكود الصح، اسأل زميل قبل الـ commit.

### 9) دمج الـ sprint في main

في نهاية الأسبوع، لما `develop` يبقى شغال و `dotnet test` ناجح:

1. افتح Pull Request من `develop` إلى `main`.
2. راجعوا التغييرات كفريق.
3. اعملوا Merge. مفيش push مباشر على `main`.

## قبل ما ترفع

```bash
dotnet build
dotnet test
```

الاتنين لازم ينجحوا. GitHub Actions بيعيد نفس الأوامر على كل Pull Request، وعلى أي push لـ `main` أو `develop`.

متعملش commit لمجلدات `bin/` و `obj/`، ولا لكلمات السر، ولا لبيانات مرضى.
