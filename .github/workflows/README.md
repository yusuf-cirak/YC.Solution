# GitHub Actions Workflows

## Workflows

### 1. publish-nuget.yml

Otomatik NuGet paket yayınlama workflow'u.

**Tetikleme:** Git tag push edildiğinde
- `v*.*.*` → YC.Monad paketi
- `efcore-v*.*.*` → YC.Monad.EntityFrameworkCore paketi

**İşlemler:**
1. Projeyi build eder
2. MinVer ile versiyon belirler
3. Commit geçmişinden changelog oluşturur
4. CHANGELOG.md dosyasını günceller
5. GitHub Release oluşturur
6. NuGet'e paket yayınlar

**Gerekli Secrets:**
- `NUGET_API_KEY`: NuGet.org API anahtarı

### 2. validate-pr.yml

Pull Request validasyon workflow'u.

**Tetikleme:** Pull Request oluşturulduğunda veya güncellendiğinde

**İşlemler:**
1. Projeyi build eder
2. Testleri çalıştırır
3. Commit mesajlarının Conventional Commits formatına uygunluğunu kontrol eder

## Setup

### GitHub Secrets Ekleme

1. Repository Settings → Secrets and variables → Actions
2. "New repository secret" tıklayın
3. Name: `NUGET_API_KEY`
4. Value: NuGet.org API anahtarınız
5. "Add secret" tıklayın

### NuGet API Key Oluşturma

1. [NuGet.org](https://www.nuget.org/) hesabınıza giriş yapın
2. Account → API Keys → Create
3. Key Name: `GitHub Actions - YC.Solution`
4. Scopes: Push new packages and package versions
5. Glob Pattern: `YC.*`
6. Create

### GitHub Permissions

Repository Settings → Actions → General → Workflow permissions:
- ✅ Read and write permissions
- ✅ Allow GitHub Actions to create and approve pull requests

## Troubleshooting

### Workflow çalışmıyor

1. Actions sekmesinde workflow'un enabled olduğundan emin olun
2. Repository permissions'ları kontrol edin
3. Secrets'ların doğru tanımlandığından emin olun

### NuGet publish başarısız

1. `NUGET_API_KEY` secret'ının doğru olduğundan emin olun
2. API key'in geçerli olduğunu kontrol edin
3. API key'in push yetkisi olduğunu doğrulayın
4. Aynı versiyon numarası zaten yayınlanmış olabilir

### Changelog güncellenmiyor

1. Commit mesajlarının Conventional Commits formatına uygun olduğundan emin olun
2. GitHub Actions'ın write permission'ı olduğunu kontrol edin
3. Main/master branch protection rules'ları kontrol edin
