// DAY 10: Knot Hash
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day10Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split(",")
  .map(Number);

export const day10TestInput = [3, 4, 1, 5];

export function structure(length) {
  const structure = [];
  let i;
  for (i = 0; i < length; i++) {
    structure.push(i);
  }
  return structure;
}

function pinchAndTwist(arr, pos, slice) {
  const arrlen = arr.length;
  const twistlen = pos + slice;
  const toTwist = [];
  let i;
  for (i = pos; i < twistlen; i++) {
    toTwist.push(arr[i < arrlen ? i : i - arrlen]);
  }
  toTwist.reverse();
  for (i = pos; i < twistlen; i++) {
    arr[i < arrlen ? i : i - arrlen] = toTwist[i - pos];
  }
  return arr;
}

function bytesToLengths(input) {
  const lengths = [];
  const ilen = input.length;
  let i;
  for (i = 0; i < ilen; i++) {
    lengths.push(input.charCodeAt(i));
  }
  return lengths.concat([17, 31, 73, 47, 23]);
}

function toHexa(input) {
  let result = input.toString(16);
  if (result.length < 2) {
    result = `0${result}`;
  }
  return result;
}

export function simpleHash(structure, input) {
  const len = structure.length;
  let currentPosition = 0;
  let skipSize = 0;
  let result = structure;
  input.forEach((length) => {
    result = pinchAndTwist(result, currentPosition, length);
    currentPosition = currentPosition + length + skipSize;
    while (currentPosition >= len) {
      currentPosition -= len;
    }
    skipSize++;
  });

  return result[0] * result[1];
}

export function hash(struct, input) {
  const len = struct.length;
  const lengths = bytesToLengths(input);
  let currentPosition = 0;
  let skipSize = 0;
  let result = struct;
  let round;
  for (round = 0; round < 64; round++) {
    lengths.forEach((length) => {
      result = pinchAndTwist(result, currentPosition, length);
      currentPosition = currentPosition + length + skipSize;
      while (currentPosition >= len) {
        currentPosition -= len;
      }
      skipSize++;
    });
  }

  const denseHash = structure(16).map((index) => {
    const start = index * 16;
    return result.slice(start, start + 16).reduce((p, n) => p ^ n);
  });

  return denseHash.reduce((p, n) => p.concat(toHexa(n)), "");
}
