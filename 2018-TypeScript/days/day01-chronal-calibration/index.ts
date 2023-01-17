import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { firstFrequencyReachedTwice, resultingFrequency } = implem(input);
console.info(colors
  .yellow('Day 1 Chronal calibration part 1 result :'), colors
    .green(resultingFrequency.toString()));

console.info(colors
  .yellow('Day 1 Chronal calibration part 2 result :'), colors
    .green(firstFrequencyReachedTwice.toString()));
