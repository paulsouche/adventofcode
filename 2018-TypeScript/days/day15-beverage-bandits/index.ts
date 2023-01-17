import colors from "colors";
import input from "./input.json";
import implem from "./lib";

const { battleScore, scoreWithMinimumAttackPower } = implem(input);
console.info(colors
  .yellow('Day 15 Beverage bandits part 1 result :'), colors
    .green(battleScore.toString())
);

console.info(colors
  .yellow('Day 15 Beverage bandits part 2 result :'), colors
    .green(scoreWithMinimumAttackPower.toString())
);
