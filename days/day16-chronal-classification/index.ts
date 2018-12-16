import colors from 'colors';
import examples from './examples.json';
import implem from './lib';
import program from './program.json';

const { result, samples } = implem(examples, program);
console.info(colors
  .yellow('Day 16 Chronal classification part 1 result :'), colors
    .green(samples.toString()));

console.info(colors
  .yellow('Day 16 Chronal classification part 2 result :'), colors
    .green(result.toString()));
