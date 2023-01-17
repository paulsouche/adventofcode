import colors from 'colors';
import input from './input.json';
import implem from './lib';

const { nonOverlapingClaimId, overlapingClaimsCount } = implem(input);
console.info(colors
  .yellow('Day 3 No matter how you slice it part 1 result :'), colors
    .green(overlapingClaimsCount.toString()));
console.info(colors
  .yellow('Day 3 No matter how you slice it part 2 result :'), colors
    .green(nonOverlapingClaimId));
