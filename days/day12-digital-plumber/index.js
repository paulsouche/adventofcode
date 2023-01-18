// DAY 12: Digital Plumber
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day12Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day12TestInput = [
  "0 <-> 2",
  "1 <-> 1",
  "2 <-> 0, 3, 4",
  "3 <-> 2, 4",
  "4 <-> 2, 3, 6",
  "5 <-> 6",
  "6 <-> 4, 5",
];

const programRegex = /(\d*)\s<->\s(.*)/;

function parseInput(input) {
  return input.map((strProgram) => {
    const matches = programRegex.exec(strProgram);
    if (matches) {
      return {
        connections: matches[2].split(",").map((c) => parseInt(c, 10)),
        id: parseInt(matches[1], 10),
      };
    }
    throw new Error(`Invalid input ${strProgram}`);
  });
}

function connectedPrograms(programs, connections) {
  let length = Number.NEGATIVE_INFINITY;

  while (length < connections.length) {
    length = connections.length;
    programs.forEach((program) => {
      if (program.connections.some((c) => connections.includes(c))) {
        connections.push(
          ...[program.id, ...program.connections].filter(
            (pid) => !connections.includes(pid)
          )
        );
      }
    });
  }

  return connections;
}

export function connectionsCount(input) {
  return connectedPrograms(parseInput(input), [0]).length;
}

export function connectionsGroups(input) {
  const programs = parseInput(input);
  let groups = 0;

  while (programs.length > 0) {
    const [program] = programs;
    connectedPrograms(programs, [program.id]).forEach((id) => {
      const index = programs.findIndex((p) => p.id === id);
      if (index >= 0) {
        programs.splice(index, 1);
      }
    });
    groups++;
  }

  return groups;
}
