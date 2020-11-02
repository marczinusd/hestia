.ONESHELL:
ifeq (run-console,$(firstword $(MAKECMDGOALS)))
  RUN_ARGS := $(wordlist 2,$(words $(MAKECMDGOALS)),$(MAKECMDGOALS))
  $(eval $(RUN_ARGS):;@:)
endif

build:
	dotnet build src/Hestia.sln
build_thesis:
	cd docs/thesis
	texfot pdflatex thesis.tex
	texfot bibtex thesis
	texfot makeindex -s nomencl.ist -t thesis.nlg -o thesis.nls thesis.nlo
	texfot pdflatex thesis.tex
	texfot pdflatex thesis.tex
	mkdir -p ../../dist/
	mv thesis.pdf ../../dist/thesis.pdf && echo "PDF built at ./dist/thesis.pdf"
test:
	dotnet test src/Hestia.sln
.SILENT:
tools:
ifeq (, $(shell which reportgenerator))
	dotnet tool install --tool-path . dotnet-reportgenerator-globaltool 2>/dev/null || exit 0
endif
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
