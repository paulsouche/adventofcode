import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { remainingUnitsWithLowerBoost, remainingUnitsWithNoBoost } = implem(input)
console.info(colors
  .yellow('Day 24 Immune system simulator 20XX part 1 result :'), colors
    .green(remainingUnitsWithNoBoost.toString()));
console.info(colors
  .yellow('Day 24 Immune system simulator 20XX part 2 result :'), colors
    .green(remainingUnitsWithLowerBoost.toString()));
