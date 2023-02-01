// DAY 15: Dueling Generators
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day15Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n")
  .reduce((generators, line) => {
    const [, name, val] = /^Generator\s([A-Z])\sstarts\swith\s(\d+)$/.exec(
      line
    );
    generators[name] = +val;
    return generators;
  }, {});

const factorA = 16807;
const factorB = 48271;
const rem = 2147483647;
const next = (val, factor, div) =>
  (val = (val * factor) % rem) % div ? next(val, factor, div) : val;

export function judge(startA, startB, divA = 1, divB = 1, times = 40_000_000) {
  let c = 0;
  for (let i = 0; i < times; i++) {
    startA = next(startA, factorA, divA);
    startB = next(startB, factorB, divB);
    c += (startA & 0xffff) === (startB & 0xffff) ? 1 : 0;
  }
  return c;
}
