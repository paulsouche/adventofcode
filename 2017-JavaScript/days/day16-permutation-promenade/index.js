// DAY 16 : Permutation Promenade
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day16Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
).toString();

export const day16Programs = [
  "a",
  "b",
  "c",
  "d",
  "e",
  "f",
  "g",
  "h",
  "i",
  "j",
  "k",
  "l",
  "m",
  "n",
  "o",
  "p",
];

export const day16TestInput = "s1,x3/4,pe/b";

export const day16TestPrograms = ["a", "b", "c", "d", "e"];

function spin(arr, nbrOfItems) {
  const spliced = arr.splice(arr.length - nbrOfItems, nbrOfItems);
  return [...spliced, ...arr];
}

function exchange(arr, index1, index2) {
  if (
    typeof index1 !== "number" ||
    typeof index2 !== "number" ||
    index1 < 0 ||
    index2 < 0
  ) {
    throw new Error("Invalid exchange indexes");
  }
  const swap = arr[index1];
  arr[index1] = arr[index2];
  arr[index2] = swap;
  return arr;
}

function partner(arr, value1, value2) {
  if (typeof value1 !== "string" || typeof value2 !== "string") {
    throw new Error("Invalid partner programs");
  }
  return exchange(
    arr,
    arr.findIndex((p) => p === value1),
    arr.findIndex((p) => p === value2)
  );
}

export function promenade(programs, input, times = 1) {
  let i = 0;
  let modulo;
  const steps = input.split(",");
  programs = [...programs];
  const step0 = programs.join("");
  while (i < times) {
    steps.forEach((step) => {
      const move = step.charAt(0);
      switch (move) {
        case "s":
          programs = spin(programs, parseInt(step.replace("s", ""), 10));
          break;
        case "x":
          programs = exchange(
            programs,
            ...step
              .replace("x", "")
              .split("/")
              .map((s) => parseInt(s, 10))
          );
          break;
        case "p":
          programs = partner(programs, ...step.replace("p", "").split("/"));
          break;
        default:
          throw new Error(`Invalid input ${step}`);
      }
    });
    i++;
    if (programs.join("") === step0) {
      // infinite loop;
      modulo = i;
      break;
    }
  }
  return {
    modulo,
    programs: programs.join(""),
  };
}
