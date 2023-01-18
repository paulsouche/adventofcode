// DAY 17 : Spinlock

export const day17Input = 312;

export function spinlock(input, operations) {
  const buffer = [0];
  let operation = 0;
  let currPosition = 0;

  while (operation < operations) {
    const blen = buffer.length;
    operation++;
    currPosition += input;
    while (currPosition >= blen) {
      currPosition -= blen;
    }
    buffer.splice(++currPosition, 0, operation);
  }

  return buffer[currPosition + 1];
}

export function neighbor0(input, operations) {
  let z = 0;
  let neighbor = 0;
  let pos = 0;

  for (let i = 1; i < operations; i++, pos++) {
    pos = (pos + input) % i;
    if (pos === z) {
      neighbor = i;
    }
    if (pos < z) {
      z++;
    }
  }
  return neighbor;
}
