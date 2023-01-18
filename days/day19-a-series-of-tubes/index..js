// DAY 19: A Series of Tubes
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day19Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day19TestInput = [
  "     |          ",
  "     |  +--+    ",
  "     A  |  C    ",
  " F---|----E|--+ ",
  "     |  |  |  D ",
  "     +B-+  +--+ ",
  "                ",
];

const DIRECTIONS = {
  TOP: 0,
  RIGHT: 1,
  BOTTOM: 2,
  LEFT: 3,
};

export function tube(input) {
  const map = input.map((x) => x.split(""));
  let x = map[0].indexOf("|");
  let y = 0;
  let direction = DIRECTIONS.BOTTOM;
  let steps = 0;
  const order = [];

  function changeDir(x, y, direction) {
    var val = map[y][x];
    if (val >= "A" && val <= "Z") {
      order.push(val);
    }
    if (val == "+") {
      if (map[y - 1][x] !== " " && direction != DIRECTIONS.BOTTOM) {
        return DIRECTIONS.TOP;
      }
      if (map[y + 1][x] !== " " && direction != DIRECTIONS.TOP) {
        return DIRECTIONS.BOTTOM;
      }
      if (map[y][x - 1] !== " " && direction != DIRECTIONS.RIGHT) {
        return DIRECTIONS.LEFT;
      }
      if (map[y][x + 1] !== " " && direction != DIRECTIONS.LEFT) {
        return DIRECTIONS.RIGHT;
      }
    }
    return direction;
  }

  while (true) {
    steps++;
    switch (direction) {
      case DIRECTIONS.TOP:
        y--;
        break;
      case DIRECTIONS.RIGHT:
        x++;
        break;
      case DIRECTIONS.BOTTOM:
        y++;
        break;
      case DIRECTIONS.LEFT:
        x--;
        break;
    }
    direction = changeDir(x, y, direction);
    if (map[y][x] === " ") {
      break;
    }
  }

  return {
    totalSteps: steps,
    word: order.join(""),
  };
}
