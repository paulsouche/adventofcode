import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { bestPos, numberOfNanoBotsInStrongestRange } = implem(input);
console.info(colors
  .yellow('Day 23 Experimental emergency teleportation part 1 result :'), colors
    .green(numberOfNanoBotsInStrongestRange.toString()));

console.info(colors
  .yellow('Day 23 Experimental emergency teleportation part 2 result :'), colors
    .green(bestPos.toString()));
