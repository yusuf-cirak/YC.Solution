<!-- @format -->

# Release Process

Bu doküman, YC.Solution projelerinin otomatik sürüm yönetimi ve NuGet yayınlama sürecini açıklar.

## Genel Bakış

Proje, modern CI/CD pratiklerini kullanarak otomatik versiyon yönetimi ve paket yayınlama sağlar:

- **MinVer**: Git tag'lerinden otomatik semantic versioning
- **Conventional Commits**: Standardize commit mesajları ve otomatik changelog
- **Git Hooks (Husky)**: Local validasyon (commit öncesi format, build, test)
- **GitHub Actions**: Otomatik build, test ve NuGet yayınlama

## İlk Kurulum

Projeyi clone ettikten sonra Git hooks'ları kurmak için:

```bash
npm install
```

Detaylı bilgi için: [Git Hooks Setup](GIT_HOOKS_SETUP.md)

## Versiyon Yönetimi

### MinVer Nasıl Çalışır?

MinVer, Git tag'lerinizi kullanarak otomatik olarak paket versiyonunu belirler:

- Tag yoksa: `1.2.0-alpha.0.1` (MinVerMinimumMajorMinor + commit sayısı)
- Tag varsa: Tag'deki versiyon kullanılır (örn: `v1.2.1` → `1.2.1`)
- Tag'den sonra commit varsa: `1.2.1-alpha.0.5` (tag + commit sayısı)

### Paket Bazlı Tag'ler

Her paket için farklı tag prefix'leri kullanılır:

- **YC.Monad**: `v*.*.*` (örn: `v1.2.1`)
- **YC.Monad.EntityFrameworkCore**: `efcore-v*.*.*` (örn: `efcore-v1.0.1`)

## Yeni Versiyon Yayınlama

### 1. Conventional Commits Kullanın

Commit mesajlarınız şu formatı takip etmelidir:

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

**Type'lar:**

- `feat`: Yeni özellik (MINOR version artışı)
- `fix`: Bug düzeltmesi (PATCH version artışı)
- `docs`: Dokümantasyon değişiklikleri
- `style`: Kod formatı değişiklikleri
- `refactor`: Kod yeniden yapılandırma
- `perf`: Performans iyileştirmeleri
- `test`: Test ekleme/güncelleme
- `chore`: Bakım işleri
- `build`: Build sistemi değişiklikleri
- `ci`: CI/CD değişiklikleri

**Breaking Changes:**

```
feat!: breaking change açıklaması

BREAKING CHANGE: detaylı açıklama
```

**Örnekler:**

```bash
git commit -m "feat: add new Map method to Option type"
git commit -m "fix: resolve null reference in Result.Bind"
git commit -m "feat!: change Option.None to use struct instead of class"
git commit -m "docs: update README with new examples"
```

### 2. Tag Oluşturun ve Push Edin

#### YC.Monad için:

```bash
# Yeni versiyon tag'i oluştur
git tag v1.2.2

# Tag'i GitHub'a push et
git push origin v1.2.2
```

#### YC.Monad.EntityFrameworkCore için:

```bash
# Yeni versiyon tag'i oluştur
git tag efcore-v1.0.1

# Tag'i GitHub'a push et
git push origin efcore-v1.0.1
```

### 3. Otomatik Süreç

Tag push edildiğinde GitHub Actions otomatik olarak:

1. ✅ Projeyi build eder
2. ✅ MinVer ile versiyonu belirler
3. ✅ Commit geçmişinden changelog oluşturur
4. ✅ CHANGELOG.md dosyasını günceller
5. ✅ GitHub Release oluşturur
6. ✅ NuGet paketini yayınlar

## Gerekli Ayarlar

### GitHub Secrets

Repository Settings → Secrets and variables → Actions → New repository secret:

- **NUGET_API_KEY**: NuGet.org API anahtarınız
  - NuGet.org → Account → API Keys → Create
  - Scope: Push new packages and package versions
  - Glob Pattern: `YC.*`

### GitHub Permissions

Repository Settings → Actions → General → Workflow permissions:

- ✅ Read and write permissions

## Changelog Formatı

Otomatik oluşturulan changelog şu bölümleri içerir:

```markdown
## YC.Monad v1.2.2

### Release Date: 2024-04-12

### ⚠️ Breaking Changes

- feat!: change API structure (abc123)

### ✨ Features

- feat: add new Map method (def456)
- feat: improve error handling (ghi789)

### 🐛 Bug Fixes

- fix: resolve null reference (jkl012)

### 🔧 Other Changes

- docs: update README (mno345)
- chore: update dependencies (pqr678)
```

## Manuel Versiyon Kontrolü

Yerel olarak versiyon numarasını görmek için:

```bash
# MinVer CLI yükle (global)
dotnet tool install --global minver-cli

# YC.Monad versiyonu
cd src/YC.Monad
dotnet minver

# YC.Monad.EntityFrameworkCore versiyonu
cd src/YC.Monad.EntityFrameworkCore
dotnet minver -t efcore-v1.0.0
```

## Pull Request Validasyonu

Her PR için otomatik olarak:

1. ✅ Build kontrolü
2. ✅ Test çalıştırma
3. ✅ Commit mesajı formatı kontrolü

Commit mesajları Conventional Commits formatına uygun değilse PR fail olur.

## Semantic Versioning Kuralları

- **MAJOR** (1.x.x): Breaking changes (manuel tag ile)
- **MINOR** (x.1.x): Yeni özellikler (feat commits)
- **PATCH** (x.x.1): Bug fixes (fix commits)

## Örnek Workflow

```bash
# 1. Yeni özellik geliştir
git checkout -b feature/new-map-method

# 2. Değişiklikleri commit et (Conventional Commits ile)
git commit -m "feat: add Map method to Option type"
git commit -m "test: add tests for Map method"
git commit -m "docs: update README with Map examples"

# 3. PR oluştur ve merge et
# GitHub Actions otomatik olarak validate eder

# 4. Main branch'e geç
git checkout main
git pull

# 5. Yeni versiyon tag'i oluştur
git tag v1.3.0

# 6. Tag'i push et
git push origin v1.3.0

# 7. GitHub Actions otomatik olarak:
#    - Build yapar
#    - Changelog oluşturur
#    - GitHub Release oluşturur
#    - NuGet'e yayınlar
```

## Sorun Giderme

### Tag yanlış oluşturuldu

```bash
# Yerel tag'i sil
git tag -d v1.2.2

# Remote tag'i sil
git push origin :refs/tags/v1.2.2

# Doğru tag'i oluştur
git tag v1.2.2
git push origin v1.2.2
```

### NuGet yayınlama başarısız

1. NUGET_API_KEY secret'ının doğru olduğundan emin olun
2. API key'in push yetkisi olduğunu kontrol edin
3. Aynı versiyon numarası zaten yayınlanmış olabilir (MinVer farklı bir versiyon oluşturmalı)

### Changelog güncellenmiyor

1. Commit mesajlarının Conventional Commits formatına uygun olduğundan emin olun
2. GitHub Actions'ın write permission'ı olduğunu kontrol edin

## Kaynaklar

- [MinVer Documentation](https://github.com/adamralph/minver)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)
- [GitHub Actions](https://docs.github.com/en/actions)
