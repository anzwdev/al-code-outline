# Language Server Module - Symbols Viewer

Module that handles symbols viewer documents for symbols extracted from AL app files and current project.

## Features

- Handle operations on symbols viewer documents (symbols with state)
  - open, refresh, close
  - get symbols tree
  - get single symbols details

## Details 

Each document holds full symbols tree and simplified one that ends on object level. 
Visual Studio Code displays the simplified tree in the symbols viewer and when user clicks on symbol, details are shown in the document.


