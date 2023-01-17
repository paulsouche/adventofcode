interface Move {
  minutes: number;
  tool: number;
  x: number;
  y: number;
}

const dxDy = [
  [0, -1],
  [1, 0],
  [0, 1],
  [-1, 0],
];

const buildCaveCoordsHasher =
  (maxHeight: number) => (x: number, y: number, tool?: number) =>
    x * 10 * maxHeight + y * 10 + (tool || 0);

export default (depth: number, target: string) => {
  const [targetX, targetY] = target.split(',').map(Number);
  const height = targetY + 1 + 2;
  const width = targetX + 1 + 22;
  const coordsHash = buildCaveCoordsHasher(height);

  const mazeErosion = new Map<number, number>();
  const mazeRisk = new Map<number, number>();
  let totalRisk = 0;
  for (let x = 0; x < width; x++) {
    for (let y = 0; y < height; y++) {
      let erosionLevel;
      if (x + y === 0 || (x === targetX && y === targetY)) {
        erosionLevel = depth % 20183;
      } else if (y === 0) {
        erosionLevel = (x * 16807 + depth) % 20183;
      } else if (x === 0) {
        erosionLevel = (y * 48271 + depth) % 20183;
      } else {
        const geoIndex =
          mazeErosion.get(coordsHash(x - 1, y))! *
          mazeErosion.get(coordsHash(x, y - 1))!;
        erosionLevel = (geoIndex + depth) % 20183;
      }
      const key = coordsHash(x, y);
      mazeErosion.set(key, erosionLevel);
      const risk = erosionLevel % 3;
      mazeRisk.set(key, risk);
      if (x <= targetX && y <= targetY) {
        totalRisk += risk;
      }
    }
  }

  const mazeTime = new Map();
  const queue: Move[] = [
    {
      minutes: 0,
      tool: 1,
      x: 0,
      y: 0,
    },
  ];
  let minMinutes = 0;

  while (queue.length > 0) {
    const { minutes, x, y, tool } = queue.shift()!;
    const bestTime =
      mazeTime.get(coordsHash(x, y, tool)) || Number.MAX_SAFE_INTEGER;

    if (bestTime <= minutes) {
      continue;
    }

    if (targetX === x && targetY === y && tool === 1) {
      minMinutes = minutes;
      break;
    }

    mazeTime.set(coordsHash(x, y, tool), minutes);
    for (let newTool = 0; newTool < 3; newTool++) {
      if (newTool !== tool && newTool !== mazeRisk.get(coordsHash(x, y))) {
        const bt =
          mazeTime.get(coordsHash(x, y, newTool)) || Number.MAX_SAFE_INTEGER;
        if (bt > minutes + 7) {
          queue.push({
            minutes: minutes + 7,
            tool: newTool,
            x,
            y,
          });
        }
      }
    }

    for (const [dx, dy] of dxDy) {
      if (x + dx < 0 || x + dx >= width || y + dy < 0 || y + dy >= height) {
        continue;
      }

      const terrain = mazeRisk.get(coordsHash(x + dx, y + dy));
      if (terrain === tool) {
        continue;
      }

      const bt =
        mazeTime.get(coordsHash(x + dx, y + dy, tool)) ||
        Number.MAX_SAFE_INTEGER;
      if (bt > minutes + 1) {
        queue.push({
          minutes: minutes + 1,
          tool,
          x: x + dx,
          y: y + dy,
        });
      }
    }

    queue.sort(({ minutes: minA }, { minutes: minB }) => minA - minB);
  }

  return {
    minMinutes,
    totalRisk,
  };
};
