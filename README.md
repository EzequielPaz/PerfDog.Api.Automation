# 🐾 PerfDog API Automation Framework

This repository contains an API Automation Framework built with **.NET 8**, **Playwright**, and **NUnit** to test the Swagger Petstore API.

## 🚀 Features
* **Service Object Model (SOM)**: Separation of API logic from test scenarios.
* **Custom Logging**: Integrated `NUnitLoggerProvider` for clean, emoji-coded console output.
* **JSON Debugging**: Automated formatting for request/response bodies in logs.
* **Robust Models**: Handles large IDs (Int64) and dynamic data generation.

---

## 🛠️ Prerequisites

Before running the tests, ensure you have the following installed:
1. [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell) (Standard on Windows)

---

## 🏗️ Getting Started

### 1. Clone the repository
```bash
git clone git@github.com:EzequielPaz/PerfDog.Api.Automation.git
cd PerfDog.Tests

2. Restore dependencies
dotnet restore

3. Install Playwright Browsers
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install

Run all tests via Console

dotnet test --logger "console;verbosity=normal"

Run a specific test part

dotnet test --filter "Name=CreateTenPetsAndVerifySoldPetDetails"

dotnet test --filter "Name=ListAvailablePetsAndCreateStoreOrders"