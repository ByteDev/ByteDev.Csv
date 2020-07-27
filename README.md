[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.Csv?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-Csv/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.Csv.svg)](https://www.nuget.org/packages/ByteDev.Csv)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/ByteDev/ByteDev.Csv/blob/master/LICENSE)

# ByteDev.Csv

Simple library to help in the reading and writing of CSV formatted content.

**Note:** this is just a pet project and has not been tested with large CSV data sets.
There are lots of other .NET CSV readers/writers out there, see: [Google](https://www.google.com/search?q=c%23+best+csv+reader)
and [StackOverflow](https://stackoverflow.com/questions/1941392/are-there-any-csv-readers-writer-libraries-in-c).

## Installation

ByteDev.Csv has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.Csv is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.Csv`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.Csv/).

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.Csv/blob/master/docs/RELEASE-NOTES.md).

## Usage

Example of reading an existing CSV file:

```csharp
ICsvFileReader reader = new CsvFileReader();

CsvFile csvFile = reader.ReadFile(@"C:\people.csv", new CsvFileReaderOptions { HasHeader = true });

Console.WriteLine(csvFile.Header.Line);
Console.WriteLine(csvFile.Body.Lines[0].Line);
Console.WriteLine(csvFile.Body.Lines[1].Line);
```

Example of creating and writing a CSV file:

```csharp
var header = new CsvFileLine("Name,Age");

var body = new CsvFileBody(new List<CsvFileLine> 
{
	new CsvFileLine("John,20"), 
	new CsvFileLine("Joe,30")
});

var csvFile = new CsvFile(header, body);

ICsvFileWriter writer = new CsvFileWriter();

writer.Write(@"C:\people.csv", csvFile);
```

