import colors from 'colors';
import input from './input.json';
import { shortestsUnits, units } from './lib';

console.info(colors
  .yellow('Day 5 Alchemical reduction part 1 result :'), colors
    .green(units(input.polymer).toString()));

console.info(colors
  .yellow('Day 5 Alchemical reduction part 2 result :'), colors
    .green(shortestsUnits(input.polymer).toString()));
