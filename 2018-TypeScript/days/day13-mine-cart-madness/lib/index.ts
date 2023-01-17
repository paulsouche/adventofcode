class Cart {
  public static orientations = ['left', 'straight', 'right'];

  public static sort(carts: Cart[]) {
    carts.sort((ca, cb) => ((ca.y > cb.y) || ((ca.y === cb.y) && (ca.x > cb.x))) ? 1 : -1);
  }

  public turnPolicy = 0;

  constructor(public orientation: string, public x: number, public y: number) { }

  public move(maze: string[][]) {
    switch (this.orientation) {
      case '>':
        this.x++;
        break;
      case '<':
        this.x--;
        break;
      case '^':
        this.y--;
        break;
      case 'v':
        this.y++;
        break;
      default:
        throw new Error(`Unknown orientation ${this.orientation}`);
    }

    const newCell = maze[this.y][this.x];

    switch (newCell) {
      case '/':
        switch (this.orientation) {
          case '>':
            this.orientation = '^';
            break;
          case '<':
            this.orientation = 'v';
            break;
          case '^':
            this.orientation = '>';
            break;
          case 'v':
            this.orientation = '<';
            break;
          default:
            throw new Error(`Unknown orientation ${this.orientation}`);
        }
        break;
      case '\\':
        switch (this.orientation) {
          case '>':
            this.orientation = 'v';
            break;
          case '<':
            this.orientation = '^';
            break;
          case '^':
            this.orientation = '<';
            break;
          case 'v':
            this.orientation = '>';
            break;
          default:
            throw new Error(`Unknown orientation ${this.orientation}`);
        }
        break;
      case '+':
        switch (Cart.orientations[this.turnPolicy]) {
          case 'left':
            switch (this.orientation) {
              case '>':
                this.orientation = '^';
                break;
              case '<':
                this.orientation = 'v';
                break;
              case '^':
                this.orientation = '<';
                break;
              case 'v':
                this.orientation = '>';
                break;
              default:
                throw new Error(`Unknown orientation ${this.orientation}`);
            }
            break;
          case 'right':
            switch (this.orientation) {
              case '>':
                this.orientation = 'v';
                break;
              case '<':
                this.orientation = '^';
                break;
              case '^':
                this.orientation = '>';
                break;
              case 'v':
                this.orientation = '<';
                break;
              default:
                throw new Error(`Unknown orientation ${this.orientation}`);
            }
            break;
          case 'straight':
            break;
          default:
            throw new Error(`Unknown policy ${Cart.orientations[this.turnPolicy]}, ${this.turnPolicy}`);
        }
        this.turnPolicy = ++this.turnPolicy % Cart.orientations.length;
        break;
      case '-':
      case '|':
        break;
      default:
        throw new Error(`Unknown cell ${newCell}`);
    }
  }

}

function inputToMine(input: string[]) {
  const maze: string[][] = [];
  const carts: Cart[] = [];
  const inputLen = input.length;
  const verticalCells = ['\\', '/', '|', '+'];
  const horizontalCells = ['\\', '/', '-', '+'];

  input
    .forEach((line, y) => {
      let x: number;
      const xlen = line.length;
      const mazeLine: string[] = [];
      for (x = 0; x < xlen; x++) {
        const cell = line[x];

        switch (cell) {
          case '>':
          case '<':
          case '^':
          case 'v':
            carts.push(new Cart(cell, x, y));

            let isVerticalTop = false;
            let isVerticalBottom = false;
            if (y === 0) {
              isVerticalBottom = verticalCells.includes(input[1][x]);
            } else if (y === (inputLen - 1)) {
              isVerticalTop = verticalCells.includes(input[y - 1][x]);
            } else {
              isVerticalTop = verticalCells.includes(input[y - 1][x]);
              isVerticalBottom = verticalCells.includes(input[y + 1][x]);
            }

            let isHorizontalLeft = false;
            let isHorizontalRight = false;
            if (x === 0) {
              isHorizontalRight = horizontalCells.includes(input[y][1]);
            } else if (x === (xlen - 1)) {
              isHorizontalLeft = horizontalCells.includes(input[y][x - 1]);
            } else {
              isHorizontalLeft = horizontalCells.includes(input[y][x - 1]);
              isHorizontalRight = horizontalCells.includes(input[y][x + 1]);
            }

            if (isVerticalTop && isVerticalBottom && isHorizontalLeft && isHorizontalRight) {
              mazeLine.push('+');
            } else if (isHorizontalLeft && isHorizontalRight) {
              mazeLine.push('-');
            } else if (isVerticalTop && isVerticalBottom) {
              mazeLine.push('|');
            } else {
              throw new Error(`Invalid line ${y}, ${line}`);
            }

            break;
          default:
            mazeLine.push(cell);
        }
      }
      maze.push(mazeLine);
    });

  return {
    carts,
    maze,
  };
}

export default (input: string[]) => {
  const { maze, carts } = inputToMine(input);
  let xFirstCrash: number | undefined;
  let yFirstCrash: number | undefined;
  let xLastCart: number | undefined;
  let yLastCart: number | undefined;

  while (true) {
    Cart.sort(carts);
    let cartInd: number;
    for (cartInd = 0; cartInd <  carts.length; cartInd++) {
      const cart = carts[cartInd];
      cart.move(maze);
      const { x, y } = cart;
      const crashedCart = carts.findIndex((c) => c !== cart && x === c.x && y === c.y);

      if (crashedCart > -1) {
        xFirstCrash = xFirstCrash || x;
        yFirstCrash = yFirstCrash || y;

        if (crashedCart < cartInd) {
          cartInd--;
        }
        carts.splice(crashedCart, 1);

        cartInd--;
        carts.splice(carts.indexOf(cart), 1);
      }
    }

    if (carts.length === 1) {
      ([{ x: xLastCart, y: yLastCart }] = carts);
    }

    if (carts.length <= 1) {
      break;
    }
  }

  return {
    firstCrash: `${xFirstCrash},${yFirstCrash}`,
    lastCart: `${xLastCart},${yLastCart}`,
  };
};
