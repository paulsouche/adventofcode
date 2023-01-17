import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { largestArea, safestArea } = implem(input, 10000);
console.info(colors
  .yellow('Day 6 Chronal coordinates part 1 result :'), colors
    .green(largestArea.toString()));
console.info(colors
  .yellow('Day 6 Chronal coordinates part 2 result :'), colors
    .green(safestArea.toString()));
