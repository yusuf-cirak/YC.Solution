# .NET Project Configuration Guide

Bu dosya, YC.Solution projelerine eklenen editor config, linter rules ve standart ayarlarını açıklamaktadır.

## Eklenen Konfigürasyon Dosyaları

### 1. `.editorconfig`
- **Konum**: Proje kökü (`D:\Yazılım\csharp\repos\YC.Solution\.editorconfig`)
- **Amaç**: Kod stili ve formatlamayı standardize eder
- **İçerik**:
  - C# kodu için indentation, spacing ve yeni satır kuralları
  - Naming conventions (adlandırma conventi'ı)
  - Code style preferences (var, pattern matching, vb.)
  - .NET Code Quality rules
  - IDE diagnostic ayarları

### 2. `stylecop.json`
- **Konum**: Proje kökü
- **Amaç**: StyleCop Analyzers konfigürasyonu
- **Özellikleri**:
  - Documentation rules (belgeler kuralı)
  - Maintainability rules (bakımlanabilirlik kuralı)
  - Naming rules
  - Readability rules (okunabilirlik kuralı)
  - Spacing and ordering rules

### 3. `roslynator.json`
- **Konum**: Proje kökü
- **Amaç**: Roslynator Analyzers konfigürasyonu
- **Özellikleri**:
  - 40+ analyzer kuralı
  - Her kural için severity (önem) seviyesi
  - Refactoring seçenekleri

### 4. `Directory.Build.props`
- **Konum**: Proje kökü
- **Amaç**: Tüm projeler için merkezi build ayarları
- **İçerik**:
  - Common properties (LangVersion, Deterministic)
  - Code analysis settings
  - Package common metadata

## Proje Dosyalarına Eklenen Ayarlar

Aşağıdaki tüm projeler güncellenmiştir:

### src/YC.Monad/YC.Monad.csproj
### src/YC.Monad.EntityFrameworkCore/YC.Monad.EntityFrameworkCore.csproj
### test/YC.Monad/YC.Monad.UnitTests/YC.Monad.UnitTests.csproj
### test/YC.Monad.EntityFrameworkCore.UnitTests/YC.Monad.EntityFrameworkCore.UnitTests.csproj

**Eklenen PropertyGroup ayarları**:
```xml
<!-- Code Analysis -->
<AnalysisLevel>latest</AnalysisLevel>
<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
<EnableNETAnalyzers>true</EnableNETAnalyzers>
<AnalyzersLanguageVersion>latest</AnalyzersLanguageVersion>
<TreatWarningsAsErrors>false</TreatWarningsAsErrors>
```

**Eklenen Analyzer Paketleri**:
1. **Microsoft.CodeAnalysis.NetAnalyzers** v8.0.1
   - .NET runtime analyzer kuralları
   - Security ve performance kuralları

2. **StyleCop.Analyzers** v1.2.0-beta.556
   - C# stili ve dokümantasyon kuralları
   - Naming conventions

3. **Roslynator.Analyzers** v4.12.0
   - Ek refactoring ve style kuralları
   - Performance optimization suggestions

## Adlandırma Kuralları (Naming Conventions)

### Public/Protected Members
- **PascalCase** ile adlandırılmalıdır
- Örnek: `PublicMethod`, `PublicProperty`

### Interfaces
- `I` harfi ile başlamalıdır
- Örnek: `IRepository`, `IService`

### Parameters
- **camelCase** ile adlandırılmalıdır
- Örnek: `productId`, `userName`

### Local Variables
- **camelCase** ile adlandırılmalıdır
- Örnek: `isValid`, `tempList`

### Private Fields
- **_camelCase** ile adlandırılmalıdır (altı çizgi ile başlar)
- Örnek: `_cache`, `_repository`

## Code Style Preferences

### Expression-Bodied Members
- Properties: `true` (expression body)
- Constructor: `false` (regular body)
- Methods: `false` (regular body)
- Accessors: `true` (expression body)

### Pattern Matching
- Tercih edilir: `true`
- Switch expressions tercih edilir

### Var Usage
- Built-in types için: `false` (`int x = 5` yazılır)
- Type apparent olduğunda: `true` (`var list = new List<int>()`)

### Object/Collection Initializers
- Tercih edilir: `true`

## Analyzer Severity Levels

- **Warning** (Uyarı): Build sırasında gösterilir ama hata olmaz
- **Suggestion** (Öneri): Hafif uyarı, best practice
- **Silent** (Sessiz): Sadece IDE'de gösterilir, build'i etkilemez

## IDE'de Kullanım

Aşağıdaki IDEs `.editorconfig` dosyasını otomatik olarak tanır:
- **Visual Studio** 2019 ve sonrası
- **Visual Studio Code** (EditorConfig extension ile)
- **JetBrains Rider**
- **Visual Studio for Mac**

## Konfigürasyon Değiştirme

İhtiyaç durumunda `.editorconfig` dosyasında ayarları değiştirebilirsiniz:

1. `.editorconfig` dosyasını açın
2. İlgili rule bölümünü bulun
3. Ayarı değiştirin
4. Dosyayı kaydedin
5. IDE'de dosyaları yeniden yükleyin (VSCode'da genellikle otomatik)

## Örnek: File Type Bazlı Ayarlar

```editorconfig
[*.{config,nuspec}]
indent_size = 2
indent_style = space

[*.cs]
indent_size = 4
indent_style = space
```

## Best Practices

1. **Consistent Formatting**: Tüm takım üyelerinin aynı formatlama kurallarını kullanması
2. **Early Analysis**: Build sırasında hataları yakalamak
3. **Code Review**: Analyzer önerileri code review sırasında göz önüne alınmalı
4. **Documentation**: Public API'ler her zaman belgelenmelidir

## Disable Etme

Belirli bir kural için disable etmek isterse:

```csharp
#pragma warning disable CA1234
// kodunuz
#pragma warning restore CA1234
```

Veya `.editorconfig` dosyasında:

```editorconfig
dotnet_diagnostic.CA1234.severity = silent
```

## Kaynaklar

- [EditorConfig Documentation](https://editorconfig.org)
- [StyleCop Analyzers GitHub](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
- [Roslynator Documentation](https://github.com/JosefPihrt/Roslynator)
- [.NET Code Analyzers](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview)

## Support

Sorularınız veya sorunlarınız için GitHub Issues'i kullanın.

