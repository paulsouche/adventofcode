// DAY 11: Hex Ed
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day11Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
).toString();

const reverseSteps = {
  n: "s",
  ne: "sw",
  nw: "se",
  s: "n",
  se: "nw",
  sw: "ne",
};

const combineSteps = {
  n: { sw: "nw", se: "ne" },
  ne: { nw: "n", s: "se" },
  nw: { s: "sw", ne: "n" },
  s: { ne: "se", nw: "sw" },
  se: { n: "ne", sw: "s" },
  sw: { se: "s", n: "nw" },
};

export function hexSteps(input) {
  const steps = input.split(",");
  const flattenSteps = [];
  let furthest = Number.NEGATIVE_INFINITY;

  steps.forEach((step) => {
    const reverseStep = reverseSteps[step];
    const indexToReverse = flattenSteps.findIndex((s) => s === reverseStep);
    if (indexToReverse >= 0) {
      flattenSteps.splice(indexToReverse, 1);
      return;
    }

    const combineStep = combineSteps[step];
    const indexToCombine = flattenSteps.findIndex((s) => !!combineStep[s]);
    if (indexToCombine >= 0) {
      const [combinedStep] = flattenSteps.splice(indexToCombine, 1);
      flattenSteps.push(combineStep[combinedStep]);
      return;
    }

    flattenSteps.push(step);

    furthest = Math.max(flattenSteps.length, furthest);
  });

  const length = flattenSteps.length;

  return {
    furthest,
    length,
  };
}
