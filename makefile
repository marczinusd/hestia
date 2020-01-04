.ONESHELL:
build:
	python src/main.py
docs:
	cd docs/thesis
	ls
	# thesis.aux fájl generálása (PDF fájl még hibás hivatkozásokat fog tartalmazni)
	pdflatex thesis.tex
	# Irodalomjegyzék generálása
	bibtex thesis
	# Jelölésjegyzék generálása (ha szükséges)
	makeindex -s nomencl.ist -t thesis.nlg -o thesis.nls thesis.nlo
# Végleges PDF fájl generálása
	pdflatex thesis.tex
	pdflatex thesis.tex
	mkdir ../../dist/ || mv thesis.pdf ../../dist/thesis.pdf 
test:
	echo "Test"
