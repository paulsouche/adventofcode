// DAY 4: High-Entropy Passphrases
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day4Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export function isValid(str, sort = false) {
  const words = str.split(" ");
  const set = new Set();
  return words.every((w) => {
    const sortedW = sort ? w.split("").sort().join("") : w;
    if (set.has(sortedW)) {
      return false;
    }
    set.add(sortedW);
    return true;
  });
}

export function valids(input, anagram = false) {
  let total = 0;
  input.forEach((pass) => {
    if (isValid(pass, anagram)) {
      total++;
    }
  });
  return total;
}
