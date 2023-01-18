// DAY 14: Disk Defragmentation

import { hash, structure } from "../day10-knot-hash/index.js";

export const day14Input = "vbqugkhl";

const convertBase = Object.assign(
  (num) => ({
    from: (baseFrom) => ({
      to: (baseTo) => parseInt(num.toString(), baseFrom).toString(baseTo),
    }),
  }),
  {
    // binary to decimal
    bin2dec: (num) => convertBase(num).from(2).to(10),
    // binary to hexadecimal
    bin2hex: (num) => convertBase(num).from(2).to(16),
    // decimal to binary
    dec2bin: (num) => convertBase(num).from(10).to(2),
    // decimal to hexadecimal
    dec2hex: (num) => convertBase(num).from(10).to(16),
    // hexadecimal to binary
    hex2bin: (num) => convertBase(num).from(16).to(2),
    // hexadecimal to decimal
    hex2dec: (num) => convertBase(num).from(16).to(10),
  }
);

function countGroups(grid) {
  const str = (x, y) => x + "," + y;
  const used = (row, col) => grid[row][col] === "1";
  const groups = new Map();

  function search(row, col, n) {
    groups.set(str(row, col), n);
    [
      [0, -1],
      [-1, 0],
      [0, 1],
      [1, 0],
    ].forEach(([rowOff, colOff]) => {
      const [newRow, newCol] = [row + rowOff, col + colOff];
      if (
        newRow >= 0 &&
        newRow < grid.length &&
        newCol >= 0 &&
        newCol < grid.length &&
        !groups.has(str(newRow, newCol)) &&
        used(row, col)
      ) {
        search(row + rowOff, col + colOff, n);
      }
    });
  }

  let grpCount = 0;
  for (let row = 0; row < grid.length; row++) {
    for (let col = 0; col < grid.length; col++) {
      if (groups.has(str(row, col))) {
        continue;
      }
      if (used(row, col)) {
        search(row, col, grpCount);
        grpCount++;
      }
    }
  }
  return grpCount;
}

export function usage(input) {
  let used = "";
  let i;
  const grid = [];
  for (i = 0; i < 128; i++) {
    const line = hash(structure(256), `${input}-${i}`)
      .split("")
      .map((d) => convertBase.hex2bin(d).padStart(4, "0"))
      .join("");
    used = used.concat(line);
    grid.push(line);
  }

  return {
    used: used.replace(/0/g, "").length,
    zones: countGroups(grid),
  };
}
