// DAY 20: Particle Swarm
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day20Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day20TestInput1 = [
  "p=<3,0,0>, v=<2,0,0>, a=<-1,0,0>",
  "p=<4,0,0>, v=<0,0,0>, a=<-2,0,0>",
];

export const day20TestInput2 = [
  "p=<-6,0,0>, v=<3,0,0>, a=<0,0,0>",
  "p=<-4,0,0>, v=<2,0,0>, a=<0,0,0>",
  "p=<-2,0,0>, v=<1,0,0>, a=<0,0,0>",
  "p=<3,0,0>, v=<1,0,0>, a=<0,0,0>",
];

const PARTICLE_REGEX =
  /^p=<(-?\d+),(-?\d+),(-?\d+)>,\sv=<(-?\d+),(-?\d+),(-?\d+)>,\sa=<(-?\d+),(-?\d+),(-?\d+)>$/;

class Particle {
  constructor(x, y, z, vx, vy, vz, ax, ay, az) {
    this.x = x;
    this.y = y;
    this.z = z;
    this.vx = vx;
    this.vy = vy;
    this.vz = vz;
    this.ax = ax;
    this.ay = ay;
    this.az = az;
  }

  get acceleration() {
    return Math.abs(this.ax) + Math.abs(this.ay) + Math.abs(this.az);
  }

  get speed() {
    return Math.abs(this.vx) + Math.abs(this.vy) + Math.abs(this.vz);
  }

  get manhattanToOrigin() {
    return Math.abs(this.x) + Math.abs(this.y) + Math.abs(this.z);
  }

  collisionTime({ ax, ay, az, vx, vy, vz, x, y, z }) {
    const [timesX, timesY, timesZ] = [
      [(this.ax - ax) / 2, this.vx - vx + (this.ax - ax) / 2, this.x - x],
      [(this.ay - ay) / 2, this.vy - vy + (this.ay - ay) / 2, this.y - y],
      [(this.az - az) / 2, this.vz - vz + (this.az - az) / 2, this.z - z],
    ].map(([A, B, C]) => {
      const solutions = new Set();
      let time;
      if (A === 0) {
        if (B !== 0) {
          time = -C / B;
          if (time > 0 && Number.isInteger(time)) {
            solutions.add(time);
          }
        } else if (C === 0) {
          solutions.add(Number.POSITIVE_INFINITY);
        }
      } else {
        const det = B * B - 4 * A * C;
        if (det >= 0) {
          time = (-B + Math.sqrt(det)) / (2 * A);
          if (time > 0 && Number.isInteger(time)) {
            solutions.add(time);
          }
          time = (-B - Math.sqrt(det)) / (2 * A);
          if (time > 0 && Number.isInteger(time)) {
            solutions.add(time);
          }
        }
      }

      return solutions;
    });

    return [...timesX].find(
      (time) =>
        (timesY.has(time) || timesY.has(Number.POSITIVE_INFINITY)) &&
        (timesZ.has(time) || timesZ.has(Number.POSITIVE_INFINITY))
    );
  }
}

function parse(input) {
  return input.map((p) => {
    const m = PARTICLE_REGEX.exec(p);
    if (!m) {
      throw new Error(`Invalid input ${p}`);
    }

    return new Particle(...m.slice(1).map(Number));
  });
}

export function closest(input) {
  const particles = parse(input);

  const [nearest] = [...particles].sort((a, b) => {
    const acceleration = a.acceleration - b.acceleration;
    if (acceleration !== 0) {
      return acceleration;
    }

    const speed = a.speed - b.speed;
    if (speed !== 0) {
      return speed;
    }

    return a.manhattanToOrigin - b.manhattanToOrigin;
  });

  return particles.indexOf(nearest);
}

export function left(input) {
  const particles = parse(input);
  const particlesSet = new Set();

  particles.forEach((_, i) => particlesSet.add(i));

  const collisions = [];
  for (let i = 0; i < particles.length; i++) {
    for (let j = i + 1; j < particles.length; j++) {
      const collisionTime = particles[i].collisionTime(particles[j]);
      if (typeof collisionTime !== "undefined") {
        collisions.push({ i, j, collisionTime });
      }
    }
  }

  collisions.sort((a, b) => a.collisionTime - b.collisionTime);

  let currentTime = 0;
  for (const { i, j, collisionTime } of collisions) {
    if (
      (particlesSet.has(i) && particlesSet.has(j)) ||
      collisionTime === currentTime
    ) {
      particlesSet.delete(i);
      particlesSet.delete(j);
    }
    currentTime = collisionTime;
  }

  return particlesSet.size;
}
