import colors from 'colors';
import implem from './lib';

const { largestSquare3x3, largestSquareNxN } = implem(8141);
console.info(colors
  .yellow('Day 10 Chronal charge part 1 result :'), colors
    .green(largestSquare3x3));

console.info(colors
  .yellow('Day 10 Chronal charge part 2 result :'), colors
    .green(largestSquareNxN));
