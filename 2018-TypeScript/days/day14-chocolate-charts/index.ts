import colors from 'colors';
import { nextTenRecipes, occurrenceOfInput } from './lib';

console.info(colors
  .yellow('Day 14 Chocolate charts part 1 result :'), colors
    .green(nextTenRecipes(157901)));

console.info(colors
  .yellow('Day 14 Chocolate charts part 2 result :'), colors
    .green(occurrenceOfInput('157901').toString()));
