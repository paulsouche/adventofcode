// DAY 5: A Maze of Twisty Trampolines, All Alike
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day5Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n")
  .map(Number);

function doSteps(instructions, incrFunc) {
  const data = [...instructions];
  let pc = 0;
  let count = 0;
  while (pc < data.length) {
    const jmp = data[pc];
    data[pc] += incrFunc(jmp);
    pc += jmp;
    count++;
  }

  return count;
}

export function steps(input) {
  return doSteps(input, () => 1);
}

export function strangerSteps(input) {
  return doSteps(input, (jmp) => (jmp >= 3 ? -1 : 1));
}
