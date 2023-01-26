// DAY 23: Coprocessor Conflagration
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day23Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const numberOfMul = (input) => {
  const setb = input.find((inst) => inst.startsWith("set b "));
  const [, , value] = setb.split(" ");
  return (+value - 2) ** 2;
};

export const programOutput = (input) => {
  // 79
  const [, , bValue] = input
    .find((inst) => inst.startsWith("set b "))
    .split(" ");
  // 100_000 , 17
  const [[, , subBValue], [, , stepBValue]] = input
    .filter((inst) => inst.startsWith("sub b "))
    .map((inst) => inst.split(" "));
  // 100
  const [, , mulBValue] = input
    .find((inst) => inst.startsWith("mul b "))
    .split(" ");
  // 17_000
  const [, , subCValue] = input
    .find((inst) => inst.startsWith("sub c "))
    .split(" ");

  let nonprimes = 0;
  let start = bValue * +mulBValue + Math.abs(subBValue);
  for (
    let n = start;
    n <= start + Math.abs(+subCValue);
    n += Math.abs(+stepBValue)
  ) {
    let d = 2;
    while (n % d !== 0) d++;
    if (n !== d) nonprimes++;
  }

  return nonprimes;
};
