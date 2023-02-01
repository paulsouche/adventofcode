// DAY 2: Corruption Checksum
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day2Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n")
  .map((line) => line.split("\t").map(Number));

export const day2TestInput1 = [
  [5, 1, 9, 5],
  [7, 5, 3],
  [2, 4, 6, 8],
];

export function maxMin(array) {
  let total = 0;

  array.forEach((line) => {
    let min = Number.POSITIVE_INFINITY;
    let max = Number.NEGATIVE_INFINITY;
    line.forEach((n) => {
      min = Math.min(min, n);
      max = Math.max(max, n);
    });
    total += max - min;
  });
  return total;
}

export const day2TestInput2 = [
  [5, 9, 2, 8],
  [9, 4, 7, 3],
  [3, 8, 6, 5],
];

export function division(array) {
  let total = 0;

  array.forEach((line) => {
    let i;
    let j;
    const ilen = line.length;
    for (i = 0; i < ilen; i++) {
      for (j = 0; j < ilen; j++) {
        if (i === j) {
          continue;
        }

        const division = line[i] / line[j];
        const ent = Math.floor(division);
        if (division === ent) {
          total += division;
        }
      }
    }
  });
  return total;
}
