import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { fewestInstructionRegisterValue, mostInstructionRegisterValue } = implem(input);
console.info(colors
  .yellow('Day 21 Chronal conversion part 1 result :'), colors
    .green(fewestInstructionRegisterValue.toString()));

console.info(colors
  .yellow('Day 21 Chronal conversion part 2 result :'), colors
    .green(mostInstructionRegisterValue.toString()));
