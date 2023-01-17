const starReg = /^position=<\s*(-?\d+),\s*(-?\d+)>\svelocity=<\s*(-?\d+),\s*(-?\d+)>$/;

class Star {
  constructor(public x: number, public y: number, public vx: number, public vy: number) { }

  public move() {
    this.x += this.vx;
    this.y += this.vy;
  }

  public back() {
    this.x -= this.vx;
    this.y -= this.vy;
  }
}

function strToStar(str: string) {
  const match = starReg.exec(str);

  if (!match) {
    throw new Error(`Invalid star input ${str}`);
  }

  return new Star(+match[1], +match[2], +match[3], +match[4]);
}

function matrixSize(stars: Star[]) {
  const xs = stars.map(({ x }) => x);
  const ys = stars.map(({ y }) => y);
  const minX = Math.min(...xs);
  const maxX = Math.max(...xs);
  const minY = Math.min(...ys);
  const maxY = Math.max(...ys);

  return {
    maxX,
    maxY,
    minX,
    minY,
    xlen: maxX - minX,
    ylen: maxY - minY,
  };
}

function printStars(stars: Star[]) {
  const {
    minX,
    minY,
    xlen,
    ylen,
  } = matrixSize(stars);
  let i: number;
  let j: number;

  const matrix: string[] = [];
  for (i = 0; i <= ylen; i++) {
    const line: string[] = [];
    for (j = 0; j <= xlen; j++) {
      const star = stars.find(({ x, y }) => x === (j + minX) && y === (i + minY));
      if (star) {
        line.push('#');
      } else {
        line.push('.');
      }
    }
    matrix.push(line.join(''));
  }

  return matrix.join('\n');
}

export default (input: string[]) => {
  const stars = input.map(strToStar);

  const gridLen = {
    xlen: Infinity,
    ylen: Infinity,
  };

  let { xlen, ylen } = matrixSize(stars);
  let seconds = 0;
  while (gridLen.xlen > xlen || gridLen.ylen > ylen) {
    gridLen.xlen = xlen;
    gridLen.ylen = ylen;

    stars.forEach((s) => s.move());
    seconds++;

    ({ xlen, ylen } = matrixSize(stars));
  }

  seconds--;
  stars.forEach((s) => s.back());

  return {
    message: printStars(stars),
    seconds,
  };
};
