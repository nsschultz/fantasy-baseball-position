# Contributing to Fantasy Baseball Position Service

Thank you for contributing to the Fantasy Baseball Position Service! This guide will help you set up your development environment and understand our development workflow.

## Table of Contents

- [Developer Setup](#developer-setup)
- [Pre-Commit Hooks](#pre-commit-hooks)
- [Development Workflow](#development-workflow)
- [Code Quality Standards](#code-quality-standards)
- [Testing Requirements](#testing-requirements)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)

## Developer Setup

### Prerequisites

- **.NET SDK 10.0** or later
- **Docker** (for containerized development)
- **Git** (with core.hooksPath support)
- **VS Code** (recommended, with DevContainer support)

### Initial Setup

```bash
# Clone the repository
git clone https://github.com/nsschultz/fantasy-baseball-position.git
cd fantasy-baseball-position

# Configure pre-commit hooks
git config core.hooksPath .githooks

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run tests
dotnet test
```

## Pre-Commit Hooks

### Automatic Code Quality Enforcement

This project uses Git pre-commit hooks to automatically enforce code quality standards before commits. These hooks run automatically when you use `git commit`.

### What Gets Checked

When you attempt to commit C# files, the pre-commit hook performs these checks:

#### 1. **Secrets Detection** 🔒
Prevents accidental commit of sensitive data:
- Passwords
- API keys and tokens
- Authentication credentials
- Private secrets

**Example (will be rejected):**
```csharp
var connectionString = "Server=db;Password=admin123;";  // ❌ Rejected
var apiKey = "secret-key-abc123";                       // ❌ Rejected
```

**Workaround:**
- Use environment variables for secrets
- Use configuration files excluded from git (.gitignore)
- Use secrets management systems

#### 2. **Code Formatting** 🎨
Enforces consistent code style using `dotnet format`:
- Consistent spacing and indentation
- Naming conventions
- Code organization

**If formatting issues found:**
```
⚠️  Code formatting issues found
   Running: dotnet format --include '<files>'
   ℹ️  Files have been formatted. Please stage them again and retry commit:
   git add <files>
```

**Manual formatting:**
```bash
dotnet format
```

#### 3. **File Size Check** 📦
Prevents accidentally committing large files (> 5MB):
- Keeps repository lean
- Prevents accidental binary commits
- Maintains clone speed

**If file too large:**
```
❌ File too large: path/to/file.bin (10MB)
```

#### 4. **Build Check** 🔨 (Optional)
Can be enabled to verify the project builds successfully. Commented out by default to keep commits fast.

To enable:
```bash
# Edit .githooks/pre-commit
# Uncomment the build check section
```

#### 5. **Test Check** ✅ (Optional)
Can be enabled to verify all tests pass. Commented out by default to keep commits fast.

To enable:
```bash
# Edit .githooks/pre-commit
# Uncomment the test check section
```

### Bypassing the Hook

For emergency situations, you can bypass the pre-commit hook:

```bash
git commit --no-verify
```

⚠️ **Use with caution!** Bypassing hooks should be rare. If you frequently bypass hooks, it indicates a workflow problem that should be addressed.

### Manual Pre-Commit Checks

Run these manually before committing (recommended):

```bash
# Check formatting
dotnet format --verify-no-changes

# Build the project
dotnet build

# Run all tests
dotnet test

# Run specific test project
dotnet test FantasyBaseball.PositionService.UnitTests
dotnet test FantasyBaseball.PositionService.IntegrationTests
```

## Development Workflow

### 1. Create a Feature Branch

```bash
git checkout -b feature/your-feature-name
```

Branch naming conventions:
- `feature/` - New features or enhancements
- `bugfix/` - Bug fixes
- `docs/` - Documentation updates
- `refactor/` - Code refactoring
- `test/` - Test improvements

Example:
```bash
git checkout -b feature/add-position-filtering
git checkout -b bugfix/fix-concurrent-requests
git checkout -b docs/update-deployment-guide
```

### 2. Make Your Changes

Follow the coding standards defined in [.github/agents/CSharpExpert.agent.md](.github/agents/CSharpExpert.agent.md):

- Write self-documenting code with XML documentation
- Include unit and integration tests
- Maintain backward compatibility
- Update documentation alongside code changes

### 3. Run Tests Locally

```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter ClassName
```

All tests must pass before committing.

### 4. Commit Your Changes

```bash
git add <files>
git commit -m "Brief description of change"
```

**Pre-commit hook runs automatically:**
- Checks for secrets
- Validates formatting
- Checks file sizes
- Optionally runs build/tests

If the hook rejects your commit:
1. Fix the issue (e.g., `dotnet format` if formatting failed)
2. Stage the fixed files: `git add <files>`
3. Try committing again

### 5. Push and Create a Pull Request

```bash
git push origin feature/your-feature-name
```

Then create a PR on GitHub for review.

## Code Quality Standards

### XML Documentation

All public APIs must have XML documentation comments:

```csharp
/// <summary>Gets all positions sorted by order.</summary>
/// <returns>A list of positions.</returns>
public async Task<List<Position>> GetPositions()
{
    // Implementation
}
```

### Required Keyword

Use the `required` keyword for non-nullable properties:

```csharp
public class Position
{
    /// <summary>Position code (e.g., "SS", "OF").</summary>
    required public string Code { get; set; }
    
    /// <summary>Full position name.</summary>
    required public string FullName { get; set; }
}
```

### Async/Await

All I/O operations must be async:

```csharp
// ✅ Correct
public async Task<IEnumerable<Position>> GetPositionsAsync()
{
    return await _repository.GetAllAsync();
}

// ❌ Wrong
public IEnumerable<Position> GetPositions()
{
    return _repository.GetAll();  // Blocking call
}
```

### Error Handling

Use appropriate exception types and provide context:

```csharp
// ✅ Good
if (string.IsNullOrWhiteSpace(positionCode))
{
    throw new ArgumentNullException(nameof(positionCode), "Position code cannot be null or empty.");
}

// ❌ Poor
if (positionCode == null)
{
    throw new Exception("Invalid input");
}
```

### SOLID Principles

- **Single Responsibility**: Classes should have one reason to change
- **Open/Closed**: Open for extension, closed for modification
- **Liskov Substitution**: Derived classes must be substitutable
- **Interface Segregation**: Clients should not depend on unused interfaces
- **Dependency Inversion**: Depend on abstractions, not concretions

## Testing Requirements

### Unit Tests

Test individual components in isolation:

```csharp
[Fact]
public void GetPositions_WithValidInput_ReturnsPositions()
{
    // Arrange
    var mockRepository = new Mock<IPositionRepository>();
    mockRepository.Setup(r => r.GetAllAsync())
        .ReturnsAsync(new List<Position> { /* ... */ });
    
    var service = new GetPositionsService(mockRepository.Object);

    // Act
    var result = await service.GetPositionsAsync();

    // Assert
    Assert.NotEmpty(result);
}
```

### Integration Tests

Test end-to-end API functionality:

```csharp
[Fact]
public async Task GetPositions_ReturnsAllPositions()
{
    // Arrange - create HTTP client from fixture
    var response = await fixture.Client.GetAsync("/api/v1/position");

    // Act/Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    var positions = await response.Content.ReadAsAsync<List<Position>>();
    Assert.NotEmpty(positions);
}
```

### Coverage Requirements

- Minimum **80% code coverage** (verified by SonarCloud)
- All public methods should be tested
- Include edge cases and error conditions
- Test both happy path and failure scenarios

### Running Coverage Report

```bash
# Install coverage tool (one-time)
dotnet tool install -g dotnet-coverage

# Generate coverage report
dotnet-coverage collect -f cobertura -o coverage.cobertura.xml dotnet test
```

## Commit Guidelines

### Commit Message Format

Follow the imperative mood, present tense:

```
Add feature description

- Detailed explanation of what changed
- Why the change was necessary
- Any breaking changes or deprecations

Fixes #123  (if applicable)
```

**Examples:**

```
Add position filtering by player type

Implement IPositionFilter interface to allow filtering positions
by batter/pitcher. Includes new endpoint /api/v1/position/filter.

Fixes #45
```

```
Fix concurrent database access issue

Use connection pooling to prevent connection exhaustion under load.
Tested with 100 concurrent requests.
```

### Commit Size

- Keep commits focused and atomic
- Each commit should be a logical unit of change
- Avoid mixing refactoring with feature additions
- Run tests after each commit

## Pull Request Process

### Before Submitting

1. Ensure all tests pass: `dotnet test`
2. Verify formatting: `dotnet format --verify-no-changes`
3. Build successfully: `dotnet build`
4. Update documentation if needed
5. Add tests for new functionality
6. Update CHANGELOG.md with your changes

### PR Description Template

```markdown
## Description
Brief description of what this PR does.

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Related Issues
Fixes #123

## Testing
Describe how you tested this change:
- [ ] Unit tests added
- [ ] Integration tests added
- [ ] Manual testing performed

## Checklist
- [ ] Code follows style guidelines
- [ ] Documentation updated
- [ ] Tests added and passing
- [ ] No breaking changes (or documented)
- [ ] CHANGELOG.md updated
```

### Review Process

1. **GitHub Actions PR Validation** (`.github/workflows/pr-validation.yml`) runs automatically:
   - **Build & Test**: Compiles code and runs 25+ unit and integration tests
   - **SonarCloud Quality Gate**: Verifies code quality and coverage ≥ 80%
   - **Code Format**: Ensures consistent style via `dotnet format`
   - **Security Scan**: Scans with Trivy for vulnerabilities
   
   All checks must pass before the PR can be merged.

2. **Code Review**:
   - Minimum one approval required
   - Address feedback promptly
   - Push updates for further review

3. **Merge**:
   - "Squash and merge" for clean history
   - Ensure PR title matches commit message

## Questions and Support

- **GitHub Issues**: Report bugs or request features
- **Discussions**: Ask questions or suggest improvements
- **Documentation**: Check README.md and DEVOPS_RUNBOOK.md for operational guidance

## Related Documentation

- [README.md](README.md) - Project overview and setup
- [DEVOPS_RUNBOOK.md](DEVOPS_RUNBOOK.md) - Operational procedures
- [DEPLOYMENT.md](DEPLOYMENT.md) - Release and deployment procedures
- [SLO_SLA.md](SLO_SLA.md) - Service level objectives
- [.github/agents/CSharpExpert.agent.md](.github/agents/CSharpExpert.agent.md) - C# coding standards
- [.github/agents/DevOpsExpert.agent.md](.github/agents/DevOpsExpert.agent.md) - DevOps practices

---

Thank you for contributing! 🎉
