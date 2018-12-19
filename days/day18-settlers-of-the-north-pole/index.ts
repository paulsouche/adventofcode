import colors from 'colors';
import input from './input.json';
import implem from './lib';

console.info(colors
  .yellow('Day 18 Settlers of the north pole part 1 result :'), colors
    .green(implem(input).toString()));

console.info(colors
  .yellow('Day 18 Settlers of the north pole part 2 result :'), colors
    .green(implem(input, 1000000000).toString()));
