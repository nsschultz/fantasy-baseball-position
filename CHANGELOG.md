# Changelog

All notable changes to the Position Service project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- DevOps Expert agent guidelines for microservice operations
- Comprehensive C# best practices documentation
- Extended README with setup instructions and architecture overview
- Health check endpoint documentation
- Environment variable configuration examples

### Changed
- Improved documentation standards and clarity

### Fixed
- Corrected documentation terminology (Context, Repository classes)
- Fixed Docker release workflow to trigger on version tags and publish the `latest` image tag

## [1.2.3] - 2026-11-16

### Added
- Comprehensive test coverage for edge cases and corner cases (15 new tests)
- Program.cs configuration documentation
- Extended project README with 13 major sections
- C# best practices alignment

### Changed
- AutoMapper updated from 16.1.1 to 17.0.0
- Microsoft.OpenApi updated from 3.5.0 to 3.11.0
- XML documentation improved for all public APIs
- Test file naming conventions updated to use "Tests" suffix (plural)

### Fixed
- Build warning cleanup (0 warnings achieved)
- Terminology corrections in class documentation
- Test class documentation enhancements

### Security
- Fixed GHSA-6vhm-j9h7-f9gh (AutoMapper vulnerability)
- Fixed GHSA-gpwp-5797-mjff (AutoMapper vulnerability)
- Fixed GHSA-v5pm-xwqc-g5wc (Microsoft.OpenApi vulnerability)

## [1.2.2] - 2026-03-29

### Initial Release
- ASP.NET Core 10 Web API for position data
- Read-only microservice serving position information
- PostgreSQL database integration
- Docker container support (multi-platform: amd64, arm64)
- Comprehensive test suite (16 unit + 9 integration tests)
- SonarCloud quality integration
- GitHub Actions CI/CD pipeline

## Versioning Policy

This project follows [Semantic Versioning](https://semver.org/):

- **MAJOR** version: Incompatible API changes or database schema changes
- **MINOR** version: Backward-compatible new features or endpoints
- **PATCH** version: Backward-compatible bug fixes or improvements

## Release Process

1. Update version in `version.txt`
2. Update this CHANGELOG with release notes
3. Create annotated tag: `git tag -a v1.2.3 -m "Release v1.2.3: Description"`
4. Push tag: `git push origin v1.2.3`
5. Docker image build and push triggered automatically
6. Create GitHub Release with changelog details

## SLO Targets

This service aims to maintain:
- **Availability**: 99.5% uptime (36 minutes downtime/month)
- **Response Time**: P95 < 200ms for read operations
- **Quality**: ≥ 80% code coverage
- **Security**: Zero critical vulnerabilities

## Documentation

For more information, see:
- [README.md](README.md) - Project overview and setup
- [DEPLOYMENT.md](DEPLOYMENT.md) - Release and deployment procedures
- [DEVOPS_RUNBOOK.md](DEVOPS_RUNBOOK.md) - Operational procedures
- [SLO_SLA.md](SLO_SLA.md) - Service level definitions
- [.github/agents/DevOpsExpert.agent.md](.github/agents/DevOpsExpert.agent.md) - DevOps guidance
- [.github/agents/CSharpExpert.agent.md](.github/agents/CSharpExpert.agent.md) - Code quality standards
