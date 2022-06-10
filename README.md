# Differential Privacy through Range Counting
This repository contains a library for applying differential privacy through range counting, together with an interactive program for performing range queries on a differentially private data structure, using varying mechanisms on a data set from [New York City Taxa & Limousine Commission trip records. (Visited 25/01/2022)](https://www1.nyc.gov/site/tlc/about/tlc-trip-record-data.page).

To run this program locally, clone the repository, and ensure that .NET 6.0 is available, together with MathNet.Numerics. The interactive program allows you to specify, which combination of mechanism, distribution, dimension, range and ε you wish to run a query on. Furthermore, the program allows you to run multiple queries on the same combination, pick a new ε, or choose an entirely new combination.

Additionally Nunit .Net is needed to run tests.
### Dependencies
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [MathNet.Numerics](https://numerics.mathdotnet.com/)
- [Nunit .NET](https://nunit.org/)
### Run Iteractive Program
Navigate to the /RangeCounting folder:
```sh
cd Project/RangeCounting
```
Run the following command:
```sh
dotnet run
```
Follow the instructions stated by the program to pick the desired combination.
### Run Tests
Navigate to the /RangeCountingTests folder:
```sh
cd Project/RangeCountingTests
```
Run the following command:
```sh
dotnet test
```

### Authors
- Frederik Kjærulf Skjellerup
- Jakob Krogh Petersen
- Jakob Pedersen

