# 📦 Clearbank Refactoring challenge

A payment gateway, an API based application that will allow a merchant to offer a way for their shoppers to pay for their product.


## 🧰 Tech Stack

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [NUnit](https://nunit.org/)
- [NSubstitute](https://nsubstitute.github.io/)

## 📁 Project Structure
```
ClearBank.DeveloperTest/
	Data - Data access classes
	Services - Business logic
	Types - Different Data types used in the application
	Extensions - Extension methods used for validation

ClearBank.DeveloperTest.Tests/
	Services - Unit tests for service classes
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- IDE like [Visual Studio 2022 version 17.8 or higher.](https://visualstudio.microsoft.com/)

### Clone and Build

```bash
git clone https://github.com/JakeLRL/clearbank_developer_test.git
cd clearbank_developer_test
dotnet build
```

Or

```
Clone through Visual Studio
Build (Ctrl + Shift + B)
```

## 🧪 Testing

Tests are located in the tests/ProjectName.Tests directory. The test suite uses:

    NUnit for assertions and test structure

    NSubstitute for mocking dependencies

### Run Tests

```
dotnet test
```

Or

Run directly in the test explorer

Common issue
    To run the tests in Visual Studio you will need "NUnit Test Generator VS2022" extension installed.