import colors from 'colors';
import input from './input.json';
import implem from './lib';

console.info(colors
  .yellow('Day 25 Four dimensional adventure part 1 result :'), colors
    .green(implem(input).toString()));
