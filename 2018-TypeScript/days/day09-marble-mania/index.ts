import colors from 'colors';
import input from './input.json';
import implem from './lib';

console.info(colors
  .yellow('Day 9 Marble mania part 1 result :'), colors
    .green(implem(input.party).toString()));

console.info(colors
  .yellow('Day 9 Marble mania part 2 result :'), colors
    .green(implem(input.party, 100).toString()));
