import { day1Input, half, sum } from "./days/day01-inverse-captcha/index.js";

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
