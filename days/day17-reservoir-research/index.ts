import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { floodedCells, retainedCells } = implem(input);
console.info(colors
  .yellow('Day 17 Reservoir research part 1 result :'), colors
    .green(floodedCells.toString()));

console.info(colors
  .yellow('Day 17 Reservoir research part 2 result :'), colors
    .green(retainedCells.toString()));
