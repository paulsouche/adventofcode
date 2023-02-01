// DAY 21: Fractal Art
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day21Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day21TestInput = [
  "../.# => ##./#../...",
  ".#./..#/### => #..#/..../..../#..#",
];

class Square {
  static flip(pixels) {
    return pixels.map((row) => row.map((_, pi) => row[row.length - pi - 1]));
  }

  static rotate(pixels) {
    return pixels.map((row, ri) =>
      row.map((_, pi) => pixels[pixels.length - pi - 1][ri])
    );
  }

  static serialize(pixels) {
    return pixels.map((row) => row.join("")).join("/");
  }

  static join(squares) {
    const gridSize = Math.sqrt(squares.length);
    const squareSize = squares[0].size;

    const grid = new Array(gridSize * squareSize)
      .fill(0)
      .map(() => new Array(gridSize * squareSize).fill(0));

    for (let sy = 0; sy < gridSize; sy++) {
      for (let sx = 0; sx < gridSize; sx++) {
        const { pixels } = squares[sy * gridSize + sx];
        for (let py = 0; py < squareSize; py++) {
          for (let px = 0; px < squareSize; px++) {
            grid[sy * squareSize + py][sx * squareSize + px] = pixels[py][px];
          }
        }
      }
    }

    return new Square(Square.serialize(grid));
  }

  constructor(pixels) {
    this.pixels = pixels.split("/").map((row) => row.split(""));
  }

  get size() {
    return this.pixels.length;
  }

  get pixelsOn() {
    return this.pixels.reduce(
      (acc, row) => acc + row.filter((p) => p === "#").length,
      0
    );
  }

  divide() {
    const squares = [];
    const into = this.size % 2 === 0 ? 2 : 3;

    for (let sy = 0; sy < this.pixels.length; sy += into) {
      for (let sx = 0; sx < this.pixels[sy].length; sx += into) {
        squares.push(
          new Square(
            Square.serialize(
              new Array(into)
                .fill(0)
                .map((_, py) =>
                  new Array(into)
                    .fill(0)
                    .map((_, px) => this.pixels[sy + py][sx + px])
                )
            )
          )
        );
      }
    }

    return squares;
  }

  permutations() {
    return [this.pixels, Square.flip(this.pixels)].flatMap((flip) => {
      let shape = flip;
      return new Array(4)
        .fill(0)
        .map(() => Square.serialize((shape = Square.rotate(shape))));
    });
  }

  serialize() {
    return Square.serialize(this.pixels);
  }
}

class Program {
  constructor(rules) {
    this.square = new Square(".#./..#/###");
    this.rules = new Map();
    for (const [key, val] of rules.map((rule) => rule.split(" => "))) {
      for (const permutation of new Square(key).permutations()) {
        this.rules.set(permutation, val);
      }
    }
  }

  enhance(square) {
    return new Square(this.rules.get(square.serialize()));
  }

  iterate(times) {
    while (times > 0) {
      this.square = Square.join(
        this.square.divide().map((square) => this.enhance(square))
      );
      times--;
    }
    return this.square.pixelsOn;
  }
}

export const pixelsOn = (input, iterations) =>
  new Program(input).iterate(iterations);
