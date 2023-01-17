enum UnitType {
  ELF,
  GOBLIN,
}

interface Point {
  x: number;
  y: number;
}

interface Unit extends Point {
  type: UnitType;
  hp: number;
}

interface Segment extends Point {
  distance: number;
  origin?: number;
}

interface BestEnemy extends Unit {
  origin: number;
}

const dx = [0, -1, 1, 0];
const dy = [-1, 0, 0, 1];

function battle(input: string[], elfPower: number = 3, noElfShouldDie = false): number {
  const map = new Map<string, string>();
  const unitsCoords = new Map<string, Unit>();
  let units: Unit[] = [];
  let maxX = Number.NEGATIVE_INFINITY;
  let maxY = Number.NEGATIVE_INFINITY;
  input.forEach((row, y) => {
    maxY = Math.max(y, maxY);
    row.split('').forEach((cell, x) => {
      maxX = Math.max(x, maxX);
      map.set(`${y},${x}`, cell);
      if (cell === 'G' || cell === 'E') {
        const unit = {
          hp: 200,
          type: cell === 'G' ? UnitType.GOBLIN : UnitType.ELF,
          x,
          y,
        };
        units.push(unit);
        unitsCoords.set(`${y},${x}`, unit);
      }
    });
  });
  const size = map.size;

  const updateMap = ({ x, y }: Point, unit?: Unit) => {
    if (unit) {
      map.set(`${y},${x}`, unit.type === UnitType.GOBLIN ? 'G' : 'E');
      unitsCoords.set(`${y},${x}`, unit);
    } else {
      map.set(`${y},${x}`, '.');
      unitsCoords.delete(`${y},${x}`);
    }
  };

  const attack = (unit: Unit) => {
    const { x, y, type } = unit;
    let enemy: Unit | undefined;
    dx.forEach((_, i) => {
      const candidate = unitsCoords.get(`${y + dy[i]},${x + dx[i]}`);
      if (
        candidate &&
        candidate.type !== type &&
        (!enemy || candidate.hp < enemy.hp)
      ) {
        enemy = candidate;
      }
    });

    if (enemy) {
      enemy.hp -= type === UnitType.GOBLIN ? 3 : elfPower;
      if (enemy.hp <= 0) {
        enemy.hp = 0;
        updateMap(enemy);
      }
      return true;
    }
    return false;
  };

  const move = ({ x, y, type }: Unit) => {
    const dist = new Array(maxY).fill(null);
    dist.forEach((_, i) => {
      dist[i] = new Array(maxX).fill(-1);
    });
    dist[y][x] = 0;
    const queue = new Array<Segment>(size);
    let from = 0;
    let to = 1;
    let enemy;
    let bestEnemy: BestEnemy | undefined;
    let nearest = size;

    queue[from] = { x, y, distance: 0 };

    while (from < to) {
      const { x: x0, y: y0, distance, origin } = queue[from++];

      if (distance > nearest) {
        break;
      }

      dx.forEach((_, i) => {
        const [x1, y1] = [x0 + dx[i], y0 + dy[i]];
        if (map.get(`${y1},${x1}`) === '.' && dist[y1]![x1] === -1) {
          dist[y1]![x1] = distance + 1;
          queue[to++] = {
            distance: distance + 1,
            origin: distance === 0 ? i : origin,
            x: x1,
            y: y1,
          };
        } else {
          enemy = unitsCoords.get(`${y1},${x1}`);
          if (
            !!enemy &&
            enemy.type !== type &&
            (!bestEnemy ||
            enemy.y < bestEnemy.y ||
            (enemy.y === bestEnemy.y && enemy.x < bestEnemy.x))
          ) {
            bestEnemy = {
              ...enemy,
              origin: origin || 0,
            };
            nearest = distance;
          }
        }
      });
    }

    return bestEnemy
      ? [x + dx[bestEnemy.origin], y + dy[bestEnemy.origin]]
      : [x, y];
  };

  let rounds = 0;
  let combatEnded = false;
  while (!combatEnded) {
    for (const unit of units) {
      const { x, y, hp, type } = unit;
      combatEnded =
        combatEnded ||
        !units.find((enemy) => enemy.type !== type && enemy.hp > 0);

      if (hp === 0 || combatEnded || attack(unit)) {
        continue;
      }

      [unit.x, unit.y] = move(unit);
      updateMap({ x, y });
      updateMap(unit, unit);
      attack(unit);
    }

    if (!combatEnded) {
      rounds++;
    }

    if (
      noElfShouldDie &&
      units.some(({ hp, type }) => hp === 0 && type === UnitType.ELF)
    ) {
      return -1;
    }

    units = units
      .filter(({ hp }) => hp > 0)
      .sort((a, b) => (a.y === b.y ? a.x - b.x : a.y - b.y));
  }

  return rounds * units.reduce((acc, { hp }) => acc + hp, 0);
}

function findScoreForMinimumAttackPower(input: string[]) {
  let notEnough = 3;
  let enough = notEnough;
  let outcome;
  let winOutcome;
  do {
    enough *= 2;
    outcome = battle(input, enough, true);
    winOutcome = outcome;
  } while (outcome < 0);

  while (notEnough + 1 < enough) {
    const middle = Math.ceil((enough + notEnough) / 2);
    outcome = battle(input, middle, true);
    if (outcome > 0) {
      enough = middle;
      winOutcome = outcome;
    } else {
      notEnough = middle;
    }
  }
  return winOutcome;
}

export default (input: string[]) => ({
  battleScore: battle(input),
  scoreWithMinimumAttackPower: findScoreForMinimumAttackPower(input),
});
