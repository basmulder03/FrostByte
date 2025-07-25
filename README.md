# FrostByte

FrostByte is a cross-platform .NET MAUI application for interacting with [Advent of Code](https://adventofcode.com/). It provides a modern, user-friendly interface for managing your Advent of Code experience, including calendar navigation, input management, and authentication.

## Features
- **Cross-platform**: Runs on Windows and other platforms supported by .NET MAUI
- **Advent of Code Calendar**: Browse and interact with the AoC calendar
- **Authentication**: Securely sign in to your Advent of Code account
- **Input Management**: Download and cache puzzle inputs
- **Modern UI**: Built with .NET MAUI and CommunityToolkit.Maui.Markup

## Automation Guidelines Compliance
This project follows the [automation guidelines on the /r/adventofcode community wiki](https://www.reddit.com/r/adventofcode/wiki/faqs/automation):
- Outbound calls to AoC are throttled to at least once every 15 minutes
- Inputs are cached locally after initial download
- Manual input refreshes are supported and also respect throttle limits
- The User-Agent header includes maintainer contact info: `github.com/BasMu/FrostByte by bas@basmu.nl`

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Visual Studio 2022+ or JetBrains Rider (with MAUI support)

### Build and Run
1. Clone the repository:
   ```sh
   git clone https://github.com/BasMu/FrostByte.git
   cd FrostByte
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Build the solution:
   ```sh
   dotnet build
   ```
4. Run the app:
   ```sh
   dotnet run --project FrostByte.App
   ```

## Project Structure
- `FrostByte.App/` - .NET MAUI app entry point and UI shell
- `FrostByte.Presentation/` - Views and ViewModels (MVVM)
- `FrostByte.Application/` - Application services, configuration, and AoC API logic
- `FrostByte.Domain/` - Domain models and business logic
- `FrostByte.Infrastructure/` - Infrastructure and data access (if applicable)

## Package Management & Updates
- Automated dependency updates are managed via [Dependabot](.github/dependabot.yml)
- Patch/minor updates are grouped weekly; major updates are grouped separately
- You can also run `update-packages.ps1` for local package management

## Contributing
Contributions are welcome! Please open issues or pull requests for bug fixes, features, or improvements.

## License
[MIT](LICENSE)

## Maintainer
Bas Mulder — [bas@basmulder.online](mailto:bas@basmu.nl)

---

> This project complies with the [Advent of Code automation guidelines](https://www.reddit.com/r/adventofcode/wiki/faqs/automation).
