interface Point {
  x: number;
  y: number;
  z: number;
}

interface NanoBot extends Point {
  r: number;
}

interface Volume {
  xmin: number;
  xmax: number;
  ymin: number;
  ymax: number;
  zmin: number;
  zmax: number;
}

interface VolumeWithBots extends Volume {
  bots: Set<NanoBot>;
}

const nanoBotReg = /^pos=<(-?\d+),(-?\d+),(-?\d+)>,\sr=(-?\d+)$/;

const strToNanoBot = (str: string) => {
  const match = nanoBotReg.exec(str);

  if (!match) {
    throw new Error(`Invalid nanobot input ${str}`);
  }

  return {
    r: +match[4],
    x: +match[1],
    y: +match[2],
    z: +match[3],
  };
};

const computeManhattanDistance = (point1: Point, point2: Point) => {
  return (
    Math.abs(point1.x - point2.x) +
    Math.abs(point1.y - point2.y) +
    Math.abs(point1.z - point2.z)
  );
};

const inRangeOfVolume = (vol: Volume, bot: NanoBot) => {
  let cost = 0;
  if (bot.x > vol.xmax) {
    cost += bot.x - vol.xmax;
  } else if (bot.x < vol.xmin) {
    cost += vol.xmin - bot.x;
  }

  if (bot.y > vol.ymax) {
    cost += bot.y - vol.ymax;
  } else if (bot.y < vol.ymin) {
    cost += vol.ymin - bot.y;
  }

  if (bot.z > vol.zmax) {
    cost += bot.z - vol.zmax;
  } else if (bot.z < vol.zmin) {
    cost += vol.zmin - bot.z;
  }

  return cost <= bot.r;
};

const nearestPoint = (vol: Volume, point: Point) => {
  return {
    x: point.x > vol.xmax ? vol.xmax : point.x < vol.xmin ? vol.xmin : point.x,
    y: point.y > vol.ymax ? vol.ymax : point.y < vol.ymin ? vol.ymin : point.y,
    z: point.z > vol.zmax ? vol.zmax : point.z < vol.zmin ? vol.zmin : point.z,
  };
};

const botsInRangeBuilder = (nanoBots: NanoBot[]) => (vol: Volume) => {
  const set = new Set<NanoBot>();
  for (const nb of nanoBots) {
    if (inRangeOfVolume(vol, nb)) {
      set.add(nb);
    }
  }
  return set;
};

const subdivide = (vol: Volume): Volume | Volume[] => {
  if (vol.xmin === vol.xmax && vol.ymin === vol.ymax && vol.zmin === vol.zmax) {
    return {
      xmax: vol.xmax,
      xmin: vol.xmin,
      ymax: vol.ymax,
      ymin: vol.ymin,
      zmax: vol.zmax,
      zmin: vol.zmin,
    };
  }

  const xmid = Math.floor((vol.xmax - vol.xmin) / 2 + vol.xmin);
  const ymid = Math.floor((vol.ymax - vol.ymin) / 2 + vol.ymin);
  const zmid = Math.floor((vol.zmax - vol.zmin) / 2 + vol.zmin);

  return [
    {
      xmax: xmid,
      xmin: vol.xmin,
      ymax: ymid,
      ymin: vol.ymin,
      zmax: zmid,
      zmin: vol.zmin,
    },
    {
      xmax: vol.xmax,
      xmin: xmid + 1,
      ymax: ymid,
      ymin: vol.ymin,
      zmax: zmid,
      zmin: vol.zmin,
    },
    {
      xmax: xmid,
      xmin: vol.xmin,
      ymax: vol.ymax,
      ymin: ymid + 1,
      zmax: zmid,
      zmin: vol.zmin,
    },
    {
      xmax: vol.xmax,
      xmin: xmid + 1,
      ymax: vol.ymax,
      ymin: ymid + 1,
      zmax: zmid,
      zmin: vol.zmin,
    },
    {
      xmax: xmid,
      xmin: vol.xmin,
      ymax: ymid,
      ymin: vol.ymin,
      zmax: vol.zmax,
      zmin: zmid + 1,
    },
    {
      xmax: vol.xmax,
      xmin: xmid + 1,
      ymax: ymid,
      ymin: vol.ymin,
      zmax: vol.zmax,
      zmin: zmid + 1,
    },
    {
      xmax: xmid,
      xmin: vol.xmin,
      ymax: vol.ymax,
      ymin: ymid + 1,
      zmax: vol.zmax,
      zmin: zmid + 1,
    },
    {
      xmax: vol.xmax,
      xmin: xmid + 1,
      ymax: vol.ymax,
      ymin: ymid + 1,
      zmax: vol.zmax,
      zmin: zmid + 1,
    },
  ];
};

const findBestBuilder =
  (origin: Point) => (v1: VolumeWithBots, v2: VolumeWithBots) =>
    v1.bots.size > v2.bots.size
      ? v1
      : v2.bots.size > v1.bots.size
      ? v2
      : nearestPoint(v1, origin) < nearestPoint(v2, origin)
      ? v1
      : nearestPoint(v2, origin) < nearestPoint(v1, origin)
      ? v2
      : v1;

export default (input: string[]) => {
  const nanoBots = input.map(strToNanoBot);

  let [strongestNanobot] = nanoBots;
  let maxx = -Infinity;
  let minx = Infinity;
  let maxy = -Infinity;
  let miny = Infinity;
  let maxz = -Infinity;
  let minz = Infinity;
  nanoBots.forEach((n) => {
    maxx = Math.max(n.x, maxx);
    minx = Math.min(n.x, minx);
    maxy = Math.max(n.x, maxy);
    miny = Math.min(n.x, miny);
    maxz = Math.max(n.x, maxz);
    minz = Math.min(n.x, minz);
    if (strongestNanobot.r < n.r) {
      strongestNanobot = n;
    }
  });

  const nanoBotsInStrongestRange = nanoBots.filter((n) =>
    computeManhattanDistance(strongestNanobot, n) <= strongestNanobot.r);
  const numberOfNanoBotsInStrongestRange = nanoBotsInStrongestRange.length;

  const botsInRange = botsInRangeBuilder(nanoBots);
  const start = { x: 0, y: 0, z: 0 };
  const findBest = findBestBuilder(start);
  const volume = {
    xmax: Math.max(maxx, maxy, maxz),
    xmin: Math.min(minx, miny, minz),
    ymax: Math.max(maxx, maxy, maxz),
    ymin: Math.min(minx, miny, minz),
    zmax: Math.max(maxx, maxy, maxz),
    zmin: Math.min(minx, miny, minz),
  };

  let volumes: VolumeWithBots[] = [
    {
      ...volume,
      bots: botsInRange(volume),
    },
  ];
  let best: VolumeWithBots | undefined;
  let bestP: Point | undefined;
  let bestPos = Infinity;

  outer: while (volumes.length) {
    let vol: VolumeWithBots;
    while (volumes.length) {
      vol = volumes.pop()!;

      if (
        vol.xmax - vol.xmin > 0 ||
        vol.ymax - vol.ymin > 0 ||
        vol.zmax - vol.zmin > 0
      ) {
        break;
      }

      best = best ? findBest(best, vol) : vol;
      bestP = nearestPoint(best, start);
      bestPos = computeManhattanDistance(bestP, start);

      if (best === vol) {
        volumes = volumes.filter((v2) =>
          v2.bots.size >= best!.bots.size && computeManhattanDistance(nearestPoint(v2, start), start) <= bestPos);
      }

      continue outer;
    }

    const subVolumes = subdivide(vol!);
    const newVolumes = Array.isArray(subVolumes)
      ? subVolumes
          .map((v) => ({
            ...v,
            bots: botsInRange(v),
          }))
          .filter((vb) => {
            if (vb.bots.size === 0) {
              return false;
            }

            if (best && vb.bots.size < best.bots.size) {
              return false;
            }

            if (
              best &&
              computeManhattanDistance(nearestPoint(vb, start), start) > bestPos
            ) {
              return false;
            }
            return true;
          })
      : [];

    volumes.push(...newVolumes);
    volumes.sort((a, b) => a.bots.size - b.bots.size);
  }

  return {
    bestPos,
    numberOfNanoBotsInStrongestRange,
  };
};
