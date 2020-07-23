# Abaddon

Abaddon is aspiring to be a simple logic engine for a logical programming game, thus simulating a minimal programming language.
Currently, it is written entirely in C#.

## The Concept

Abaddon evolved from the idea of a Turing Machine with two-dimensional memory array (the Board) and minimal syntax that is still Turing-complete.

### Board

The Board is meant to represent a two-dimensional memory space. The memory entry type is generic (`TBoardEntry`) to allow type flexibility and enables inputing logic for memory value change observation.

### Syntax

In its current state, the syntax allows movement of the memory pointer, copying values to and from memory, jump and comparison operations.
Every operation is represented by a single character, sometimes with an additional integer parameter.
Concatenated, they compose a program.

### Example

For a complete example, please see the [Simple Scenario](AbaddonTests/Scenarios/SimpleThreeByThreeScenario.cs) in AbaddonTests.

## Next steps and ideas

* create easier type construction - fluent interface?
* test value change observability
* n-dimensional boards
* implement undo / step back

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

Licensed under the [MIT License](LICENSE)
