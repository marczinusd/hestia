.ONESHELL:
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
cover:
	dotnet tool install --tool-path . dotnet-reportgenerator-globaltool & exit 0
	dotnet test src/Hestia.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude="[xunit*]*" /p:CoverletOutput="coverage.json"
	reportgenerator "-reports:**/coverage.json" "-targetdir:coveragereport" "-reporttypes:Html;HtmlSummary;Cobertura;lcov;XML;JsonSummary"
