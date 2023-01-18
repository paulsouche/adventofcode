// DAY 13: Packet Scanners
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day13Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day13TestInput = ["0: 3", "1: 2", "4: 4", "6: 4"];

const caughtByGuard =
  (delay) =>
  ([d, r]) =>
    (delay + d) % (2 * (r - 1)) === 0;

export const severity = (input) =>
  input
    .map((s) => s.match(/\d+/g).map(Number))
    .filter(caughtByGuard(0))
    .reduce((n, [d, r]) => n + d * r, 0);

export const delay = (input) => {
  const guards = input.map((s) => s.match(/\d+/g).map(Number));
  let d = -1;
  while (guards.some(caughtByGuard(++d)));
  return d;
};
