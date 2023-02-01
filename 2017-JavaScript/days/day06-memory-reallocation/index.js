// DAY 6: Memory Reallocation
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day6Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\t")
  .map(Number);

export function redistribution(input) {
  const map = new Map();
  const ilen = input.length;
  let allocations = 0;
  let size;

  while (true) {
    const key = input.join();
    if (map.has(key)) {
      size = allocations - map.get(key);
      break;
    }
    map.set(key, allocations);
    let i;
    let bankIndex = 0;
    let bankMax = Number.NEGATIVE_INFINITY;
    for (i = 0; i < ilen; i++) {
      if (input[i] > bankMax) {
        bankIndex = i;
        bankMax = input[bankIndex];
      }
    }

    let memory = input[bankIndex];
    input[bankIndex] = 0;
    while (memory > 0) {
      bankIndex = bankIndex + 1 < ilen ? bankIndex + 1 : 0;
      input[bankIndex]++;
      memory--;
    }

    allocations++;
  }

  return { allocations, size };
}
