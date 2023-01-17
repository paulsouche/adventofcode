import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { firstCrash, lastCart } = implem(input);
console.info(colors
  .yellow('Day 13 Mine cart madness part 1 result :'), colors
    .green(firstCrash));

console.info(colors
  .yellow('Day 13 Mine cart madness part 2 result :'), colors
    .green(lastCart));
