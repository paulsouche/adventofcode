export interface Point {
  t: number;
  x: number;
  y: number;
  z: number;
}

export function computeManhattanDistance(point1: Point, point2: Point) {
  return Math.abs(point1.x - point2.x) +
    Math.abs(point1.y - point2.y) +
    Math.abs(point1.z - point2.z) +
    Math.abs(point1.t - point2.t);
}

export function strToPoint(str: string) {
  const match = str
    .split(',');

  return {
    t: +match[3],
    x: +match[0],
    y: +match[1],
    z: +match[2],
  };
}

export default (input: string[]) => {
  const points = input.map(strToPoint);
  const constellations: Point[][] = [];

  points
    .forEach((p) => {
      const availableConstellations = constellations
        .filter((c) => c
          .some((pt) => computeManhattanDistance(p, pt) <= 3));

      const availableConstellation = availableConstellations.shift();

      if (!availableConstellation) {
        constellations.push([p]);
        return;
      }

      availableConstellation.push(p);

      if (availableConstellations.length) {
        availableConstellations
          .reverse()
          .forEach((c) => {
            availableConstellation.push(...c);
            constellations.splice(constellations.indexOf(c), 1);
          });
      }
    });

  return constellations.length;
};
