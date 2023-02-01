// DAY 22: Sporifica Virus
import { readFileSync } from "fs";
import { dirname, join } from "path";
import { fileURLToPath } from "url";

export const day22Input = readFileSync(
  join(dirname(fileURLToPath(import.meta.url)), "input.txt")
)
  .toString()
  .split("\n");

export const day22TestInput = ["..#", "#..", "..."];

class VirusCarrier {
  static DIRECTIONS = {
    UP: 0,
    RIGHT: 1,
    DOWN: 2,
    LEFT: 3,
  };

  static STATES = {
    CLEAN: 0,
    WEAKENED: 1,
    INFECTED: 2,
    FLAGGED: 3,
  };

  static coordsHash({ y, x }) {
    return `${y},${x}`;
  }

  constructor(map, evolvedVirus) {
    this.map = new Map();
    map.forEach((line, y) =>
      line
        .split("")
        .forEach((char, x) =>
          this.map.set(
            VirusCarrier.coordsHash({ y, x }),
            char === "#"
              ? VirusCarrier.STATES.INFECTED
              : VirusCarrier.STATES.CLEAN
          )
        )
    );

    this.evolvedVirus = !!evolvedVirus;
    this.x = Math.floor(map.length / 2);
    this.y = Math.floor(map.length / 2);
    this.direction = VirusCarrier.DIRECTIONS.UP;
    this.causedInfections = 0;
  }

  iterate(times) {
    while (times > 0) {
      this.burst();
      times--;
    }
    return this.causedInfections;
  }

  burst() {
    const hash = VirusCarrier.coordsHash(this);
    const cellState = this.map.get(hash) ?? VirusCarrier.STATES.CLEAN;
    this.direction = this.changeDirection(cellState);
    this.incrementInfections(cellState);
    this.changeCellState(hash, cellState);
    this.move();
  }

  changeDirection(cellState) {
    switch (cellState) {
      case VirusCarrier.STATES.CLEAN:
        return this.turnLeft();
      case VirusCarrier.STATES.WEAKENED:
        return this.direction;
      case VirusCarrier.STATES.INFECTED:
        return this.turnRight();
      case VirusCarrier.STATES.FLAGGED:
        return this.reverse();
    }
  }

  incrementInfections(cellState) {
    if (this.evolvedVirus) {
      if (cellState === VirusCarrier.STATES.WEAKENED) {
        this.causedInfections++;
      }
    } else {
      if (cellState === VirusCarrier.STATES.CLEAN) {
        this.causedInfections++;
      }
    }
  }

  changeCellState(hash, cellState) {
    let newState;
    if (this.evolvedVirus) {
      switch (cellState) {
        case VirusCarrier.STATES.CLEAN:
          newState = VirusCarrier.STATES.WEAKENED;
          break;
        case VirusCarrier.STATES.WEAKENED:
          newState = VirusCarrier.STATES.INFECTED;
          break;
        case VirusCarrier.STATES.INFECTED:
          newState = VirusCarrier.STATES.FLAGGED;
          break;
        case VirusCarrier.STATES.FLAGGED:
          newState = VirusCarrier.STATES.CLEAN;
          break;
      }
    } else {
      newState =
        cellState === VirusCarrier.STATES.INFECTED
          ? VirusCarrier.STATES.CLEAN
          : VirusCarrier.STATES.INFECTED;
    }
    this.map.set(hash, newState);
  }

  turnRight() {
    switch (this.direction) {
      case VirusCarrier.DIRECTIONS.UP:
        return VirusCarrier.DIRECTIONS.RIGHT;
      case VirusCarrier.DIRECTIONS.RIGHT:
        return VirusCarrier.DIRECTIONS.DOWN;
      case VirusCarrier.DIRECTIONS.DOWN:
        return VirusCarrier.DIRECTIONS.LEFT;
      case VirusCarrier.DIRECTIONS.LEFT:
        return VirusCarrier.DIRECTIONS.UP;
    }
  }

  turnLeft() {
    switch (this.direction) {
      case VirusCarrier.DIRECTIONS.UP:
        return VirusCarrier.DIRECTIONS.LEFT;
      case VirusCarrier.DIRECTIONS.RIGHT:
        return VirusCarrier.DIRECTIONS.UP;
      case VirusCarrier.DIRECTIONS.DOWN:
        return VirusCarrier.DIRECTIONS.RIGHT;
      case VirusCarrier.DIRECTIONS.LEFT:
        return VirusCarrier.DIRECTIONS.DOWN;
    }
  }

  reverse() {
    switch (this.direction) {
      case VirusCarrier.DIRECTIONS.UP:
        return VirusCarrier.DIRECTIONS.DOWN;
      case VirusCarrier.DIRECTIONS.RIGHT:
        return VirusCarrier.DIRECTIONS.LEFT;
      case VirusCarrier.DIRECTIONS.DOWN:
        return VirusCarrier.DIRECTIONS.UP;
      case VirusCarrier.DIRECTIONS.LEFT:
        return VirusCarrier.DIRECTIONS.RIGHT;
    }
  }

  move() {
    switch (this.direction) {
      case VirusCarrier.DIRECTIONS.UP:
        this.y--;
        break;
      case VirusCarrier.DIRECTIONS.RIGHT:
        this.x++;
        break;
      case VirusCarrier.DIRECTIONS.DOWN:
        this.y++;
        break;
      case VirusCarrier.DIRECTIONS.LEFT:
        this.x--;
        break;
    }
  }
}

export const infections = (input, bursts, evolvedVirus) =>
  new VirusCarrier(input, evolvedVirus).iterate(bursts);
