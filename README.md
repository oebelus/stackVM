## A Turing-Complete Stack-Based Virtual Machine

The Stack-Based Virtual Machine (SstackVM) is a Turing Complete virtual machine created using C#. It's designed to handle various tasks like arithmetic operations, manipulating stacks, control flow, and managing memory.

> It executes bytecode instructions, with the possibility to convert human-readable mnemonics to bytecode. The program can either look like this:

```c#
byte[] program = [0, 0, 0, 0, 5, // PUSH 5
    0, 0, 0, 0, 5, // PUSH 5
    2, // ADD
    0, 0, 0, 0 ,7, // PUSH 7
    3 // SUB
];
```

> Or like this:

```c#
string program = "PUSH 5 PUSH 7 CALL <add> PUSH 4 PUSH 8 CALL <sub> HALT </add> ADD RET </sub> SUB RET"
```

The program above is parsed and turned into bytecode before getting executed by the stackVM;

> It supports:

- Stack manipulation: `PUSH`, `POP`;
- Binary operations: `ADD`, `SUB`, `MUL`, `DIV`, `NEG`, `EXP`, `MOD`, `LET`, `GT`, `EQ`;
- Logical operators: `AND`, `OR`, `NOT`, `XOR`, `LS`, `RS`;
- Local and Global Storage: `LOAD`, `GLOAD`, `STORE`, `GSTORE`;
- Control Flow: `JUMP`, `CJUMP`;
- Function calls : `CALL`, `RET`;
- A Stop: `HALT`;

The execution of the first program:

```
STACK:

[ 25 25 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ]

Head: 2

MEMORY:


[ 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ]

PROGRAM:

[ 0 0 0 0 25 0 0 0 0 25 ]

Counter: 10

CALL STACK:

[ 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ]

Call Stack Pointer: 0
```

- More code:

```
Equality: PUSH 5 PUSH 5 EQ PUSH 23 CJUMP PUSH 0 HALT PUSH 1 HALT
Even: PUSH 2 PUSH 7 MOD PUSH 23 CJUMP PUSH 1 HALT PUSH 0 HALT
Max: PUSH 23 PUSH 7 PUSH 4 PUSH 69 CALL <func> CALL <func> CALL <func> HALT </func> PUSH 0 STORE PUSH 1 STORE PUSH 0 LOAD PUSH 1 LOAD GT PUSH 74 CJUMP PUSH 0 LOAD RET PUSH 1 LOAD RET
Min: PUSH 23 PUSH 7 PUSH 4 PUSH 0 CALL <func> CALL <func> CALL <func> HALT </func> PUSH 0 STORE PUSH 1 STORE PUSH 0 LOAD PUSH 1 LOAD GT PUSH 74 CJUMP PUSH 1 LOAD RET PUSH 0 LOAD RET
```
