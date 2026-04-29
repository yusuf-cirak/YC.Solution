# Merkezi Paket Yönetimi (Central Package Management)

## 📋 Yapılan İşlemler

Analyzer paketlerini ve diğer bağımlılıkları merkezi yönetim yapısına taşıdım.

### ✅ Eklenen/Güncelenen Dosyalar

#### 1. **`Directory.Packages.props`** (YENİ - Proje Kökü)
Tüm paket versiyonlarının merkezi yönetimi:

```xml
<ItemGroup>
  <PackageVersion Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="8.0.1" />
  <PackageVersion Include="StyleCop.Analyzers" Version="1.2.0-beta.556" />
  <PackageVersion Include="Roslynator.Analyzers" Version="4.12.0" />
  <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
  <PackageVersion Include="xunit" Version="2.6.6" />
  <!-- ... etc ... -->
</ItemGroup>
```

**Avantajları:**
- ✅ Tek yerden tüm paket versiyonlarını yönet
- ✅ Version güncelleme işini kolaylaştır
- ✅ Tüm projelerde aynı paket versiyonlarını garantile
- ✅ NuGet restore'ı daha verimli hale getir

#### 2. **`Directory.Build.props`** (GÜNCELLENDI)
Analyzer paketlerinin özniteliklerini merkezi yönetim:

```xml
<ItemGroup>
  <PackageReference Update="Microsoft.CodeAnalysis.NetAnalyzers">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
  <!-- ... etc ... -->
</ItemGroup>
```

**Bu nedir:**
- `Update` → Paket referanslarını güncellemek (merkezi config)
- `PrivateAssets=all` → Paket bağımlılık yönetimine dahil olmaz
- `IncludeAssets` → Hangi assetlerin hedef projede kullanılacağı

#### 3. **Proje Dosyaları** (GÜNCELLENDI)

Aşağıdaki dosyalar basitleştirildi:

```
✓ src/YC.Monad/YC.Monad.csproj
✓ src/YC.Monad.EntityFrameworkCore/YC.Monad.EntityFrameworkCore.csproj
✓ test/YC.Monad/YC.Monad.UnitTests/YC.Monad.UnitTests.csproj
✓ test/YC.Monad.EntityFrameworkCore.UnitTests/YC.Monad.EntityFrameworkCore.UnitTests.csproj
```

**ÖLUMU:**
```xml
<!-- ESKI approach (her projede tekrar yazılıyor) -->
<PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="8.0.1">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>

<!-- YENİ approach (merkezi yönetim) -->
<PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" />
```

---

## 🎯 Merkezi Yönetim Avantajları

### 1. **Tek Yerden Versiyonlama**
```
Directory.Packages.props dosyasında tüm versiyonlar tanımlanır
↓
Tüm projeler bu versiyonları otomatik kullanır
↓
Version güncellemesi kolay ve merkezi
```

### 2. **DRY Prensibi (Don't Repeat Yourself)**
- Paket tanımları 1 kez yazılır
- 4 proje dosyasında tekrar yazılmaz
- Bakım ve update işi kolaylaşır

### 3. **Tutarlılık**
- Tüm projeler aynı paket versiyonlarını kullanır
- Version "drift" sorunu ortadan kalkır
- Dependency confusion riski azalır

### 4. **Skalabilite**
- 10 proje olsa da, 100 proje olsa da merkezi
- Yeni proje eklemesi çok kolay
- Global güncellemeler tek noktadan yapılır

---

## 📦 Merkezi Yönetimde Tanımlanan Paketler

### Code Analysis
- `Microsoft.CodeAnalysis.NetAnalyzers` v8.0.1
- `StyleCop.Analyzers` v1.2.0-beta.556
- `Roslynator.Analyzers` v4.12.0

### Testing
- `Microsoft.NET.Test.Sdk` v17.8.0
- `xunit` v2.6.6
- `xunit.runner.visualstudio` v2.5.6
- `coverlet.collector` v6.0.0

### Database & ORM
- `Microsoft.EntityFrameworkCore` v6.0.36
- `Microsoft.EntityFrameworkCore.InMemory` v6.0.36

### Internal Dependencies
- `YC.Monad` v1.1.0

---

## 🔄 Proje Yapısı Hiyerarşisi

```
Directory.Packages.props (Paket versiyonları)
           ↓
Directory.Build.props (Ortak build ayarları + paket özellikleri)
           ↓
src/YC.Monad/YC.Monad.csproj (Sadece paket adı tanımlanır)
src/YC.Monad.EntityFrameworkCore/YC.Monad.EntityFrameworkCore.csproj
test/YC.Monad/YC.Monad.UnitTests/YC.Monad.UnitTests.csproj
test/YC.Monad.EntityFrameworkCore.UnitTests/YC.Monad.EntityFrameworkCore.UnitTests.csproj
```

---

## ✅ Test Sonuçları

```
Build Status: ✅ SUCCESS
Restore: ✅ SUCCESS (8 warnings - version resolution)
Compilation: ✅ SUCCESS
Analyzer Detection: ✅ SUCCESS (331 analyzer warnings)
Package Management: ✅ WORKING CORRECTLY
```

**Uyarılar açıklaması:**
- Package source mapping uyarısı (production'da configure edilebilir)
- NetAnalyzers version resolution (9.0.0 ← 8.0.1, compatible)
- Analyzer warnings (code quality - expected)

---

## 🚀 Version Güncelleme Örneği

**Eski yöntem (tedious):**
```
ADIM 1: YC.Monad.csproj → Version="8.0.2" değiştir
ADIM 2: YC.Monad.EntityFrameworkCore.csproj → Version="8.0.2" değiştir
ADIM 3: YC.Monad.UnitTests.csproj → Version="8.0.2" değiştir
ADIM 4: YC.Monad.EntityFrameworkCore.UnitTests.csproj → Version="8.0.2" değiştir
```

**Yeni yöntem (fast):**
```
ADIM 1: Directory.Packages.props → Version="8.0.2" değiştir
HAZIR! Tüm projeler otomatik kullanır
```

---

## 📚 Kaynaklar

- **Microsoft Docs**: https://learn.microsoft.com/en-us/nuget/consume/central-package-management
- **NuGet Central Package Management**: Tutorial ve best practices

---

## ✨ Best Practices

### ✅ DO

1. **DevDependency vs NuGet Dependency Ayır**
   ```xml
   <!-- Sadece dev'de gerekli -->
   <PackageVersion Include="StyleCop.Analyzers" Version="1.2.0-beta.556" />
   
   <!-- Production'da da kullanılır -->
   <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="6.0.36" />
   ```

2. **Versions Dosyasını Version Control'de Tut**
   - Git'e commit et
   - Team'e share et
   - History koru

3. **Düzenli Güncellemeler Yap**
   - Security patches için sık kontrol
   - Breaking changes için test et

### ❌ DON'T

1. **Pagetleri Tek Proje'de Override Etme**
   - Merkezi yönetimin amacını bozar
   - Karışıklık yaratır

2. **Directory.Packages.props'u .gitignore'a Ekleme**
   - Version history kaybı
   - Reproducible builds sorunu

---

## 🔧 Konfigürasyon Aktivasyon

`Directory.Packages.props` otomatik olarak algılanır:
- ✅ .NET 7.0+
- ✅ .NET 6.0+ with NuGet 6.4+
- ⚠️ Eski versiyonlar için: `ManagePackageVersionsCentrally` özniteliği manual set etmek gerekebilir

**Kontrol:**
```bash
dotnet restore --verbose  # Logs gösterir
```

---

## 📍 Dosya Konumları

```
YC.Solution/
├── Directory.Build.props ← Common build settings
├── Directory.Packages.props ← ALL PACKAGE VERSIONS HERE
├── .editorconfig
├── stylecop.json
├── roslynator.json
├── src/
│   ├── YC.Monad/
│   │   └── YC.Monad.csproj ← Basit paket ref
│   └── YC.Monad.EntityFrameworkCore/
│       └── YC.Monad.EntityFrameworkCore.csproj
├── test/
│   ├── YC.Monad/
│   │   └── YC.Monad.UnitTests/
│   │       └── YC.Monad.UnitTests.csproj
│   └── YC.Monad.EntityFrameworkCore.UnitTests/
│       └── YC.Monad.EntityFrameworkCore.UnitTests.csproj
```

---

**Kurulum Tarihi**: 26 Nisan 2026  
**Durum**: ✅ Merkezi Paket Yönetimi Aktif  
**Proje Sayısı**: 4 (Tüm projeler merkezi yapıya geçti)


