import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { strategy1, strategy2 } = implem(input);
console.info(colors
  .yellow('Day 4 Repose record part 1 result :'), colors
    .green(strategy1.toString()));

console.info(colors
  .yellow('Day 4 Repose record part 2 result :'), colors
    .green(strategy2.toString()));
