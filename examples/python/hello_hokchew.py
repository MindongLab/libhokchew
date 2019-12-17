import sys

from python.pyhokchew.parser import parse_foochow_romanized_phrase as parse
from python.pyhokchew.version import VERSION

print("Hello Hokchew")
print("Python version:", sys.version)
print("PyHokchew version:", VERSION)

print()
print("Parsing Foochow Romanized:")
print(parse("cṳ̄ng"))
