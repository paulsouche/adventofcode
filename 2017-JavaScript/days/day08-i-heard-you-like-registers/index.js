// DAY 8: I Heard You Like Registers
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day8Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day8TestInput = [
  "b inc 5 if a > 1",
  "a inc 1 if b < 5",
  "c dec -10 if a >= 1",
  "c inc -20 if c == 10",
];

const operationsReg =
  /([a-z]*)\s([a-z]*)\s(-?\d*)\sif\s([a-z]*)\s([^\s]*)\s(-?\d*)/;

function parseOperations(input) {
  return input.map((op) => {
    const matches = operationsReg.exec(op);
    if (matches) {
      return {
        condition: {
          operator: matches[5],
          value: parseInt(matches[6], 10),
          variable: matches[4],
        },
        offset: parseInt(matches[3], 10),
        operator: matches[2],
        variable: matches[1],
      };
    }
    throw new Error(`Invalid input ${op}`);
  });
}

function initializeMap(operations) {
  const map = new Map();
  operations.forEach((o) => {
    const variable = o.variable;
    if (!map.has(variable)) {
      map.set(variable, 0);
    }
    const conditionVariabe = o.condition.variable;
    if (!map.has(conditionVariabe)) {
      map.set(conditionVariabe, 0);
    }
  });
  return map;
}

function isConditionFullfilled(condition, map) {
  const variable = map.get(condition.variable) || 0;
  const operator = condition.operator;
  const value = condition.value;
  switch (operator) {
    case "==":
      return variable === value;
    case "!=":
      return variable !== value;
    case ">":
      return variable > value;
    case "<":
      return variable < value;
    case ">=":
      return variable >= value;
    case "<=":
      return variable <= value;
    default:
      throw new Error(`Unknown condition operator ${operator}`);
  }
}

function doOperation(operation, map) {
  const variable = operation.variable;
  const value = map.get(variable) || 0;
  const operator = operation.operator;
  const offset = operation.offset;
  switch (operator) {
    case "inc":
      return map.set(variable, value + offset);
    case "dec":
      return map.set(variable, value - offset);
    default:
      throw new Error(`Unknown condition operator ${operator}`);
  }
}

function doOperations(operations, map) {
  let largest = Number.NEGATIVE_INFINITY;
  operations.forEach((op) => {
    if (isConditionFullfilled(op.condition, map)) {
      doOperation(op, map);
      largest = findMapLargest(map, largest);
    }
  });
  return largest;
}

function findMapLargest(map, largest = Number.NEGATIVE_INFINITY) {
  const iterator = map.values();
  let iterat = iterator.next();
  while (!iterat.done) {
    largest = Math.max(iterat.value, largest);
    iterat = iterator.next();
  }
  return largest;
}

export function largest(input) {
  const operations = parseOperations(input);
  const mapOfValues = initializeMap(operations);
  doOperations(operations, mapOfValues);
  return findMapLargest(mapOfValues);
}

export function largestEver(input) {
  const operations = parseOperations(input);
  const mapOfValues = initializeMap(operations);
  return doOperations(operations, mapOfValues);
}
