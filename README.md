[![codecov](https://codecov.io/gl/marczinusd/hestia/branch/master/graph/badge.svg?token=h6C3x4EsIe)](https://codecov.io/gl/marczinusd/hestia)
[![pipeline status](https://gitlab.com/marczinusd/hestia/badges/master/pipeline.svg)](https://gitlab.com/marczinusd/hestia/commits/master)

# Hestia

TODO: Add _meaningful_ description :)

## Build

Run `make build` or build manually by running `dotnet build src/Hestia/Hestia.sln`.

## Build thesis

With LaTeX installed run `make build_thesis`.

## Test

Run `make test` or test using dotnet CLI by running `dotnet test src/Hestia/Hestia.sln`.

## Cover

Run `make cover` or generate manually by running `dotnet test src/Hestia/Hestia.sln /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov`
