import { day1Input, half, sum } from "./days/day01-inverse-captcha/index.js";
import {
  day2Input,
  day2TestInput1,
  day2TestInput2,
  division,
  maxMin,
} from "./days/day02-corruption-checksum/index.js";
import {
  day3Input,
  manhattanDistance,
  manhattanValue,
} from "./days/day03-spiral-memory/index.js";
import {
  day4Input,
  isValid,
  valids,
} from "./days/day04-high-entropy-passphrases/index.js";
import {
  day5Input,
  steps,
  strangerSteps,
} from "./days/day05-a-maze-of-twisty-trampolines--all-alike/index.js";
import {
  day6Input,
  redistribution,
} from "./days/day06-memory-reallocation/index.js";
import {
  day7Input,
  day7TestInput,
  rootProgram,
  weight,
} from "./days/day07-recursive-circus/index.js";

// DAY 1: Inverse Captcha
console.assert(sum("1122") === 3, "sum(1122) NOK");
console.assert(sum("1111") === 4, "sum(1111) NOK");
console.assert(sum("1324") === 0, "sum(1324) NOK");
console.assert(sum("91212129") === 9, "sum(91212129) NOK");
console.info("Day 1-1:", sum(day1Input));
console.assert(half("1212") === 6, "half(1212) NOK");
console.assert(half("1221") === 0, "half(1221) NOK");
console.assert(half("123425") === 4, "half(123425) NOK");
console.assert(half("123123") === 12, "half(123123) NOK");
console.assert(half("12131415") === 4, "half(12131415) NOK");
console.info("Day 1-2:", half(day1Input));

// DAY 2: Corruption Checksum
console.assert(maxMin(day2TestInput1) === 18, "maxMin(day2TestInput1): NOK");
console.info("Day 2-1:", maxMin(day2Input));
console.assert(division(day2TestInput2) === 9, "division(day2TestInput2): NOK");
console.info("Day 2-2:", division(day2Input));

// DAY 3: Spiral Memory
console.assert(manhattanDistance(1) === 0, "manhattanDistance(1): NOK");
console.assert(manhattanDistance(12) === 3, "manhattanDistance(12): NOK");
console.assert(manhattanDistance(23) === 2, "manhattanDistance(23): NOK");
console.assert(manhattanDistance(1024) === 31, "manhattanDistance(1024): NOK");
console.info("Day 3-1:", manhattanDistance(day3Input));
console.assert(manhattanValue(747) === 806, "manhattanValue(747): NOK");
console.info("Day 3-2:", manhattanValue(day3Input));

// DAY 4: High-Entropy Passphrases
console.assert(isValid("aa bb cc dd ee"), "isValid(aa bb cc dd ee) NOK");
console.assert(!isValid("aa bb cc dd aa"), "isValid(aa bb cc dd aa) NOK");
console.assert(isValid("aa bb cc dd aaa"), "isValid(aa bb cc dd aaa) NOK");
console.info("Day 4-1:", valids(day4Input));
console.assert(isValid("abcde fghij", true), "isValid(abcde fghij) NOK");
console.assert(
  !isValid("abcde xyz ecdab", true),
  "isValid(abcde xyz ecdab) NOK"
);
console.assert(
  isValid("a ab abc abd abf abj", true),
  "isValid(a ab abc abd abf abj) NOK"
);
console.assert(
  isValid("iiii oiii ooii oooi oooo", true),
  "isValid(iiii oiii ooii oooi oooo) NOK"
);
console.assert(
  !isValid("oiii ioii iioi iiio", true),
  "isValid(a ab abc abd abf abj) NOK"
);
console.info("Day 4-2:", valids(day4Input, true));

// DAY 5: A Maze of Twisty Trampolines, All Alike
console.assert(steps([0, 3, 0, 1, -3]) === 5, "steps([0, 3, 0, 1, -3]) NOK");
console.info("Day 5-1:", steps(day5Input));
console.assert(
  strangerSteps([0, 3, 0, 1, -3]) === 10,
  "strangerSteps([0, 3, 0, 1, -3]) NOK"
);
console.info("Day 5-2:", strangerSteps(day5Input));

// DAY 6: Memory Reallocation
console.assert(
  redistribution([0, 2, 7, 0]).allocations === 5,
  "redistribution([0, 2, 7, 0]).allocations NOK"
);
console.info("Day 6-1:", redistribution(day6Input).allocations);
console.assert(
  redistribution([0, 2, 7, 0]).size === 4,
  "redistribution([0, 2, 7, 0]).size NOK"
);
console.info("Day 6-2:", redistribution(day6Input).size);

// DAY 7: Recursive Circus
console.assert(
  rootProgram(day7TestInput) === "tknk",
  "rootProgram(day7TestInput) NOK"
);
console.info("Day 7-1:", rootProgram(day7Input));
console.assert(weight(day7TestInput) === 60, "findWeight(day7TestInput) NOK");
console.info("Day 7-2:", weight(day7Input));
