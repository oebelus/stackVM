## A Turing-Complete Stack-Based Virtual Machine

The Stack-Based Virtual Machine (SVM) is a Turing Complete virtual machine created using C#. It's designed to handle various tasks like arithmetic operations, manipulating stacks, controlling flow, and managing memory.

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

The program above is parsed and turned into bytecode before getting executed by the VM;

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
