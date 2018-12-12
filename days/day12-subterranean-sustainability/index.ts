import colors from 'colors';
import input from './input.json';
import implem from './lib';

console.info(colors
  .yellow('Day 12 Subterranean sustainability part 1 result :'), colors
    .green(implem(input).toString()));

console.info(colors
  .yellow('Day 12 Subterranean sustainability part 2 result :'), colors
    .green(implem(input, 50000000000).toString()));
