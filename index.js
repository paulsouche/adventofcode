import { day1Input, half, sum } from "./days/day01-inverse-captcha/index.js";
import {
  day2Input,
  day2TestInput1,
  day2TestInput2,
  division,
  maxMin,
} from "./days/day02-corruption-checksum/index.js";

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
