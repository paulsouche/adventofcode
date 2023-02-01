// DAY 25: The Halting Problem
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day25Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n\n");

export const day25TestInput = [
  "Begin in state A.\nPerform a diagnostic checksum after 6 steps.",
  "In state A:\n  If the current value is 0:\n    - Write the value 1.\n    - Move one slot to the right.\n    - Continue with state B.\n  If the current value is 1:\n    - Write the value 0.\n    - Move one slot to the left.\n    - Continue with state B.",
  "In state B:\n  If the current value is 0:\n    - Write the value 1.\n    - Move one slot to the left.\n    - Continue with state A.\n  If the current value is 1:\n    - Write the value 1.\n    - Move one slot to the right.\n    - Continue with state A.",
];

class Program {
  static START_REGEX =
    /^Begin\sin\sstate\s([A-Z])\.\nPerform\sa\sdiagnostic\schecksum\safter\s(\d+)\ssteps\.$/;

  static INSTRUCTION_REGEX =
    /^In\sstate\s([A-Z]):\n\s{2}If\sthe\scurrent\svalue\sis\s(0|1):\n\s{4}\-\sWrite\sthe\svalue\s(0|1)\.\n\s{4}\-\sMove\sone\sslot\sto\sthe\s(right|left)\.\n\s{4}\-\sContinue\swith\sstate\s([A-Z])\.\n\s{2}If\sthe\scurrent\svalue\sis\s(0|1):\n\s{4}\-\sWrite\sthe\svalue\s(0|1)\.\n\s{4}\-\sMove\sone\sslot\sto\sthe\s(right|left)\.\n\s{4}\-\sContinue\swith\sstate\s([A-Z])\.$/;

  constructor(input) {
    const [start, ...instructions] = input;

    const [, startState, steps] = Program.START_REGEX.exec(start);
    this.tape = new Map();
    this.cursor = 0;
    this.state = startState;
    this.steps = steps;

    this.states = {};

    instructions.forEach((instruction) => {
      const [
        ,
        state,
        valueA,
        writeA,
        moveA,
        continueA,
        valueB,
        writeB,
        moveB,
        continueB,
      ] = Program.INSTRUCTION_REGEX.exec(instruction);

      this.states[state] = () => {
        const value = this.tape.get(this.cursor) || 0;
        switch (value) {
          case +valueA:
            this.tape.set(this.cursor, +writeA);
            this.cursor += moveA === "left" ? -1 : +1;
            return continueA;
          case +valueB:
            this.tape.set(this.cursor, +writeB);
            this.cursor += moveB === "left" ? -1 : +1;
            return continueB;
          default:
            throw new Error(`Unknown value ${value}`);
        }
      };
    });
  }

  run() {
    for (let i = 0; i < this.steps; i++) {
      this.state = this.states[this.state]();
    }

    return Array.from(this.tape.values()).filter((v) => v === 1).length;
  }
}

export const diagnosticChecksum = (input) => new Program(input).run();
