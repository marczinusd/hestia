# Hestia

![Hestia master](https://github.com/marczinusd/hestia/workflows/Hestia%20master/badge.svg)
[![Coverage](https://codecov.io/gh/marczinusd/hestia/branch/master/graph/badge.svg?token=IZF6men3ZB)](https://codecov.io/gh/marczinusd/hestia)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=marczinusd_hestia&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=marczinusd_hestia)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=marczinusd_hestia&metric=sqale_index)](https://sonarcloud.io/dashboard?id=marczinusd_hestia)

WIP application that can analyze a project's code quality by looking at statistics from git and code coverage.

Features working currently:

* Shared model for git / coverage statistics
* Console runner for easily repeatable analysis runs
* Avalonia/ReactiveUI-based cross-platform UI runner to create repository snapshots to analyze
* Persistence with EFCore targeting Sqlite
* Simple web API to serve statistics
* Angular-based web client here: [marczinusd/hestia-ui](https://github.com/marczinusd/hestia-ui)

## General usage

The easiest way to generate reports for your project is via the console runner, which can be invoked with `make -- run-console --configPath "yourConfig.json"`. An example json config file along with a schema can be found under `src/Hestia.ConsoleRunner/config.json`.

The console runner will then persist the results into a Sqlite instance that can be served through the REST service, which can be started with `make run-webservice`. Do note that the service is currently configured for CORS only on localhost.

## Build

Run `make build` or build manually by running `dotnet build src/Hestia/Hestia.sln`.

## Build thesis

With LaTeX installed run `make build_thesis`.

## Test

Run `make test` or test using dotnet CLI by running `dotnet test src/Hestia/Hestia.sln`.

## Cover

Run `make cover` or generate manually by running `dotnet test src/Hestia/Hestia.sln /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov`
