<!-- @format -->

# Git Hooks ile Local Validation

Bu proje, Git hooks kullanarak tüm validasyonları local'de yapar. Bu sayede hatalı kod GitHub'a push edilmeden önce yakalanır.

## 🎯 Kurulum

### 1. Node.js Yükleyin

Git hooks için Node.js gereklidir (Husky ve commitlint için).

```bash
# Node.js yüklü mü kontrol edin
node --version
npm --version
```

[Node.js indirin](https://nodejs.org/) (LTS versiyonu önerilir)

### 2. Bağımlılıkları Yükleyin

```bash
# Proje dizininde
npm install
```

Bu komut otomatik olarak:

- Husky'yi kurar
- Git hooks'ları `.husky/` klasörüne kurar
- commitlint'i yapılandırır

### 3. Doğrulama

```bash
# Git hooks kurulu mu kontrol edin
ls -la .git/hooks/

# Şunları görmelisiniz:
# commit-msg -> ../.husky/commit-msg
# pre-commit -> ../.husky/pre-commit
# pre-push -> ../.husky/pre-push
```

## 🔒 Kurulu Hooks

### **commit-msg** Hook

Her commit mesajını Conventional Commits formatına göre kontrol eder.

**Çalışma Zamanı:** `git commit` komutu çalıştırıldığında

**Kontroller:**

- ✅ Commit mesajı formatı: `type(scope): description`
- ✅ Geçerli type'lar: feat, fix, docs, style, refactor, perf, test, chore, build, ci, revert
- ✅ Maksimum başlık uzunluğu: 100 karakter
- ✅ Nokta ile bitmemeli

**Örnek:**

```bash
# ✅ Geçerli
git commit -m "feat: add new Map method"
git commit -m "fix: resolve null reference in Result.Bind"
git commit -m "feat(monad)!: breaking API change"

# ❌ Geçersiz
git commit -m "Added new feature"  # type yok
git commit -m "FEAT: new feature"  # büyük harf
git commit -m "feat: add feature." # nokta ile bitiyor
```

### **pre-commit** Hook

Kod kalitesi kontrollerini yapar.

**Çalışma Zamanı:** `git commit` komutu çalıştırıldığında (mesaj kontrolünden önce)

**Kontroller:**

1. ✅ **Code Formatting**: `dotnet format --verify-no-changes`
2. ✅ **Build**: `dotnet build --configuration Release`
3. ✅ **Tests**: `dotnet test --configuration Release`

**Özellikler:**

- Sadece staged `.cs` dosyalarını kontrol eder
- Hızlı çalışır (incremental build)
- Hata durumunda commit engellenir

**Bypass (acil durumlar için):**

```bash
# Sadece acil durumlarda kullanın!
git commit --no-verify -m "fix: emergency hotfix"
```

### **pre-push** Hook

Push öncesi tam validasyon yapar.

**Çalışma Zamanı:** `git push` komutu çalıştırıldığında

**Kontroller:**

1. ✅ **Full Build**: Tüm solution Release modda
2. ✅ **All Tests**: Tüm testler detaylı çıktı ile

**Özellikler:**

- Daha kapsamlı kontrol (pre-commit'ten daha yavaş)
- Push öncesi son güvenlik ağı
- CI/CD'de çalışacak testlerin local'de de çalıştığını garanti eder

## 📝 Kullanım Örnekleri

### Normal Workflow

```bash
# 1. Değişiklik yap
# ... kod yazın ...

# 2. Stage edin
git add .

# 3. Commit edin (pre-commit hook çalışır)
git commit -m "feat: add new feature"
# 🔍 Running pre-commit checks...
# 📝 Found staged C# files, running format check...
# ✅ Code formatting check passed
# 🔨 Building solution...
# ✅ Build successful
# 🧪 Running tests...
# ✅ All tests passed
# ✨ Pre-commit checks completed successfully!
# [commit-msg hook çalışır]
# ✅ Commit message validated

# 4. Push edin (pre-push hook çalışır)
git push
# 🚀 Running pre-push checks...
# 🔨 Building solution (Release)...
# ✅ Build successful
# 🧪 Running all tests...
# ✅ All tests passed
# 🎉 Ready to push!
```

### Commitizen ile Interactive Commit

Daha kolay commit mesajı yazmak için:

```bash
# Commitizen kullanarak commit
npm run commit

# Veya
npx git-cz

# Interactive olarak:
# ? Select the type of change: (Use arrow keys)
# ❯ feat:     A new feature
#   fix:      A bug fix
#   docs:     Documentation only changes
#   style:    Changes that don't affect code meaning
#   ...
```

### Format Düzeltme

Pre-commit hook format hatası verirse:

```bash
# Otomatik format düzelt
dotnet format

# Tekrar commit dene
git commit -m "feat: add new feature"
```

## 🛠️ Sorun Giderme

### Hook çalışmıyor

```bash
# Hooks'ları yeniden kur
npm install
npx husky install

# Hook dosyalarının executable olduğundan emin olun (Linux/Mac)
chmod +x .husky/commit-msg
chmod +x .husky/pre-commit
chmod +x .husky/pre-push
```

### Windows'ta hook çalışmıyor

```bash
# Git Bash kullanın (PowerShell yerine)
# Veya WSL kullanın

# Alternatif: core.hooksPath ayarlayın
git config core.hooksPath .husky
```

### Commit mesajı reddediliyor

```bash
# Commitlint kurallarını kontrol edin
npx commitlint --help-url

# Son commit mesajını test edin
echo "feat: test message" | npx commitlint
```

### Pre-commit çok yavaş

```bash
# Sadece değişen dosyaları test etmek için
# .husky/pre-commit dosyasını düzenleyin
# veya geçici olarak bypass edin:
git commit --no-verify -m "feat: quick fix"
```

## ⚙️ Konfigürasyon

### commitlint Kuralları

Kurallar `package.json` dosyasının `commitlint` bölümünde tanımlıdır.

Kuralları özelleştirmek için `package.json` içindeki `commitlint` bölümünü düzenleyin.

### Hook Davranışını Değiştirme

Hook dosyalarını düzenleyin:

- `@d:\Yazılım\csharp\repos\YC.Solution\.husky\commit-msg:1`
- `@d:\Yazılım\csharp\repos\YC.Solution\.husky\pre-commit:1`
- `@d:\Yazılım\csharp\repos\YC.Solution\.husky\pre-push:1`

### Hooks'ları Devre Dışı Bırakma

```bash
# Geçici olarak (önerilmez)
git commit --no-verify

# Kalıcı olarak (önerilmez)
git config core.hooksPath /dev/null

# Tekrar aktif et
git config --unset core.hooksPath
```

## 🎓 Best Practices

### ✅ Yapılması Gerekenler

- Her commit'te conventional commits formatı kullanın
- Commit öncesi `dotnet format` çalıştırın
- Testleri local'de çalıştırıp geçtiğinden emin olun
- Küçük, atomik commit'ler yapın
- Commitizen kullanarak interactive commit yapın

### ❌ Yapılmaması Gerekenler

- `--no-verify` bayrağını rutin olarak kullanmayın
- Hooks'ları devre dışı bırakmayın
- Büyük, monolitik commit'ler yapmayın
- Commit mesajlarında Türkçe karakter kullanmayın (ASCII only)
- Breaking change'leri belirtmeden yapmayın

## 📊 Hook Performansı

| Hook       | Ortalama Süre | Ne Zaman Çalışır          |
| ---------- | ------------- | ------------------------- |
| commit-msg | < 1 saniye    | Her commit                |
| pre-commit | 5-30 saniye   | Her commit (staged files) |
| pre-push   | 30-60 saniye  | Her push (full build)     |

## 🔗 Kaynaklar

- [Husky Documentation](https://typicode.github.io/husky/)
- [Commitlint](https://commitlint.js.org/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Commitizen](https://github.com/commitizen/cz-cli)

## 🆘 Yardım

Sorun yaşıyorsanız:

1. `npm install` komutunu tekrar çalıştırın
2. `.husky/` klasörünün var olduğundan emin olun
3. Git version'ınızı kontrol edin: `git --version` (2.9+ gerekli)
4. Node.js version'ınızı kontrol edin: `node --version` (16+ önerilir)
