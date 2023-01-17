import implem from '.';

describe('Reservoir research', () => {
  it('Should output the total resource value of the lumber collection for 10 minutes', () => {
    expect(implem([
      '.#.#...|#.',
      '.....#|##|',
      '.|..|...#.',
      '..|#.....#',
      '#.#|||#|#|',
      '...#.||...',
      '.|....|...',
      '||...#|.#|',
      '|.||||..|.',
      '...#.|..|.',
    ])).toBe(1147);
  });

  it('Should output the total resource value of the lumber collection for 1000000000 minutes', () => {
    expect(implem([
      '.#.#...|#.',
      '.....#|##|',
      '.|..|...#.',
      '..|#.....#',
      '#.#|||#|#|',
      '...#.||...',
      '.|....|...',
      '||...#|.#|',
      '|.||||..|.',
      '...#.|..|.',
    ], 1000000000)).toBe(0);
  });
});
