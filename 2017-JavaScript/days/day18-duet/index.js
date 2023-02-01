// DAY 18: Duet
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day18Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day18TestInput = [
  "set a 1",
  "add a 2",
  "mul a a",
  "mod a 5",
  "snd a",
  "set a 0",
  "rcv a",
  "jgz a -1",
  "set a 1",
  "jgz a -2",
];

export function duet(input) {
  const registers = new Map();
  let i = 0;
  const sounds = [];
  let rcv;

  while (i < input.length) {
    const [instruction, variable, value] = input[i].split(" ");
    const prev = registers.get(variable) || 0;
    const mapValue = registers.get(value);
    const instValue =
      typeof mapValue === "number" ? mapValue : parseInt(value, 10);
    switch (instruction) {
      case "set":
        registers.set(variable, instValue);
        break;
      case "add":
        registers.set(variable, prev + instValue);
        break;
      case "mul":
        registers.set(variable, prev * instValue);
        break;
      case "mod":
        registers.set(variable, prev % instValue);
        break;
      case "snd":
        sounds.push(prev);
        break;
      case "rcv":
        if (prev !== 0) {
          [rcv] = sounds.slice(-1);
        }
        break;
      case "jgz":
        if (prev > 0) {
          i += instValue - 1;
        }
        break;
      default:
        throw new Error(`Unknown instruction ${instruction}`);
    }
    if (typeof rcv !== "undefined") {
      break;
    }
    i++;
  }
  return rcv;
}

function getVal(rs, v) {
  const num = parseInt(v, 10);
  return isNaN(num) ? rs[v] : num;
}

class Program {
  constructor(id, instructions) {
    this.sent = 0;
    this.instructions = instructions;
    this.id = id;
    this.queue = [];
    this.registers = { p: id };
    this.index = 0;
  }

  run() {
    let locked = true;
    const registers = this.registers;

    while (true) {
      const [instruction, variable, value] = this.instructions[this.index];
      switch (instruction) {
        case "set":
          registers[variable] = getVal(registers, value);
          break;
        case "add":
          registers[variable] += getVal(registers, value);
          break;
        case "mul":
          registers[variable] *= getVal(registers, value);
          break;
        case "mod":
          registers[variable] %= getVal(registers, value);
          break;
        case "snd":
          this.sent++;
          this.link.queue.push(getVal(registers, variable));
          break;
        case "rcv":
          if (this.queue.length > 0) {
            registers[variable] = this.queue.shift();
          } else {
            return locked;
          }
          break;
        case "jgz":
          if (getVal(registers, variable) > 0) {
            this.index += getVal(registers, value) - 1;
          }
          break;
      }
      this.index++;
      locked = false;
    }
  }

  linkProgram(p) {
    this.link = p;
  }
}

export function duetWithDocumentation(input) {
  const instructions = input.map((l) => l.split(" "));

  const pA = new Program(0, instructions);
  const pB = new Program(1, instructions);

  pA.linkProgram(pB);
  pB.linkProgram(pA);

  while (true) {
    const aLocked = pA.run();
    const bLocked = pB.run();

    if (aLocked && bLocked) {
      break;
    }
  }
  return pB.sent;
}
