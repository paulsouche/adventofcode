import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { message, seconds } = implem(input);
console.info(colors
  .yellow('Day 10 The stars align part 1 result :\n'), colors
    .green(message));

console.info(colors
  .yellow('Day 10 The stars align part 2 result :\n'), colors
    .green(seconds.toString()));
