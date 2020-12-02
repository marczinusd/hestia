.ONESHELL:
ifeq (run-console,$(firstword $(MAKECMDGOALS)))
  RUN_ARGS := $(wordlist 2,$(words $(MAKECMDGOALS)),$(MAKECMDGOALS))
  $(eval $(RUN_ARGS):;@:)
endif

build:
	dotnet build src/Hestia.sln
test:
	dotnet test src/Hestia.sln
.SILENT:
tools:
	dotnet tool install --tool-path . dotnet-reportgenerator-globaltool 2>/dev/null || exit 0
cover:
	$(MAKE) tools
	dotnet test src/Hestia.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude="[xunit*]*" /p:CoverletOutput="coverage.json"
	reportgenerator "-reports:**/coverage.json" "-targetdir:coveragereport" "-reporttypes:Html;HtmlSummary;Cobertura;lcov;XML;JsonSummary;SonarQube"
run-webservice:
	$(MAKE)
	dotnet run --project src/Hestia.WebService/Hestia.WebService.csproj
run-console:
	$(MAKE)
	dotnet run --project src/Hestia.ConsoleRunner/Hestia.ConsoleRunner.csproj -- $(RUN_ARGS)
