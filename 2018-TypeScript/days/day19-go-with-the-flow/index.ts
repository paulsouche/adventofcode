import colors from 'colors';
import input from './input.json';
import implem from './lib';

console.info(colors
  .yellow('Day 19 Go with the flow part 1 result :'), colors
    .green(implem(input).toString()));

console.info(colors
  .yellow('Day 19 Go with the flow part 2 result :'), colors
    .green(implem(input, 1).toString()));
