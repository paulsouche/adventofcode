import colors from 'colors';
import input from './input.json';
import { checkSum, commonLetters } from './lib';

console.info(colors
  .yellow('Day 2 Inventory Management system part 1 result :'), colors
    .green(checkSum(input).toString()));

console.info(colors
  .yellow('Day 2 Inventory Management system part 2 result :'), colors
    .green(commonLetters(input)));
