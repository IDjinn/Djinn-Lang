# Djinn Language

Simple and basic compiler of Djinn language to generate native code binaries with LLVM support.

### Example code

```fsharp
function int32 add(int32 a, int32 b) {
    ret a + b;
}

function int32 main() {
    int32 counter = 0;
    while (counter < 10 ) {
        printf(add(counter, 1));
        counter++;
    }

    if ( counter < 10 ) {
        printf("counter = %d", counter);
        ret counter;
    }    

    ret 0;
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
- [X] Loops
    - [ ] For
    - [X] While
    - [ ] Do-while

#### Future
- [ ] Standard Library
- [ ] Lists/Sets
- [ ] Maps/Dictionaries
- [ ] Compile time expressions
- [ ] Memory Management
