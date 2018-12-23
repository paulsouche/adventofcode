import colors from "colors";
import input from "./input.json";
import implem from "./lib";

const { minMinutes, totalRisk } = implem(input.depth, input.target);
console.info(
  colors.yellow("Day 22 Node maze part 1 result :"),
  colors.green(totalRisk.toString())
);

console.info(colors
  .yellow('Day 22 Node maze part 2 result :'), colors
    .green(minMinutes.toString()));
