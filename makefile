.ONESHELL:
build:
	dotnet build src/Hestia/Hestia.sln
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
cover:
# for some reason this generates coverage per-assembly, which doesn't play well with literally ANYTHING
# how to merge these reports together?
	dotnet test src/Hestia/Hestia.sln /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov
