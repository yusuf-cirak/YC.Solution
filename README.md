<!-- @format -->

# YC.Solution

Functional programming patterns for .NET - Result, Option, and Error types with full LINQ support.

## Packages

| Package                      | NuGet                                                                                                                                     | Description                       |
| ---------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------- |
| YC.Monad                     | [![NuGet](https://img.shields.io/nuget/v/YC.Monad.svg)](https://www.nuget.org/packages/YC.Monad/)                                         | Core functional programming types |
| YC.Monad.EntityFrameworkCore | [![NuGet](https://img.shields.io/nuget/v/YC.Monad.EntityFrameworkCore.svg)](https://www.nuget.org/packages/YC.Monad.EntityFrameworkCore/) | EF Core extensions                |

## Features

- **Result Type**: Railway-oriented programming for error handling
- **Option Type**: Null-safe value handling
- **Error Type**: Rich error information
- **LINQ Support**: Full query syntax support
- **Async Support**: Async/await compatible
- **EF Core Integration**: Seamless database query integration

## Installation

```bash
dotnet add package YC.Monad
dotnet add package YC.Monad.EntityFrameworkCore
```

## Development Setup

After cloning the repository, install Git hooks for local validation:

```bash
npm install
```

This sets up:

- ✅ Commit message validation (Conventional Commits)
- ✅ Pre-commit checks (format, build, tests)
- ✅ Pre-push validation (full build and tests)

See [Git Hooks Setup](docs/GIT_HOOKS_SETUP.md) for details.

## Release Process

This project uses automated versioning and publishing. See [Release Process Documentation](docs/RELEASE_PROCESS.md) for details.

### Quick Release Guide

1. Make changes using [Conventional Commits](https://www.conventionalcommits.org/):

   ```bash
   git commit -m "feat: add new feature"
   git commit -m "fix: resolve bug"
   ```

2. Create and push a version tag:

   ```bash
   # For YC.Monad
   git tag v1.2.2
   git push origin v1.2.2

   # For YC.Monad.EntityFrameworkCore
   git tag efcore-v1.0.1
   git push origin efcore-v1.0.1
   ```

3. GitHub Actions automatically:
   - ✅ Builds and tests
   - ✅ Generates changelog
   - ✅ Creates GitHub release
   - ✅ Publishes to NuGet

## Contributing

Please follow [Conventional Commits](https://www.conventionalcommits.org/) for all commit messages.

## License

See [LICENSE](LICENSE.txt) for details.
