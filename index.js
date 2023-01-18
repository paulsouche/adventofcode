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
import {
  day8Input,
  day8TestInput,
  largest,
  largestEver,
} from "./days/day08-i-heard-you-like-registers/index.js";
import { day9Input, stream } from "./days/day09-stream-processing/index.js";
import {
  day10Input,
  day10TestInput,
  hash,
  simpleHash,
  structure,
} from "./days/day10-knot-hash/index.js";
import { day11Input, hexSteps } from "./days/day11-hex-ed/index.js";
import {
  connectionsCount,
  connectionsGroups,
  day12Input,
  day12TestInput,
} from "./days/day12-digital-plumber/index.js";
import {
  day13Input,
  day13TestInput,
  delay,
  severity,
} from "./days/day13-packet-scanners/index.js";
import { day14Input, usage } from "./days/day14-disk-defragmentation/index.js";
import { day15Input, judge } from "./days/day15-dueling-generators/index.js";
import {
  day16Input,
  day16Programs,
  day16TestInput,
  day16TestPrograms,
  promenade,
} from "./days/day16-permutation-promenade/index.js";
import {
  day17Input,
  neighbor0,
  spinlock,
} from "./days/day17-spinlock/index.js";
import {
  day18Input,
  day18TestInput,
  duet,
  duetWithDocumentation,
} from "./days/day18-duet/index.js";
import {
  day19Input,
  day19TestInput,
  tube,
} from "./days/day19-a-series-of-tubes/index..js";

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

// DAY 8: I Heard You Like Registers
console.assert(largest(day8TestInput) === 1, "largest(day8TestInput) NOK");
console.info("Day 8-1:", largest(day8Input));
console.assert(
  largestEver(day8TestInput) === 10,
  "largestEver(day8TestInput) NOK"
);
console.info("Day 8-2:", largestEver(day8Input));

// DAY 9: Stream Processing
console.assert(stream("{}").score === 1, "stream('{}').score NOK");
console.assert(stream("{{{}}}").score === 6, "stream('{{{}}}').score NOK");
console.assert(stream("{{},{}}").score === 5, "stream('{{},{}}').score NOK");
console.assert(
  stream("{{{},{},{{}}}}").score === 16,
  "stream('{{{},{},{{}}}}').score NOK"
);
console.assert(
  stream("{<a>,<a>,<a>,<a>}").score === 1,
  "stream('{<a>,<a>,<a>,<a>}').score NOK"
);
console.assert(
  stream("{{<ab>},{<ab>},{<ab>},{<ab>}}").score === 9,
  "stream('{{<ab>},{<ab>},{<ab>},{<ab>}}').score NOK"
);
console.assert(
  stream("{{<!!>},{<!!>},{<!!>},{<!!>}}").score === 9,
  "stream('{{<!!>},{<!!>},{<!!>},{<!!>}}').score NOK"
);
console.assert(
  stream("{{<a!>},{<a!>},{<a!>},{<ab>}}").score === 3,
  "stream('{{<a!>},{<a!>},{<a!>},{<ab>}}').score NOK"
);
console.info("Day 9-1:", stream(day9Input).score);
console.assert(stream("<>").garbage === 0, "stream('<>').garbage NOK");
console.assert(
  stream("<random characters>").garbage === 17,
  "stream('<random characters>').garbage NOK"
);
console.assert(stream("<<<<>").garbage === 3, "stream('<<<<>').garbage NOK");
console.assert(stream("<{!>}>").garbage === 2, "stream('<{!>}>').garbage NOK");
console.assert(stream("<!!>").garbage === 0, "stream('<!!>').garbage NOK");
console.assert(stream("<!!!>>").garbage === 0, "stream('<!!!>>').garbage NOK");
console.assert(
  stream("<{o'i!a,<{i<a>").garbage === 10,
  "stream('{o'i!a,<{i<a>').garbage NOK"
);
console.info("Day 9-2:", stream(day9Input).garbage);

// DAY 10: Knot Hash
console.assert(
  simpleHash(structure(5), day10TestInput) === 12,
  "simpleHash(structure(5), day10TestInput) NOK"
);
console.info("Day 10-1", simpleHash(structure(256), day10Input));
console.assert(
  hash(structure(256), "") === "a2582a3a0e66e6e86e3812dcb672a272",
  "hash(structure(256), '') NOK"
);
console.assert(
  hash(structure(256), "AoC 2017") === "33efeb34ea91902bb2f59c9920caa6cd",
  "hash(structure(256), 'AoC 2017') NOK"
);
console.assert(
  hash(structure(256), "1,2,3") === "3efbe78a8d82f29979031a4aa0b16a9d",
  "hash(structure(256), '1,2,3') NOK"
);
console.assert(
  hash(structure(256), "1,2,4") === "63960835bcdc130f0b66d7ff4f6a5a8e",
  "hash(structure(256), '1,2,4') NOK"
);
console.info("Day 10-2", hash(structure(256), day10Input.join()));

// DAY 11: Hex Ed
console.assert(
  hexSteps("ne,ne,ne").length === 3,
  "hexSteps('ne,ne,ne').length NOK"
);
console.assert(
  hexSteps("ne,ne,sw,sw").length === 0,
  "hexSteps('ne,ne,sw,sw').length NOK"
);
console.assert(
  hexSteps("ne,ne,s,s").length === 2,
  "hexSteps('ne,ne,s,s').length NOK"
);
console.assert(
  hexSteps("se,sw,se,sw,sw").length === 3,
  "hexSteps('se,sw,se,sw,sw').length NOK"
);
console.info("Day 11-1", hexSteps(day11Input).length);
console.assert(
  hexSteps("ne,ne,ne").furthest === 3,
  "hexSteps('ne,ne,ne').furthest NOK"
);
console.assert(
  hexSteps("ne,ne,sw,sw").furthest === 2,
  "hexSteps('ne,ne,sw,sw').furthest NOK"
);
console.assert(
  hexSteps("ne,ne,s,s").furthest === 2,
  "hexSteps('ne,ne,s,s').furthest NOK"
);
console.assert(
  hexSteps("se,sw,se,sw,sw").furthest === 3,
  "hexSteps('se,sw,se,sw,sw').furthest NOK"
);
console.info("Day 11-2", hexSteps(day11Input).furthest);

// DAY 12: Digital Plumber
console.assert(
  connectionsCount(day12TestInput) === 6,
  "connectionsCount(day12TestInput) NOK"
);
console.info("Day 12-1", connectionsCount(day12Input));
console.assert(
  connectionsGroups(day12TestInput) === 2,
  "connectionsGroups(day12TestInput) NOK"
);
console.info("Day 12-2", connectionsGroups(day12Input));

// DAY 13: Packet Scanners
console.assert(severity(day13TestInput) === 24, "severity(day13TestInput) NOK");
console.info("Day 13-1", severity(day13Input));
console.assert(delay(day13TestInput) === 10, "delay(day13TestInput) NOK");
console.info("Day 13-2", delay(day13Input));

// DAY 14: Disk Defragmentation
console.assert(usage("flqrgnkx").used === 8108, "usage('flqrgnkx').used NOK");
console.info("Day 14-1", usage(day14Input).used);
console.assert(usage("flqrgnkx").zones === 1242, "usage('flqrgnkx').zones NOK");
console.info("Day 14-2", usage(day14Input).zones);

// DAY 15: Dueling Generators
console.assert(judge(65, 8921) === 588, "judge(65, 8921) NOK");
console.info("Day 15-1", judge(day15Input.A, day15Input.B));
console.assert(
  judge(65, 8921, 4, 8, 5_000_000) === 309,
  "judge(65, 8921, 4, 8, 5_000_000) NOK"
);
console.info("Day 15-2", judge(day15Input.A, day15Input.B, 4, 8, 5_000_000));

// DAY 16 : Permutation Promenade
console.assert(
  promenade(day16TestPrograms, day16TestInput).programs === "baedc",
  "promenade(day16TestPrograms, day16TestInput) NOK"
);
console.info("Day 16-1", promenade(day16Programs, day16Input).programs);
// This is reached in 42 operations 1_000_000_000 % 42 = 34
console.assert(
  promenade(day16Programs, day16Input, 1_000_000_000).modulo === 42,
  "promenade(day16Programs, day16Input, 1_000_000_000).modulo NOK"
);
console.info("Day 16-2", promenade(day16Programs, day16Input, 34).programs);

// DAY 17 : Spinlock
console.assert(spinlock(3, 2017) === 638, "spinlock(3, 2017) NOK");
console.info("Day 17-1", spinlock(day17Input, 2017));
console.info("Day 17-2", neighbor0(day17Input, 50_000_000));

// DAY 18: Duet
console.assert(duet(day18TestInput) === 4, "duet(day18TestInput) NOK");
console.info("Day 18-1", duet(day18Input));
console.info("Day 18-2", duetWithDocumentation(day18Input));

// DAY 19: A Series of Tubes
console.assert(
  tube(day19TestInput).word === "ABCDEF",
  "tube(day19TestInput).word NOK"
);
console.info("Day 19-1", tube(day19Input).word);
console.assert(
  tube(day19TestInput).totalSteps === 38,
  "tube(day19TestInput).step NOK"
);
console.info("Day 19-2", tube(day19Input).totalSteps);
