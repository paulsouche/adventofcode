// DAY 3: Spiral Memory

export const day3Input = 368078;

export function manhattanDistance(num) {
  let sign = 1;
  let times = 0;
  let i = 1;
  let j;
  let x = 0;
  let y = 0;

  while (i < num) {
    times++;

    j = 0;
    while (i < num && j < times) {
      x += sign;
      j++;
      i++;
    }

    j = 0;
    while (i < num && j < times) {
      y += sign;
      j++;
      i++;
    }

    sign = -sign;
  }
  return Math.abs(x) + Math.abs(y);
}

// TODO : need refactor
export function manhattanValue(value) {
  let sign = 1;
  let times = 0;
  let i = 1;
  let j;
  let x = 0;
  let y = 0;
  let mapValue;
  const map = [{ x, y, i }];

  function findAjacentTotal(abs, ord) {
    return map.reduce((p, n) => {
      const isX = n.x === abs || n.x + 1 === abs || n.x - 1 === abs;
      const isY = n.y === ord || n.y + 1 === ord || n.y - 1 === ord;
      if (isX && isY) {
        p += n.i;
      }
      return p;
    }, 0);
  }

  while (true) {
    times++;

    j = 0;
    while (j < times) {
      x += sign;
      mapValue = findAjacentTotal(x, y);
      if (mapValue > value) {
        break;
      }
      map.push({
        i: mapValue,
        x,
        y,
      });

      j++;
      i++;
    }

    if (mapValue && mapValue > value) {
      value = mapValue;
      break;
    }

    j = 0;
    while (j < times) {
      y += sign;
      mapValue = findAjacentTotal(x, y);
      if (mapValue > value) {
        break;
      }
      map.push({
        i: mapValue,
        x,
        y,
      });
      j++;
      i++;
    }

    if (mapValue && mapValue > value) {
      value = mapValue;
      break;
    }

    sign = -sign;
  }
  return value;
}
