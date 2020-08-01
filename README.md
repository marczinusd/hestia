# Hestia

![Hestia master](https://github.com/marczinusd/hestia/workflows/Hestia%20master/badge.svg)
![Sonar scan](https://github.com/marczinusd/hestia/workflows/Sonar%20scan/badge.svg)
[![Coverage](https://codecov.io/gh/marczinusd/hestia/branch/master/graph/badge.svg?token=IZF6men3ZB)](https://codecov.io/gh/marczinusd/hestia)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=marczinusd_hestia&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=marczinusd_hestia)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=marczinusd_hestia&metric=sqale_index)](https://sonarcloud.io/dashboard?id=marczinusd_hestia)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=marczinusd_hestia&metric=alert_status)](https://sonarcloud.io/dashboard?id=marczinusd_hestia)

WIP application that can analyze a project's code quality by looking at statistics from git and code coverage.

Features working currently:

* Shared model for git / coverage statistics
* Simple UI runner to create repository snapshots to analyze
* Console runner

WIP:

* Persistence with EFCore
* Simple web API to serve statistics
* React-based web client here: [marczinusd/hestia-client](https://github.com/marczinusd/hestia-ui)

## Build

Run `make build` or build manually by running `dotnet build src/Hestia/Hestia.sln`.

## Build thesis

With LaTeX installed run `make build_thesis`.

## Test

Run `make test` or test using dotnet CLI by running `dotnet test src/Hestia/Hestia.sln`.

## Cover

Run `make cover` or generate manually by running `dotnet test src/Hestia/Hestia.sln /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov`
