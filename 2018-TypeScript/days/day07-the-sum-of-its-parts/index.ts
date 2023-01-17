import colors from 'colors';
import input from './input.json';
import implem from './lib';

console.info(colors
  .yellow('Day 7 The sum of its parts part 1 result :'), colors
    .green(implem(input).steps));

console.info(colors
  .yellow('Day 7 The sum of its parts part 2 result :'), colors
    .green(implem(input, { workersNum: 5, stepsBaseTime: 60 }).time.toString()));
