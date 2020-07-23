# Hestia

[![codecov](https://codecov.io/gh/marczinusd/hestia/branch/master/graph/badge.svg?token=IZF6men3ZB)](https://codecov.io/gh/marczinusd/hestia)
![Hestia master](https://github.com/marczinusd/hestia/workflows/Hestia%20master/badge.svg)

WIP application that can analyze a project's code quality by looking at statistics from git and code coverage.

Features working currently:

* Shared model for git / coverage statistics
* Simple UI runner to create repository snapshots to analyze
* Console runner

Planned / partially working:

* MongoDB store for project statistics
* Simple web API to serve statistics stored in Mongo store

## Build

Run `make build` or build manually by running `dotnet build src/Hestia/Hestia.sln`.

## Build thesis

With LaTeX installed run `make build_thesis`.

## Test

Run `make test` or test using dotnet CLI by running `dotnet test src/Hestia/Hestia.sln`.

## Cover

Run `make cover` or generate manually by running `dotnet test src/Hestia/Hestia.sln /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov`
