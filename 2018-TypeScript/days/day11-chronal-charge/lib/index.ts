const gridSize = 300;

export interface Cell {
  powerLevel: number;
  x: number;
  y: number;
}

export function toCell(serial: number) {
  return (_: any, index: number) => {
    const x = (index % gridSize) + 1;
    const y = Math.floor(index / gridSize) + 1;
    const rackId = x + 10;
    let powerLevel = rackId * y;
    powerLevel += serial;
    powerLevel *= rackId;
    powerLevel = Math.floor((powerLevel / 100) % 10);
    powerLevel -= 5;
    return {
      powerLevel,
      x,
      y,
    };
  };
}

export default (serial: number) => {
  const grid = new Array(gridSize * gridSize)
    .fill(null)
    .map(toCell(serial));

  let maxPower3x3 = -Infinity;
  let maxPowerNxN = -Infinity;
  let cell3x3: Cell | undefined;
  let cellNxN: Cell | undefined;
  let sizeN = 0;

  grid.forEach((c, i) => {
    const { x, y } = c;
    const sizeLimit = Math.min(gridSize - x + 1, gridSize - y + 1);
    let size: number;
    let squarePower = 0;
    for (size = 1; size <= sizeLimit; size++) {
      let sizeX: number;
      for (sizeX = 0; sizeX < size; sizeX++) {
        squarePower += grid[i + ((size - 1) * gridSize) + sizeX].powerLevel;
      }

      let sizeY: number;
      for (sizeY = 0; sizeY < (size - 1); sizeY++) {
        squarePower += grid[i + sizeY * gridSize + size - 1].powerLevel;
      }

      if ((size === 3) && (squarePower > maxPower3x3)) {
        maxPower3x3 = squarePower;
        cell3x3 = c;
      }

      if (squarePower > maxPowerNxN) {
        maxPowerNxN = squarePower;
        cellNxN = c;
        sizeN = size;
      }
    }
  });

  if (!cell3x3) {
    throw new Error(`No 3x3 cell found for serial ${serial}`);
  }

  if (!cellNxN || !sizeN) {
    throw new Error(`No NxN cell found for serial ${serial}`);
  }

  return {
    largestSquare3x3: `${cell3x3.x},${cell3x3.y}`,
    largestSquareNxN: `${cellNxN.x},${cellNxN.y},${sizeN}`,
  };
};
