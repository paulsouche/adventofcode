import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { sum, value } = implem(input.license);
console.info(colors
  .yellow('Day 8 Memory maneuver part 1 result :'), colors
    .green(sum.toString()));

console.info(colors
  .yellow('Day 8 Memory maneuver part 2 result :'), colors
    .green(value.toString()));
