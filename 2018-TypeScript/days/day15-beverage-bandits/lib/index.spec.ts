import implem from '.';

describe('Beverage bandits', () => {
  it('Should play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#.G...#',
      '#...EG#',
      '#.#.#G#',
      '#..G#E#',
      '#.....#',
      '#######',
    ]).battleScore).toBe(27730);
  });

  it('Should play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#G..#E#',
      '#E#E.E#',
      '#G.##.#',
      '#...#E#',
      '#...E.#',
      '#######',
    ]).battleScore).toBe(36334);
  });

  it('Should play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#E..EG#',
      '#.#G.E#',
      '#E.##E#',
      '#G..#.#',
      '#..E#.#',
      '#######',
    ]).battleScore).toBe(39514);
  });

  it('Should play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#E.G#.#',
      '#.#G..#',
      '#G.#.G#',
      '#G..#.#',
      '#...E.#',
      '#######',
    ]).battleScore).toBe(27755);
  });

  it('Should play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#.E...#',
      '#.#..G#',
      '#.###.#',
      '#E#G#G#',
      '#...#G#',
      '#######',
    ]).battleScore).toBe(28944);
  });

  it('Should play the battle and output the final score', () => {
    expect(implem([
      '#########',
      '#G......#',
      '#.E.#...#',
      '#..##..G#',
      '#...##..#',
      '#...#...#',
      '#.G...G.#',
      '#.....G.#',
      '#########',
    ]).battleScore).toBe(18740);
  });

  it('Should find the lowest attack power, play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#.G...#',
      '#...EG#',
      '#.#.#G#',
      '#..G#E#',
      '#.....#',
      '#######',
    ]).scoreWithMinimumAttackPower).toBe(4988);
  });

  it('Should find the lowest attack power, play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#E..EG#',
      '#.#G.E#',
      '#E.##E#',
      '#G..#.#',
      '#..E#.#',
      '#######',
    ]).scoreWithMinimumAttackPower).toBe(31284);
  });

  it('Should find the lowest attack power, play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#E.G#.#',
      '#.#G..#',
      '#G.#.G#',
      '#G..#.#',
      '#...E.#',
      '#######',
    ]).scoreWithMinimumAttackPower).toBe(3478);
  });

  it('Should find the lowest attack power, play the battle and output the final score', () => {
    expect(implem([
      '#######',
      '#.E...#',
      '#.#..G#',
      '#.###.#',
      '#E#G#G#',
      '#...#G#',
      '#######',
    ]).scoreWithMinimumAttackPower).toBe(6474);
  });

  it('Should find the lowest attack power, play the battle and output the final score', () => {
    expect(implem([
      '#########',
      '#G......#',
      '#.E.#...#',
      '#..##..G#',
      '#...##..#',
      '#...#...#',
      '#.G...G.#',
      '#.....G.#',
      '#########',
    ]).scoreWithMinimumAttackPower).toBe(1140);
  });
});
