# Djinn Language

Simple and basic compiler of Djinn language to generate native code binaries with LLVM support.

### Example code

```fsharp
function int64 add(int64 a, int64 b) {
    ret b + c;
}

function void main() {
    printf(add(1,2));
}
```

### Language Features

- [x] Functions
- [x] Imports
- [x] If
- [x] Else
- [x] Switch/case
- [x] Basic Types
    - [x] Integers
    - [x] Floating-point numbers
    - [x] Strings
    - [x] Booleans
    - [ ] Characters
- [ ] Custom types
    - [ ] Structs
    - [ ] Enums
    - [ ] Classes
    - [ ] Interfaces
    - [ ] Abstract classes
    - [ ] Generics
- [ ] Arrays
- [ ] Loops
    - [ ] For
    - [ ] While
    - [ ] Do-while

#### Future
- [ ] Standard Library
- [ ] Lists/Sets
- [ ] Maps/Dictionaries
- [ ] Compile time expressions
- [ ] Memory Management
