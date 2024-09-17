# Currency Conversion Dashboard

## Overview

The Avalonia Currency Conversion Dashboard is a cross-platform desktop application that provides real-time currency conversion rates from multiple API sources. It offers users a comprehensive view of exchange rates in a user-friendly interface, allowing for accurate currency conversions and rate comparisons.

## Features

- Real-time currency conversion using data from 5 different API sources
- Dashboard display of exchange rates from all sources
- Average rate calculation for more accurate conversions
- Historical rate tracking and visualization
- Customizable currency pairs and alerts
- User-friendly interface with dropdown menus for currency selection
- Detailed view of rates from each source with timestamps

## Technology Stack

- C# with .NET 6.0+
- Avalonia UI for cross-platform desktop development
- ReactiveUI for MVVM implementation
- SQLite for local data storage (historical rates)
- HttpClient for API calls

## Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022, JetBrains Rider, or Visual Studio Code

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/dev-alt/avalonia-currency-dashboard.git
   ```

2. Navigate to the project directory:
   ```
   cd avalonia-currency-dashboard
   ```

3. Restore the NuGet packages:
   ```
   dotnet restore
   ```

4. Build the project:
   ```
   dotnet build
   ```

5. Run the application:
   ```
   dotnet run
   ```

## Project Structure

```
CurrencyConverterDashboard/
├── Models/
├── ViewModels/
├── Views/
├── Services/
│   └── ApiClients/
├── App.axaml
├── App.axaml.cs
└── Program.cs
```

## Configuration

Before running the application, you need to set up API keys for the currency data providers. Create a `config.json` file in the project root with the following structure:

```json
{
  "ApiKeys": {
    "Provider1": "your-api-key-1",
    "Provider2": "your-api-key-2",
    "Provider3": "your-api-key-3",
    "Provider4": "your-api-key-4",
    "Provider5": "your-api-key-5"
  }
}
```

Replace `"your-api-key-x"` with your actual API keys.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

- Thanks to the Avalonia UI team for their excellent cross-platform framework.
- Shoutout to all the currency API providers that make this dashboard possible.

## Contact

If you have any questions or feedback, please open an issue on this repository.
