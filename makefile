.ONESHELL:
build:
	dotnet build src/Hestia/Hestia.sln
build-ui:
	cd src/hestia-web-ui
	npm build
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
	dotnet test src/Hestia/Hestia.sln
test-ui:
	cd src/hestia-web-ui
	npm test
test-e2e:
	echo todo
	cd src/hestia-web-ui
	npm test-e2e
cover:
	dotnet test src/Hestia/Hestia.sln /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov
