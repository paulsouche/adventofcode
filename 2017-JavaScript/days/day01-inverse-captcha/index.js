// DAY 1: Inverse Captcha
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day1Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
).toString();

export function sum(str) {
  let total = 0;
  const ilen = str.length;

  for (let i = 0; i < ilen; i++) {
    const stopIndex = i + 1 < ilen ? i + 1 : 0;
    if (str[i] === str[stopIndex]) {
      total += parseInt(str[i], 10);
    }
  }
  return total;
}

export function half(str) {
  let total = 0;
  const ilen = str.length;
  const half = ilen / 2;
  for (let i = 0; i < ilen; i++) {
    const stopIndex = half + i < ilen ? half + i : i - half;
    if (str[i] === str[stopIndex]) {
      total += parseInt(str[i], 10);
    }
  }
  return total;
}
