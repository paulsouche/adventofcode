const clayReg = /^(x|y)=(\d+),\s(x|y)=(\d+)\.\.(\d+)$/;

interface Point {
  x: number;
  y: number;
}

function inputToClayMap(input: string[]) {
  const clayMap: { [coordinates: string]: boolean } = {};
  let maxY = 0;
  let minY = Infinity;
  let maxX = 0;
  let minX = Infinity;

  input.forEach((str) => {
    const match = clayReg.exec(str);

    if (!match) {
      throw new Error(`Invalid scan ${str}`);
    }

    let i: number;
    const isVertical = match[1] === 'x';
    for (i = +match[4]; i <= +match[5]; i++) {
      if (isVertical) {
        clayMap[`${match[2]},${i}`] = true;
        maxX = Math.max(maxX, +match[2]);
        maxY = Math.max(maxY, i);
        minX = Math.min(minX, +match[2]);
        minY = Math.min(minY, i);
      } else {
        clayMap[`${i},${match[2]}`] = true;
        maxX = Math.max(maxY, i);
        maxY = Math.max(maxY, +match[2]);
        minX = Math.min(minX, i);
        minY = Math.min(minY, +match[2]);
      }
    }
  });

  return {
    clayMap,
    maxX,
    maxY,
    minX,
    minY,
  };
}

export default (input: string[]) => {
  const { clayMap, maxY, minY } = inputToClayMap(input);
  const floodedCells: { [coordinates: string]: boolean } = {};
  const retainedCells: { [coordinates: string]: boolean } = {};

  const walkHorizontally = ({ x, y }: Point, direction: number) => {
    while (true) {
      x += direction;
      if (floodedCells[`${x},${y}`] || clayMap[`${x},${y}`]) {
        return x - direction;
      }

      floodedCells[`${x},${y}`] = true;

      if (!clayMap[`${x},${y + 1}`]) {
        walkVertically({
          x,
          y: y + 1,
        });
        if (!clayMap[`${x},${y + 1}`]) {
          return;
        }
      }
    }
  };

  const walkVertically = ({ x, y }: Point): void => {
    if (y > maxY || floodedCells[`${x},${y}`]) {
      return;
    }

    floodedCells[`${x},${y}`] = true;

    if (!clayMap[`${x},${y + 1}`]) {
      walkVertically({ x, y: y + 1 });
    }

    if (clayMap[`${x},${y + 1}`]) {
      const left = walkHorizontally({ x, y }, -1);
      const right = walkHorizontally({ x, y }, 1);

      if (typeof left === 'number' && typeof right === 'number') {
        let xRetained: number;
        for (xRetained = left; xRetained <= right; xRetained++) {
          clayMap[`${xRetained},${y}`] = true;
          retainedCells[`${xRetained},${y}`] = true;
        }
      }
    }
  };

  walkVertically({ x: 500, y: 0 });

  return {
    floodedCells: Object.keys(floodedCells).length - minY,
    retainedCells: Object.keys(retainedCells).length,
  };
};
