const openGround = '.';
const tree = '|';
const lumberYard = '#';

const yOffset = [-1, -1, -1, 0, 0, 1, 1, 1];
const xOffset = [-1, 0, 1, -1, 1, -1, 0, 1];

function strangeMagic(wood: string[][], lineInd: number) {
  return (cell: string, i: number) => {
    const adjacentForest = new Array(8)
      .fill(0)
      .map((_, ind) => {
        const currentLine = lineInd + yOffset[ind];
        if (wood[currentLine]) {
          return wood[currentLine][i + xOffset[ind]];
        }
        return '';
      })
      .join('');

    const numberOfTrees = adjacentForest.split(tree).length - 1;
    const numberOfLumberYard = adjacentForest.split(lumberYard).length - 1;
    switch (cell) {
      case openGround:
        if (numberOfTrees >= 3) {
          return tree;
        }
        break;
      case tree:
        if (numberOfLumberYard >= 3) {
          return lumberYard;
        }
        break;
      case lumberYard:
        if (numberOfLumberYard < 1 || numberOfTrees < 1) {
          return openGround;
        }
        break;
      default:
        throw new Error(`Unknown cell ${cell}`);
    }

    return cell;
  };
}

export default (input: string[], minutes = 10) => {
  let wood = input.map((l) => l.split(''));
  const knownWoods: string[] = [];

  let minute: number;
  for (minute = 0; minute < minutes; minute++) {
    wood = wood
      .map((line, lineInd) => line.map(strangeMagic(wood, lineInd)));

    const woodStr = wood.join('');
    const woodIndex = knownWoods.indexOf(woodStr);

    if (woodIndex >= 0) {
      const iteration = minute - woodIndex;
      minute += Math.floor((minutes - minute - 1) / iteration) * iteration;
      continue;
    }

    knownWoods.push(woodStr);
  }

  const finalWood = wood.join('');
  const numberOfTrees = finalWood.split(tree).length - 1;
  const numberOfLumberYard = finalWood.split(lumberYard).length - 1;

  return numberOfTrees * numberOfLumberYard;
};
