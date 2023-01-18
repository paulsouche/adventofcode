// DAY 9: Stream Processing
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day9Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
).toString();

export function stream(input) {
  let i = 0;
  let step = 0;
  let score = 0;
  let garbage = 0;
  while (i < input.length) {
    switch (input[i]) {
      case "!":
        i++;
        break;
      case "<":
        i++;
        let exit = false;
        while (!exit && i < input.length) {
          switch (input[i]) {
            case "!":
              i += 2;
              break;
            case ">":
              exit = true;
              break;
            default:
              garbage++;
              i++;
              break;
          }
        }
        break;
      case "{":
        step++;
        score += step;
        break;
      case "}":
        step--;
        break;
      default:
    }
    i++;
  }
  return { score, garbage };
}
